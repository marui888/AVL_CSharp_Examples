//
// Adaptive Vision Library .NET Example - "Nails, screws and nuts" example
//
// Simple application that uses Adaptive Vision Library .NET to separate nails from other objects on image.
//
// Copyright (C) 2020 Adaptive Vision Sp. z o.o.
// Version: 4.12_74433
//
using System;
using System.Windows.Forms;
using AvlNet;

namespace nails_screws_and_nuts_avlNET
{
    public partial class MainWindow : Form
    {
        #region Private fields

        /// <summary>
        /// Image object to work with
        /// </summary>
        private Image partsImage;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor which initializes MainWindow instance
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            partsImage = new Image();
        }

        #endregion

        #region Events handling
        
        /// <summary>
        /// Handles "Separete nails" button click by performing classification on image
        /// </summary>
        private void separateBtn_Click(object sender, EventArgs e)
        {
            if (partsImage == null)
                return;


            //mr!! 注意 Image,Region都需要Dispose. Bolbs还是SaleList<Region>类型.
            using (var output = new Image())
            using (var objects = new Region())
            using (var blobs = new SafeList<Region>())
            {
                try
                {
                    // Extract all objects from image
                    //public static void ThresholdToRegion
                    //(
                    //    AvlNet.Image inImage,
                    //    NullableRef<AvlNet.Region> inRoi,
                    //    float? inMinValue,
                    //    float? inMaxValue,
                    //    float inHysteresis,
                    //    AvlNet.Region outRegion
                    //)
                    AVL.ThresholdToRegion(partsImage, null, null, 200.0f, 0.0f, objects);

                    // Prepare image for drawing results
                    //public static void ConvertToMultichannel
                    //(
                    //    AvlNet.Image inMonoImage,
                    //    int inNewDepth,
                    //    AvlNet.Image outImage
                    //)
                    AVL.ConvertToMultichannel(partsImage, 3, output);

                    // Split objects into separate regions - parts
                    //public static void SplitRegionIntoBlobs(
                    //    AvlNet.Region inRegion,
                    //    AvlNet.RegionConnectivity inConnectivity,
                    //    int inMinBlobArea,
                    //    int? inMaxBlobArea,
                    //    bool inRemoveBoundaryBlobs,
                    //    out AvlNet.Region[] outBlobs,
                    //    out int[] diagBlobAreas
                    //)
                    AVL.SplitRegionIntoBlobs(objects, RegionConnectivity.EightDirections, 10, false, blobs);

                    // Draw all blobs which elongation is big enough
                    foreach (var blob in blobs)
                    {
                        float elongation;
                        AVL.RegionElongation(blob, out elongation);
                        if (elongation >= 10.0f)
                            AVL.DrawRegion(output, blob, null, Pixel.Green, 0.8f);
                    }

                    if (pictureBox1.Image != null)
                        pictureBox1.Image.Dispose();

                    // Show results
                    pictureBox1.Image = output.CreateBitmap();
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }
        }

        /// <summary>
        /// Loads example image when window is opened
        /// </summary>
        private void MainWindow_Load(object sender, EventArgs e)
        {
            AVL.LoadImage("../../../../../_media/parts.png", false, partsImage);
            pictureBox1.Image = partsImage.CreateBitmap();
        }

        #endregion
    }
}
