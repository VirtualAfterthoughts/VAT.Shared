using UnityEngine;

using Unity.Burst;

using static Unity.Mathematics.math;

namespace VAT.Shared.Extensions
{
    using Unity.Mathematics;

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

        public static void UpdateRotation(this ConfigurableJoint joint, Transform transform, quaternion rotation)
        {
            Quaternion original = transform.rotation;

            transform.rotation = rotation;
            joint.RefreshJointSpace();
            transform.rotation = original;
        }

        public static quaternion GetJointRotation(this ConfigurableJoint joint)
        {
            quaternion initialRotation = joint.configuredInWorldSpace ? quaternion.identity : joint.transform.rotation;
            BurstConfigurableJointExtensions.GetJointRotation(initialRotation, joint.axis, joint.secondaryAxis, out var result);
            return result;
        }

        public static void SetTargetPositionAndVelocity(this ConfigurableJoint joint, Vector3 targetPosition)
        {
            // Getting the difference between the last target and the current is an easy way to set velocity without doing another conversion.
            BurstDerivatives.GetLinearVelocity(joint.targetPosition, targetPosition, Time.deltaTime, out var result);
            joint.targetVelocity = result;
            joint.targetPosition = targetPosition;
        }

        public static void SetTargetRotationAndVelocity(this ConfigurableJoint joint, Quaternion targetRotation)
        {
            // Getting the difference between the last target and the current is an easy way to set angular velocity without doing another conversion.
            BurstDerivatives.GetAngularVelocity(joint.targetRotation, targetRotation, Time.deltaTime, out var result);
            joint.targetAngularVelocity = result;
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

    [BurstCompile]
    public static partial class BurstConfigurableJointExtensions
    {
        [BurstCompile]
        public static void GetJointRotation(in quaternion initialRotation, in float3 axis, in float3 secondaryAxis, out quaternion jointRotation)
        {
            // Calculate each axis of the joint space
            float3 right = axis.ForceNormalize(math.right());
            float3 nSecondaryAxis = secondaryAxis.ForceNormalize(math.up());
            float3 forward = cross(right, nSecondaryAxis).ForceNormalize(math.forward());
            float3 up = -cross(right, forward).ForceNormalize(math.up());

            // float3x3s have a "ZXY" order
            var matrix = orthonormalize(new float3x3(forward, right, up));

            // Create the look rotation and modify it by starting rotation
            quaternion rotation = quaternion.LookRotation(matrix.c0, matrix.c2);
            jointRotation = mul(initialRotation, rotation);
        }

        [BurstCompile]
        public static void GetTargetRotationWorld(in quaternion jointRotation, in quaternion initialRotation, in quaternion targetRotation, in quaternion initialConnectedRotation, in quaternion connectedRotation, out quaternion result)
        {
            result = inverse(jointRotation);
            result = mul(result, mul(initialRotation, inverse(targetRotation)));
            result = mul(result, inverse(mul(initialConnectedRotation, inverse(connectedRotation))));
            result = mul(result, jointRotation);
        }

        [BurstCompile]
        public static void GetTargetRotationWorld(in quaternion jointRotation, in quaternion initialRotation, in quaternion targetRotation, out quaternion result)
        {
            result = inverse(jointRotation);
            result = mul(result, mul(initialRotation, inverse(targetRotation)));
            result = mul(result, jointRotation);
        }
    }
}