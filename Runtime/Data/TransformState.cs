using UnityEngine;

using VAT.Shared.Extensions;

namespace VAT.Shared.Data
{
    using Unity.Mathematics;

    /// <summary>
    /// A snapshot state of a transform, allowing for the parent and positions to be reset at any time.
    /// </summary>
    public class TransformState {
        /// <summary>
        /// The world space position of this state.
        /// </summary>
        public readonly float3 position;
        
        /// <summary>
        /// The world space rotation of this state.
        /// </summary>
        public readonly quaternion rotation;

        /// <summary>
        /// The world space scale of this state.
        /// </summary>
        public readonly float3 lossyScale;

        /// <summary>
        /// The local space position of this state.
        /// </summary>
        public readonly float3 localPosition;

        /// <summary>
        /// The local space rotation of this state.
        /// </summary>
        public readonly quaternion localRotation;

        /// <summary>
        /// The local space scale of this state.
        /// </summary>
        public readonly float3 localScale;

        /// <summary>
        /// The transform this was made from.
        /// </summary>
        public readonly Transform transform;

        /// <summary>
        /// The parent of the transform.
        /// </summary>
        public readonly Transform parent;

        /// <summary>
        /// Does this state have an assigned transform?
        /// </summary>
        public readonly bool HasTransform;

        /// <summary>
        /// Does this state have an assigned parent?
        /// </summary>
        public readonly bool HasParent;

        /// <summary>
        /// Creates a transform state with default values.
        /// </summary>
        public TransformState() {
            position = float3.zero;
            rotation = quaternion.identity;
            lossyScale = new float3(1f);

            localPosition = float3.zero;
            localRotation = quaternion.identity;
            localScale = new float3(1f);

            transform = null;
            parent = null;

            HasTransform = false;
            HasParent = false;
        }

        /// <summary>
        /// Creates a transform state from this transform.
        /// </summary>
        /// <param name="transform">The transform to snapshot.</param>
        public TransformState(Transform transform)
        {
            position = transform.position;
            rotation = transform.rotation;
            lossyScale = transform.lossyScale;

            localPosition = transform.localPosition;
            localRotation = transform.localRotation;
            localScale = transform.localScale;

            this.transform = transform;
            parent = transform.parent;

            HasTransform = true;
            HasParent = parent != null;
        }

        /// <summary>
        /// Creates a transform state given a transform and a parent.
        /// </summary>
        /// <param name="transform">The transform to snapshot.</param>
        /// <param name="parent">The transform's parent.</param>
        public TransformState(Transform transform, Transform parent)
        {
            position = transform.position;
            rotation = transform.rotation;
            lossyScale = transform.lossyScale;

            localPosition = parent.InverseTransformPoint(transform.position);
            localRotation = parent.InverseTransformRotation(transform.rotation);
            localScale = parent.InverseTransformVector(transform.lossyScale);

            this.transform = transform;
            this.parent = parent;

            HasTransform = true;
            HasParent = parent != null;
        }

        /// <summary>
        /// Creates a transform state given positions and a parent.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="parent">The parent of the state.</param>
        public TransformState(float3 position, quaternion rotation, Transform parent)
        {
            this.position = position;
            this.rotation = rotation;
            lossyScale = new float3(1f);

            localPosition = parent.InverseTransformPoint(position);
            localRotation = parent.InverseTransformRotation(rotation);
            localScale = parent.InverseTransformVector(new float3(1f));

            transform = null;
            this.parent = parent;

            HasTransform = false;
            HasParent = parent != null;
        }

        /// <summary>
        /// Creates a transform state given positions.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="rotation">The rotation.</param>
        public TransformState(float3 position, quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
            lossyScale = new float3(1f);

            localPosition = position;
            localRotation = rotation;
            localScale = new float3(1f);

            transform = null;
            parent = null;

            HasTransform = false;
            HasParent = false;
        }

        /// <summary>
        /// Transforms position from local space to world space.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public float3 TransformPoint(float3 position)
        {
            BurstCompiled_Transform.BurstCompiled_TransformPoint(position, this.position, rotation, lossyScale, out var result);
            return result;
        }

        /// <summary>
        /// Transforms direction from local space to world space.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public float3 TransformDirection(float3 direction)
        {
            BurstCompiled_Transform.BurstCompiled_TransformDirection(direction, rotation, out var result);
            return result;
        }

        /// <summary>
        /// Transforms vector from local space to world space.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public float3 TransformVector(float3 vector)
        {
            BurstCompiled_Transform.BurstCompiled_TransformVector(vector, rotation, lossyScale, out var result);
            return result;
        }

        /// <summary>
        /// Transforms rotation from local space to world space.
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public quaternion TransformRotation(quaternion rotation)
        {
            BurstCompiled_Transform.BurstCompiled_TransformRotation(rotation, this.rotation, out var result);
            return result;
        }

        /// <summary>
        /// Transforms position from world space to local space.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public float3 InverseTransformPoint(float3 position)
        {
            BurstCompiled_Transform.BurstCompiled_InverseTransformPoint(position, this.position, rotation, lossyScale, out var result);
            return result;
        }

        /// <summary>
        /// Transforms direction from world space to local space.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public float3 InverseTransformDirection(float3 direction)
        {
            BurstCompiled_Transform.BurstCompiled_InverseTransformDirection(direction, rotation, out var result);
            return result;
        }

        /// <summary>
        /// Transforms vector from world space to local space.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public float3 InverseTransformVector(float3 vector)
        {
            BurstCompiled_Transform.BurstCompiled_InverseTransformVector(vector, rotation, lossyScale, out var result);
            return result;
        }

        /// <summary>
        /// Transforms rotation from world space to local space.
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public quaternion InverseTransformRotation(quaternion rotation)
        {
            BurstCompiled_Transform.BurstCompiled_InverseTransformRotation(rotation, this.rotation, out var result);
            return result;
        }

        /// <summary>
        /// Moves the transform to its cached positions and parent.
        /// </summary>
        public void MoveToState() {
            if (HasTransform)
                transform.EnsureParent(parent, Internal_OnParented);
        }

        /// <summary>
        /// Moves the transform to its cached positions without setting the parent.
        /// </summary>
        public void MoveToPosition() {
            if (HasTransform) {
                var initialParent = transform.parent;
                transform.EnsureParent(parent, () => {
                    Internal_OnParented();
                    transform.parent = initialParent;
                });
            }
        }

        private void Internal_OnParented() {
            transform.localPosition = localPosition;
            transform.localRotation = localRotation;
            transform.localScale = localScale;
        }
    }

}
