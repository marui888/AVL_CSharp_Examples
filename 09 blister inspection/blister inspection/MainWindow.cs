//
// Adaptive Vision Library .NET Example - "Blister inspection" example
//
// Simple application that uses Adaptive Vision Library .NET to detect missing pills in blister.
//
// Copyright (C) 2020 Adaptive Vision Sp. z o.o.
// Version: 4.12_74433
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AvlNet;

namespace blister_inspection
{
    public partial class MainWindow : Form
    {
        #region Private fields
        private Image currentImageBuffer = new Image();

        /// <summary>Custom enumerator that enumerates through sample images</summary>
        private readonly string ImageDir = "../../../../../_media/blister_inspection_img";
        private int currentImageIndex;
        private readonly string[] imageFiles;

        /// <summary>
        /// Inspection algorithm object
        /// </summary>
        private readonly BlisterInspector blisterInspector = new BlisterInspector();
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            imageFiles = System.IO.Directory.GetFiles(ImageDir, "*.png");
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if (!imageFiles.Any())
            {
                MessageBox.Show(this, string.Format("No image found in directory '{0}'", ImageDir));
            }
            else
            {
                startButton.Enabled = false;
                timer.Enabled = true;
                currentImageIndex = 0;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (imageFiles.Any() && currentImageIndex < imageFiles.Length)
            {
                AVL.LoadImage(imageFiles[currentImageIndex], false, currentImageBuffer);
                try
                {
                    blisterInspector.DoInspection(currentImageBuffer);
                    if (pictureBox.Image != null)
                        pictureBox.Image.Dispose();

                    pictureBox.Image = currentImageBuffer.CreateBitmap();

                    ++currentImageIndex;
                }
                catch (Exception error)
                {
                    timer.Enabled = false;
                    MessageBox.Show(error.Message);
                }
            }
            else
            {
                startButton.Enabled = true;
                timer.Enabled = false;
            }
        }
    }
}
