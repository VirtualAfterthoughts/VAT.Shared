using System.Collections.Generic;
using System;
using System.Linq;

using UnityEngine;

using static Unity.Mathematics.math;

namespace VAT.Shared.Extensions {
    using Unity.Mathematics;

    public static partial class PhysicsExtensions {
        /// <summary>
        /// Returns true if the rigidbody should be asleep. This does not necessarily mean it is sleeping.
        /// </summary>
        /// <param name="rb"></param>
        /// <returns></returns>
        public static bool NeedsSleep(this Rigidbody rb) => length(rb.velocity) < rb.sleepThreshold;

        /// <summary>
        /// If the rigidbody is awake and needs sleep, it will be forced asleep.
        /// </summary>
        /// <param name="rb"></param>
        /// <returns></returns>
        public static bool SleepCheck(this Rigidbody rb)
        {
            if (rb.IsSleeping()) return true;

            if (rb.NeedsSleep()) {
                rb.Sleep();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the velocity at a point in space, with a different velocity value.
        /// </summary>
        /// <param name="rb"></param>
        /// <param name="point"></param>
        /// <param name="velocity"></param>
        /// <returns></returns>
        // Thanks to https://answers.unity.com/questions/1192716/get-velocity-of-point-offset-from-center-of-rigidb.html
        public static Vector3 GetPointVelocity(this Rigidbody rb, Vector3 point, Vector3 velocity) {
            Vector3 localP = rb.transform.InverseTransformPoint(point);
            Vector3 vel = Vector3.Cross(rb.angularVelocity, localP - rb.centerOfMass);
            vel = rb.transform.TransformDirection(vel) + velocity;

            return vel;
        }

        /// <summary>
        /// Returns the velocity at a point in space, with a different velocity and angularVelocity value.
        /// </summary>
        /// <param name="rb"></param>
        /// <param name="point"></param>
        /// <param name="velocity"></param>
        /// <param name="angularVelocity"></param>
        /// <returns></returns>
        public static Vector3 GetPointVelocity(this Rigidbody rb, Vector3 point, Vector3 velocity, Vector3 angularVelocity)
        {
            Vector3 localP = rb.transform.InverseTransformPoint(point);
            Vector3 vel = Vector3.Cross(angularVelocity, localP - rb.centerOfMass);
            vel = rb.transform.TransformDirection(vel) + velocity;

            return vel;
        }

        /// <summary>
        /// Returns all active colliders attached to this rigidbody. (CACHING IS RECOMMENDED!)
        /// </summary>
        /// <param name="rb"></param>
        /// <returns></returns>
        public static Collider[] GetColliders(this Rigidbody rb) {
            var children = rb.GetComponentsInChildren<Collider>().ToList();
            children.RemoveAll((c) => c.attachedRigidbody != rb);
            return children.ToArray();
        }

        /// <summary>
        /// Ignores the collision between two rigidbodies.
        /// </summary>
        /// <param name="rb1"></param>
        /// <param name="rb2"></param>
        /// <param name="ignore"></param>
        public static void IgnoreCollision(this Rigidbody rb1, Rigidbody rb2, bool ignore = true) {
            var self = rb1.GetColliders();
            var other = rb2.GetColliders();

            for (int i = 0; i < self.Length; i++) {
                Collider col1 = self[i];
                for (int c = 0; c < other.Length; c++)
                    Physics.IgnoreCollision(col1, other[c]);
            }
        }

        /// <summary>
        /// Scales the vector by the rigidbody's inertia tensor.
        /// </summary>
        /// <param name="rb"></param>
        /// <param name="vector"></param>
        public static float3 ScaleInertia(Rigidbody rb, float3 vector)
        {
            vector = ToLocalInertia(rb, vector);
            vector *= rb.inertiaTensor;
            vector = ToWorldInertia(rb, vector);
            return vector;
        }

        /// <summary>
        /// Scales the vector by the rigidbody's inertia tensor with a clamped length.
        /// </summary>
        /// <param name="rb"></param>
        /// <param name="vector"></param>
        /// <param name="maxLength"></param>
        public static void ScaleInertia(Rigidbody rb, ref float3 vector, float maxLength = Mathf.Infinity)
        {
            vector = ToLocalInertia(rb, vector);
            vector *= Vector3.ClampMagnitude(rb.inertiaTensor, maxLength);
            vector = ToWorldInertia(rb, vector);
        }

        /// <summary>
        /// Divides the vector by the rigidbody's inertia tensor.
        /// </summary>
        /// <param name="rb"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static float3 DivideInertia(Rigidbody rb, float3 vector)
        {
            vector = ToLocalInertia(rb, vector);
            vector /= rb.inertiaTensor;
            vector = ToWorldInertia(rb, vector);
            return vector;
        }

        /// <summary>
        /// Divides the vector by the rigidbody's inertia tensor.
        /// </summary>
        /// <param name="rb"></param>
        /// <param name="vector"></param>
        public static void DivideInertia(Rigidbody rb, ref float3 vector)
        {
            vector = ToLocalInertia(rb, vector);
            vector /= rb.inertiaTensor;
            vector = ToWorldInertia(rb, vector);
        }

        /// <summary>
        /// Returns the rigidbody's inertia tensor rotation in world space.
        /// </summary>
        /// <param name="rb"></param>
        /// <returns></returns>
        public static quaternion ToWorldInertiaRotation(this Rigidbody rb) => rb.inertiaTensorRotation * rb.transform.rotation;

        /// <summary>
        /// Transforms the vector from local inertia space to world inertia space.
        /// </summary>
        /// <param name="rb"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector3 ToWorldInertia(this Rigidbody rb, float3 vector) => mul(ToWorldInertiaRotation(rb), vector);

        /// <summary>
        /// Transforms the vector from world space to local inertia space.
        /// </summary>
        /// <param name="rb"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector3 ToLocalInertia(this Rigidbody rb, float3 vector) => mul(inverse(ToWorldInertiaRotation(rb)), vector);

        /// <summary>
        /// Calculates the inertia tensor of a cube with dimensions based on size and mass.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="mass"></param>
        /// <returns></returns>
        public static Vector3 CalculateInertiaTensor(Vector3 size, float mass)
        {
            // Thanks to PuppetMaster's PhysXTools
            var size2 = Vector3.Scale(size, size);

            float mlp = 1f / 12f * mass;

            return mlp * new Vector3(size2.y + size2.z, size2.x + size2.z, size2.x + size2.y);
        }
    }
}
