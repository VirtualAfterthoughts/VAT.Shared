using UnityEngine;

using VAT.Shared.Extensions;

namespace VAT.Shared.Data
{
    using Unity.Mathematics;

    public class TransformState
    {
        public float3 Position { get; }

        public quaternion Rotation { get; }

        public float3 LossyScale { get; }

        public float3 LocalPosition { get; }

        public quaternion LocalRotation { get; }

        public float3 LocalScale { get; }

        public Transform Transform { get; }

        public Transform Parent { get; }

        public bool HasTransform { get; }

        public bool HasParent { get; }

        public TransformState()
        {
            Position = float3.zero;
            Rotation = quaternion.identity;
            LossyScale = new float3(1f);

            LocalPosition = float3.zero;
            LocalRotation = quaternion.identity;
            LocalScale = new float3(1f);

            Transform = null;
            Parent = null;

            HasTransform = false;
            HasParent = false;
        }

        public TransformState(Transform transform)
        {
            Position = transform.position;
            Rotation = transform.rotation;
            LossyScale = transform.lossyScale;

            LocalPosition = transform.localPosition;
            LocalRotation = transform.localRotation;
            LocalScale = transform.localScale;

            this.Transform = transform;
            Parent = transform.parent;

            HasTransform = true;
            HasParent = Parent != null;
        }

        public TransformState(Transform transform, Transform parent)
        {
            Position = transform.position;
            Rotation = transform.rotation;
            LossyScale = transform.lossyScale;

            LocalPosition = parent.InverseTransformPoint(transform.position);
            LocalRotation = parent.InverseTransformRotation(transform.rotation);
            LocalScale = parent.InverseTransformVector(transform.lossyScale);

            this.Transform = transform;
            this.Parent = parent;

            HasTransform = true;
            HasParent = parent != null;
        }

        public TransformState(float3 position, quaternion rotation, Transform parent)
        {
            this.Position = position;
            this.Rotation = rotation;
            LossyScale = new float3(1f);

            LocalPosition = parent.InverseTransformPoint(position);
            LocalRotation = parent.InverseTransformRotation(rotation);
            LocalScale = parent.InverseTransformVector(new float3(1f));

            Transform = null;
            this.Parent = parent;

            HasTransform = false;
            HasParent = parent != null;
        }

        public TransformState(float3 position, quaternion rotation)
        {
            this.Position = position;
            this.Rotation = rotation;
            LossyScale = new float3(1f);

            LocalPosition = position;
            LocalRotation = rotation;
            LocalScale = new float3(1f);

            Transform = null;
            Parent = null;

            HasTransform = false;
            HasParent = false;
        }

        public float3 TransformPoint(float3 position)
        {
            BurstTransformExtensions.TransformPoint(position, this.Position, Rotation, LossyScale, out var result);
            return result;
        }

        public float3 TransformDirection(float3 direction)
        {
            BurstTransformExtensions.TransformDirection(direction, Rotation, out var result);
            return result;
        }

        public float3 TransformVector(float3 vector)
        {
            BurstTransformExtensions.TransformVector(vector, Rotation, LossyScale, out var result);
            return result;
        }

        public quaternion TransformRotation(quaternion rotation)
        {
            BurstTransformExtensions.TransformRotation(rotation, this.Rotation, this.LossyScale, out var result);
            return result;
        }

        public float3 InverseTransformPoint(float3 position)
        {
            BurstTransformExtensions.InverseTransformPoint(position, this.Position, Rotation, LossyScale, out var result);
            return result;
        }

        public float3 InverseTransformDirection(float3 direction)
        {
            BurstTransformExtensions.InverseTransformDirection(direction, Rotation, out var result);
            return result;
        }

        public float3 InverseTransformVector(float3 vector)
        {
            BurstTransformExtensions.InverseTransformVector(vector, Rotation, LossyScale, out var result);
            return result;
        }

        public quaternion InverseTransformRotation(quaternion rotation)
        {
            BurstTransformExtensions.InverseTransformRotation(rotation, this.Rotation, LossyScale, out var result);
            return result;
        }
    }

}
