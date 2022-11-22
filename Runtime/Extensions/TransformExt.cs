using UnityEngine;

using System.Collections.Generic;
using System.Collections;
using System;

using Object = UnityEngine.Object;

using static Unity.Mathematics.math;

#if USE_UNITASK
using Cysharp.Threading.Tasks;
#endif

namespace VAT.Shared.Extensions {
    using Unity.Burst;
    using Unity.Mathematics;

    /// <summary>
    /// Extension methods for Transforms.
    /// </summary>
    public static partial class TransformExtensions {
        /// <summary>
        /// Ensures the parent is set even when called in a method that does not allow parent setting.
        /// Requires UniTask to function properly.
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="parent"></param>
        public static void EnsureParent(this Transform transform, Transform parent, Action onFinish = null) {
            if (transform == null)
                return;

            transform.parent = parent;

#if USE_UNITASK
            EnsureParentAsync(transform, parent, onFinish).Forget();
#endif
        }

#if USE_UNITASK
        private static async UniTaskVoid EnsureParentAsync(Transform transform, Transform parent, Action onFinish = null) {
            while (transform.parent != parent) {
                transform.parent = parent;
                await UniTask.Yield();
            }

            if (onFinish != null)
                onFinish?.Invoke();
        }
#endif

        /// <summary>
        /// Transforms rotation from local space to world space.
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static Quaternion TransformRotation(this Transform transform, Quaternion rotation) => transform.rotation * rotation;

        /// <summary>
        /// Transforms rotation from world space to local space.
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static Quaternion InverseTransformRotation(this Transform transform, Quaternion rotation) => Quaternion.Inverse(transform.rotation) * rotation;

        /// <summary>
        /// Removes all children and components from a transform.
        /// </summary>
        /// <param name="transform"></param>
        public static void EmptyTransform(this Transform transform) {
            foreach (var child in transform.GetChildren())
                Object.DestroyImmediate(child.gameObject);
            transform.RemoveComponents();
        }

        /// <summary>
        /// Removes every component on a transform.
        /// </summary>
        /// <param name="transform"></param>
        public static void RemoveComponents(this Transform transform) {
            var components = transform.GetComponents<Component>();
            foreach (var component in components) {
                if (component is Transform)
                    continue;
                Object.DestroyImmediate(component);
            }
        }

        /// <summary>
        /// Returns all direct children on a transform.
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static Transform[] GetChildren(this Transform transform) {
            var children = new Transform[transform.childCount];
            for (int i = 0; i < children.Length; i++)
                children[i] = transform.GetChild(i);
            return children;
        }

        /// <summary>
        /// Returns all children in the hierarchy of a transform.
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static Transform[] GetGrandChildren(this Transform transform) {
            var directChildren = transform.GetChildren();
            var children = new List<Transform>();
            children.AddRange(directChildren);
            for (int i = 0; i < directChildren.Length; i++)
                children.AddRange(directChildren[i].GetChildren());

            return children.ToArray();
        }

        /// <summary>
        /// Destroys an existing component on the transform if found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transform"></param>
        /// <param name="immediate"></param>
        public static void DestroyComponent<T>(this Transform transform, bool immediate = false) where T : Component {
            if (transform.TryGetComponent(out T comp)) {
                if (immediate)
                    Object.DestroyImmediate(comp);
                else
                    Object.Destroy(comp);
            }
        }

        /// <summary>
        /// Destroys all components in children of the transform if found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transform"></param>
        /// <param name="includeInactive"></param>
        /// <param name="immediate"></param>
        public static void DestroyChildComponents<T>(this Transform transform, bool includeInactive = false, bool immediate = false) where T : Component{
            var components = transform.GetComponentsInChildren<T>(includeInactive);
            foreach (var component in components) {
                if (immediate)
                    Object.DestroyImmediate(component);
                else
                    Object.Destroy(component);
            }
        }

        /// <summary>
        /// Sets the parent of all child transforms to this transform.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="children"></param>
        public static void SetChildren(this Transform t, params Transform[] children) {
            for (int i = 0; i < children.Length; i++)
                children[i].parent = t;
        }

        /// <summary>
        /// Copies position and rotation from this transform to another.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="other"></param>
        public static void CopyTo(this Transform from, Transform to) {
            if (!from || !to) return;

            to.SetPositionAndRotation(from.position, from.rotation);
        }

        /// <summary>
        /// Copies position and rotation from another transform to this one.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="other"></param>
        public static void CopyFrom(this Transform to, Transform from) {
            if (!to || !from) return;

            to.SetPositionAndRotation(from.position, from.rotation);
        }

        /// <summary>
        /// Resets the local position, rotation, and scale of this transform to default values.
        /// </summary>
        /// <param name="transform"></param>
        public static void Reset(this Transform transform) {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
    }

    [BurstCompile(FloatMode = FloatMode.Fast)]
    public static class BurstCompiled_Transform
    {
        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void BurstCompiled_TransformPoint(in float3 input, in float3 position, in quaternion rotation, in float3 lossyScale, out float3 result) => result = mul(rotation, input * lossyScale) + position;

        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void BurstCompiled_TransformDirection(in float3 input, in quaternion rotation, out float3 result) => result = mul(rotation, input);

        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void BurstCompiled_TransformVector(in float3 input, in quaternion rotation, in float3 lossyScale, out float3 result) => BurstCompiled_TransformDirection(input / lossyScale, rotation, out result);

        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void BurstCompiled_TransformRotation(in quaternion input, in quaternion rotation, out quaternion result) => result = mul(rotation, input);

        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void BurstCompiled_InverseTransformPoint(in float3 input, in float3 position, in quaternion rotation, in float3 lossyScale, out float3 result) => result = mul(inverse(rotation), input - position) / lossyScale;

        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void BurstCompiled_InverseTransformDirection(in float3 input, in quaternion rotation, out float3 result) => result = mul(inverse(rotation), input);

        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void BurstCompiled_InverseTransformVector(in float3 input, in quaternion rotation, in float3 lossyScale, out float3 result)
        {
            BurstCompiled_InverseTransformDirection(input, rotation, out float3 vector);
            result = vector * lossyScale;
        }

        [BurstCompile(FloatMode = FloatMode.Fast)]
        public static void BurstCompiled_InverseTransformRotation(in quaternion input, in quaternion rotation, out quaternion result) => result = mul(inverse(rotation), input);
    }
}

