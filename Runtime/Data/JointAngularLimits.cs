using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using VAT.Shared.Utilities;

namespace VAT.Shared.Data
{
    /// <summary>
    /// Data structure for storing specific angular limits on a joint.
    /// </summary>
    [Serializable]
    public struct JointAngularLimits
    {
        [Tooltip("The lower (negative) limit of the angular X axis. A value of -180 is completely free.")]
        [Range(-180f, 180f)]
        public float lowAngularXLimit;

        [Tooltip("The upper (positive) limit of the angular X axis. A value of 180 is completely free.")]
        [Range(-180f, 180f)]
        public float highAngularXLimit;

        [Tooltip("The limit of the angular Y axis. A value of 180 is completely free.")]
        [Range(-180f, 180f)]
        public float angularYLimit;

        [Tooltip("The limit of the angular Z axis. A value of 180 is completely free.")]
        [Range(-180f, 180f)]
        public float angularZLimit;

        public JointAngularLimits(float lowAngularXLimit, float highAngularXLimit, float angularYLimit, float angularZLimit)
        {
            this.lowAngularXLimit = lowAngularXLimit;
            this.highAngularXLimit = highAngularXLimit;
            this.angularYLimit = angularYLimit;
            this.angularZLimit = angularZLimit;
        }

        /// <summary>
        /// Returns if this drive axis should be free or limited.
        /// </summary>
        /// <param name="axis">The drive axis.</param>
        /// <returns></returns>
        public bool IsFree(Axis axis) {
            return axis switch
            {
                Axis.Y => angularYLimit > 177f,
                Axis.Z => angularZLimit > 177f,
                _ => lowAngularXLimit < -177f && highAngularXLimit > 177f,
            };
        }
    }
}
