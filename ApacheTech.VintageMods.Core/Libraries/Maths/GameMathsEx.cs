using System;
using Vintagestory.API.MathTools;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Libraries.Maths
{
    public class GameMathsEx
    {
        public static double I { get; } = GameMath.Sqrt(-1);

        public static double DistributiveProduct(double a1, double a2, double b1, double b2)
        {
            var first = a1 * b1;
            var outside = a1 * b2;
            var inside = a2 * b1;
            var last = a2 * b2;
            return first + outside + inside + last;
        }

        public static float DistributiveProduct(float a1, float a2, float b1, float b2)
        {
            var first = a1 * b1;
            var outside = a1 * b2;
            var inside = a2 * b1;
            var last = a2 * b2;
            return first + outside + inside + last;
        }

        public static float ClampWrap(float val, float min, float max)
        {
            val -= (float)Math.Round((val - min) / (max - min)) * (max - min);
            if (val < 0)
                val = val + max - min;
            return val;
        }
    }
}