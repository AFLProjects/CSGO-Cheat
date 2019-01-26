using System;

using System.Numerics;

namespace GameMath
{
    /// <summary>
    /// Provides extension methods for Vector3
    /// </summary>
    public static class VectorExtensions
    {
        /// <summary>
        /// Adds the specified vec.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="vec">The vec.</param>
        public static void Add(this Vector3 obj, Vector3 vec)
        {
            obj.X += vec.X;
            obj.Y += vec.Y;
            obj.Z += vec.Z;
        }

        /// <summary>
        /// Adds the specified vec.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="vec">The vec.</param>
        public static void Add(this Vector3 obj, float vec)
        {
            obj.X += vec;
            obj.Y += vec;
            obj.Z += vec;
        }

        /// <summary>
        /// Adds the specified vec.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="vec">The vec.</param>
        public static void Add(this Vector3 obj, int vec)
        {
            obj.X += vec;
            obj.Y += vec;
            obj.Z += vec;
        }

        /// <summary>
        /// Subtracts the specified vec.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="vec">The vec.</param>
        public static void Subtract(this Vector3 obj, Vector3 vec)
        {
            obj.X -= vec.X;
            obj.Y -= vec.Y;
            obj.Z -= vec.Z;
        }

        /// <summary>
        /// Subtracts the specified vec.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="vec">The vec.</param>
        public static void Subtract(this Vector3 obj, float vec)
        {
            obj.X -= vec;
            obj.Y -= vec;
            obj.Z -= vec;
        }

        /// <summary>
        /// Subtracts the specified vec.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="vec">The vec.</param>
        public static void Subtract(this Vector3 obj, int vec)
        {
            obj.X -= vec;
            obj.Y -= vec;
            obj.Z -= vec;
        }

        /// <summary>
        /// Multiplies the specified vec.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="vec">The vec.</param>
        public static void Multiply(this Vector3 obj, Vector3 vec)
        {
            obj.X *= vec.X;
            obj.Y *= vec.Y;
            obj.Z *= vec.Z;
        }

        /// <summary>
        /// Multiplies the specified vec.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="vec">The vec.</param>
        public static void Multiply(this Vector3 obj, float vec)
        {
            obj.X *= vec;
            obj.Y *= vec;
            obj.Z *= vec;
        }

        /// <summary>
        /// Multiplies the specified vec.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="vec">The vec.</param>
        public static void Multiply(this Vector3 obj, int vec)
        {
            obj.X *= vec;
            obj.Y *= vec;
            obj.Z *= vec;
        }

        /// <summary>
        /// Divides the specified vec.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="vec">The vec.</param>
        public static void Divide(this Vector3 obj, Vector3 vec)
        {
            if (vec.X == 0.0f) vec.X = float.Epsilon;
            if (vec.Y == 0.0f) vec.Y = float.Epsilon;
            if (vec.Z == 0.0f) vec.Z = float.Epsilon;

            obj.X /= vec.X;
            obj.Y /= vec.Y;
            obj.Z /= vec.Z;
        }

        /// <summary>
        /// Divides the specified vec.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="vec">The vec.</param>
        public static void Divide(this Vector3 obj, float vec)
        {
            if (vec == 0.0f) vec = float.Epsilon;

            obj.X /= vec;
            obj.Y /= vec;
            obj.Z /= vec;
        }

        /// <summary>
        /// Divides the specified vec.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="vec">The vec.</param>
        public static void Divide(this Vector3 obj, int vec)
        {
            float tmp = (float)vec;
            if (tmp == 0.0f) tmp = float.Epsilon;

            obj.X /= tmp;
            obj.Y /= tmp;
            obj.Z /= tmp;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public static Vector3 Clone(this Vector3 obj)
        {
            return new Vector3(obj.X, obj.Y, obj.Z);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public static void Clear(this Vector3 obj)
        {
            obj.X = 0.0f;
            obj.Y = 0.0f;
            obj.Z = 0.0f;
        }

        /// <summary>
        /// Distances to.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="vec">The vec.</param>
        /// <returns></returns>
        public static float DistanceTo(this Vector3 obj, Vector3 vec)
        {
            return (obj - vec).Length();
        }

        /// <summary>
        /// Dots the product.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static float DotProduct(this Vector3 obj, Vector3 right)
        {
            return obj.X * right.X + obj.Y * right.Y + obj.Z * right.Z;
        }

        /// <summary>
        /// Crosses the product.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static Vector3 CrossProduct(this Vector3 obj, Vector3 right)
        {
            return new Vector3
            {
                X = obj.Y * right.Z - obj.Z * right.Y,
                Y = obj.Z * right.X - obj.X * right.Z,
                Z = obj.X * right.Y - obj.Y * right.X
            };
        }

        /// <summary>
        /// Lerps the specified right.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="right">The right.</param>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public static Vector3 Lerp(this Vector3 obj, Vector3 right, float amount)
        {
            return new Vector3(obj.X + (right.X - obj.X) * amount, obj.Y + (right.Y - obj.Y) * amount, obj.Z + (right.Z - obj.Z) * amount);
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <returns></returns>
        public static byte[] GetBytes(this Vector3 obj)
        {
            byte[] bytes = new byte[12];
            Buffer.BlockCopy(BitConverter.GetBytes(obj.X), 0, bytes, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(obj.Y), 0, bytes, 4, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(obj.Z), 0, bytes, 8, 4);
            return bytes;
        }

        /// <summary>
        /// Determines whether this instance is empty.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEmpty(this Vector3 obj)
        {
            return obj.X == 0.0f && obj.Y == 0.0f && obj.Z == 0.0f;
        }

        /// <summary>
        /// Reals the is empty.
        /// </summary>
        /// <returns></returns>
        public static bool RealIsEmpty(this Vector3 obj)
        {
            return (obj.X < float.Epsilon && obj.X > (-float.Epsilon))
                && (obj.Y < float.Epsilon && obj.Y > (-float.Epsilon))
                && (obj.Z < float.Epsilon && obj.Z > (-float.Epsilon));
        }

        /// <summary>
        /// Determines whether [is na n].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is na n]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNaN(this Vector3 obj)
        {
            return float.IsNaN(obj.X) || float.IsNaN(obj.Y) || float.IsNaN(obj.Z);
        }

        /// <summary>
        /// Determines whether this instance is infinity.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is infinity; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInfinity(this Vector3 obj)
        {
            return float.IsInfinity(obj.X) || float.IsInfinity(obj.Y) || float.IsInfinity(obj.Z);
        }

        /// <summary>
        /// Returns true if ... is valid.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValid(this Vector3 obj)
        {
            if (obj.IsNaN()) return false;
            if (obj.IsInfinity()) return false;

            return true;
        }

        /// <summary>
        /// Angles the clamp.
        /// </summary>
        /// <returns></returns>
        public static bool AngleClamp(this Vector3 obj)
        {
            if (!obj.IsValid()) return false;

            obj.X = MathF.Clamp(obj.X, -89.0f, 89.0f);

            obj.Z = 0.0f;

            return obj.IsValid();
        }

        /// <summary>
        /// Angles the clamp.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns></returns>
        public static bool AngleClamp(this Vector3 obj, float min, float max)
        {
            if (!obj.IsValid()) return false;

            obj.X = MathF.Clamp(obj.X, min, max);

            obj.Z = 0.0f;

            return obj.IsValid();
        }

        /// <summary>
        /// Angles the normalize.
        /// </summary>
        /// <returns></returns>
        public static bool AngleNormalize(this Vector3 obj)
        {
            if (!obj.IsValid()) return false;

            obj.Y = MathF.Normalize(obj.Y, -180.0f, 180.0f, 360.0f);

            obj.Z = 0.0f;

            return obj.IsValid();
        }

        /// <summary>
        /// Angles the normalize.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="norm">The norm.</param>
        /// <returns></returns>
        public static bool AngleNormalize(this Vector3 obj, float min, float max, float norm)
        {
            if (!obj.IsValid()) return false;

            obj.Y = MathF.Normalize(obj.Y, min, max, norm);

            obj.Z = 0.0f;

            return obj.IsValid();
        }

        /// <summary>
        /// Angles the clamp and normalize.
        /// </summary>
        /// <returns></returns>
        public static bool AngleClampAndNormalize(this Vector3 obj)
        {
            if (!obj.IsValid()) return false;

            obj.X = MathF.Clamp(obj.X, -89.0f, 89.0f);
            obj.Y = MathF.Normalize(obj.Y, -180.0f, 180.0f, 360.0f);
            obj.Z = 0.0f;

            return obj.IsValid();
        }

        /// <summary>
        /// Vectors the normalize.
        /// </summary>
        /// <returns></returns>
        public static bool VectorNormalize(this Vector3 obj)
        {
            if (!obj.IsValid()) return false;

            float length = obj.Length();

            if (length == 0) return true;

            obj.X /= length;
            obj.Y /= length;
            obj.Z /= length;

            return obj.IsValid();
        }
    }
}
