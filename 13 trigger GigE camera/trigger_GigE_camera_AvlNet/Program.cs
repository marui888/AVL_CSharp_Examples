//
// Adaptive Vision Library .NET Example - "Trigger GigE camera AvlNet" example
//
// Simple application that uses Adaptive Vision Library .NET to connect to the GigEVision camera.
// Images are acquired in software trigger mode.
//
// Copyright (C) 2020 Adaptive Vision Sp. z o.o.
// Version: 4.12_74433
//

using AvlNet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace trigger_GigE_camera_AvlNet
{

    class Program
    {
        static string defaultTriggerSource = null;
        static string defaultTriggerMode = null;
        static string defaultTriggerSelector = null;

        static bool FindFirstAvailableDevice(out GigEVision_DeviceDescriptor device)
        {
            var devices = new List<GigEVision_DeviceDescriptor>();
            device = null;

            GenICam.GigEVision_FindDevices(800, 1, devices);

            if (devices == null || !devices.Any())
                return false;

            device = devices.First();

            Console.WriteLine(string.Format("{0} {1} [{2}]", device.ManufacturerName, device.ModelName, device.IpAddress));

            return true;
        }

        /// <summary>
        /// <para>Setups and verifies capabilities of general camera device of unknown type for this demo. Usually
        /// application should be created for specific device or device class, some of this setup and verification is
        /// not obligatory.</para>
        /// <para>Device parameter setup is based on GenICam SFNC standard.</para>
        /// </summary>
        static void SetupDevice(int handle)
        {
            Console.WriteLine("Setting up device...");

            // Device parameter setup is based on GenICam SFNC standard.

            // When selector is available, select "FrameStart" trigger in device. This parameter
            // must be set first because it determines what further parameters point to.
            if (GenICam.GenApi_GetParamExists(handle, "TriggerSelector"))
            {
                GenICam.GenApi_GetEnumParam(handle, "TriggerSelector", true, out defaultTriggerSelector);
                GenICam.GenApi_SetEnumParam(handle, "TriggerSelector", "FrameStart", true);
            }

            // Setup device so that it waits for software command to trigger new frame.
            GenICam.GenApi_GetEnumParam(handle, "TriggerSource", true, out defaultTriggerSource);
            GenICam.GenApi_SetEnumParam(handle, "TriggerSource", "Software", true);

            // Verify that command for software trigger exists.
            if (!GenICam.GenApi_GetParamExists(handle, "TriggerSoftware"))
                throw new MemberAccessException("Command \"TriggerSoftware\" is not defined by device description.");

            // Enable trigger
            if (GenICam.GenApi_GetParamExists(handle, "TriggerMode"))
            {
                GenICam.GenApi_GetEnumParam(handle, "TriggerMode", true, out defaultTriggerMode);
                GenICam.GenApi_SetEnumParam(handle, "TriggerMode", "On", true);
            }

            // Set acquisition mode to continuous - device should continue to
            // wait for triggers after each frame capture.
            GenICam.GenApi_SetEnumParam(handle, "AcquisitionMode", "Continuous", true);


            // If this device has automatic exposure, turn it off - we will be controlling
            // this parameter manually.
            if (GenICam.GenApi_GetParamExists(handle, "ExposureAuto"))
                GenICam.GenApi_SetEnumParam(handle, "ExposureAuto", "Off", false);

            // Set initial exposure value (this will also verify, if parameter is available).
            GenICam.GenApi_SetFloatParam(handle, "ExposureTimeAbs", 15000, true);
        }

        /// <summary>Resets the trigger-related parameters so other camera clients can acquire images without the need of adjusting camera.</summary>
        static void ResetDeviceSetup(int handle)
        {
            if (!string.IsNullOrEmpty(defaultTriggerSelector))
                GenICam.GenApi_SetEnumParam(handle, "TriggerSelector", defaultTriggerSelector, true);

            if (!string.IsNullOrEmpty(defaultTriggerSource))
                GenICam.GenApi_SetEnumParam(handle, "TriggerSource", defaultTriggerSource, true);

            if (!string.IsNullOrEmpty(defaultTriggerMode))
                GenICam.GenApi_SetEnumParam(handle, "TriggerMode", defaultTriggerMode, true);
        }

        static void RunAcquisition(GigEVision_DeviceDescriptor device)
        {
            var handle = GenICam.GigEVision_OpenDevice(device.IpAddress);

            try
            {
                SetupDevice(handle);
            }
            catch (Exception e)
            {
                ResetDeviceSetup(handle);
                Console.Error.WriteLine("Cannot setup camera device. Device may not be compatible with this demo.");
                Console.Error.WriteLine(e.Message);
                GenICam.GigEVision_CloseHandle(handle);
                return;
            }

            Console.WriteLine("Starting acquisition, close any preview window to stop...");

            // Start acquisition. That will init video stream and activate device, but frames will not be captured until triggered by software command.
            GenICam.GigEVision_StartAcquisition(handle, string.Empty, 1);

            DebugPreviewWindowState window1 = new DebugPreviewWindowState();
            DebugPreviewWindowState window2 = new DebugPreviewWindowState();

            int captureErrors = 0;

            try
            {
                using (Image
                    frameBuffer1 = new Image(),
                    frameBuffer2 = new Image())
                {
                    while (true)
                    {
                        if (!CaptureFramesPair(handle, 8000, 55000, frameBuffer1, frameBuffer2))
                        {
                            // Failed to receive frames.
                            if (++captureErrors >= 3)
                            {
                                // Three capture errors in row
                                throw new TimeoutException("Unable to capture frames.");
                            }
                            else
                            {
                                // Try again from beginning.
                                continue;
                            }
                        }

                        captureErrors = 0;

                        if (!AVL.DebugPreviewShowImage(window1, frameBuffer1) || !AVL.DebugPreviewShowImage(window2, frameBuffer2))
                            break;
                    }
                }
            }
            finally
            {
                ResetDeviceSetup(handle);
                GenICam.GigEVision_CloseHandle(handle);
            }
        }

        /// <summary>
        /// Captures two sequent frames from the camera with different exposure parameter values.
        /// Each frame is captured after explicit software trigger is sent to the device.
        /// </summary>
        private static bool CaptureFramesPair(int handle, double exposure1, double exposure2, Image outFrame1, Image outFrame2)
        {
            const int FrameTimeout = 150;    // ms

            // Flush input queue to remove any desync with device stream.
            GenICam.GigEVision_FlushInputQueue(handle);

            // Calculate timeouts for frames depending on required exposure time.
            int timeoutFrame1 = FrameTimeout + (int)(exposure1 / 1000);
            int timeoutFrame2 = FrameTimeout + (int)(exposure2 / 1000);

            // Set first exposure time and capture the frame.
            GenICam.GenApi_SetFloatParam(handle, "ExposureTimeAbs", exposure1);
            GenICam.GenApi_ExecuteCommand(handle, "TriggerSoftware");

            // Await and receive first frame. If this frame gets corrupted or lost we need to explicitly react, otherwise we could
            // crash because of single frame error or desync with device stream.
            if (!GenICam.GigEVision_TryReceiveImage(handle, timeoutFrame1, outFrame1))
                return false;

            // Set second exposure time, capture and receive frame.
            GenICam.GenApi_SetFloatParam(handle, "ExposureTimeAbs", exposure2);
            GenICam.GenApi_ExecuteCommand(handle, "TriggerSoftware");

            if (!GenICam.GigEVision_TryReceiveImage(handle, timeoutFrame2, outFrame2))
                return false;

            return true;
        }

        static int Main(string[] args)
        {
            Console.WriteLine("Retrieving device address...");

            GigEVision_DeviceDescriptor device;
            if (!FindFirstAvailableDevice(out device))
            {
                Console.Error.WriteLine("Couldn't find any GigE device.");
                return 1;
            }

            try
            {
                RunAcquisition(device);
            }
            catch(Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return 1;
            }

            return 0;
        }
    }
}
