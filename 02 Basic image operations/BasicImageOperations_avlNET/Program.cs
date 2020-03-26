//
// Adaptive Vision Library .NET Example - 'Basic image operations' example
//
// Simple console application that uses Adaptive Vision Library .NET to load the image and perform basic operations on it.
//
// Copyright (C) 2020 Adaptive Vision Sp. z o.o.
// Version: 4.12_74433
//

using System;
using AvlNet;
using System.Threading;
using System.Threading.Tasks;


namespace BasicImageOperations_avlNET
{
    class Program
    {
        /// <summary>
        /// Path of the example image.
        /// </summary>
        //private const string ImagePath = @"..\..\..\..\..\_media\face.png";
        private const string ImagePath = @"..\..\..\..\..\..\_media\face.png";
        private const string ImagePath2 = @"..\..\..\..\..\..\_media\face.png";


        /// <summary>
        /// Starting point of the application.
        /// </summary>
        static void Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine("Adaptive Vision Library .NET Example - \"Basic image operations\".");
            Console.WriteLine();

            try
            {
                var task = Task.Factory.StartNew(new Action(() => ExampleCode()));

                //ExampleCode();
                // Wait for preview GUI to close (only for purpose of this demo)
                //Thread.Sleep(3000);
                //Task.Delay(3000).Wait();
                task.Wait();
                AVL.DebugPreviewWaitForWindowsClose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
            }
            finally
            {
                // Make sure to free all resources reserved by DebugPreviewShowNewImage functions.
                AVL.DebugPreviewCloseAllWindows();
            }
        }

        /// <summary>
        /// Loads the image and performs basic operations on it.
        /// </summary>
        private static void ExampleCode()
        {
            // Prepare the image, minding proper object disposing.
            using (Image inputImage = new Image())
            {
                AVL.LoadImage(ImagePath, false, inputImage);

                // Open new preview window and show image in it. All new windows
                // will be presented side by side.
                AVL.DebugPreviewShowNewImage(inputImage);

                // Multiple pixels values by 5.
                using (Image mulImage = new Image())
                {
                    AVL.MultiplyImage(inputImage, null, 5, mulImage);

                    AVL.DebugPreviewShowNewImage(mulImage, "Multiply by 5 image", -1, -1);
                }


                // Apply the threshold operation.
                using (Image thresholdImage = new Image())
                {
                    AVL.ThresholdImage(inputImage, null, 150, null, 0, thresholdImage);

                    AVL.DebugPreviewShowNewImage(thresholdImage, "Threshold image", -1, -1);
                }


                // Mirror input image vertically.
                using (Image mirrorImage = new Image())
                {
                    AVL.MirrorImage(inputImage, MirrorDirection.Vertical, mirrorImage);

                    AVL.DebugPreviewShowNewImage(mirrorImage, "Flipped Image", -1, -1);
                }


                // Apply smoothing to image.
                using (Image gaussImage = new Image())
                {
                    AVL.SmoothImage_Gauss(inputImage, null, 3.5f, 3.5f, 2, gaussImage);

                    AVL.DebugPreviewShowNewImage(gaussImage, "Gauss Image", -1, -1);
                }


                // Calculate image gradient.
                using (Image gradientImage = new Image())
                {
                    AVL.GradientImage_Mask(inputImage, null, GradientMaskOperator.Sobel, 1, gradientImage);

                    // For better preview in window - remove negative values from image by applying Absolute function.
                    AVL.AbsoluteValueImage(gradientImage, null, gradientImage);

                    AVL.DebugPreviewShowNewImage(gradientImage, "Gradient Image", -1, -1);
                }
            }

            
        }
    }
}
