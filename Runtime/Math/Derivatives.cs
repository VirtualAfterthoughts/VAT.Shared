using UnityEngine;

using VAT.Shared.Extensions;

namespace VAT.Shared.Math
{
    /// <summary>
    /// Functions for getting changes in physical quantities.
    /// </summary>
    public static class Derivatives
    {
        /// <summary>
        /// Gets the linear displacement between two points.
        /// </summary>
        /// <param name="from">The start point.</param>
        /// <param name="to">The end point.</param>
        /// <returns></returns>
        public static Vector3 GetLinearDisplacement(Vector3 from, Vector3 to) => to - from;

        /// <summary>
        /// Gets the angular displacement between two rotations.
        /// </summary>
        /// <param name="from">The start rotation.</param>
        /// <param name="to">The end rotation.</param>
        /// <returns></returns>
        public static Vector3 GetAngularDisplacement(Quaternion from, Quaternion to)
        {
            // We get the displacement between the quaternions, normalize it to ensure there are no math errors
            // Finally, we check the w component to make sure it is the shortest possible rotation
            Quaternion delta = to * Quaternion.Inverse(from);
            delta = delta.Shortest();

            var angularDisplacement = GetVectorAngularDisplacement(delta);

            return angularDisplacement;
        }

        /// <summary>
        /// Gets the linear velocity between two points over the last frame.
        /// </summary>
        /// <param name="from">The start point.</param>
        /// <param name="to">The end point.</param>
        /// <returns></returns>
        public static Vector3 GetLinearVelocity(Vector3 from, Vector3 to) => GetLinearVelocity(from, to, Time.deltaTime);

        /// <summary>
        /// Gets the linear velocity between two points over a given delta time.
        /// </summary>
        /// <param name="from">The start point.</param>
        /// <param name="to">The end point.</param>
        /// <param name="deltaTime">The change in time that the translation occurred over.</param>
        /// <returns></returns>
        public static Vector3 GetLinearVelocity(Vector3 from, Vector3 to, float deltaTime) => GetLinearDisplacement(from, to) / deltaTime;

        /// <summary>
        /// Gets the angular velocity between two rotations over the last frame.
        /// </summary>
        /// <param name="from">The start rotation.</param>
        /// <param name="to">The end rotation.</param>
        /// <returns></returns>
        public static Vector3 GetAngularVelocity(Quaternion from, Quaternion to) => GetAngularVelocity(from, to, Time.deltaTime);

        /// <summary>
        /// Gets the angular velocity between two rotations over a given delta time.
        /// </summary>
        /// <param name="from">The start rotation.</param>
        /// <param name="to">The end rotation.</param>
        /// <param name="deltaTime">The change in time that the rotation occurred over.</param>
        /// <returns></returns>
        public static Vector3 GetAngularVelocity(Quaternion from, Quaternion to, float deltaTime) => GetAngularDisplacement(from, to) / deltaTime;

        /// <summary>
        /// Gets the next position given a start position, constant velocity, and the change in time.
        /// </summary>
        /// <param name="from">The start position.</param>
        /// <param name="linearVelocity">The velocity that will remain constant over the given delta time.</param>
        /// <param name="deltaTime">The change in time.</param>
        /// <returns></returns>
        public static Vector3 GetNextPosition(Vector3 from, Vector3 linearVelocity, float deltaTime) => from + (linearVelocity * deltaTime);

        /// <summary>
        /// Gets the next position for the next frame given a start position and constant velocity.
        /// </summary>
        /// <param name="from">The start position.</param>
        /// <param name="linearVelocity">The velocity that will remain constant over the next frame.</param>
        /// <returns></returns>
        public static Vector3 GetNextPosition(Vector3 from, Vector3 linearVelocity) => GetNextPosition(from, linearVelocity, Time.deltaTime);

        /// <summary>
        /// Gets the vector equivalent of an angular displacement quaternion.
        /// </summary>
        /// <param name="angularDisplacement"></param>
        /// <returns></returns>
        public static Vector3 GetVectorAngularDisplacement(Quaternion angularDisplacement)
        {
            angularDisplacement.ToAngleAxis(out var angle, out var axis);

            float magnitude = angle * Mathf.Deg2Rad;

            return axis * magnitude;
        }

        /// <summary>
        /// Gets the quaternion equivalent of an angular displacement vector.
        /// </summary>
        /// <param name="angularDisplacement"></param>
        /// <returns></returns>
        public static Quaternion GetQuaternionAngularDisplacement(Vector3 angularDisplacement)
        {
            float xMag = angularDisplacement.magnitude * Mathf.Rad2Deg;

            Vector3 x = angularDisplacement.normalized;

            return Quaternion.AngleAxis(xMag, x);
        }

        /// <summary>
        /// Gets the next rotation given a start rotation, constant angular velocity, and the change in time.
        /// </summary>
        /// <param name="from">The start rotation.</param>
        /// <param name="angularVelocity">The angular velocity that will remain constant over the given delta time.</param>
        /// <param name="deltaTime">The change in time.</param>
        /// <returns></returns>
        public static Quaternion GetNextRotation(Quaternion from, Vector3 angularVelocity, float deltaTime) => GetQuaternionAngularDisplacement(angularVelocity * deltaTime) * from;

        /// <summary>
        /// Gets the next rotation for the next frame given a start rotation and constant angular velocity.
        /// </summary>
        /// <param name="from">The start rotation.</param>
        /// <param name="angularVelocity">The angular velocity that will remain constant over the next frame.</param>
        /// <returns></returns>
        public static Quaternion GetNextRotation(Quaternion from, Vector3 angularVelocity) => GetNextRotation(from, angularVelocity, Time.deltaTime);
    }
}