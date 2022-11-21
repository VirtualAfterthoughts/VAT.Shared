using System;

using UnityEngine;

using static Unity.Mathematics.math;
using Unity.Burst;

namespace VAT.Shared.Extensions {
    using Unity.Mathematics;

    /// <summary>
    /// Helper struct for storing initial ConfigurableJoint slerp drive values. Allows for easy modification of each value.
    /// </summary>
    public struct JointSlerpMod {
        /// <summary>
        /// The joint that is being modified.
        /// </summary>
        public ConfigurableJoint joint { get; }

        /// <summary>
        /// The initial position spring of the slerp drive.
        /// </summary>
        public float positionSpring { get; }

        /// <summary>
        /// The initial position damper of the slerp drive.
        /// </summary>
        public float positionDamper { get; }

        /// <summary>
        /// The initial maximum force of the slerp drive.
        /// </summary>
        public float maximumForce { get; }

        public JointSlerpMod(ConfigurableJoint joint) {
            this.joint = joint;

            positionSpring = joint.slerpDrive.positionSpring;
            positionDamper = joint.slerpDrive.positionDamper;
            maximumForce = joint.slerpDrive.maximumForce;
        }

        /// <summary>
        /// Multiplies all values of the slerp drive.
        /// </summary>
        /// <param name="multiplier"></param>
        public void SetModMultiplier(float multiplier) {
            var drive = joint.slerpDrive;

            drive.positionSpring = positionSpring * multiplier;
            drive.positionDamper = positionDamper * multiplier;
            drive.maximumForce = maximumForce * multiplier;

            joint.slerpDrive = drive;
        }

        /// <summary>
        /// Multiplies each value by a certain amount.
        /// </summary>
        /// <param name="positionSpring"></param>
        /// <param name="positionDamper"></param>
        /// <param name="maximumForce"></param>
        public void SetMultipliers(float positionSpring, float positionDamper, float maximumForce) {
            var drive = joint.slerpDrive;

            drive.positionSpring = this.positionSpring * positionSpring;
            drive.positionDamper = this.positionDamper * positionDamper;
            drive.maximumForce = this.maximumForce * maximumForce;

            joint.slerpDrive = drive;
        }

        /// <summary>
        /// Lerps each value by a certain amount.
        /// </summary>
        /// <param name="positionSpring"></param>
        /// <param name="positionDamper"></param>
        /// <param name="maximumForce"></param>
        public void LerpMultipliers(float positionSpring, float positionDamper, float maximumForce, float lerp)
        {
            var drive = joint.slerpDrive;

            drive.positionSpring = Mathf.Lerp(drive.positionSpring, this.positionSpring * positionSpring, lerp);
            drive.positionDamper = Mathf.Lerp(drive.positionDamper, this.positionDamper * positionDamper, lerp);
            drive.maximumForce = Mathf.Lerp(drive.maximumForce, this.maximumForce * maximumForce, lerp);

            joint.slerpDrive = drive;
        }
    }

    /// <summary>
    /// Helper struct for setting the angular limit of all axes of a joint.
    /// </summary>
    [Serializable]
    public struct JointAngularLimits {
        [Tooltip("The lower (negative) limit of the angular X axis. A value of -180 is completely free.")]
        [Range(-180f, 180f)]
        public float lowAngularXLimit;

        [Tooltip("The upper (positive) limit of the angular X axis. A value of 180 is completely free.")]
        [Range(-180f, 180f)]
        public float highAngularXLimit;

        [Tooltip("The limit of the angular Y axis. A value of 180 is completely free.")]
        [Range(-180f, 180f)]
        public float angularYLimit;

        [Tooltip("The limit of the angular Z axis. A value of 180 is completely free.")]
        [Range(-180f, 180f)]
        public float angularZLimit;

        public JointAngularLimits(float lowAngularXLimit, float highAngularXLimit, float angularYLimit, float angularZLimit) {
            this.lowAngularXLimit = lowAngularXLimit;
            this.highAngularXLimit = highAngularXLimit;
            this.angularYLimit = angularYLimit;
            this.angularZLimit = angularZLimit;
        }

        public bool IsFree(Axis axis) {
            switch (axis) {
                case Axis.X:
                default:
                    return lowAngularXLimit < -177f && highAngularXLimit > 177f;
                case Axis.Y:
                    return angularYLimit > 177f;
                case Axis.Z:
                    return angularZLimit > 177f;
            }
        }
    }

    public static partial class ConfigurableJointExtensions {
        /// <summary>
        /// Resets the joint space to the current rotation.
        /// </summary>
        /// <param name="joint"></param>
        public static void UpdateSpace(this ConfigurableJoint joint) {
            joint.swapBodies = !joint.swapBodies;
            joint.swapBodies = !joint.swapBodies;
        }

        /// <summary>
        /// Resets the joint space to a specified rotation and transform.
        /// </summary>
        /// <param name="joint"></param>
        /// <param name="transform"></param>
        /// <param name="rotation"></param>
        public static void UpdateRotation(this ConfigurableJoint joint, Transform transform, Quaternion rotation) {
            Quaternion startRot = transform.rotation;
            transform.rotation = rotation;
            joint.UpdateSpace();
            transform.rotation = startRot;
        }

        /// <summary>
        /// Returns the joint space rotation used for target rotation solving.
        /// </summary>
        /// <param name="joint"></param>
        /// <returns></returns>
        public static quaternion GetJointRotation(this ConfigurableJoint joint) {
            // Get default transform rotation (identity if world space)
            quaternion initialRotation = joint.configuredInWorldSpace ? quaternion.identity : joint.transform.rotation;

            // Calculate each axis of the joint space
            float3 right = joint.axis.ForceNormalize(math.right());
            float3 secondaryAxis = joint.secondaryAxis.ForceNormalize(math.up());
            float3 forward = cross(right, secondaryAxis).forcenormalize(math.forward());
            float3 up = -cross(right, forward).forcenormalize(math.up());

            // float3x3s have a "ZXY" order
            var matrix = orthonormalize(new float3x3(forward, right, up));

            // Create the look rotation and modify it by starting rotation
            quaternion rotation = quaternion.LookRotation(matrix.c0, matrix.c2);
            return mul(initialRotation, rotation);
        }

        /// <summary>
        /// Sets the target position and velocity given a new value. Note this value must already be converted to joint space.
        /// </summary>
        /// <param name="joint"></param>
        /// <param name="targetPosition"></param>
        public static void SetTargetPositionAndVelocity(this ConfigurableJoint joint, Vector3 targetPosition)
        {
            // Getting the difference between the last target and the current is an easy way to set velocity without doing another conversion.
            BurstCompiled_PhysicsExtensions.BurstCompiled_GetLinearVelocity(joint.targetPosition, targetPosition, Time.deltaTime, out var result);
            joint.targetVelocity = result;
            joint.targetPosition = targetPosition;
        }

        /// <summary>
        /// Sets the target rotation and angular velocity given a new value. Note this value must already be converted to joint space.
        /// </summary>
        /// <param name="joint"></param>
        /// <param name="targetRotation"></param>
        public static void SetTargetRotationAndVelocity(this ConfigurableJoint joint, Quaternion targetRotation) {
            // Getting the difference between the last target and the current is an easy way to set angular velocity without doing another conversion.
            BurstCompiled_PhysicsExtensions.BurstCompiled_GetAngularVelocity(joint.targetRotation, targetRotation, Time.deltaTime, out var result);
            joint.targetAngularVelocity = result;
            joint.targetRotation = targetRotation;
        }

        /// <summary>
        /// Returns the anchor of the joint in world space.
        /// </summary>
        /// <param name="joint"></param>
        /// <returns></returns>
        public static Vector3 GetWorldAnchor(this ConfigurableJoint joint) => joint.transform.TransformPoint(joint.anchor);

        /// <summary>
        /// Returns the connected anchor of the joint in world space.
        /// </summary>
        /// <param name="joint"></param>
        /// <returns></returns>
        public static Vector3 GetWorldConnectedAnchor(this ConfigurableJoint joint) => joint.connectedBody ? joint.connectedBody.transform.TransformPoint(joint.connectedAnchor) : joint.connectedAnchor;

        /// <summary>
        /// Sets the anchor of the joint in world space.
        /// </summary>
        /// <param name="joint"></param>
        /// <param name="anchor"></param>
        public static void SetWorldAnchor(this ConfigurableJoint joint, Vector3 anchor) => joint.anchor = joint.transform.InverseTransformPoint(anchor);
        
        /// <summary>
        /// Sets the connected anchor of the joint in world space.
        /// </summary>
        /// <param name="joint"></param>
        /// <param name="anchor"></param>
        public static void SetWorldConnectedAnchor(this ConfigurableJoint joint, Vector3 anchor) => joint.connectedAnchor = joint.connectedBody ? joint.connectedBody.transform.InverseTransformPoint(anchor) : anchor;

        /// <summary>
        /// Sets the joint motion constraints for linear and angular axes.
        /// </summary>
        /// <param name="joint"></param>
        /// <param name="linearMotion"></param>
        /// <param name="angularMotion"></param>
        public static void SetJointMotion(this ConfigurableJoint joint, ConfigurableJointMotion linearMotion = ConfigurableJointMotion.Free, ConfigurableJointMotion angularMotion = ConfigurableJointMotion.Free) {
            joint.xMotion = joint.yMotion = joint.zMotion = linearMotion;
            joint.angularXMotion = joint.angularYMotion = joint.angularZMotion = angularMotion;
        }

        /// <summary>
        /// Sets the angular limit of each axis of a joint.
        /// </summary>
        /// <param name="joint"></param>
        /// <param name="limits"></param>
        public static void SetAngularLimits(this ConfigurableJoint joint, JointAngularLimits limits) {
            joint.lowAngularXLimit = new SoftJointLimit() { limit = limits.lowAngularXLimit };
            joint.highAngularXLimit = new SoftJointLimit() { limit = limits.highAngularXLimit };
            joint.angularYLimit = new SoftJointLimit() { limit = limits.angularYLimit };
            joint.angularZLimit = new SoftJointLimit() { limit = limits.angularZLimit };
        }
    }

    [BurstCompile]
    public static partial class BurstCompiled_ConfigurableJointExtensions {
        /// <summary>
        /// Converts the world space target rotation to joint space (BURST).
        /// </summary>
        /// <param name="jointRotation"></param>
        /// <param name="initialRotation"></param>
        /// <param name="targetRotation"></param>
        /// <param name="initialConnectedRotation"></param>
        /// <param name="connectedRotation"></param>
        /// <param name="result"></param>
        [BurstCompile]
        public static void BurstCompiled_GetTargetRotationWorld(in quaternion jointRotation, in quaternion initialRotation, in quaternion targetRotation, in quaternion initialConnectedRotation, in quaternion connectedRotation, out quaternion result) {
            result = inverse(jointRotation);
            result = mul(result, mul(initialRotation, inverse(targetRotation)));
            result = mul(result, inverse(mul(initialConnectedRotation, inverse(connectedRotation))));
            result = mul(result, jointRotation);
        }

        /// <summary>
        /// Converts the world space target rotation to joint space (BURST).
        /// </summary>
        /// <param name="jointRotation"></param>
        /// <param name="initialRotation"></param>
        /// <param name="targetRotation"></param>
        /// <param name="result"></param>
        [BurstCompile]
        public static void BurstCompiled_GetTargetRotationWorld(in quaternion jointRotation, in quaternion initialRotation, in quaternion targetRotation, out quaternion result) {
            result = inverse(jointRotation);
            result = mul(result, mul(initialRotation, inverse(targetRotation)));
            result = mul(result, jointRotation);
        }
    }
}