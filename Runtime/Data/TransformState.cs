using System.Collections;
using System.Collections.Generic;

using static Unity.Mathematics.math;

using UnityEngine;

using VAT.Shared.Extensions;

namespace VAT.Shared.Data
{
    using Unity.Mathematics;

    public class TransformState {
        public readonly float3 position;
        public readonly quaternion rotation;
        public readonly float3 lossyScale;

        public readonly float3 localPosition;
        public readonly quaternion localRotation;
        public readonly float3 localScale;

        public readonly Transform transform;
        public readonly Transform parent;

        public TransformState() {
            position = float3.zero;
            rotation = quaternion.identity;
            lossyScale = new float3(1f);

            localPosition = float3.zero;
            localRotation = quaternion.identity;
            localScale = new float3(1f);

            transform = null;
            parent = null;
        }

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
        }

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
        }

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
        }

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
            if (transform != null)
                transform.EnsureParent(parent, OnParented);
        }

        /// <summary>
        /// Moves the transform to its cached positions without setting the parent.
        /// </summary>
        public void MoveToPosition() {
            var initialParent = transform.parent;
            transform.EnsureParent(parent, () => {
                OnParented();
                transform.parent = initialParent;
            });
        }

        private void OnParented() {
            transform.localPosition = localPosition;
            transform.localRotation = localRotation;
            transform.localScale = localScale;
        }
    }

}
