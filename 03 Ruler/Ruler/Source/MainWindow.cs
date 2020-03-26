//
// Adaptive Vision Library .NET Example - 'Ruler' example
//
// Simple application that uses Adaptive Vision Library .NET to find the stripe on image and measure its width.
//
// Copyright (C) 2020 Adaptive Vision Sp. z o.o.
// Version: 4.12_74433
//

using System;
using System.Windows.Forms;
using AvlNet;
using System.Threading.Tasks;
using System.Threading;

namespace Ruler
{
    /// <summary>
    /// Main window of the application.
    /// </summary>
    public partial class MainWindow : Form
    {

        #region Private fields

        /// <summary>
        /// Image loaded from file
        /// </summary>
        private AvlNet.Image image = new AvlNet.Image();

        /// <summary>
        /// Path of example image, loaded after start of the application
        /// </summary>
        private const string ExampleImagePath = "..\\..\\..\\..\\..\\_media\\example.png";

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            // Initialize window
            InitializeComponent();

            // Try to load an example image
            if (!System.IO.File.Exists(ExampleImagePath))
            {
                MessageBox.Show( string.Format("File {0} not found", ExampleImagePath));
                return;
            }

            textImagePath.Text = System.IO.Path.GetFullPath(ExampleImagePath);
            LoadImage();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Loads image from path specified in text box and saves it in 'image' variable.
        /// </summary>
        private void LoadImage()
        {
            try
            {
                //mr?? 为啥不将这个代码封装到ExtendedPictureBox内部.
                if (pictureBox.Image != null)
                {
                    pictureBox.Image.Dispose();
                    pictureBox.Image = null;
                }

                string imagePath = textImagePath.Text;
                AVL.LoadImage(imagePath, false, image);
                pictureBox.Image = image.CreateBitmap();
                SetStatusLabel("Please, select the scanning segment on image");
                openFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(System.IO.Path.GetFullPath(imagePath));
            }
            catch (Exception ex)
            {
                SetStatusLabel(string.Format("Error: {0}", ex.Message));
                return;
            }
        }

        /// <summary>
        /// Sets the text on status label.
        /// </summary>
        /// <param name="status">Application status to set</param>
        private void SetStatusLabel(string status)
        {
            labelStatus.Text = status;
        }

        #endregion

        #region Events handling

        /// <summary>
        /// Handles click on 'loadImage' button.
        /// </summary>
        private void loadImage_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK) 
                return;

            textImagePath.Text = openFileDialog.FileName;
            LoadImage();
        }

        /// <summary>
        /// Handles segment selection - initialized the measurement
        /// </summary>
        private async void pictureBox_ScanningSegmentChosen(object sender, ScanningSegementEventArgs args)
        {
            // Clear the previous result
            pictureBox.SetMeasurementResult(null, null);

            try
            {
                //mr:: Cursor是Control的一个属性, 类型为Cursor; Cursors是一个 sealed class, 
                //     内部定义了很多只读静态属性,例如: public static Cursor PanSW { get; }
                Cursor = Cursors.WaitCursor;

                Segment measuredSegment;
                float measuredSegmentLength=float.NaN;

                measuredSegment = new Segment();

                //mr:: 使用UI线程计算stripe
                //bool measuredOk = Measurements.DoMeasurement(image,
                //    args.Segment,
                //    radioButtonAnyStripe.Checked //mr:: ?. 运算符的串接使用
                //        ? AvlNet.Polarity.Any
                //        : (radioButtonDarkStripe.Checked ? AvlNet.Polarity.Dark : AvlNet.Polarity.Bright),
                //    out measuredSegment,
                //    out measuredSegmentLength);

                //mr:: 使用worker线程计算stripe
                bool measuredOk = false;
                var task = new Task<int>(() =>
                {
                    measuredOk = Measurements.DoMeasurement(image,
                       args.Segment,
                       radioButtonAnyStripe.Checked //mr:: ?. 运算符的串接使用
                           ? AvlNet.Polarity.Any
                           : (radioButtonDarkStripe.Checked ? AvlNet.Polarity.Dark : AvlNet.Polarity.Bright),
                       out measuredSegment,
                       out measuredSegmentLength);
                    return Thread.CurrentThread.ManagedThreadId;
                });

                task.Start();
                int TaskThreadId = await task;
                this.Text = $"UI thread : {Thread.CurrentThread.ManagedThreadId} ; Task thread : {TaskThreadId}";

                // Show the measurement result
                if (measuredOk)
                {                    
                    SetStatusLabel(string.Format("Segment length: {0}", measuredSegmentLength));
                    pictureBox.SetMeasurementResult(measuredSegment, measuredSegmentLength.ToString());
                }
                else // Stripe not found
                {
                    SetStatusLabel("Found no segment to measure");
                    pictureBox.SetMeasurementResult(null, "Found no segment to measure");
                }                
            }
            catch (Exception ex)
            {
                // Show exception information
                pictureBox.SetMeasurementResult(null, "Error");
                SetStatusLabel(string.Format("Measurement error: {0}", ex.Message));
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        #endregion

    }
}
