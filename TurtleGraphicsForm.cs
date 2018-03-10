using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Turtle
{
    internal partial class TurtleGraphicsForm : Form
    {
        internal TurtleGraphicsForm()
        {
            InitializeComponent();
            Graph.Controls.Add(TurtleHead);
            //Validated += Graph_Validated;
            //Application.Idle += Application_Idle;
        }

        private void TurtleGraphicsForm_Load(object sender, EventArgs e)
        {
            Scale(AutoScaleFactor);
            //BackingBMP = new Bitmap(Width, Height);
            //GraphGraphics = Graphics.FromImage(BackingBMP);
            
            //    GraphGraphics.ScaleTransform(1f, -1f); // flips the Y-Axis
            //    GraphGraphics.TranslateTransform(Width / 2f, -Height / 2f);  // sets the orgin to the middle of the form
            //    if (GraphRendered)
            //    {
            //        if (!GraphGraphicsRendered)
            //        {
            //            Assembly.CommandCaller(GraphGraphics);
            //        }
            //    }
            //    GraphGraphicsRendered = true;
            
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            if (!GraphGraphicsRendered)
                Graph.Refresh();
        }

        private static Graphics GraphGraphics;
        //internal static PointF CurrentGraphPostion;
        //internal static PointF NewGraphPostion;

        private static bool GraphRendered = true;
        private static bool GraphGraphicsRendered = false;

        private void Graph_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);
            if (BackingBMP != null)
            {
                GraphGraphics = e.Graphics;
                GraphGraphics.DrawImage(BackingBMP, 0, 0, Width, Height);
            }
        }

        private void Graph_SizeChanged(object sender, EventArgs e)
        {
            Graph.Refresh();
        }

        internal Bitmap BackingBMP { get; set; }
        

        private void TurtleGraphicsForm_Shown(object sender, EventArgs e)
        {
            BackingBMP = new Bitmap(Width, Height);
            GraphGraphics = Graphics.FromImage(BackingBMP);
            //using (GraphGraphics = Graphics.FromImage(BackingBMP))
            //{
            GraphGraphics.ScaleTransform(1f, -1f); // flips the Y-Axis
            GraphGraphics.TranslateTransform(Width / 2f, -Height / 2f);  // sets the orgin to the middle of the form
            if (GraphRendered)
            {
                if (!GraphGraphicsRendered)
                {
                    Assembly.CommandCaller(GraphGraphics, Graph, this);
                    
                }
            }
            GraphGraphicsRendered = true;
            //}
        }

        /// <summary>
        /// Update the location and roatation of the Turtle
        /// </summary>
        internal void UpdateTurtlePosition()
        {
            //Graphics TransformationGraphics = this.CreateGraphics();
            //TransformationGraphics.ScaleTransform(1f, -1f); // flips the Y-Axis
            //TransformationGraphics.TranslateTransform(Width / 2f, -Height / 2f);  // sets the orgin to the middle of the form
            var TurtleHeadImg = Properties.Resources.TurtleHeadIMG;
            PointF Offset = new PointF(Width / 2f, Height / 2f);
            TurtleHead.Left = (int)(Offset.X + Drawing.CurrentPostion.X) - 8;
            TurtleHead.Top = (int)(Offset.Y - Drawing.CurrentPostion.Y) - 8;
            TurtleHeadImg = RotateImage(TurtleHeadImg, (float)Settings.AngleUnitInstance.ToDegrees(Drawing.CurrentAngle));
            TurtleHead.Image = TurtleHeadImg;
        }

        /// <summary>
        /// Rotates the specifed Bitmap Image by the specifed angle
        /// </summary>
        /// <param name="IntialBitmap">The Bitmap Image to rotate</param>
        /// <param name="AngleOfRotation">The angle (degrees) to rotate the Image by</param>
        /// <returns>The Rotated Bitmap Image</returns>
        private static Bitmap RotateImage(Bitmap IntialBitmap, float AngleOfRotation)
        {
            Matrix RotationMatrix = new Matrix();
            RotationMatrix.Translate(IntialBitmap.Width / -2, IntialBitmap.Height / -2, MatrixOrder.Append);
            RotationMatrix.RotateAt(AngleOfRotation, new Point(0, 0), MatrixOrder.Append);
            using (GraphicsPath RoatationGraphicsPath = new GraphicsPath())
            {
                // transform image points by rotation matrix
                PointF[] ImagePolygonPoints = new PointF[] 
                {
                    new PointF(0, 0),
                    new PointF(IntialBitmap.Width, 0),
                    new PointF(0, IntialBitmap.Height)
                };

                RoatationGraphicsPath.AddPolygon(ImagePolygonPoints);
                RoatationGraphicsPath.Transform(RotationMatrix);
                PointF[] CurrentPathPoints = RoatationGraphicsPath.PathPoints;

                // create destination bitmap sized to contain rotated source image
                Rectangle BoundingRectangle = FetchBoundingRectangle(IntialBitmap, RotationMatrix);
                Bitmap RotatedBitmap = new Bitmap(BoundingRectangle.Width, BoundingRectangle.Height);

                using (Graphics RoatationGraphics = Graphics.FromImage(RotatedBitmap))
                {  
                    // draw source into dest
                    Matrix TransformationMatrix = new Matrix();
                    TransformationMatrix.Translate(RotatedBitmap.Width / 2, RotatedBitmap.Height / 2, MatrixOrder.Append);
                    RoatationGraphics.Transform = TransformationMatrix;
                    RoatationGraphics.DrawImage(IntialBitmap, CurrentPathPoints);
                    return RotatedBitmap;
                }
            }
        }

        
        /// <summary>
        /// Finds the Bounding Rectangle for a given Bitmap Image with the corresponding Matrix for Roatation
        /// </summary>
        /// <param name="IntialBitmap">The Bitmap Image to find the Bounding Rectangle of</param>
        /// <param name="RotationMatrix">The Roatation Matrix of the Bitmap Image</param>
        /// <returns>The Bounding Image</returns>
        private static Rectangle FetchBoundingRectangle(Image IntialBitmap, Matrix RotationMatrix)
        {
            GraphicsUnit BaseGraphicsUnit = new GraphicsUnit();
            Rectangle RectangleImage = Rectangle.Round(IntialBitmap.GetBounds(ref BaseGraphicsUnit));

            // Transform the four points of the image, to get the resized bounding box.
            PointF TopLeftCorner = new PointF(RectangleImage.Left, RectangleImage.Top);
            PointF TopRightCorner = new PointF(RectangleImage.Right, RectangleImage.Top);
            PointF BottomLeftCorner = new PointF(RectangleImage.Left, RectangleImage.Bottom);
            PointF BottomRightCorner = new PointF(RectangleImage.Right, RectangleImage.Bottom);

            PointF[] RectangleCorners = new PointF[] 
            {
                TopLeftCorner,
                TopRightCorner,
                BottomRightCorner,
                BottomLeftCorner
            };

            byte[] PathTypes = new byte[]
            {
                (byte)PathPointType.Start,
                (byte)PathPointType.Line,
                (byte)PathPointType.Line,
                (byte)PathPointType.Line
            };

            GraphicsPath BoundingGraphicsPath = new GraphicsPath(RectangleCorners, PathTypes);
            BoundingGraphicsPath.Transform(RotationMatrix);
            return Rectangle.Round(BoundingGraphicsPath.GetBounds());
        }
    }
}
