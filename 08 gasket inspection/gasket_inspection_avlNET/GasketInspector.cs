//
// Adaptive Vision Library .NET Example - "Gasket inspection" example
//
// Simple application that uses Adaptive Vision Library .NET to inspect rubber gasket.
//
// Copyright (C) 2020 Adaptive Vision Sp. z o.o.
// Version: 4.12_74433
//

using System;
using AvlNet;

namespace gasket_inspection_avlNET
{
    static class GasketInspector
    {
        #region Private fields
        /// <summary>
        /// Indicates whether class has been initialized, i.e. model was created
        /// </summary>
        private static bool isInitialized = false;

        /// <summary>
        /// Edge model used to find gasket on image
        /// </summary>
        private static SafeNullableRef<EdgeModel> edgeModel = AvlNet.Nullable.CreateSafe<EdgeModel>();

        /// <summary>
        /// Expected circle in bigger gasket hole
        /// </summary>
        private static readonly Circle2D largeExpectedCircle = new Circle2D(512.450f, 105.146f, 44.50f);

        /// <summary>
        /// Expected circle in smaller gasket hole
        /// </summary>
        private static readonly Circle2D smallExpectedCircle = new Circle2D(489.639f, 406.396f, 25.526f);

        /// <summary>
        /// Expected arc in upper gasket mounting spot
        /// </summary>
        private static readonly Arc2D upperExpectedArc = new Arc2D(243.357f, 102.957f, 20.479f, 192.894f, 275.407f);

        /// <summary>
        /// Expected arc in lower gasket mounting spot
        /// </summary>
        private static readonly Arc2D lowerExpectedArc = new Arc2D(224.885f, 390.420f, 21.004f, 236.677f, 281.082f);

        /// <summary>
        /// Parameters used to scan edges in shape fitting
        /// </summary>
        private static readonly EdgeScanParams scanParams = new EdgeScanParams(ProfileInterpolationMethod.Quadratic4, 1.0f, 5.0f, EdgeTransition.BrightToDark);

        #endregion

        #region Public properties

        /// <summary>
        /// Indicates whether class has been initialized
        /// </summary>
        public static bool IsInitialized { get { return isInitialized; } }

        #endregion

        #region Public methods

        /// <summary>
        /// Initializes the GasketInspector
        /// </summary>
        /// <param name="templateImage">Image with template gasket</param>
        public static void Init(Image templateImage)
        {
            Box modelBoundingBox;

            AVL.CreateBox(new Location(30, 185), Anchor2D.TopLeft, 100, 125, out modelBoundingBox);

            using (var modelRegion = new Region())
            {
                AVL.CreateBoxRegion(modelBoundingBox, templateImage.Width, templateImage.Height, modelRegion);

                AVL.CreateEdgeModel(templateImage, modelRegion, null, 0, null, 0.0f, 35.0f, 15.0f, 0.0f, 360.0f, 1.0f, 1.0f,
                    1.0f, 1.0f, 1.0f, edgeModel);
            }

            isInitialized = true;
        }

        /// <summary>
        /// Performs gasket inspection
        /// </summary>
        /// <param name="gasketImage">Image with gasket</param>
        /// <param name="detectedCircles">Detected circles in gasket holes</param>
        /// <param name="detectedArcs">Detected arcs in gasket mounting spots</param>
        /// <param name="conntectingSegments">Segments connecting detected primitives</param>
        public static void Inspect(Image gasketImage, out Circle2D[] detectedCircles, out Arc2D[] detectedArcs,
            out Segment2D[] conntectingSegments)
        {
            if (!isInitialized)
                throw new ApplicationException("GasketInspector has not been initialized!");

            var localSystem = GetGasketLocalCoordinateSystem(gasketImage);

            DetectCircles(gasketImage, out detectedCircles, out conntectingSegments, localSystem);

            DetectArcs(gasketImage, out detectedArcs, conntectingSegments, localSystem);
        }

        /// <summary>
        /// Detects arcs on gasket image
        /// </summary>
        private static void DetectArcs(Image gasketImage, out Arc2D[] detectedArcs, Segment2D[] conntectingSegments,
            CoordinateSystem2D localSystem)
        {
            using (ArcFittingMap
                arcMap1 = new ArcFittingMap(),
                arcMap2 = new ArcFittingMap())
            {
                //public static void CreateArcFittingMap
                //(
                //    AvlNet.ImageFormat inImageFormat,
                //    AvlNet.ArcFittingField inFittingField,
                //    AvlNet.CoordinateSystem2D? inFittingFieldAlignment,
                //    int inScanCount,
                //    int inScanWidth,
                //    AvlNet.InterpolationMethod inImageInterpolation,
                //    AvlNet.ArcFittingMap outFittingMap,
                //    IList<AvlNet.Segment2D> diagScanSegments,
                //    IList<AvlNet.Rectangle2D> diagSamplingAreas
                //)

                AVL.CreateArcFittingMap(new ImageFormat(gasketImage), 
                    new ArcFittingField(upperExpectedArc, 20.0f), 
                    localSystem, 
                    10, 5, 
                    InterpolationMethod.Bilinear,
                    arcMap1);

                AVL.CreateArcFittingMap(new ImageFormat(gasketImage), 
                    new ArcFittingField(lowerExpectedArc, 20.0f), 
                    localSystem, 
                    10, 5, 
                    InterpolationMethod.Bilinear,
                    arcMap2);

                Arc2D? upperArc, lowerArc;


                //public static void FitArcToEdges
                //(
                //    AvlNet.Image inImage,
                //    AvlNet.ArcFittingMap inFittingMap,
                //    AvlNet.EdgeScanParams inEdgeScanParams,
                //    AvlNet.Selection inEdgeSelection,
                //    AvlNet.LocalBlindness? inLocalBlindness,
                //    float inMaxIncompleteness,
                //    AvlNet.CircleFittingMethod inFittingMethod,
                //    AvlNet.MEstimator? inOutlierSuppression,
                //    out AvlNet.Arc2D? outArc,
                //    IList<AvlNet.Edge1D?> outEdges,
                //    INullable<AvlNet.Profile> outDeviationProfile,
                //    IList<AvlNet.Point2D> outInliers,
                //    IList<AvlNet.Profile> diagBrightnessProfiles,
                //    IList<AvlNet.Profile> diagResponseProfiles
                //)

                AVL.FitArcToEdges(gasketImage, 
                    arcMap1, 
                    scanParams, 
                    Selection.Best, 0.1f, CircleFittingMethod.GeometricLandau,
                    out upperArc);

                AVL.FitArcToEdges(gasketImage, 
                    arcMap2, 
                    scanParams, 
                    Selection.Best, 0.1f, CircleFittingMethod.GeometricLandau,
                    out lowerArc);

                if (upperArc.HasValue && lowerArc.HasValue)
                {
                    //mr:: 这里使用了 Value 属性来取值.
                    detectedArcs = new[] { upperArc.Value, lowerArc.Value };

                    conntectingSegments[1] = new Segment2D(detectedArcs[0].Center, detectedArcs[1].Center);
                }
                else
                    detectedArcs = new Arc2D[0];
            }
        }

        /// <summary>
        /// Detects circles on gasket image
        /// </summary>
        private static void DetectCircles(Image gasketImage, out Circle2D[] detectedCircles, out Segment2D[] conntectingSegments,
            CoordinateSystem2D localSystem)
        {
            using (CircleFittingMap
                circleMap1 = new CircleFittingMap(),
                circleMap2 = new CircleFittingMap())
            {
                AVL.CreateCircleFittingMap(new ImageFormat(gasketImage),
                    new CircleFittingField(largeExpectedCircle, 35.0f),

                    localSystem, 16, 5, InterpolationMethod.Bilinear,
                    circleMap1);

                AVL.CreateCircleFittingMap(new ImageFormat(gasketImage),
                     new CircleFittingField(smallExpectedCircle, 35.0f),
                    localSystem, 16, 5, InterpolationMethod.Bilinear,
                    circleMap2);

                Circle2D? largeCircle, smallCircle;

                AVL.FitCircleToEdges(gasketImage, 
                    circleMap1, 
                    scanParams, 
                    Selection.Best, 0.1f, CircleFittingMethod.GeometricLandau,
                    out largeCircle);

                AVL.FitCircleToEdges(gasketImage, 
                    circleMap2, 
                    scanParams, 
                    Selection.Best, 0.1f, CircleFittingMethod.GeometricLandau,
                    out smallCircle);

                conntectingSegments = new Segment2D[2];

                if (largeCircle.HasValue && smallCircle.HasValue)
                {
                    detectedCircles = new[] { largeCircle.Value, smallCircle.Value };
                    conntectingSegments[0] = new Segment2D(detectedCircles[0].Center, detectedCircles[1].Center);
                }
                else
                    detectedCircles = new Circle2D[0];
            }
        }

        /// <summary>
        /// Finds gasket on image and computes its local coordinate system
        /// </summary>
        /// <param name="gasketImage">Image with gasket</param>
        /// <returns>Gasket local coordinate system</returns>
        private static CoordinateSystem2D GetGasketLocalCoordinateSystem(Image gasketImage)
        {
            var localSystem = CoordinateSystem2D.Default;

            using (var searchRegion = new Region())
            {
                AVL.CreateBoxRegion(new Box(17, 145, 126, 213), 656, 492, searchRegion);

                var gasket = AvlNet.Nullable.Create<Object2D>();

                if (edgeModel.HasValue)
                    AVL.LocateSingleObject_Edges(gasketImage, 
                        searchRegion, 
                        edgeModel.Value, 
                        0, 3, 40.0f, 
                        EdgePolarityMode.MatchStrictly, EdgeNoiseLevel.High, 
                        false, 0.8f, 
                        gasket);

                if (gasket.HasValue)
                    localSystem = gasket.Value.Alignment; //mr:: 模板匹配之后,提取匹配项的坐标系

                return localSystem;
            }
        }

        /// <summary>
        /// Has to be called before program exit to clear internal resources
        /// </summary>
        public static void ClearResources()
        {
            edgeModel.Dispose();
        }

        #endregion
    }
}
