using System;

using System.Numerics;

namespace GameMath
{
    /// <summary>
    /// Provides extension methods for Matrix4x4
    /// </summary>
    public static class MatrixExtensions
    {
        /*
        /// <summary>
        /// Worlds to screen.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="vec">The vec.</param>
        /// <param name="screenSize">Size of the screen.</param>
        /// <returns></returns>
        public static Vector2 WorldToScreen(this Matrix4x4 obj, Vector3 vec, Vector2 screenSize)
        {
            return obj.WorldToScreen(vec, screenSize.X, screenSize.Y);
        }
        */
        /// <summary>
        /// Worlds to screen.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="vec">The vec.</param>
        /// <param name="sizeX">The size x.</param>
        /// <param name="sizeY">The size y.</param>
        /// <returns></returns>
        public static Vector2 WorldToScreen(this Matrix4x4 obj, Vector3 vec, float sizeX, float sizeY)
        {
            Vector2 returnVector = new Vector2(0, 0);
            float w = obj.M41 * vec.X + obj.M42 * vec.Y + obj.M43 * vec.Z + obj.M44;
            if (w >= 0.01f)
            {
                float inverseX = 1f / w;
                returnVector.X =
                    (sizeX / 2f) +
                    (0.5f * (
                    (obj.M11 * vec.X + obj.M12 * vec.Y + obj.M13 * vec.Z + obj.M14)
                    * inverseX)
                    * sizeX + 0.5f);
                returnVector.Y =
                    (sizeY / 2f) -
                    (0.5f * (
                    (obj.M21 * vec.X + obj.M22 * vec.Y + obj.M23 * vec.Z + obj.M24)
                    * inverseX)
                    * sizeY + 0.5f);
            }
            return returnVector;
        }

        /// <summary>
        /// Gets the left.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static Vector3 GetLeft(this Matrix4x4 obj)
        {
            return obj.GetRight() * (-1f);
        }

        /// <summary>
        /// Gets the right.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static Vector3 GetRight(this Matrix4x4 obj)
        {
            return new Vector3(obj.M11, obj.M21, obj.M31);
        }

        /// <summary>
        /// Gets up.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static Vector3 GetUp(this Matrix4x4 obj)
        {
            return new Vector3(obj.M12, obj.M22, obj.M33);
        }

        /// <summary>
        /// Gets down.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static Vector3 GetDown(this Matrix4x4 obj)
        {
            return obj.GetUp() * (-1f);
        }

        /// <summary>
        /// Gets the forward.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static Vector3 GetForward(this Matrix4x4 obj)
        {
            return obj.GetBackward() * (-1f);
        }

        /// <summary>
        /// Gets the backward.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static Vector3 GetBackward(this Matrix4x4 obj)
        {
            return new Vector3(obj.M13, obj.M23, obj.M33);
        }
    }
}
