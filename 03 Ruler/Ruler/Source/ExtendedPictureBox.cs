//
// Adaptive Vision Library .NET Example - 'Ruler' example
//
// Simple application that uses Adaptive Vision Library .NET to find the stripe on image and measure its width.
//
// Copyright (C) 2020 Adaptive Vision Sp. z o.o.
// Version: 4.12_74433
//

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ruler
{
    /// <summary>
    /// ExtendedPictureBox extends the standard PictureBox class to allow marking scanning segments on image and presenting results.
    /// </summary>
    public class ExtendedPictureBox : PictureBox
    {

        #region Private fields

        private const int CrossSize = 5;

        private Point? mouseDownPoint;
        private Point mouseCurrentPoint;

        private Segment resultSegment;
        private string resultString;

        #endregion

        #region Public events

        /// <summary>
        /// Event raised when user selected scanning segment on image.
        /// </summary>
        public event EventHandler<ScanningSegementEventArgs> ScanningSegmentChosen;

        #endregion

        #region Public interface

        /// <summary>
        /// Image shown in picture box. The Scanning segment will be selectable in context of this image.
        /// </summary>
        public new Image Image
        {
            get
            {
                return base.Image;
            }

            set
            {
                base.Image = value;
                SetMeasurementResult(null, null);
            }
        }

        /// <summary>
        /// Presents the results of scanning.
        /// </summary>
        /// <param name="segment">Found segment</param>
        /// <param name="result">Description of scanning result.</param>
        public void SetMeasurementResult(Segment segment, string result)
        {
            resultSegment = segment;
            resultString = result;
            Invalidate();
        }

        #endregion

        #region Private methods

        private static void paintCross(Graphics g, Pen pen, Point point)
        {
            g.DrawLine(pen, point.X, point.Y - CrossSize, point.X, point.Y + CrossSize);
            g.DrawLine(pen, point.X - CrossSize, point.Y, point.X + CrossSize, point.Y);
        }

        #endregion

        #region Overridden event handling

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (Image != null)
            {
                mouseDownPoint = mouseCurrentPoint = e.Location;
                //mr?? 为啥滴啊哦哟Invalidate()
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            //mr?? 这里并没有调用 base.OnMouseUp(e); 
            if (mouseDownPoint.HasValue)
            {
                if (ScanningSegmentChosen != null)
                    ScanningSegmentChosen(this, new ScanningSegementEventArgs { Segment = new Segment ( mouseDownPoint.Value, e.Location ) });
            }

            mouseDownPoint = null;

            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!mouseDownPoint.HasValue) 
                return;

            Rectangle boundingBox = Rectangle.FromLTRB(
                Math.Min(Math.Min(mouseCurrentPoint.X, e.Location.X), mouseDownPoint.Value.X) - CrossSize,
                Math.Min(Math.Min(mouseCurrentPoint.Y, e.Location.Y), mouseDownPoint.Value.Y) - CrossSize,
                Math.Max(Math.Max(mouseCurrentPoint.X, e.Location.X), mouseDownPoint.Value.X) + CrossSize + 1,
                Math.Max(Math.Max(mouseCurrentPoint.Y, e.Location.Y), mouseDownPoint.Value.Y) + CrossSize + 1);

            mouseCurrentPoint = e.Location;
            

            //mr:: 在这个小程序中, 没有体现出 Invalidate(boundingBox) 比 Invalidate()更快.
            Invalidate(boundingBox);
            //Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            //mr:: 调用父类方法
            base.OnPaint(pe);

            //mr:: 参看Graphics类型的定义.
            /*
                // Summary:
                //     Encapsulates a GDI+ drawing surface. This class cannot be inherited.
                public sealed class Graphics : MarshalByRefObject, IDeviceContext, IDisposable

            */
            var g = pe.Graphics;
            
            //mr:: 备份SmoothingMode
            var smoothingModeBackup = g.SmoothingMode; 

            try
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                if (mouseDownPoint.HasValue)
                {
                    var startPoint = mouseDownPoint.Value;

                    //mr:: mouseCurrentPoint在MouseMove事件中赋值. 
                    g.DrawLine(Pens.Orange, startPoint, mouseCurrentPoint);
                    paintCross(g, Pens.Red, startPoint);
                    paintCross(g, Pens.Red, mouseCurrentPoint);
                }

                if (resultSegment != null)
                {
                    g.DrawLine(Pens.Green, resultSegment.StartPoint, resultSegment.EndPoint);
                    paintCross(g, Pens.Blue, resultSegment.StartPoint);
                    paintCross(g, Pens.Blue, resultSegment.EndPoint);
                }

                if (resultString != null)

                    //mr:: 显示文本用Forms空间中的类TextReader. DrawText以IDeviceContext为参数
                    //  public static void DrawText(IDeviceContext dc, string text, Font font, Point pt, Color foreColor);
                    TextRenderer.DrawText(g, resultString, Font, resultSegment != null ? resultSegment.MiddlePoint : new Point(10, Height - 30), 
                        Color.Green, TextFormatFlags.Default);
            }
            finally
            {
                g.SmoothingMode = smoothingModeBackup;
            }
        }



        #endregion
        /// <summary>
        /// //mr?? 去掉这个方法也没有任何问题.
        /// </summary>
        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }


    }


    /// <summary>
    /// Class representing segment - a set of two System.Drawing.Point objects.
    /// </summary>
    public class Segment
    {
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }

        public Point MiddlePoint
        {
            get { return new Point((StartPoint.X + EndPoint.X) / 2, (StartPoint.Y + EndPoint.Y) / 2); }
        }

        public Segment(Point start, Point end)
        {
            StartPoint = start;
            EndPoint = end;
        }
        public Segment()
        {
           
        }


    }


    /// <summary>
    /// Class representing an EventArgs object for ScanningSegmentChoiceEventHandler.
    /// //mr:: 
    /// </summary>
    public class ScanningSegementEventArgs : EventArgs
    {
        public Segment Segment;
    }


}
