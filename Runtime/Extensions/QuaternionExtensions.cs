using UnityEngine;

namespace VAT.Shared.Extensions
{
    /// <summary>
    /// Extension functions for Unity Quaternions.
    /// </summary>
    public static class QuaternionExtensions
    {
        /// <summary>
        /// Returns the quaternion modified so that it is guaranteed to be the short way around.
        /// </summary>
        /// <param name="quaternion"></param>
        /// <returns></returns>
        public static Quaternion Shortest(this Quaternion quaternion)
        {
            if (quaternion.w < 0f)
            {
                quaternion.x *= -1f;
                quaternion.y *= -1f;
                quaternion.z *= -1f;
                quaternion.w *= -1f;
            }

            return quaternion;
        }
    }
}
