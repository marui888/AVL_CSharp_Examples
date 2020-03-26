//
// Adaptive Vision Library .NET Example - 'Ruler' example
//
// Simple application that uses Adaptive Vision Library .NET to find the stripe on image and measure its width.
//
// Copyright (C) 2020 Adaptive Vision Sp. z o.o.
// Version: 4.12_74433
//

using AvlNet;

namespace Ruler
{
    /// <summary>
    /// Access to AVL.NET library - provides measurement methods.
    /// </summary>
    class Measurements
    {

        /// <summary>
        /// Finds the stripe on the image.
        /// </summary>
        /// <param name="image">Image to find the stripe on.</param>
        /// <param name="scanningSegment">Scanning segment chosen by user.</param>
        /// <param name="selectedPolarity">Polarity of the stripe - dark, bright or any.</param>
        /// <param name="measuredSegment">Segment representing the stripe that has been found.</param>
        /// <param name="measuredSegmentLength">Length of the segment.</param>
        /// <returns></returns>
        public static bool DoMeasurement(Image image, Segment scanningSegment, Polarity selectedPolarity,
            out Segment measuredSegment, out float measuredSegmentLength)
        {
            // Create disposable scanning parameters
            //mr:: 生成扫描线段. 注意Segment2D, Segment,是不同类型.Segment是程序自定义类型.
            Segment2D avlScanningSegment = new Segment2D(
                scanningSegment.StartPoint.X, 
                scanningSegment.StartPoint.Y,
                scanningSegment.EndPoint.X, 
                scanningSegment.EndPoint.Y);

            //mr:: 生成Stripe扫描参数
            StripeScanParams scanParams = new StripeScanParams()
            {
                StripePolarity = selectedPolarity
            };

            //mr:: Nullable.Create<Stripe1D>()
            var stripe = Nullable.Create<Stripe1D>();

            // Create the ScanMap object
            //mr:: ScanMap需要Dispose
            using (var scanMap = new ScanMap())
            {
                //mr:: 生成Image格式.
                ImageFormat imageFormat = new ImageFormat(image);
                //mr:: 生成扫描Map
                /*
                 * public static void CreateScanMap
                (
                    AvlNet.ImageFormat inImageFormat,
                    AvlNet.Path inScanPath,
                    AvlNet.CoordinateSystem2D? inScanPathAlignment,
                    int inScanWidth,
                    AvlNet.InterpolationMethod inImageInterpolation,
                    AvlNet.ScanMap outScanMap,
                    AvlNet.Path outAlignedScanPath,
                    IList<AvlNet.Path> diagSamplingPoints,
                    out float diagSamplingStep
                )
                */
                AVL.CreateScanMap(imageFormat, 
                    new Path(avlScanningSegment),
                    null, 
                    5, 
                    InterpolationMethod.Bilinear, 
                    scanMap);

                // Do the scanning
                //mr:: Stripe1D is a class, declared without out keyword in parameters of AVL.ScanSingleStripe
                /*
                 * public static void ScanSingleStripe
                (
                    AvlNet.Image inImage,
                    AvlNet.ScanMap inScanMap,
                    AvlNet.StripeScanParams inStripeScanParams,
                    AvlNet.Selection inStripeSelection,
                    AvlNet.LocalBlindness? inLocalBlindness,
                    INullable<AvlNet.Stripe1D> outStripe,
                    AvlNet.Profile diagBrightnessProfile,
                    AvlNet.Profile diagResponseProfile
                )
                */
                AVL.ScanSingleStripe(image, scanMap, scanParams, Selection.Best, null, stripe);
            }

            if (stripe.HasValue) // Stripe has been found
            {
                //mr:: Segment is a class; measuredSegment is declared with out keyword ;
                //mr:: Segment的ctor输入参数是System.Drawing.Point类型.
                measuredSegment = new Segment(
                    new System.Drawing.Point((int)stripe.Value.Point1.X, (int)stripe.Value.Point1.Y),
                    new System.Drawing.Point((int)stripe.Value.Point2.X, (int)stripe.Value.Point2.Y));

                measuredSegmentLength = stripe.Value.Width;

                return true;
            }
            else // Stripe not found
            {
                // Set values of output parameters
                measuredSegment = null;
                measuredSegmentLength = 0.0f;
                return false;
            }
        }

    }
}
