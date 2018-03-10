using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;
using System.Drawing.Drawing2D;

namespace Turtle
{
    /// <summary>
    /// The Drawing Commands for the Turtle
    /// </summary>
    public class Commands
    {
        internal static TurtleGraphicsForm Graph { get; private set; }
        // Access Point For User - Public Methods

        /// <summary>
        /// Runs the turtle, with the specifed commands. 
        /// n.b. This should be at the end of any and all drawing you wish to do with this library
        /// </summary>
        public static void Run() 
        {
            //Settings;
            Assembly.LoadFinalCommand();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Idle += delegate { Console.WriteLine(Application.MessageLoop); };
            Graph = new TurtleGraphicsForm();
            Graph.BackingBMP = new Bitmap(Graph.Width, Graph.Height);
            Application.Run(Graph);
        }


        /// <summary>
        /// Moves the Turtle Forward by the set number of Pixels
        /// </summary>
        /// <param name="PixelAmount">Distance, in Pixels, you want the Turtle to travel</param>
        public static void Forward(uint PixelAmount = 0)
        {
            Assembly.CommandQueueLoader(new Action<Graphics>(Drawing.Line));
            Vector CurrentLineParams = new Vector(PixelAmount, Drawing.CurrentAngle);
            Assembly.DrawingParameters.LineParams = CurrentLineParams;
        }

        /// <summary>
        /// Moves the Turtle Backwards by the set number of Pixels
        /// </summary>
        /// <param name="PixelAmount">Distance, in Pixels, you want the Turtle to travel</param>
        public static void Backward(uint PixelAmount = 0)
        {
            Assembly.CommandQueueLoader(new Action<Graphics>(Drawing.Line));
            Vector CurrentLineParams = new Vector(-PixelAmount, Drawing.CurrentAngle);
            Assembly.DrawingParameters.LineParams = CurrentLineParams;
        }

        /// <summary>
        /// Rotates the Turtle Counter-Clockwise by the specifed Angle
        /// </summary>
        /// <param name="Angle">The angle to turn by</param>
        public static void Left(double Angle = 0)
        {
            //counterclockwise 
            Drawing.CurrentAngle += Angle;
        }

        /// <summary>
        /// Rotates the Turtle Clockwise by the specifed Angle
        /// </summary>
        /// <param name="Angle">The angle to turn by</param>
        public static void Right(double Angle = 0)
        {
            //clockwise
            Drawing.CurrentAngle -= Angle;
        }

        
        /// <summary>
        /// Draws a Regular Polygon with a Specified Side length
        /// </summary>
        /// <param name="AmountOfSides">The Number of Sides of the Polygon</param>
        /// <param name="Length">The side length of the Polygon</param>
        //<param name="TurtleIsPolyCentre">The centre of the Polygon</param>
        public static void RegularPolygon(ulong AmountOfSides = 0, ulong Length = 0)//, bool TurtleIsPolyCentre = false)
        {
            if (AmountOfSides > 1) //prevents argument out of range exception
            {
                Assembly.DrawingParameters.GenericRegularPolygonParams CurrentPolygonParams = new Assembly.DrawingParameters.GenericRegularPolygonParams(AmountOfSides, Length);
                Assembly.CommandQueueLoader(new Action<Graphics>(Drawing.GernericRegularPolygon));
                Assembly.DrawingParameters.RegularPolygonParams = CurrentPolygonParams;
            }
        }
        
        /// <summary>
        /// Draws a Circle with the specifed radius
        /// </summary>
        /// <param name="Radius">The radius of the circle</param>
        public static void Circle(ulong Radius)
        {
            Assembly.DrawingParameters.CircleParams CurrentCircleParams = new Assembly.DrawingParameters.CircleParams(Radius);
            Assembly.CommandQueueLoader(new Action<Graphics>(Drawing.DrawCircle));
            Assembly.DrawingParameters.CircleParameters = CurrentCircleParams;
        }

    }

    internal sealed class Drawing
    {
        #region Points
        internal static PointF CurrentPostion { get; private set; } = new PointF(0, 0);
        private static PointF NewPostion;
        #endregion

        #region Angles
        internal static double CurrentAngle { get; set; } = 0d;
        private static Angles.AngleType CurrentAngleType { get; set; } = Angles.AcceptedAngleUnits[0];

        internal static void ChangeAngleType(Graphics GraphicsObj = null)
        {
            CurrentAngleType = Settings.CurrentAngleType;
        }

        #endregion
        
        private static GraphicsPath LinePath = new GraphicsPath();
        private static bool AppendToLinePath { get; set; }

        private static Pen TurtlePen = new Pen(Color.Black, 1f);
        private static SolidBrush FillBrush = new SolidBrush(Color.FromArgb(0x00FFFFFF));
        
        internal static void ChangePen(Graphics GraphicsObj = null)
        {
            TurtlePen = Settings.NewPen;
        }

        internal static void ChangeBrush(Graphics GraphicsObj = null)
        {
            FillBrush = Settings.NewBrush;
        }

        internal static void FillRegion(Graphics GraphicsObj = null)
        {
            AppendToLinePath = Settings.AppendCurrent;
            if (!AppendToLinePath)
            {
                LinePath.FillMode = FillMode.Winding;
                LinePath.CloseAllFigures();
                GraphicsObj.FillPath(FillBrush, LinePath);
                GraphicsObj.DrawPath(TurtlePen, LinePath);
                LinePath.Reset();
            }
        }

        internal static void Line(Graphics CurrentGraphGraphics)
        {
            Vector LineParameters = Assembly.DrawingParameters.LineParams;
            //for (uint CurrentPixel = 0; CurrentPixel < LineParameters.Magnitude; CurrentPixel++)
            //{
            //    NewPostion = new PointF(CurrentPostion.X + LineParameters.Direction.UViComponent, CurrentPostion.Y + LineParameters.Direction.UVjComponent);
            //    CurrentGraphGraphics.DrawLine(TurtlePen, CurrentPostion, NewPostion);
            //    CurrentPostion = NewPostion;
            //}
            NewPostion = new PointF(CurrentPostion.X + LineParameters.iComponent, CurrentPostion.Y + LineParameters.jComponent);
            if (AppendToLinePath) LinePath.AddLine(CurrentPostion, NewPostion);
            CurrentGraphGraphics.DrawLine(TurtlePen, CurrentPostion, NewPostion);
            CurrentAngle = LineParameters.AngleWithiComponent;
            CurrentPostion = NewPostion;
        }

        internal static void GernericRegularPolygon(Graphics CurrentGraphGraphics)
        {
            ulong SideLength, SideQuantity;
            double ExteriorAngle, AngleOffset, Circumradius;
            var RegularPolygonParameters = Assembly.DrawingParameters.RegularPolygonParams;
            SideQuantity = RegularPolygonParameters.SideAmount;
            SideLength = RegularPolygonParameters.SideLength;
            AngleOffset = CurrentAngle;

            PointF[] PolygonPoints = new PointF[SideQuantity];
            Circumradius = (SideLength / (2d * (Math.Sin(Math.PI / SideQuantity))));
            ExteriorAngle = 360d / SideQuantity;   // deg to rad needed 2*halfTurn

            /*if (StartAtCurrentPos)*/ PolygonPoints[0] = CurrentPostion;
            for (ulong CurrentPoint = 1; CurrentPoint < SideQuantity; CurrentPoint++)
            {
                NewPostion.X = CurrentPostion.X - (float)Circumradius * (float)Math.Cos(CurrentAngleType.ToRadians((CurrentPoint * ExteriorAngle) + AngleOffset));
                NewPostion.Y = CurrentPostion.Y + (float)Circumradius * (float)Math.Sin(CurrentAngleType.ToRadians((CurrentPoint * ExteriorAngle) + AngleOffset));
                CurrentPostion = NewPostion;
                PolygonPoints[CurrentPoint] = CurrentPostion;
            }

            CurrentGraphGraphics.FillPolygon(FillBrush, PolygonPoints);
            CurrentGraphGraphics.DrawPolygon(TurtlePen, PolygonPoints);
        }

        internal static void DrawCircle(Graphics CurrentGraphGraphics)
        {
            ulong Radius, Diameter;
            double AngleOffset;
            bool CurrentlyAtCentre;
            Vector CicrumPointToCentre;
            PointF Centre;
            PointF CircumPoint; // Point on the Circumference
            PointF RectangleCorner;
            var CircleParameters = Assembly.DrawingParameters.CircleParameters;
            Radius = CircleParameters.Radius;
            CurrentlyAtCentre = CircleParameters.AtCentre;
            Diameter = 2ul * Radius;

            if (CircleParameters.Centre.HasValue)
            {
                Centre = CircleParameters.Centre.Value;
            }
            else if (!CurrentlyAtCentre)
            {
                CircumPoint = CurrentPostion;
                CicrumPointToCentre = new Vector((long)Radius, CurrentAngle);
                Centre = new PointF(CircumPoint.X - CicrumPointToCentre.iComponent, CircumPoint.Y + CicrumPointToCentre.jComponent);
            }
            else
            {
                Centre = CurrentPostion;
            }
            
            RectangleCorner = new PointF(Centre.X, Centre.Y);
            SizeF RectangleDimenions = new SizeF(Diameter, Diameter);
            RectangleF BoundingRectangle = new RectangleF(RectangleCorner, RectangleDimenions);
            CurrentGraphGraphics.FillEllipse(FillBrush, BoundingRectangle);
            CurrentGraphGraphics.DrawEllipse(TurtlePen, BoundingRectangle);
        }
    }
}
