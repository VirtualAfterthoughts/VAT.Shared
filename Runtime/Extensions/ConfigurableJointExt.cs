using System;

using UnityEngine;
using Unity.Burst;
using static Unity.Mathematics.math;

using VAT.Shared.Data;

namespace VAT.Shared.Extensions {
    using Unity.Mathematics;

    public enum JointDriveType {
        XDRIVE = 1 << 0,
        YDRIVE = 1 << 1,
        ZDRIVE = 1 << 2,
        ANGULARXDRIVE = 1 << 3,
        ANGULARYZDRIVE = 1 << 4,
        SLERPDRIVE = 1 << 5, 
    }

    public static partial class ConfigurableJointExtensions {
        /// <summary>
        /// Returns the joint drive from this Configurable Joint.
        /// </summary>
        /// <param name="joint">The joint.</param>
        /// <param name="type">The drive type.</param>
        /// <returns></returns>
        public static JointDrive GetJointDrive(this ConfigurableJoint joint, in JointDriveType type) {
            return type switch {
                JointDriveType.XDRIVE => joint.xDrive,
                JointDriveType.YDRIVE => joint.yDrive,
                JointDriveType.ZDRIVE => joint.zDrive,
                JointDriveType.ANGULARXDRIVE => joint.angularXDrive,
                JointDriveType.ANGULARYZDRIVE => joint.angularYZDrive,
                JointDriveType.SLERPDRIVE => joint.slerpDrive,
                _ => joint.xDrive,
            };
        }

        /// <summary>
        /// Sets the ConfigurableJoint's drive to the desired drive.
        /// </summary>
        /// <param name="joint">The joint.</param>
        /// <param name="drive">The new drive.</param>
        /// <param name="type">The drive type.</param>
        public static void SetJointDrive(this ConfigurableJoint joint, in JointDrive drive, in JointDriveType type) {
            switch (type) {
                case JointDriveType.XDRIVE:
                    joint.xDrive = drive;
                    break;
                case JointDriveType.YDRIVE:
                    joint.yDrive = drive;
                    break;
                case JointDriveType.ZDRIVE:
                    joint.zDrive = drive;
                    break;
                case JointDriveType.ANGULARXDRIVE:
                    joint.angularXDrive = drive;
                    break;
                case JointDriveType.ANGULARYZDRIVE:
                    joint.angularYZDrive = drive;
                    break;
                case JointDriveType.SLERPDRIVE:
                    joint.slerpDrive = drive;
                    break;
            }
        }

        /// <summary>
        /// Refreshes the joint so that the current rotation and (if autoConfigureConnectedAnchor is enabled) position are the defaults.
        /// </summary>
        /// <param name="joint">The joint.</param>
        public static void RefreshJointSpace(this ConfigurableJoint joint) {
            // Flipping swapBodies causes a change in the joint. Useful since unity doesn't provide this to us.
            joint.swapBodies = !joint.swapBodies;
            joint.swapBodies = !joint.swapBodies;
        }

        /// <summary>
        /// Refreshes the joint space so that the desired rotation is the initial rotation.
        /// </summary>
        /// <param name="joint">The joint.</param>
        /// <param name="transform">The transform to move.</param>
        /// <param name="rotation">The desired rotation.</param>
        public static void UpdateRotation(this ConfigurableJoint joint, Transform transform, quaternion rotation) {
            Quaternion original = transform.rotation;

            transform.rotation = rotation;
            joint.RefreshJointSpace();
            transform.rotation = original;
        }

        /// <summary>
        /// Calculates the joint space rotation as of this frame.
        /// </summary>
        /// <param name="joint">The joint.</param>
        /// <returns></returns>
        public static quaternion GetJointRotation(this ConfigurableJoint joint) {
            quaternion initialRotation = joint.configuredInWorldSpace ? quaternion.identity : joint.transform.rotation;
            BurstCompiled_ConfigurableJointExtensions.BurstCompiled_GetJointRotation(initialRotation, joint.axis, joint.secondaryAxis, out var result);
            return result;
        }

        /// <summary>
        /// Sets the target position and velocity given a new value. Note this value must already be converted to joint space.
        /// </summary>
        /// <param name="joint">The joint.</param>
        /// <param name="targetPosition">The targetPosition in joint space.</param>
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
        /// <param name="joint">The joint.</param>
        /// <param name="targetRotation">The targetRotation in joint space.</param>
        public static void SetTargetRotationAndVelocity(this ConfigurableJoint joint, Quaternion targetRotation) {
            // Getting the difference between the last target and the current is an easy way to set angular velocity without doing another conversion.
            BurstCompiled_PhysicsExtensions.BurstCompiled_GetAngularVelocity(joint.targetRotation, targetRotation, Time.deltaTime, out var result);
            joint.targetAngularVelocity = result;
            joint.targetRotation = targetRotation;
        }

        /// <summary>
        /// Gets the joint anchor in world space.
        /// </summary>
        /// <param name="joint">The joint.</param>
        /// <returns>The world space anchor.</returns>
        public static Vector3 GetWorldAnchor(this ConfigurableJoint joint) => joint.transform.TransformPoint(joint.anchor);

        /// <summary>
        /// Gets the joint connected anchor in world space.
        /// </summary>
        /// <param name="joint"></param>
        /// <returns>The world space connected anchor.</returns>
        public static Vector3 GetWorldConnectedAnchor(this ConfigurableJoint joint) => joint.connectedBody ? joint.connectedBody.transform.TransformPoint(joint.connectedAnchor) : joint.connectedAnchor;

        /// <summary>
        /// Sets the anchor of the joint in world space.
        /// </summary>
        /// <param name="joint">The joint.</param>
        /// <param name="anchor">The world space anchor.</param>
        public static void SetWorldAnchor(this ConfigurableJoint joint, Vector3 anchor) => joint.anchor = joint.transform.InverseTransformPoint(anchor);
        
        /// <summary>
        /// Sets the connected anchor of the joint in world space.
        /// </summary>
        /// <param name="joint">The joint.</param>
        /// <param name="anchor">The world space connected anchor.</param>
        public static void SetWorldConnectedAnchor(this ConfigurableJoint joint, Vector3 anchor) => joint.connectedAnchor = joint.connectedBody ? joint.connectedBody.transform.InverseTransformPoint(anchor) : anchor;

        /// <summary>
        /// Sets the joint motion constraints for linear and angular axes.
        /// </summary>
        /// <param name="joint">The joint.</param>
        /// <param name="linearMotion">The joint motion for linear axes.</param>
        /// <param name="angularMotion">The joint motion for angular axes.</param>
        public static void SetJointMotion(this ConfigurableJoint joint, ConfigurableJointMotion linearMotion = ConfigurableJointMotion.Free, ConfigurableJointMotion angularMotion = ConfigurableJointMotion.Free) {
            joint.xMotion = joint.yMotion = joint.zMotion = linearMotion;
            joint.angularXMotion = joint.angularYMotion = joint.angularZMotion = angularMotion;
        }

        /// <summary>
        /// Sets the angular limit of each axis of a joint.
        /// </summary>
        /// <param name="joint">The joint.</param>
        /// <param name="limits">The angular limits.</param>
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
        /// Calculates the joint space rotation as of this frame (BURST).
        /// </summary>
        [BurstCompile]
        public static void BurstCompiled_GetJointRotation(in quaternion initialRotation, in float3 axis, in float3 secondaryAxis, out quaternion jointRotation) {
            // Calculate each axis of the joint space
            float3 right = axis.forcenormalize(math.right());
            float3 nSecondaryAxis = secondaryAxis.forcenormalize(math.up());
            float3 forward = cross(right, nSecondaryAxis).forcenormalize(math.forward());
            float3 up = -cross(right, forward).forcenormalize(math.up());

            // float3x3s have a "ZXY" order
            var matrix = orthonormalize(new float3x3(forward, right, up));

            // Create the look rotation and modify it by starting rotation
            quaternion rotation = quaternion.LookRotation(matrix.c0, matrix.c2);
            jointRotation = mul(initialRotation, rotation);
        }

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