using System;
using Vintagestory.API.MathTools;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Libraries.Maths
{
    public static class GameMathsEx
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

        public static byte[] ScaleBy(this byte[] source, Vec3f factor)
        {
            var r = (byte)GameMath.Clamp(source[0] * factor.R, 0, 255);
            var g = (byte)GameMath.Clamp(source[1] * factor.G, 0, 255);
            var b = (byte)GameMath.Clamp(source[2] * factor.B, 0, 255);
            return new[] { r, g, b };
        }

        public static byte[] ScaleBy(this byte[] source, float factor)
        {
            var r = (byte)GameMath.Clamp(source[0] * factor, 0, 255);
            var g = (byte)GameMath.Clamp(source[1] * factor, 0, 255);
            var b = (byte)GameMath.Clamp(source[2] * factor, 0, 255);
            return new[] { r, g, b };
        }

        public static byte[] Increment(this byte[] source, Vec3f factor)
        {
            var r = (byte)GameMath.Clamp(source[0] + factor.R, 0, 255);
            var g = (byte)GameMath.Clamp(source[1] + factor.G, 0, 255);
            var b = (byte)GameMath.Clamp(source[2] + factor.B, 0, 255);
            return new[] { r, g, b };
        }

        public static byte[] Increment(this byte[] source, float factor)
        {
            var r = (byte)GameMath.Clamp(source[0] + factor, 0, 255);
            var g = (byte)GameMath.Clamp(source[1] + factor, 0, 255);
            var b = (byte)GameMath.Clamp(source[2] + factor, 0, 255);
            return new[] { r, g, b };
        }

        public static Vec3f Brighten(this Vec3f source, Vec3f factor)
        {
            var r = Math.Abs(source.R);
            var g = Math.Abs(source.G);
            var b = Math.Abs(source.B);
            return new Vec3f(
                (byte)GameMath.Clamp(r * factor.R, r, 255), 
                (byte)GameMath.Clamp(g * factor.G, g, 255), 
                (byte)GameMath.Clamp(b * factor.B, b, 255));
        }

        public static Vec3f ScaleBy(this Vec3f source, Vec3f factor)
        {
            return new Vec3f(source.X * factor.X, source.Y * factor.Y, source.Z * factor.Z);
        }

        public static Vec3d ScaleBy(this Vec3d source, Vec3d factor)
        {
            return new Vec3d(source.X * factor.X, source.Y * factor.Y, source.Z * factor.Z);
        }

        public static Vec3f ScaleBy(this Vec3f source, double[] factor)
        {
            return new Vec3f(source.X * (float)factor[0], source.Y * (float)factor[1], source.Z * (float)factor[2]);
        }

        public static Vec3d ScaleBy(this Vec3d source, double[] factor)
        {
            return new Vec3d(source.X * factor[0], source.Y * factor[1], source.Z * factor[2]);
        }
    }
}