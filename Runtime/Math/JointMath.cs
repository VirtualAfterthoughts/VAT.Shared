using UnityEngine;

namespace VAT.Shared.Math
{
    /// <summary>
    /// Functions for calculating properties of PhysX joints.
    /// </summary>
    public static class JointMath
    {
        /// <summary>
        /// Gets the joint's space conversion rotation for calculating target positions and rotations.
        /// </summary>
        /// <param name="initialRotation">The joint's initial rotation, stored from when the joint is first created.</param>
        /// <param name="axis">The joint's axis.</param>
        /// <param name="secondaryAxis">The joint's secondary axis.</param>
        /// <returns></returns>
        public static Quaternion GetJointRotation(Quaternion initialRotation, Vector3 axis, Vector3 secondaryAxis)
        {
            Vector3 forward = Vector3.Cross(axis, secondaryAxis);
            Vector3 up = Vector3.Cross(forward, axis);

            var jointRotation = initialRotation * Quaternion.LookRotation(forward, up);

            return jointRotation;
        }

        /// <summary>
        /// Gets a joint target rotation from a world-space rotation given initial conditions.
        /// </summary>
        /// <param name="jointRotation">The joint's solving rotation, calculated from <see cref="GetJointRotation(Quaternion, Vector3, Vector3)"/> when the joint is first created.</param>
        /// <param name="initialRotation">The joint's initial rotation, stored from when the joint is first created.</param>
        /// <param name="targetRotation">The target rotation in world space.</param>
        /// <param name="initialConnectedRotation">The joint connected body's initial rotation, stored from when the joint is first created.</param>
        /// <param name="connectedRotation">The joint connected body's current rotation.</param>
        /// <returns></returns>
        public static Quaternion GetTargetRotationWorld(Quaternion jointRotation, Quaternion initialRotation, Quaternion targetRotation, Quaternion initialConnectedRotation, Quaternion connectedRotation)
        {
            var result = Quaternion.Inverse(jointRotation);
            result *= initialRotation * Quaternion.Inverse(targetRotation);
            result *= Quaternion.Inverse(initialConnectedRotation * Quaternion.Inverse(connectedRotation));
            result *= jointRotation;

            return result;
        }

        /// <summary>
        /// Gets a joint target rotation from a world-space rotation given initial conditions.
        /// </summary>
        /// <param name="jointRotation">The joint's solving rotation, calculated from <see cref="GetJointRotation(Quaternion, Vector3, Vector3)"/> when the joint is first created.</param>
        /// <param name="initialRotation">The joint's initial rotation, stored from when the joint is first created.</param>
        /// <param name="targetRotation">The target rotation in world space.</param>
        /// <returns></returns>
        public static Quaternion GetTargetRotationWorld(Quaternion jointRotation, Quaternion initialRotation, Quaternion targetRotation)
        {
            var result = Quaternion.Inverse(jointRotation);
            result *= initialRotation * Quaternion.Inverse(targetRotation);
            result *= jointRotation;

            return result;
        }
    }
}
