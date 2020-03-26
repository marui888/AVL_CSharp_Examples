//
// Adaptive Vision Library .NET Example - "Streaming GigE camera AvlNet" example
//
// Simple application that uses Adaptive Vision Library .NET to connect to the GigEVision camera and display acquired images in the ZoomingVideoBox control.
//
// Copyright (C) 2020 Adaptive Vision Sp. z o.o.
// Version: 4.12_74433
//

using AvlNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace streaming_GigE_camera_AvlNet
{
    public partial class Form1 : Form
    {
        Thread acquisitionThread;
        SynchronizationContext uiContext;
        CancellationTokenSource cancellationTokenSource;
        int frameCount;

        private int? deviceHandle;
        private GigEVision_DeviceDescriptor device;

        public int? DeviceHandle
        {
            get { return deviceHandle; }
            private set
            {
                if (deviceHandle != value)
                {
                    if (deviceHandle.HasValue)
                        GenICam.GigEVision_CloseHandle(deviceHandle.Value);

                    deviceHandle = value;

                    connectionStatusLabel.Text = IsConnected ? "Connected" : "Disconnected";
                }
            }
        }

        public bool IsConnected { get { return deviceHandle.HasValue; } }

        /// <summary>Gets or sets the current GigEVision device description.</summary>
        public GigEVision_DeviceDescriptor Device
        {
            get { return device; }
            private set
            {
                if (device != value)
                {
                    device = value;

                    manufacturerLabel.Text = device != null ? device.ManufacturerName : string.Empty;
                    modelLabel.Text = device != null ? device.ModelName : string.Empty;
                    addressLabel.Text = device != null ? device.IpAddress : string.Empty;
                }
            }
        }

        public Form1()
        {
            InitializeComponent();

            uiContext = SynchronizationContext.Current;
        }

        /// <summary>Image acquisition and presentation thread function.</summary>
        ///             
        //mr?? 这样使用Thread类型来运行AcquisitionThread, 
        //  1. AcquisitionThread() 是成员方法, C#里没有全局方法.
        //  2. 类里面的所有字段和方法在成员方法里是可以访问的. 
        //
        //acquisitionThread = new Thread(AcquisitionThread);
        //cancellationTokenSource = new CancellationTokenSource();
        //acquisitionThread.Start();
        void AcquisitionThread()
        {
            Image previousImage = null;
            DateTime lastFrameRateTime = DateTime.Now;

            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                Image frame;
                if (CaptureFrame(out frame))
                {
                    //mr:: 在Form的ctor里有 uiContext = SynchronizationContext.Current;
                    uiContext.Post((o) =>
                    {
                        videoBox.SetImage(frame);

                        // calculate current FPS
                        ++frameCount;
                        double seconds = (DateTime.Now - lastFrameRateTime).TotalSeconds;
                        if (seconds > 3)
                        {
                            lastFrameRateTime = DateTime.Now;
                            frameRateLabel.Text = string.Format("{0:0.0} fps", frameCount / seconds);
                            frameCount = 0;
                        }

                        // dispose previous frame
                        if (previousImage != null)
                            previousImage.Dispose();

                        previousImage = frame;
                    }, null);
                }
            }

            frameRateLabel.Text = string.Empty;
        }

        /// <summary>Connects to the first available GigEVision camera device and runs the acquisition thread.</summary>
        void StartAcquisition()
        {
            connectionStatusLabel.Text = "Connecting...";

            Device = FindFirstAvailableDevice();

            if (Device == null)
            {
                connectionStatusLabel.Text = "Disconnected";
                throw new Exception("Couldn't find any GigEVision camera device");
            }

            DeviceHandle = GenICam.GigEVision_OpenDevice(Device.IpAddress);

            GenICam.GigEVision_StartAcquisition(DeviceHandle.Value, string.Empty, 1);

            acquisitionThread = new Thread(AcquisitionThread);
            cancellationTokenSource = new CancellationTokenSource();

            acquisitionThread.Start();
        }

        /// <summary>Signals the acquisition thread to stop and waits asynchronously for actual stop</summary>
        void StopAcquisition()
        {
            if (cancellationTokenSource != null)
                cancellationTokenSource.Cancel();

            DeviceHandle = null;

            if (acquisitionThread != null)
                acquisitionThread.Join();

            cancellationTokenSource = null;
            Device = null;
            acquisitionThread = null;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            StopAcquisition();
            base.OnClosing(e);
        }

        /// <summary>Asynchronously looks for first available GigEVision camera device.</summary>
        GigEVision_DeviceDescriptor FindFirstAvailableDevice()
        {
            var devices = new List<GigEVision_DeviceDescriptor>();
            GenICam.GigEVision_FindDevices(2000, 1, devices);

            if (!devices.Any())
                return null;

            return devices.FirstOrDefault();
        }

        /// <summary>Flushes the input buffer and tries to receive the next frame</summary>
        bool CaptureFrame(out Image frame)
        {
            frame = new Image();

            if (!IsConnected)
                throw new ApplicationException("Cannot capture frames when no device is connected.");

            const int FrameTimeout = 150;    // ms

            // Flush input queue to remove any desync with device stream.
            GenICam.GigEVision_FlushInputQueue(DeviceHandle.Value);

            // Await and receive first frame. If this frame gets corrupted or lost we need to explicitly react, otherwise we could
            // crash because of single frame error or desync with device stream.
            //mr?? GigEVision_TryReceiveImage() 是异步方法吗
            //public static bool GigEVision_TryReceiveImage
            //(
            //    int inDeviceHandle,
            //    int inTimeout,
            //    AvlNet.Image outImage,
            //    out ulong outFrameId,
            //    out ulong outTimestamp
            //)
            if (!GenICam.GigEVision_TryReceiveImage(DeviceHandle.Value, FrameTimeout, frame))
                return false;

            return true;
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            startButton.Enabled = false;
            try
            {
                StartAcquisition();
                stopButton.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
                startButton.Enabled = true;
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            StopAcquisition();
            stopButton.Enabled = false;
            startButton.Enabled = true;
        }
    }
}
