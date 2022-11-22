using System;

using Unity.Mathematics;

using UnityEngine;

using VAT.Shared.Extensions;

namespace VAT.Shared.Data {
    /// <summary>
    /// A data structure containing a snapshot of a transform.
    /// </summary>
    [Serializable]
    public struct SimpleTransform {
        /// <summary>
        /// The world space position of the snapshot.
        /// </summary>
        public float3 position;

        /// <summary>
        /// The world space rotation of the snapshot.
        /// </summary>
        public quaternion rotation;

        /// <summary>
        /// The global scale of the snapshot.
        /// </summary>
        public float3 lossyScale;

        /// <summary>
        /// Matrix that transforms a point from local space into world space.
        /// </summary>
        public Matrix4x4 localToWorldMatrix;

        /// <summary>
        /// Creates a snapshot of this transform.
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static SimpleTransform Create(Transform transform) {
            SimpleTransform simple;
            simple.position = transform.position;
            simple.rotation = transform.rotation;
            simple.lossyScale = transform.lossyScale;
            simple.localToWorldMatrix = Matrix4x4.TRS(simple.position, simple.rotation, simple.lossyScale);
            return simple;
        }

        /// <summary>
        /// Creates a snapshot of this position and rotation with a scale of (1, 1, 1).
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static SimpleTransform Create(float3 position, quaternion rotation)
        {
            SimpleTransform simple;
            simple.position = position;
            simple.rotation = rotation;

            float3 scale;
            scale.x = 1f;
            scale.y = 1f;
            scale.z = 1f;

            simple.lossyScale = scale;

            simple.localToWorldMatrix = Matrix4x4.TRS(simple.position, simple.rotation, simple.lossyScale);
            return simple;
        }

        /// <summary>
        /// Creates a snapshot of this position, rotation, and scale.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="lossyScale"></param>
        /// <returns></returns>
        public static SimpleTransform Create(float3 position, quaternion rotation, float3 lossyScale)
        {
            SimpleTransform simple;
            simple.position = position;
            simple.rotation = rotation;
            simple.lossyScale = lossyScale;
            simple.localToWorldMatrix = Matrix4x4.TRS(simple.position, simple.rotation, simple.lossyScale);
            return simple;
        }

        /// <summary>
        /// Transforms position from local space to world space.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public float3 TransformPoint(float3 position) {
            BurstCompiled_Transform.BurstCompiled_TransformPoint(position, this.position, rotation, lossyScale, out var result);
            return result;
        }

        /// <summary>
        /// Transforms direction from local space to world space.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public float3 TransformDirection(float3 direction) {
            BurstCompiled_Transform.BurstCompiled_TransformDirection(direction, rotation, out var result);
            return result;
        }

        /// <summary>
        /// Transforms vector from local space to world space.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public float3 TransformVector(float3 vector) {
            BurstCompiled_Transform.BurstCompiled_TransformVector(vector, rotation, lossyScale, out var result);
            return result;
        }

        /// <summary>
        /// Transforms rotation from local space to world space.
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public quaternion TransformRotation(quaternion rotation) {
            BurstCompiled_Transform.BurstCompiled_TransformRotation(rotation, this.rotation, out var result);
            return result;
        }

        /// <summary>
        /// Transforms position from world space to local space.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public float3 InverseTransformPoint(float3 position) {
            BurstCompiled_Transform.BurstCompiled_InverseTransformPoint(position, this.position, rotation, lossyScale, out var result);
            return result;
        }

        /// <summary>
        /// Transforms direction from world space to local space.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public float3 InverseTransformDirection(float3 direction) {
            BurstCompiled_Transform.BurstCompiled_InverseTransformDirection(direction, rotation, out var result);
            return result;
        }

        /// <summary>
        /// Transforms vector from world space to local space.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public float3 InverseTransformVector(float3 vector) {
            BurstCompiled_Transform.BurstCompiled_InverseTransformVector(vector, rotation, lossyScale, out var result);
            return result;
        }

        /// <summary>
        /// Transforms rotation from world space to local space.
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public quaternion InverseTransformRotation(quaternion rotation) { 
            BurstCompiled_Transform.BurstCompiled_InverseTransformRotation(rotation, this.rotation, out var result);
            return result;
        }

        public static implicit operator SimpleTransform(Transform transform) => SimpleTransform.Create(transform);
    }
}
