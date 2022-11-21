using UnityEngine;

using static Unity.Mathematics.math;

using Unity.Burst;

namespace VAT.Shared.Extensions
{
    using Unity.Mathematics;

    public static partial class PhysicsExtensions {
        /// <summary>
        /// Gets the center point of all contacts.
        /// </summary>
        /// <param name="collision"></param>
        /// <returns></returns>
        public static Vector3 GetContactCenter(this Collision collision) {
            if (collision.contactCount <= 0)
                return Vector3.zero;

            var center = Vector3.zero;

            for (var i = 0; i < collision.contactCount; i++) {
                center += collision.GetContact(i).point;
            }

            return center / (float)collision.contactCount;
        }

        /// <summary>
        /// Gets the average of all separations.
        /// </summary>
        /// <param name="collision"></param>
        /// <returns></returns>
        public static float GetSeparationCenter(this Collision collision) {
            if (collision.contactCount <= 0)
                return 0f;

            var center = 0f;

            for (var i = 0; i < collision.contactCount; i++) {
                center += collision.GetContact(i).separation;
            }

            return center / (float)collision.contactCount;
        }

        /// <summary>
        /// Gets the center direction of all normals.
        /// </summary>
        /// <param name="collision"></param>
        /// <returns></returns>
        public static Vector3 GetNormalCenter(this Collision collision)
        {
            if (collision.contactCount <= 0)
                return Vector3.zero;

            var center = Vector3.zero;

            for (var i = 0; i < collision.contactCount; i++) {
                center += collision.GetContact(i).normal;
            }

            return center / (float)collision.contactCount;
        }
    }
}