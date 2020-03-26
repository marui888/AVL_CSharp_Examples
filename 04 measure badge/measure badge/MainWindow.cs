//
// Adaptive Vision Library .NET Example - "Measure badge" example
//
// Simple application that uses Adaptive Vision Library .NET. It performs simple 1D measurement on provided metal badge images.
//
// Copyright (C) 2020 Adaptive Vision Sp. z o.o.
// Version: 4.12_74433
//

using System;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace MeasureBadge
{
    /// <summary>
    /// Main window of the application
    /// </summary>
    public partial class MainWindow : Form
    {
        #region Private fields
        /// <summary>
        /// BadgeMeasurement object which allows to perform 1D measurements on provided images
        /// </summary>
        private readonly BadgeMeasurement measurement = new BadgeMeasurement();

        /// <summary>
        /// Path to directory with provided badge images
        /// </summary>
        private const string ImagePath = "../../../../../_media/badge_measurement_img";
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes instance of the MainWindow class
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            //set default images path
            measurement.ImagesDirectory = ImagePath;
        }
        #endregion

        #region Events handling

        /// <summary>
        /// Handles click on "NextImage" button.
        /// </summary>
        private void nextImageBtn_Click(object sender, EventArgs e)
        {
            try
            {
                //mr:: 1. 
                var task = new Task(() =>
                {
                    using (var currImage = new AvlNet.Image())
                    {
                        measurement.GetNextMeasuredImage(currImage);

                        Invoke(new Action(() =>
                        {
                            if (pictureBox1.Image != null)
                                pictureBox1.Image.Dispose();

                            pictureBox1.Image = currImage.CreateBitmap();

                            var distance = measurement.LastMeasuredDistance;
                            distLabel.Text = string.IsNullOrEmpty(distance) ? "unknown" : distance;
                        }));

                    }
                    //distLabel.Text = "unknown";  
                });

                task.Start();



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
                return;
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null) components.Dispose();
            }

            base.Dispose(disposing);
        }

        #endregion

    }
}
