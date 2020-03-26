//
// Adaptive Vision Library .NET Example - "Cap" example
//
// Simple application that uses Adaptive Vision Library .NET to find whether the bottle cap is correctly closed
//
// Copyright (C) 2020 Adaptive Vision Sp. z o.o.
// Version: 4.12_74433
//

using AvlNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace cap
{
    public partial class MainWindow : Form
    {
        #region Private fields

        /// <summary>
        /// Inspection algorithm object
        /// </summary>
        private CapInspection inspection;

        private readonly string ImageDir = "../../../../../_media/cap_img";

        /// <summary>Buffer that each iteration is filled with the loaded image</summary>
        private Image currentImageBuffer;
        private int currentImageIndex;

        private readonly string[] imagePaths;

        private Path scanPath;

        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();

            scanPath = new Path(new Segment2D(new Point2D(2.0f, 100.0f), new Point2D(380.0f, 100.0f)));

            currentImageBuffer = new Image();

            //mr:: imagePaths是string[]类型
            imagePaths = System.IO.Directory.GetFiles(ImageDir, "*.png");
            if (!imagePaths.Any())
            {
                MessageBox.Show(this, string.Format("No image found in directory {0}", ImageDir));
                toggleButton.Enabled = false;
            }
        }

        #endregion

        #region Events' handling

        /// <summary>
        /// Handles button click, performs inspection on image.
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (GetNextImage(currentImageBuffer))
                {
                    if (inspection == null)
                    {
                        inspection = new CapInspection(new ImageFormat(currentImageBuffer), scanPath);
                        toggleButton.Text = "Next image";
                    }

                    inspection.DoInspection(currentImageBuffer);

                    if (pictureBox1.Image != null)
                        pictureBox1.Image.Dispose();

                    pictureBox1.Image = currentImageBuffer.CreateBitmap();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
                Application.Exit();
            }
        }

        #endregion

        #region Private methods
        private bool GetNextImage(Image buffer)
        {
            if (!imagePaths.Any())
                return false;

            if (currentImageIndex >= imagePaths.Length)
                currentImageIndex = 0;

            AVL.LoadImage(imagePaths[currentImageIndex], false, buffer);
            ++currentImageIndex;

            return true;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //mr:: 注意哪些类型是需要Dispose的.
                if (inspection != null) inspection.Dispose();
                if (components != null) components.Dispose();
                if (scanPath != null) scanPath.Dispose();
                currentImageBuffer.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
