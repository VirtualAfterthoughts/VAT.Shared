using System.Collections.Generic;
using System;
using System.Linq;

using UnityEngine;

using static Unity.Mathematics.math;

namespace VAT.Shared.Extensions
{
    using Unity.Mathematics;

    public static partial class PhysicsExtensions {
        /// <summary>
        /// Returns pos clamped within the range of the joint's linear limit.
        /// </summary>
        /// <param name="joint"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static Vector3 GetClampedJointPos(this ConfigurableJoint joint, Vector3 pos) {
            if (joint) {
                var transform = joint.transform;
                var connectedAnchor = joint.GetWorldConnectedAnchor();
                var anchorDirection = transform.TransformPoint(joint.anchor) - transform.position;
                var posDist = pos - connectedAnchor + anchorDirection;
                posDist = Vector3.ClampMagnitude(posDist, joint.linearLimit.limit) - anchorDirection;
                return connectedAnchor + posDist;
            }
            else
                return pos;
        }
    }
}