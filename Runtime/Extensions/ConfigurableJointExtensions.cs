using UnityEngine;

namespace VAT.Shared.Extensions
{
    using VAT.Shared.Math;

    public static class ConfigurableJointExtensions
    {
        public static Vector3 GetLimitedPosition(this ConfigurableJoint joint, Vector3 position)
        {
            var transform = joint.transform;
            var connectedAnchor = joint.GetWorldConnectedAnchor();
            var anchorDirection = transform.TransformPoint(joint.anchor) - transform.position;

            var posDist = position - connectedAnchor + anchorDirection;

            posDist = Vector3.ClampMagnitude(posDist, joint.linearLimit.limit) - anchorDirection;

            return connectedAnchor + posDist;
        }

        public static void RefreshJointSpace(this ConfigurableJoint joint)
        {
            // Flipping swapBodies causes a change in the joint. Useful since unity doesn't provide this to us.
            joint.swapBodies = !joint.swapBodies;
            joint.swapBodies = !joint.swapBodies;
        }

        public static void RotateJointSpace(this ConfigurableJoint joint, Transform transform, Quaternion rotation)
        {
            Quaternion original = transform.rotation;

            transform.rotation = rotation;
            joint.RefreshJointSpace();
            transform.rotation = original;
        }

        public static Quaternion GetJointRotation(this ConfigurableJoint joint)
        {
            Quaternion initialRotation = joint.configuredInWorldSpace ? Quaternion.identity : joint.transform.rotation;

            return JointMath.GetJointRotation(initialRotation, joint.axis, joint.secondaryAxis);
        }

        public static void SetTargetPositionAndVelocity(this ConfigurableJoint joint, Vector3 targetPosition)
        {
            // Getting the difference between the last target and the current is an easy way to set velocity without doing another conversion.
            var targetVelocity = Derivatives.GetLinearVelocity(joint.targetPosition, targetPosition, Time.deltaTime);

            joint.targetVelocity = targetVelocity;
            joint.targetPosition = targetPosition;
        }

        public static void SetTargetRotationAndVelocity(this ConfigurableJoint joint, Quaternion targetRotation)
        {
            // Getting the difference between the last target and the current is an easy way to set angular velocity without doing another conversion.
            var targetAngularVelocity = Derivatives.GetAngularVelocity(joint.targetRotation, targetRotation, Time.deltaTime);

            joint.targetAngularVelocity = targetAngularVelocity;
            joint.targetRotation = targetRotation;
        }

        public static Vector3 GetWorldAnchor(this ConfigurableJoint joint) => joint.transform.TransformPoint(joint.anchor);

        public static Vector3 GetWorldConnectedAnchor(this ConfigurableJoint joint) => joint.connectedBody ? joint.connectedBody.transform.TransformPoint(joint.connectedAnchor) : joint.connectedAnchor;

        public static void SetWorldAnchor(this ConfigurableJoint joint, Vector3 anchor) => joint.anchor = joint.transform.InverseTransformPoint(anchor);

        public static void SetWorldConnectedAnchor(this ConfigurableJoint joint, Vector3 anchor) => joint.connectedAnchor = joint.connectedBody ? joint.connectedBody.transform.InverseTransformPoint(anchor) : anchor;

        public static void SetMotion(this ConfigurableJoint joint, ConfigurableJointMotion linearMotion = ConfigurableJointMotion.Free, ConfigurableJointMotion angularMotion = ConfigurableJointMotion.Free)
        {
            joint.xMotion = joint.yMotion = joint.zMotion = linearMotion;
            joint.angularXMotion = joint.angularYMotion = joint.angularZMotion = angularMotion;
        }
    }
}