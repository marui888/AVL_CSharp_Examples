//
// Adaptive Vision Library .NET Example - "WPF Example" example
//
// Simple application that uses Adaptive Vision Library .NET in WPF.
//
// Copyright (C) 2020 Adaptive Vision Sp. z o.o.
// Version: 4.12_74433
//

using System;
using System.Windows;
//using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using AvlNet;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private IntPtr ptrBitmap;
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                UpdateImage(0.0f);
            }
            catch(Exception e)
            {
                MessageBox.Show("Unable to start application: " + e.Message);
            }
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        extern static bool DeleteObject(IntPtr hObject);

        private void UpdateImage(float angle)
        {
            //mr:: imgColor,imgResult ��AvlNet.Image����
            using (Image
                imgColor = new Image(),
                imgResult = new Image())
            {

                // Call basic AVL functions
                AVL.TestImage(TestImageId.Lena, imgColor, null);

                //public static void RotateImage(
                //    AvlNet.Image inImage,
                //    float inAngle,
                //    AvlNet.RotationSizeMode inSizeMode,
                //    AvlNet.InterpolationMethod inInterpolationMethod,
                //    bool inInverse,
                //    out AvlNet.Image outImage
                //)
                AVL.RotateImage(imgColor, 
                    angle, 
                    RotationSizeMode.Preserve, 
                    InterpolationMethod.Bilinear, 
                    false, 
                    imgResult);

                // ���� System.Drawing.Bitmap ����
                using (var imgBitmap = imgResult.CreateBitmap())
                {

                    //mr:: ��DeleteObject()�Ķ���
                    //mr:: ��Wind32APIɾ��IntPtrָ��ptrBitmap
                    if (ptrBitmap != IntPtr.Zero)
                        DeleteObject(ptrBitmap);

                    //mr:: System.Drawing.Bitmap��GetHbitmap()����IntPtr����
                    ptrBitmap = imgBitmap.GetHbitmap();

                    // Create WPF image source.
                    //mr:: CreateBitmapSourceFromHBitmap() ����BitmapSource ����; image1��WPF��Image�ؼ�.
                    //mr:: Imaging�� System.Windows.Interop �ռ�; BitmapSizeOptions �� System.Windows.Media.Imaging �ռ�

                    //  [System.Security.SecurityCritical]
                    //public static System.Windows.Media.Imaging.BitmapSource 
                    //    CreateBitmapSourceFromHBitmap(IntPtr bitmap,
                    //                IntPtr palette, 
                    //                System.Windows.Int32Rect sourceRect,
                    //                System.Windows.Media.Imaging.BitmapSizeOptions sizeOptions);
                    image1.Source = Imaging.CreateBitmapSourceFromHBitmap(
                                            ptrBitmap, IntPtr.Zero, Int32Rect.Empty,
                                            BitmapSizeOptions.FromEmptyOptions());
                }
            }
        }

        /// <summary>
        /// Rotate Lena image using the slider.
        /// </summary>
        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateImage((float) e.NewValue);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ptrBitmap != IntPtr.Zero)
                DeleteObject(ptrBitmap);
        }
    }
}
