using UnityEngine;

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
        public Quaternion JointRotation { get; }

        /// <summary>
        /// The inverse of the joint rotation.
        /// </summary>
        public Quaternion InverseJointRotation { get; }

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
            InverseJointRotation = Quaternion.Inverse(JointRotation);

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
        public void SetTargetPositionWorld(Vector3 target) => Joint.targetPosition = GetTargetPositionWorld(target);

        /// <summary>
        /// Sets the target position of the joint in local space.
        /// </summary>
        /// <param name="target"></param>
        public void SetTargetPositionLocal(Vector3 target) => Joint.targetPosition = GetTargetPositionLocal(target);

        /// <summary>
        /// Converts the world space target position to joint space.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public Vector3 GetTargetPositionWorld(Vector3 target)
        {
            Quaternion worldRotation = InverseJointRotation;

            if (!SwapBodies)
            {
                worldRotation *= InitialJoint.Rotation * Quaternion.Inverse(Transform.rotation);
            }
            else if (ConnectedBody)
            {
                worldRotation *= InitialConnected.Rotation * Quaternion.Inverse(ConnectedTransform.rotation);
            }

            Vector3 resultPosition = InitialJoint.Position - target;
            resultPosition -= InitialJoint.Position - Joint.GetWorldConnectedAnchor();
            resultPosition -= Transform.rotation * Vector3.Scale(Joint.anchor, Transform.lossyScale);

            resultPosition = worldRotation * resultPosition;

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
        public Vector3 GetTargetPositionLocal(Vector3 target)
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
        public void SetTargetRotationWorld(Quaternion target) => Joint.targetRotation = GetTargetRotationWorld(target);

        /// <summary>
        /// Sets the target rotation of the joint in local space.
        /// </summary>
        /// <param name="target"></param>
        public void SetTargetRotationLocal(Quaternion target) => Joint.targetRotation = GetTargetRotationLocal(target);

        /// <summary>
        /// Converts the world space target rotation to joint space.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public Quaternion GetTargetRotationWorld(Quaternion target)
        {
            Quaternion result;

            if (HasConnectedBody)
            {
                result = JointMath.GetTargetRotationWorld(JointRotation, InitialJoint.Rotation, target, InitialConnected.Rotation, ConnectedTransform.rotation);
            }
            else
            {
                result = JointMath.GetTargetRotationWorld(JointRotation, InitialJoint.Rotation, target);
            }

            if (SwapBodies)
            {
                result = Quaternion.Inverse(result);
            }

            return result;
        }

        /// <summary>
        /// Converts the local space target rotation to joint space.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public Quaternion GetTargetRotationLocal(Quaternion target)
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
        public void SetTargetVelocityWorld(Vector3 target) => Joint.targetVelocity = GetTargetVelocityWorld(target);

        /// <summary>
        /// Converts the world target velocity to joint space.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public Vector3 GetTargetVelocityWorld(Vector3 target)
        {
            if (ConnectedBody)
            {
                target -= ConnectedBody.velocity;
            }

            Vector3 from = Transform.position;
            Vector3 to = Derivatives.GetNextPosition(from, target);

            return Derivatives.GetLinearVelocity(GetTargetPositionWorld(from), GetTargetPositionWorld(to));
        }

        /// <summary>
        /// Sets the target angular velocity of the joint in world space.
        /// </summary>
        /// <param name="target"></param>
        public void SetTargetAngularVelocityWorld(Vector3 target) => Joint.targetAngularVelocity = GetTargetAngularVelocityWorld(target);

        /// <summary>
        /// Converts the world target angular velocity to joint space.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public Vector3 GetTargetAngularVelocityWorld(Vector3 target)
        {
            if (ConnectedBody)
            {
                target -= ConnectedBody.angularVelocity;
            }

            Quaternion from = Transform.rotation;
            Quaternion to = Derivatives.GetNextRotation(from, target);

            return Derivatives.GetAngularVelocity(GetTargetRotationWorld(from), GetTargetRotationWorld(to));
        }
    }
}
