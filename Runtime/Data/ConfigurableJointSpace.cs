using Unity.Mathematics;

using UnityEngine;

using static Unity.Mathematics.math;

using VAT.Shared.Extensions;

namespace VAT.Shared.Data
{
    /// <summary>
    /// Helper struct for storing initial ConfigurableJoint information. Used for setting target values.
    /// </summary>
    public class ConfigurableJointSpace
    {
        /// <summary>
        /// The joint that this ConfigurableJointSpace is based on.
        /// </summary>
        public readonly ConfigurableJoint joint;

        /// <summary>
        /// The rigidbody on this joint.
        /// </summary>
        public readonly Rigidbody rigidbody;

        /// <summary>
        /// The rigidbody that the joint is connected to.
        /// </summary>
        public readonly Rigidbody connectedBody;

        /// <summary>
        /// The default rotation of the joint space.
        /// </summary>
        public readonly quaternion jointRotation;

        /// <summary>
        /// The initial transform values of the joint.
        /// </summary>
        public readonly TransformState initialJoint;

        /// <summary>
        /// The initial transform values of the connected body.
        /// </summary>
        public readonly TransformState initialConnected;

        /// <summary>
        /// Caches the joint space as of this frame. Note that it is recommended to cache this as soon as the joint is created or initialized or else values may be broken.
        /// </summary>
        /// <param name="joint"></param>
        public ConfigurableJointSpace(ConfigurableJoint joint)
        {
            // Store the input joint
            this.joint = joint;
            rigidbody = joint.GetComponent<Rigidbody>();

            // Get the joint space rotation
            jointRotation = joint.GetJointRotation();

            // Cache all involved transforms
            initialJoint = new TransformState(joint.transform);

            connectedBody = joint.connectedBody;
            if (connectedBody)
                initialConnected = new TransformState(connectedBody.transform);
            else
                initialConnected = new TransformState();
        }

        /// <summary>
        /// Sets the target position of the joint in world space.
        /// </summary>
        /// <param name="target"></param>
        public void SetTargetPositionWorld(float3 target) => joint.targetPosition = GetTargetPositionWorld(target);

        /// <summary>
        /// Sets the target position of the joint in local space.
        /// </summary>
        /// <param name="target"></param>
        public void SetTargetPositionLocal(float3 target) => joint.targetPosition = GetTargetPositionLocal(target);

        /// <summary>
        /// Converts the world space target position to joint space.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public float3 GetTargetPositionWorld(float3 target)
        {
            quaternion worldRotation = inverse(jointRotation);

            if (!joint.swapBodies)
            {
                worldRotation = mul(worldRotation, mul(initialJoint.Rotation, inverse(rigidbody.rotation)));
            }
            else if (connectedBody)
            {
                worldRotation = mul(worldRotation, mul(initialConnected.Rotation, inverse(connectedBody.rotation)));
            }

            float3 resultPosition = initialJoint.Position - target;
            resultPosition -= initialJoint.Position - (float3)joint.GetWorldConnectedAnchor();
            resultPosition -= mul(rigidbody.rotation, (float3)joint.anchor * (float3)joint.transform.lossyScale);

            resultPosition = mul(worldRotation, resultPosition);

            if (joint.swapBodies)
            {
                resultPosition = -resultPosition;
            }

            return resultPosition;
        }

        /// <summary>
        /// Converts the local space target position to joint space.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public float3 GetTargetPositionLocal(float3 target)
        {
            if (connectedBody) target = connectedBody.transform.TransformPoint(target);
            return GetTargetPositionWorld(target);
        }

        /// <summary>
        /// Sets the target rotation of the joint in world space.
        /// </summary>
        /// <param name="target"></param>
        public void SetTargetRotationWorld(quaternion target) => joint.targetRotation = GetTargetRotationWorld(target);

        /// <summary>
        /// Sets the target rotation of the joint in local space.
        /// </summary>
        /// <param name="target"></param>
        public void SetTargetRotationLocal(quaternion target) => joint.targetRotation = GetTargetRotationLocal(target);

        /// <summary>
        /// Converts the world space target rotation to joint space.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public quaternion GetTargetRotationWorld(quaternion target)
        {
            quaternion result;

            if (joint.connectedBody && joint.connectedBody == connectedBody)
                BurstConfigurableJointExtensions.GetTargetRotationWorld(jointRotation, initialJoint.Rotation, target, initialConnected.Rotation, connectedBody.transform.rotation, out result);
            else
                BurstConfigurableJointExtensions.GetTargetRotationWorld(jointRotation, initialJoint.Rotation, target, out result);

            if (joint.swapBodies)
            {
                result = inverse(result);
            }

            return result;
        }

        /// <summary>
        /// Converts the local space target rotation to joint space.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public quaternion GetTargetRotationLocal(quaternion target)
        {
            if (connectedBody) target = connectedBody.rotation * target;
            return GetTargetRotationWorld(target);
        }

        /// <summary>
        /// Sets the target velocity of the joint in world space.
        /// </summary>
        /// <param name="target"></param>
        public void SetTargetVelocityWorld(float3 target) => joint.targetVelocity = GetTargetVelocityWorld(target);

        /// <summary>
        /// Converts the world target velocity to joint space.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public float3 GetTargetVelocityWorld(float3 target)
        {
            if (connectedBody)
                target -= (float3)connectedBody.velocity;

            Vector3 from = joint.transform.position;
            Vector3 to = DerivativeExtensions.GetNextPosition(from, target);

            return DerivativeExtensions.GetLinearVelocity(GetTargetPositionWorld(from), GetTargetPositionWorld(to));
        }

        /// <summary>
        /// Sets the target angular velocity of the joint in world space.
        /// </summary>
        /// <param name="target"></param>
        public void SetTargetAngularVelocityWorld(float3 target) => joint.targetAngularVelocity = GetTargetAngularVelocityWorld(target);

        /// <summary>
        /// Converts the world target angular velocity to joint space.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public float3 GetTargetAngularVelocityWorld(float3 target)
        {
            if (connectedBody)
                target -= (float3)connectedBody.angularVelocity;

            Quaternion from = joint.transform.rotation;
            Quaternion to = DerivativeExtensions.GetNextRotation(from, target);

            return DerivativeExtensions.GetAngularVelocity(GetTargetRotationWorld(from), GetTargetRotationWorld(to));
        }
    }

}
