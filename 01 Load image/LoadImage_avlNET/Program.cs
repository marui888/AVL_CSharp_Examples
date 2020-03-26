//
// Adaptive Vision Library .NET Example - 'Load image' example
//
// Simple console application that uses Adaptive Vision Library .NET to load the image, show it in a new window and present its properties.
//
// Copyright (C) 2020 Adaptive Vision Sp. z o.o.
// Version: 4.12_74433
//

using System;
using AvlNet;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace LoadImage
{
    class Program
    {
        /// <summary>
        /// Path of the example image.
        /// </summary>
        private const string ImagePath = @"..\..\..\..\..\..\_media\face.png";

        ConcurrentDictionary<string, int> a;

        /// <summary>
        /// Starting point of the application.
        /// </summary>
        static void Main()
        {
            Console.WriteLine();
            Console.WriteLine("Adaptive Vision Library .NET Example - \"Load Image\".");
            Console.WriteLine();

            try
            {
                ExampleCode();

                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
                throw;
            }
            finally
            {
                // Make sure to free all resources reserved by DebugPreviewShowNewImage function.
                AVL.DebugPreviewCloseAllWindows();
            }
        }

        /// <summary>
        /// Loads image from path, shows it in new window and presents its properties in Console window.
        /// </summary>
        private static void ExampleCode()
        {
            // Prepare local object for image.
            using (Image image = new Image())
            {
                // Load an image from file specified by its relative path. Image type will
                // be recognized automatically using file extension.
                AVL.LoadImage(ImagePath, false, image);

                // Access loaded image properties.
                Console.WriteLine(string.Format("        Size: {0}x{1}", image.Width, image.Height));
                Console.WriteLine(string.Format("Num channels: {0}", image.Depth));
                Console.WriteLine(string.Format("  Plain type: {0}", image.Type.ToString()));
                Console.WriteLine(string.Format("   Line size: {0}", image.Pitch));

                // Image is ready to use - preview its content for purpose of this demo.
                AVL.DebugPreviewShowNewImage(image);
            }
        }
    }
}
