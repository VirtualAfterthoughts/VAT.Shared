using Unity.Mathematics;

using UnityEngine;

using static Unity.Mathematics.math;

using VAT.Shared.Extensions;
using VAT.Shared.Math;

namespace VAT.Shared.Data
{
    /// <summary>
    /// Initial conditions of a ConfigurableJoint used for calculating target position and rotation.
    /// </summary>
    public class JointSpace
    {
        /// <summary>
        /// The joint.
        /// </summary>
        public ConfigurableJoint Joint { get; }

        /// <summary>
        /// The joint's rigidbody.
        /// </summary>
        public Rigidbody Rigidbody { get; }

        /// <summary>
        /// The joint's transform.
        /// </summary>
        public Transform Transform { get; }

        /// <summary>
        /// The joint's connected body.
        /// </summary>
        public Rigidbody ConnectedBody { get; }

        /// <summary>
        /// The joint's connected transform.
        /// </summary>
        public Transform ConnectedTransform { get; }

        /// <summary>
        /// Does this joint have a connected body?
        /// </summary>
        public bool HasConnectedBody { get; }

        /// <summary>
        /// Is SwapBodies enabled on this joint?
        /// </summary>
        public bool SwapBodies { get; }

        /// <summary>
        /// The default rotation of the joint space.
        /// </summary>
        public quaternion JointRotation { get; }

        /// <summary>
        /// The initial transform of the joint.
        /// </summary>
        public SimpleTransform InitialJoint { get; }

        /// <summary>
        /// The initial transform of the connected body.
        /// </summary>
        public SimpleTransform InitialConnected { get; }

        /// <summary>
        /// Caches the joint space as of this frame. Note that it is recommended to cache this as soon as the joint is created or initialized or else values may be broken.
        /// </summary>
        /// <param name="joint"></param>
        public JointSpace(ConfigurableJoint joint)
        {
            // Store the input joint
            this.Joint = joint;
            Rigidbody = joint.GetComponent<Rigidbody>();
            Transform = joint.transform;

            // Get the joint space rotation
            JointRotation = joint.GetJointRotation();

            // Cache all involved transforms
            InitialJoint = new(Transform.position, Transform.rotation);

            SwapBodies = joint.swapBodies;

            ConnectedBody = joint.connectedBody;

            HasConnectedBody = ConnectedBody != null;

            if (HasConnectedBody)
            {
                ConnectedTransform = ConnectedBody.transform;

                InitialConnected = new(ConnectedTransform.position, ConnectedTransform.rotation);
            }
            else
            {
                InitialConnected = SimpleTransform.Identity;
            }
        }

        /// <summary>
        /// Sets the target position of the joint in world space.
        /// </summary>
        /// <param name="target"></param>
        public void SetTargetPositionWorld(float3 target) => Joint.targetPosition = GetTargetPositionWorld(target);

        /// <summary>
        /// Sets the target position of the joint in local space.
        /// </summary>
        /// <param name="target"></param>
        public void SetTargetPositionLocal(float3 target) => Joint.targetPosition = GetTargetPositionLocal(target);

        /// <summary>
        /// Converts the world space target position to joint space.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public float3 GetTargetPositionWorld(float3 target)
        {
            quaternion worldRotation = inverse(JointRotation);

            if (!SwapBodies)
            {
                worldRotation = mul(worldRotation, mul(InitialJoint.Rotation, inverse(Transform.rotation)));
            }
            else if (ConnectedBody)
            {
                worldRotation = mul(worldRotation, mul(InitialConnected.Rotation, inverse(ConnectedTransform.rotation)));
            }

            float3 resultPosition = InitialJoint.Position - target;
            resultPosition -= InitialJoint.Position - (float3)Joint.GetWorldConnectedAnchor();
            resultPosition -= mul(Transform.rotation, (float3)Joint.anchor * (float3)Transform.lossyScale);

            resultPosition = mul(worldRotation, resultPosition);

            if (SwapBodies)
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
            if (HasConnectedBody) 
            { 
                target = ConnectedTransform.TransformPoint(target); 
            }

            return GetTargetPositionWorld(target);
        }

        /// <summary>
        /// Sets the target rotation of the joint in world space.
        /// </summary>
        /// <param name="target"></param>
        public void SetTargetRotationWorld(quaternion target) => Joint.targetRotation = GetTargetRotationWorld(target);

        /// <summary>
        /// Sets the target rotation of the joint in local space.
        /// </summary>
        /// <param name="target"></param>
        public void SetTargetRotationLocal(quaternion target) => Joint.targetRotation = GetTargetRotationLocal(target);

        /// <summary>
        /// Converts the world space target rotation to joint space.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public quaternion GetTargetRotationWorld(quaternion target)
        {
            quaternion result;

            if (HasConnectedBody)
            {
                BurstConfigurableJointExtensions.GetTargetRotationWorld(JointRotation, InitialJoint.Rotation, target, InitialConnected.Rotation, ConnectedTransform.rotation, out result);
            }
            else
            {
                BurstConfigurableJointExtensions.GetTargetRotationWorld(JointRotation, InitialJoint.Rotation, target, out result);
            }

            if (SwapBodies)
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
            if (ConnectedBody) 
            { 
                target = ConnectedTransform.rotation * target; 
            }

            return GetTargetRotationWorld(target);
        }

        /// <summary>
        /// Sets the target velocity of the joint in world space.
        /// </summary>
        /// <param name="target"></param>
        public void SetTargetVelocityWorld(float3 target) => Joint.targetVelocity = GetTargetVelocityWorld(target);

        /// <summary>
        /// Converts the world target velocity to joint space.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public float3 GetTargetVelocityWorld(float3 target)
        {
            if (ConnectedBody)
            {
                target -= (float3)ConnectedBody.velocity;
            }

            Vector3 from = Transform.position;
            Vector3 to = Derivatives.GetNextPosition(from, target);

            return Derivatives.GetLinearVelocity(GetTargetPositionWorld(from), GetTargetPositionWorld(to));
        }

        /// <summary>
        /// Sets the target angular velocity of the joint in world space.
        /// </summary>
        /// <param name="target"></param>
        public void SetTargetAngularVelocityWorld(float3 target) => Joint.targetAngularVelocity = GetTargetAngularVelocityWorld(target);

        /// <summary>
        /// Converts the world target angular velocity to joint space.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public float3 GetTargetAngularVelocityWorld(float3 target)
        {
            if (ConnectedBody)
            {
                target -= (float3)ConnectedBody.angularVelocity;
            }

            Quaternion from = Transform.rotation;
            Quaternion to = Derivatives.GetNextRotation(from, target);

            return Derivatives.GetAngularVelocity(GetTargetRotationWorld(from), GetTargetRotationWorld(to));
        }
    }
}
