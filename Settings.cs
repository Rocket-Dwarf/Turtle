using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;

namespace Turtle
{
    /// <summary>
    /// The Settings of the turtle 
    /// i.e The angle unit; The Pen state; and the Fill Color.
    /// </summary>
    public class Settings
    {
        #region Angles
        //Angles
        private static Angles.AngleType CurrentAngleUnit = Angles.AcceptedAngleUnits[0];
        /// <summary>
        /// The Angular unit of the Turtle
        /// </summary>
        public static string AngleUnit
        {
            get
            {
                Angles.Intialise();
                return CurrentAngleUnit.Name;
            }

            set
            {
                Angles.Intialise();
                string UserMnemonic = value;
                if (value.Length > 3)
                    UserMnemonic = value.Remove(3).ToUpper();
                else
                    UserMnemonic = null;

                foreach (Angles.AngleType AngleUnit in Angles.AcceptedAngleUnits)
                {
                    if (AngleUnit.Mnemonic == UserMnemonic)
                    {
                        CurrentAngleUnit = AngleUnit;
                        Assembly.CommandQueueLoader(Drawing.ChangeAngleType);
                        CurrentAngleTypeList.Enqueue(CurrentAngleUnit);
                    }
                }
            }
        }

        internal static Angles.AngleType AngleUnitInstance { get { return CurrentAngleUnit; } }

        private static Queue<Angles.AngleType> CurrentAngleTypeList = new Queue<Angles.AngleType> ();
        internal static Angles.AngleType CurrentAngleType
        {
            get
            {
                return CurrentAngleTypeList.Dequeue();
            }
        }
        #endregion

        //begin_fill()
        //end_fill()

        //Pen
        #region Pen 
        // pendown()
        // penup() 
        private static Queue<Pen> PensList = new Queue<Pen> ();
        internal static Pen NewPen
        {
            get
            {
                return PensList.Dequeue();
            }
        }

        private static Color CurrentColour = Color.Black;
        private static Color FormerVisibleColour = Color.Black;
        private static bool IsVisible = true;
        private static Color InvisibleColor = Color.FromArgb(0x00FFFFFF); // 100% transpant white
        private static float CurrentWidth = 1f;
        private static Pen CurrentPen;

        /// <summary>
        /// Stops the Turtle Drawing on the form - movement is stil allowed
        /// </summary>
        public static void PenUp()
        {
            IsVisible = false;
            PenLoader();
        }

        /// <summary>
        /// Restarts the Turtle Drawing on the form
        /// </summary>
        public static void PenDown()
        {
            IsVisible = true;
            CurrentColour = FormerVisibleColour;
            PenLoader();
        }

        #region PenColor
        /// <summary>
        /// Prevents the user from changing color values directly
        /// </summary>
        private static Dictionary<string, string> PropertyInterpreter = new Dictionary<string, string>
        {
            {"A", null },
            {"R", "Red"},
            {"G", "Green"},
            {"B", "Blue"},
        };

        /// <summary>
        /// Changes the Turtle's Pen Color to the Color specifed by the string - numerical values are allowed as well 
        /// </summary>
        /// <param name="Colour">The new Pen Color of the Turtle</param>
        public static void PenColor(string Colour)
        {
            if (PropertyInterpreter.ContainsKey(Colour))
                Colour = PropertyInterpreter[Colour];
            if (Colour.StartsWith("#") || Colour.Any(char.IsNumber))
            {
                PenColor(new RGB(Colour));
                return;
            }

            CurrentColour = Color.FromName(Colour);
            PenLoader();
        }

        /// <summary>
        /// Changes the Turtle's Pen Color to the specifed RGB Color 
        /// </summary>
        /// <param name="ColourCode">The new Pen Color, specifed by a RGB Color Sturcture </param>
        public static void PenColor(RGB ColourCode)
        {
            CurrentColour = Color.FromArgb(ColourCode.A, ColourCode.R, ColourCode.G, ColourCode.B);
            PenLoader();
        }

        /// <summary>
        /// Changes the Turtle's Pen Colour to an RGB Colour given its Component Values
        /// </summary>
        /// <param name="Red">The Red Value of the Colour</param>
        /// <param name="Green">The Green Value of the Colour</param>
        /// <param name="Blue">The Blue Value of the Colour</param>
        public static void PenColor(byte Red, byte Green, byte Blue)
        {
            CurrentColour = Color.FromArgb(0xFF, Red, Green, Blue);
            PenLoader();
        }

        /// <summary>
        /// Changes the Turtle's Pen Color to a 8-bit integer returning a KnownColor 
        /// </summary>
        /// <param name="KnownColorVal">The 8-bit integer used to specify a color from KnownColor</param>
        public static void PenColor(byte KnownColorVal)
        {
            CurrentColour = Color.FromKnownColor((KnownColor)KnownColorVal);
            PenLoader();
        }

        /// <summary>
        /// Changes the Turtle's Pen Color to the vaule of an (unsigned) integer containing a RGB or ARGB Colour Value
        /// </summary>
        /// <param name="RGBColorCode">An (unsigned) integer containing a RGB or ARGB Color Value</param>
        public static void PenColor(uint RGBColorCode)
        {
            PenColor(new RGB(RGBColorCode));
        }
        #endregion

        /// <summary>
        /// Changes the width of the Pen to specifed (unsigned 8-bit) integer value
        /// </summary>
        /// <param name="Size">The width of the Pen</param>
        public static void PenWidth(byte Size = 1)
        {
            CurrentWidth = Size;
            PenLoader();
        }

        /// <summary>
        /// Changes the width of the pen to specifed floating point value
        /// </summary>
        /// <param name="Size">The width of the Pen</param>
        public static void PenWidth(float Size = 1f)
        {
            CurrentWidth = Size;
            PenLoader();
        }

        /// <summary>
        /// Loades the current Pen into the Pen Queue
        /// </summary>
        private static void PenLoader()
        {
            if (IsVisible)
            {
                CurrentPen = new Pen(CurrentColour, CurrentWidth);
            }
            else
            {
                FormerVisibleColour = CurrentColour;
                CurrentPen = new Pen(InvisibleColor, CurrentWidth);
            }
            Assembly.CommandQueueLoader(Drawing.ChangePen);
            PensList.Enqueue(CurrentPen);
        }
        #endregion

        #region FillColour 
        private static Queue<SolidBrush> BrushsList = new Queue<SolidBrush>();
        internal static SolidBrush NewBrush
        {
            get
            {
                return BrushsList.Dequeue();
            }
        }

        private static Queue<bool> AppendToGraphicsPathList = new Queue<bool>();
        internal static bool AppendCurrent
        {
            get
            {
                return AppendToGraphicsPathList.Dequeue();
            }
        }

        private static Color CurrentFillColour = Color.Black;
        private static Color FormerVisibleFillColour = Color.Black;
        private static bool IsFillVisible = true;
        private static SolidBrush CurrentBrush;

        /// <summary>
        /// Starts Filling in the encolsed region
        /// </summary>
        public static void BeginFill()
        {
            IsFillVisible = true;
            CurrentColour = FormerVisibleColour;
            BrushLoader();
        }

        /// <summary>
        /// Stops Filling in the enclosed region
        /// </summary>
        public static void EndFill()
        {
            IsFillVisible = false;
            BrushLoader();
        }

        #region BrushColor
        /// <summary>
        /// Changes the Turtle's Fill Color to the Color specifed by the string - numerical values are allowed as well 
        /// </summary>
        /// <param name="Colour">The new Fill Color of the Turtle</param>
        public static void FillColor(string Colour)
        {
            if (PropertyInterpreter.ContainsKey(Colour))
                Colour = PropertyInterpreter[Colour];
            if (Colour.StartsWith("#") || Colour.Any(char.IsNumber))
            {
                FillColor(new RGB(Colour));
                return;
            }

            CurrentFillColour = Color.FromName(Colour);
            BrushLoader();
        }

        /// <summary>
        /// Changes the Turtle's Fill Color to the specifed RGB Color 
        /// </summary>
        /// <param name="ColourCode">The new Fill Color, specifed by a RGB Color Sturcture </param>
        public static void FillColor(RGB ColourCode)
        {
            CurrentFillColour = Color.FromArgb(ColourCode.A, ColourCode.R, ColourCode.G, ColourCode.B);
            BrushLoader();
        }

        /// <summary>
        /// Changes the Turtle's Fill Colour to an RGB Colour given its Component Values
        /// </summary>
        /// <param name="Red">The Red Value of the Colour</param>
        /// <param name="Green">The Green Value of the Colour</param>
        /// <param name="Blue">The Blue Value of the Colour</param>
        public static void FillColor(byte Red, byte Green, byte Blue)
        {
            CurrentFillColour = Color.FromArgb(0xFF, Red, Green, Blue);
            BrushLoader();
        }

        /// <summary>
        /// Changes the Turtle's Fill Color to a 8-bit integer returning a KnownColor 
        /// </summary>
        /// <param name="KnownColorVal">The 8-bit integer used to specify a color from KnownColor</param>
        public static void FillColor(byte KnownColorVal)
        {
            CurrentFillColour = Color.FromKnownColor((KnownColor)KnownColorVal);
            BrushLoader();
        }

        /// <summary>
        /// Changes the Turtle's Fill Color to the vaule of an (unsigned) integer containing a RGB or ARGB Colour Value
        /// </summary>
        /// <param name="RGBColorCode">An (unsigned) integer containing a RGB or ARGB Color Value</param>
        public static void FillColor(uint RGBColorCode)
        {
            FillColor(new RGB(RGBColorCode));
        }
        #endregion

        private static void BrushLoader()
        {
            if (IsFillVisible)
            {
                CurrentBrush = new SolidBrush(CurrentFillColour);
                AppendToGraphicsPathList.Enqueue(true);
                Assembly.CommandQueueLoader(Drawing.FillRegion);
            }
            else
            {
                AppendToGraphicsPathList.Enqueue(false);
                Assembly.CommandQueueLoader(Drawing.FillRegion);
                FormerVisibleFillColour = CurrentFillColour;
                CurrentBrush = new SolidBrush(InvisibleColor);
            }
            Assembly.CommandQueueLoader(Drawing.ChangeBrush);
            BrushsList.Enqueue(CurrentBrush);
        }
        #endregion

        //Turtle Apperance

    }

    internal class Angles
    {
        private static AngleType Degrees = new AngleType("DEG", "Degrees", 180d);
        private static AngleType Radians = new AngleType("RAD", "Radians", Math.PI);
        private static AngleType Gradians = new AngleType("GRA", "Gradians", 200d);
        internal static void Intialise() { Degrees.SetConversionFactors(); Radians.SetConversionFactors(); Gradians.SetConversionFactors(); }

        private static List<AngleType> AcceptedAngleUnitsList = new List<AngleType>
        {
            Degrees,
            Radians,
            Gradians
        };

        internal static List<AngleType> AcceptedAngleUnits
        {
            get
            {
                return AcceptedAngleUnitsList;
            }
        }

        internal class AngleType
        {
            internal string Mnemonic { get; }
            internal string Name { get; }
            internal double HalfTurn { get; }
            private ConversionFactor DegreesFactor { get; set; }
            private ConversionFactor RadiansFactor { get; set; }
            private ConversionFactor GradiansFactor { get; set; }
            private bool ConversionFactorsSet = false;

            internal void SetConversionFactors()
            {
                if (!ConversionFactorsSet)
                {
                    DegreesFactor = new ConversionFactor(Degrees.HalfTurn, HalfTurn);
                    RadiansFactor = new ConversionFactor(Radians.HalfTurn, HalfTurn);
                    GradiansFactor = new ConversionFactor(Gradians.HalfTurn, HalfTurn);
                    ConversionFactorsSet = true;
                }
            }

            internal struct ConversionFactor
            {
                internal double NumeratorFactor { get; }
                internal double DenominatorFactor { get; }

                // Angle Conversions:
                // from degrees to degrees:     Degrs = Degrs * 1
                // from degrees to radians:     Rads = Degrs * (Math.PI / 180f)
                // from radians to degrees:     Degrs = Rads * (180f / Math.PI)
                // from radians to radians:     Rads = Rads * 1

                // from radians to gradians:    Grads = Rads * (200f / Math.PI)
                // from degrees to gradians:    Grads = Degrs * (200f / 180f)
                // from gradians to degrees:    Degrs = Grads * (180f / 200f)
                // from gradians to radians:    Rads = Grads * (Math.PI / 200f)
                // from gradians to gradians:   Grads = Grads * 1
                
                /// <summary>
                /// 
                /// </summary>
                /// <param name="Numerator"></param>
                /// <param name="Denominator"></param>
                internal ConversionFactor(double Numerator, double Denominator)
                {
                    NumeratorFactor = Numerator;
                    DenominatorFactor = Denominator;
                }

            }

            internal AngleType(string MnemonicName, string FullName, double HalfTurnVal)
            {
                Mnemonic = MnemonicName;
                Name = FullName;
                HalfTurn = HalfTurnVal;
            }

            internal double ToDegrees(double Angle)
            {
                return Angle * (DegreesFactor.NumeratorFactor / DegreesFactor.DenominatorFactor);
            }

            internal double ToRadians(double Angle)
            {
                return Angle * (RadiansFactor.NumeratorFactor / RadiansFactor.DenominatorFactor);
            }

            internal double ToGradians(double Angle)
            {
                return Angle * (GradiansFactor.NumeratorFactor / GradiansFactor.DenominatorFactor);
            }
        }
    }

    /// <summary>
    /// Manages the Drawing Queues
    /// </summary>
    internal static class Assembly
    {

        #region CommandQueue Variables
        private static CommandQuantityDuad CurrentCommandQuantityPair = new CommandQuantityDuad(null, 0u); // initailisation required 
        private static CommandQuantityDuad CurrentCommand;
        private static Queue<CommandQuantityDuad> MovementCommandsQueue = new Queue<CommandQuantityDuad>();

        /// <summary>
        /// Represents a Command and Quantity Pair
        /// </summary>
        internal class CommandQuantityDuad
        {
            internal Action<Graphics> Command { get; }
            internal uint Quantity { get; set; }

            internal CommandQuantityDuad(Action<Graphics> IntialCommandAsignment, uint IntialQuantityAsignment)
            {
                Command = IntialCommandAsignment;
                Quantity = IntialQuantityAsignment;
            }
        }

        internal class DrawingParameters
        {
            private static Queue<Vector> LineParametersQueue = new Queue<Vector>();
            private static Queue<GenericRegularPolygonParams> RegularPolygonParametersQueue = new Queue<GenericRegularPolygonParams>();
            private static Queue<CircleParams> CircleParametersQueue = new Queue<CircleParams>();

            internal static Vector LineParams
            {
                get
                {
                    return LineParametersQueue.Dequeue();
                }
                set
                {
                    LineParametersQueue.Enqueue(value);
                }
            }

            internal static GenericRegularPolygonParams RegularPolygonParams
            {
                get
                {
                    return RegularPolygonParametersQueue.Dequeue();
                }
                set
                {
                    RegularPolygonParametersQueue.Enqueue(value);
                }
            }

            internal static CircleParams CircleParameters
            {
                get
                {
                    return CircleParametersQueue.Dequeue();
                }
                set
                {
                    CircleParametersQueue.Enqueue(value);
                }
            }

            ///// <summary>
            ///// Parameters for a line
            ///// </summary>
            //internal struct LineParams
            //{
            //    internal long PixelAmount;
            //    internal double AngleAtTime;


            //    internal LineParams(long Pixels = 0, double Angle = 0)
            //    {
            //        PixelAmount = Pixels;
            //        AngleAtTime = Angle;

            //    }

            //}

            /// <summary>
            /// Parameters for a Regular Polygon
            /// </summary>
            internal struct GenericRegularPolygonParams
            {
                internal ulong SideAmount;
                internal ulong SideLength;
                //internal bool ;

                internal GenericRegularPolygonParams(ulong Quantity = 0, ulong Length = 0)
                {
                    SideAmount = Quantity;
                    SideLength = Length;

                }

            }

            /// <summary>
            /// Parameters for a Circle
            /// </summary>
            internal struct CircleParams
            {
                internal ulong Radius;
                internal PointF? Centre;
                internal bool AtCentre;

                internal CircleParams(ulong RadiusParam = 0, bool AtCentreParam = false, PointF? CentreParam = null)
                {
                    Radius = RadiusParam;
                    AtCentre = AtCentreParam;
                    if (!AtCentre) Centre = CentreParam;
                    else Centre = null;
                }
            }
        }
        #endregion


        /// <summary>
        /// Loads the Specified Drawing Command into the Queue
        /// </summary>
        /// <param name="CurrentlyCalledCommand">The Drawing Command (from the Drawing Class) in the form of a Paramterless Procudure to be queued</param>
        internal static void CommandQueueLoader(Action<Graphics> CurrentlyCalledCommand = null)
        {
            if (CurrentCommandQuantityPair.Command != null)
            {
                if (CurrentCommandQuantityPair.Command == CurrentlyCalledCommand && CurrentCommandQuantityPair.Quantity < uint.MaxValue)
                {
                    CurrentCommandQuantityPair.Quantity++;
                }
                else
                {
                    MovementCommandsQueue.Enqueue(CurrentCommandQuantityPair);
                    CurrentCommandQuantityPair = new CommandQuantityDuad(CurrentlyCalledCommand, 1);
                }
            }
            else
            {
                CurrentCommandQuantityPair = new CommandQuantityDuad(CurrentlyCalledCommand, 1);
                Angles.Intialise();
            }
        }


        /// <summary>
        /// Loads the Contents of CurrentCommandQuantityPair into the queue, and once loaded clears the objects contents
        /// </summary>
        internal static void LoadFinalCommand()
        {
            while (CurrentCommandQuantityPair.Command != null)
                CommandQueueLoader();

        }



        /// <summary>
        /// Dequeues the Commands in MovementCommandsQueue and Executes them
        /// </summary>
        internal static async void CommandCaller(Graphics GraphicsObj, PictureBox DrawSurface, TurtleGraphicsForm ActiveForm)
        {
            System.Windows.Forms.Timer DrawingWait = new System.Windows.Forms.Timer();
            DrawingWait.Interval = 2000;

            Drawing.CurrentAngle = 0;
            while (MovementCommandsQueue.Count != 0)
            {
                CurrentCommand = MovementCommandsQueue.Dequeue();
                for (ulong RepeatCommand = 1; RepeatCommand <= CurrentCommand.Quantity; RepeatCommand++)
                {
                    ActiveForm.UpdateTurtlePosition();
                    DrawSurface.Invalidate();
                    await Task.Delay(125);
                    CurrentCommand.Command?.Invoke(GraphicsObj);
                }
            }
            DrawSurface.Invalidate();
        }
    }
}
