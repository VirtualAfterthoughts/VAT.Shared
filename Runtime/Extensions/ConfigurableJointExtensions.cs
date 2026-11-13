using UnityEngine;

using VAT.Shared.Math;
using VAT.Shared.Utilities;

namespace VAT.Shared.Extensions
{
    /// <summary>
    /// Extension functions for ConfigurableJoints.
    /// </summary>
    public static class ConfigurableJointExtensions
    {
        /// <summary>
        /// Returns a position clamped to the joint's linear limit.
        /// </summary>
        /// <param name="joint"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vector3 GetLimitedPosition(this ConfigurableJoint joint, Vector3 position)
        {
            var transform = joint.transform;
            var connectedAnchor = joint.GetWorldConnectedAnchor();
            var anchorDirection = transform.TransformPoint(joint.anchor) - transform.position;

            var posDist = position - connectedAnchor + anchorDirection;

            posDist = Vector3.ClampMagnitude(posDist, joint.linearLimit.limit) - anchorDirection;

            return connectedAnchor + posDist;
        }

        /// <summary>
        /// Refreshes the joint's solving space.
        /// <para>
        /// This will make the joint act as if its initial conditions are the current ones.
        /// For example, target rotation and angular limits will be based on the current rotation,
        /// and if auto configure connected anchors is enabled, the connected anchor will be recalculated.
        /// </para>
        /// </summary>
        /// <param name="joint"></param>
        public static void RefreshJointSpace(this ConfigurableJoint joint)
        {
            // Changing the connectedBody forces the joint to be recalculated
            var connectedBody = joint.connectedBody;

            joint.connectedBody = StaticRigidbodyReference.Instance;
            joint.connectedBody = connectedBody;
        }

        /// <summary>
        /// Rotates the joint's solving space to a given rotation.
        /// <para>
        /// Note that this will also refresh the connected anchor if auto configure connected anchors is enabled,
        /// so if you do not want this behavior make sure that it is toggled off.
        /// </para>
        /// </summary>
        /// <param name="joint"></param>
        /// <param name="rotation">The rotation that the space will solve relative to.</param>
        /// <param name="rotateConnectedBody">If true, the rotation will affect the connected body instead of the joint.</param>
        public static void RotateJointSpace(this ConfigurableJoint joint, Quaternion rotation, bool rotateConnectedBody = false)
        {
            if (rotateConnectedBody)
            {
                joint.RotateJointConnectedBodySpace(rotation);
            }
            else
            {
                joint.RotateJointBodySpace(rotation);
            }
        }

        /// <summary>
        /// Rotates the joint's solving space to a given rotation using the main joint body.
        /// <para>
        /// Note that this will also refresh the connected anchor if auto configure connected anchors is enabled,
        /// so if you do not want this behavior make sure that it is toggled off.
        /// </para>
        /// </summary>
        /// <param name="joint"></param>
        /// <param name="rotation">The rotation that the joint's body will solve relative to.</param>
        public static void RotateJointBodySpace(this ConfigurableJoint joint, Quaternion rotation) => RotateJointTransformSpace(joint, joint.transform, rotation);

        /// <summary>
        /// Rotates the joint's solving space to a given rotation using the connected body.
        /// <para>
        /// Note that this will also refresh the connected anchor if auto configure connected anchors is enabled,
        /// so if you do not want this behavior make sure that it is toggled off.
        /// </para>
        /// </summary>
        /// <param name="joint"></param>
        /// <param name="rotation">The rotation that the joint's connected body will solve relative to.</param>
        public static void RotateJointConnectedBodySpace(this ConfigurableJoint joint, Quaternion rotation) => RotateJointTransformSpace(joint, joint.connectedBody.transform, rotation);

        public static Quaternion GetJointRotation(this ConfigurableJoint joint)
        {
            Quaternion initialRotation = joint.configuredInWorldSpace ? Quaternion.identity : joint.transform.rotation;

            return JointMath.GetJointRotation(initialRotation, joint.axis, joint.secondaryAxis);
        }

        /// <summary>
        /// Sets the joint's target position, as well as the target velocity based on the previous target position with <see cref="Time.deltaTime"/>.
        /// </summary>
        /// <param name="joint"></param>
        /// <param name="targetPosition"></param>
        public static void SetTargetPositionAndVelocity(this ConfigurableJoint joint, Vector3 targetPosition) => joint.SetTargetPositionAndVelocity(targetPosition, Time.deltaTime);

        /// <summary>
        /// Sets the joint's target position, as well as the target velocity based on the previous target position with the given delta time.
        /// </summary>
        /// <param name="joint"></param>
        /// <param name="targetPosition"></param>
        /// <param name="deltaTime"></param>
        public static void SetTargetPositionAndVelocity(this ConfigurableJoint joint, Vector3 targetPosition, float deltaTime)
        {
            // Getting the difference between the last target and the current is an easy way to set velocity without doing another conversion.
            var targetVelocity = Derivatives.GetLinearVelocity(joint.targetPosition, targetPosition, deltaTime);

            joint.targetVelocity = targetVelocity;
            joint.targetPosition = targetPosition;
        }

        /// <summary>
        /// Sets the joint's target rotation, as well as the target angular velocity based on the previous target rotation with <see cref="Time.deltaTime"/>.
        /// </summary>
        /// <param name="joint"></param>
        /// <param name="targetRotation"></param>
        public static void SetTargetRotationAndVelocity(this ConfigurableJoint joint, Quaternion targetRotation) => joint.SetTargetRotationAndVelocity(targetRotation, Time.deltaTime);

        /// <summary>
        /// Sets the joint's target rotation, as well as the target angular velocity based on the previous target rotation with the given delta time.
        /// </summary>
        /// <param name="joint"></param>
        /// <param name="targetRotation"></param>
        /// <param name="deltaTime"></param>
        public static void SetTargetRotationAndVelocity(this ConfigurableJoint joint, Quaternion targetRotation, float deltaTime)
        {
            // Getting the difference between the last target and the current is an easy way to set angular velocity without doing another conversion.
            var targetAngularVelocity = Derivatives.GetAngularVelocity(joint.targetRotation, targetRotation, deltaTime);

            joint.targetAngularVelocity = targetAngularVelocity;
            joint.targetRotation = targetRotation;
        }

        /// <summary>
        /// Gets the joint's anchor in world space.
        /// </summary>
        /// <param name="joint"></param>
        /// <returns></returns>
        public static Vector3 GetWorldAnchor(this ConfigurableJoint joint) => joint.transform.TransformPoint(joint.anchor);

        /// <summary>
        /// Gets the joint's connected anchor in world space.
        /// </summary>
        /// <param name="joint"></param>
        /// <returns></returns>
        public static Vector3 GetWorldConnectedAnchor(this ConfigurableJoint joint) => joint.connectedBody ? joint.connectedBody.transform.TransformPoint(joint.connectedAnchor) : joint.connectedAnchor;

        /// <summary>
        /// Sets the joint's anchor in world space.
        /// </summary>
        /// <param name="joint"></param>
        /// <param name="anchor"></param>
        public static void SetWorldAnchor(this ConfigurableJoint joint, Vector3 anchor) => joint.anchor = joint.transform.InverseTransformPoint(anchor);

        /// <summary>
        /// Sets the joint's connected anchor in world space.
        /// </summary>
        /// <param name="joint"></param>
        /// <param name="anchor"></param>
        public static void SetWorldConnectedAnchor(this ConfigurableJoint joint, Vector3 anchor) => joint.connectedAnchor = joint.connectedBody ? joint.connectedBody.transform.InverseTransformPoint(anchor) : anchor;

        /// <summary>
        /// Sets the joint's linear and angular xyz motion to the given values.
        /// </summary>
        /// <param name="joint"></param>
        /// <param name="linearMotion"></param>
        /// <param name="angularMotion"></param>
        public static void SetMotion(this ConfigurableJoint joint, ConfigurableJointMotion linearMotion = ConfigurableJointMotion.Free, ConfigurableJointMotion angularMotion = ConfigurableJointMotion.Free)
        {
            joint.xMotion = joint.yMotion = joint.zMotion = linearMotion;
            joint.angularXMotion = joint.angularYMotion = joint.angularZMotion = angularMotion;
        }

        private static void RotateJointTransformSpace(ConfigurableJoint joint, Transform transform, Quaternion rotation)
        {
            Quaternion startRotation = transform.rotation;

            transform.rotation = rotation;

            joint.RefreshJointSpace();

            transform.rotation = startRotation;
        }
    }
}