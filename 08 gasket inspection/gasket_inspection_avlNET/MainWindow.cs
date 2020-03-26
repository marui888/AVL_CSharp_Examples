//
// Adaptive Vision Library .NET Example - "Gasket inspection" example
//
// Simple application that uses Adaptive Vision Library .NET to inspect rubber gasket.
//
// Copyright (C) 2020 Adaptive Vision Sp. z o.o.
// Version: 4.12_74433
//

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using AvlNet;

namespace gasket_inspection_avlNET
{
    public partial class MainWindow : Form
    {
        #region Private fields
        
        /// <summary>
        /// Directory with sample gasket images
        /// </summary>
        private const string ImagesDirectory = "../../../../../_media/gasket_inspection_img";

        private readonly string[] ImagePaths = System.IO.Directory.GetFiles(ImagesDirectory, "*.png");
        private int currentImageIndex;

        /// <summary>
        /// Image from which model will be generated
        /// </summary>
        private const string TemplateImagePath = ImagesDirectory + "/00003.png";

        /// <summary>
        /// Used to store detected circles
        /// </summary>
        private Circle2D[] circles;
        
        /// <summary>
        /// Used to store detected arcs
        /// </summary>
        private Arc2D[] arcs;

        /// <summary>
        /// Used to store measured segments
        /// </summary>
        private Segment2D[] segments;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes MainWindow and initializes algorithm object
        /// </summary>
        public MainWindow()
        {
            using (var templateImage = new AvlNet.Image())
            {
                try
                {
                    InitializeComponent();

                    AVL.LoadImage(TemplateImagePath, false, templateImage);
                    GasketInspector.Init(templateImage);

                }
                catch (Exception e)
                {
                    startButton.Enabled = false;
                    MessageBox.Show(e.Message);
                }
            }
        }

        #endregion

        #region Events' handling
        /// <summary>
        /// Handles clicking on "Start" button. Starts inspection.
        /// </summary>
        private void startButton_Click(object sender, EventArgs e)
        {
            if (!ImagePaths.Any())
                return;

            startButton.Enabled = false;
            timer.Enabled = true;
            currentImageIndex = 0;
        }

        /// <summary>
        /// Cleans resources
        /// </summary>
        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            GasketInspector.ClearResources();
        }

        /// <summary>
        /// Performs inspection in 500ms interval.
        /// </summary>
        private void timer_Tick(object sender, EventArgs e)
        {
            using (var currentImage = new AvlNet.Image())
            {
                if (currentImageIndex < ImagePaths.Length)
                {
                    AVL.LoadImage(ImagePaths[currentImageIndex], false, currentImage);
                    GasketInspector.Inspect(currentImage, out circles, out arcs, out segments);

                    if (pictureBox1.Image != null)
                        pictureBox1.Image.Dispose();

                    pictureBox1.Image = currentImage.CreateBitmap();
                    ++currentImageIndex;
                }
                else
                {
                    timer.Enabled = false;
                    startButton.Enabled = true;
                }
            }
        }

        /// <summary>
        /// Draw results on the original image
        /// </summary>
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            DrawResultsOnPictureBox(e.Graphics);
        }

        /// <summary>
        /// Draws inspection results on form, using .NET standard classes
        /// </summary>
        private void DrawResultsOnPictureBox(Graphics g)
        {
            var smoothingModeBackup = g.SmoothingMode;
            try
            {
                using (var pen = new Pen(Color.Orange, 6.0f))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    if (circles != null)
                    {
                        foreach (var circle in circles)
                        {
                            g.DrawEllipse(pen, circle.Center.X - circle.Radius, circle.Center.Y - circle.Radius,
                                circle.Radius*2, circle.Radius*2);
                        }
                    }

                    if (arcs != null)
                    {
                        foreach (var arc in arcs)
                        {
                            g.DrawArc(pen, arc.Center.X - arc.Radius, arc.Center.Y - arc.Radius, arc.Radius*2,
                                arc.Radius*2, arc.StartAngle, arc.SweepAngle);
                        }
                    }
                }
                
                using (var pen = new Pen(Color.DarkGreen, 3.0f))
                {
                    if (segments != null)
                    {
                        foreach (var segment in segments)
                        {
                            g.DrawLine(pen, segment.Point1.X, segment.Point1.Y, segment.Point2.X, segment.Point2.Y);
                        }
                    }
                }
            }
            finally
            {
                g.SmoothingMode = smoothingModeBackup;
            }
        }

        #endregion

    }
}
