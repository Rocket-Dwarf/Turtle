using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turtle
{
    /// <summary>
    /// A 2D Vector Object
    /// </summary>
    internal struct Vector
    {
        internal double Magnitude { get; }
        internal float iComponent { get; }  // floats for use with PointF structures
        internal float jComponent { get; }
        internal UnitVector Direction { get; }
        internal double AngleWithiComponent { get; }
        private Angles.AngleType AngleUnit { get; }

        /// <summary>
        /// Calculates the Magnitude of the Vector given it's i and j components
        /// </summary>
        /// <param name="VectoriComponent">The i Component of the Vector</param>
        /// <param name="VectorjComponent">The j Component of the Vector</param>
        /// <returns>The Magnitude of the Vector</returns>
        private static double CalculateMagnitude(float VectoriComponent = 0, float VectorjComponent = 0)
        {
            return Math.Sqrt((Math.Pow(VectoriComponent, 2)) + (Math.Pow(VectorjComponent, 2)));
        }

        /// <summary>
        /// Calculates the Angle of the Vector with i given it's i and j components
        /// </summary>
        /// <param name="VectoriComponent">The i Component of the Vector</param>
        /// <param name="VectorjComponent">The j Component of the Vector</param>
        /// <returns>The Vector's Angle with i of the Vector</returns>
        private static double CalculateAngleWithiComponent(float VectoriComponent = 0, float VectorjComponent = 0)
        {
            return Math.Atan(VectorjComponent / VectoriComponent); // Tan x = O/A
        }

        /// <summary>
        /// Calculates the Angle of the Vector with j given it's i and j components
        /// </summary>
        /// <param name="VectoriComponent">The i Component of the Vector</param>
        /// <param name="VectorjComponent">The j Component of the Vector</param>
        /// <returns>The Vector's Angle with j of the Vector</returns>
        private static double CalculateAngleWithjComponent(float VectoriComponent = 0, float VectorjComponent = 0)
        {
            return Math.Sqrt(VectoriComponent / VectorjComponent);  // Tan x = O/A
        }

        /// <summary>
        /// The Unit Vector of the current 2D Vector Object
        /// </summary>
        internal struct UnitVector
        {
            internal double UVMagnitude { get; }
            internal float UViComponent { get; }   // floats for use with PointF structures
            internal float UVjComponent { get; }
            private bool IsUnitVector { get; }

            /// <summary>
            /// Calculates the Unit Vector of a given Vector
            /// </summary>
            /// <param name="VectorMagnitude">The Magnitude of the Vector</param>
            /// <param name="VectoriComponent">The i component of the Vector</param>
            /// <param name="VectorjComponent">The j component of the Vector</param>
            internal UnitVector(double VectorMagnitude = 0, float VectoriComponent = 0, float VectorjComponent = 0)
            {
                UVMagnitude = 1;
                // For Vector V: Unit Vector = V / |V| 
                if (VectorMagnitude != 0)
                {
                    UViComponent = VectoriComponent / (float)VectorMagnitude;
                    UVjComponent = VectorjComponent / (float)VectorMagnitude;
                }
                else
                {
                    UViComponent = 0f;
                    UVjComponent = 0f;
                }

                if (UVMagnitude == CalculateMagnitude(UViComponent, UVjComponent))
                    IsUnitVector = true;
                else
                    IsUnitVector = false;
            }
        }
        
        /// <summary>
        /// Builds a 2D Vector From it's Magnitude and the angle it makes with i
        /// </summary>
        /// <param name="KnownMagnitude">The Magnitude of the vector</param>
        /// <param name="AngleWithi">The Angle the vector makes with i</param>
        internal Vector(long KnownMagnitude = 0, double AngleWithi = 0)
        {
            Magnitude = KnownMagnitude;
            AngleUnit = Settings.AngleUnitInstance;
            iComponent = (float)(KnownMagnitude * Math.Cos(AngleUnit.ToRadians(AngleWithi)));        //cos for x or i
            jComponent = (float)(KnownMagnitude * Math.Sin(AngleUnit.ToRadians(AngleWithi)));        //sin for y or j
            Direction = new UnitVector(Magnitude, iComponent, jComponent);
            AngleWithiComponent = AngleWithi;
        }
        
        /// <summary>
        /// Builds a 2D vector From it's i and j Componets
        /// </summary>
        /// <param name="iVal">The i Value of the Vector</param>
        /// <param name="jVal">The j Value of the Vector</param>
        internal Vector(float iVal = 0, float jVal = 0)
        {
            iComponent = iVal;
            jComponent = jVal;
            AngleUnit = Settings.AngleUnitInstance;
            Magnitude = CalculateMagnitude(iComponent, jComponent);
            Direction = new UnitVector(Magnitude, iComponent, jComponent);
            AngleWithiComponent = CalculateAngleWithiComponent(iComponent, jComponent);
        }
    }

    /// <summary>
    /// A Colour Stucture represented by an RGB Value
    /// </summary>
    public struct RGB
    {
        internal byte A { get; }
        internal byte R { get; }
        internal byte G { get; }
        internal byte B { get; }


        /// <summary>
        /// Creates a RGB Colour from a string containing a RGB or ARGB Colour Value
        /// </summary>
        /// <param name="HexCode">The string containing the Colour Value </param>
        public RGB(string HexCode)
        {
            HexCode = HexCode.Replace("#", "");
            if (HexCode.Length == 8)
                HexCode = HexCode.Remove(0, 2); //Removes A val
            A = 0xFF;
            R = Convert.ToByte(HexCode.Substring(0, 2), 16);
            G = Convert.ToByte(HexCode.Substring(2, 2), 16);
            B = Convert.ToByte(HexCode.Substring(4, 2), 16);
        }

        /// <summary>
        /// Creates a RGB Colour from an (unsigned) integer value which contains a RGB or ARGB Colour Value
        /// </summary>
        /// <param name="HexCode">The numeric RGB or ARGB value</param>
        public RGB(uint HexCode)
        {
            byte[] Bytes = BitConverter.GetBytes(HexCode);              // works because uint -> UInt32 and therefore the HexCode value will always contains 4 bytes
            if (BitConverter.IsLittleEndian) Array.Reverse(Bytes);      // forces value to be right to left i.e. 0b0010 = 2 rather than 0b0100 = 2
            A = 0xFF;
            R = Bytes[1];
            G = Bytes[2];
            B = Bytes[3];
        }

        /// <summary>
        /// Creates a RGB Colour from the Given string which contain the Component Values
        /// </summary>
        /// <param name="Red">The string containing the Red Value of the Colour</param>
        /// <param name="Green">The string containing the Green Value of the Colour</param>
        /// <param name="Blue">The string containing the Blue Value of the Colour</param>
        public RGB(string Red, string Green, string Blue)
        {
            A = 0xFF;
            R = Convert.ToByte(Red, 16);
            G = Convert.ToByte(Green, 16);
            B = Convert.ToByte(Blue, 16);
        }

        /// <summary>
        /// Creates a RGB Colour from the Given Component Values
        /// </summary>
        /// <param name="Red">The Red Value of the Colour</param>
        /// <param name="Green">The Green Value of the Colour</param>
        /// <param name="Blue">The Blue Value of the Colour</param>
        public RGB(byte Red, byte Green, byte Blue)
        {
            A = 0xFF;
            R = Red;
            G = Green;
            B = Blue;
        }
    }
}
