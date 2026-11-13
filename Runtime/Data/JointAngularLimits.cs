using System;

using UnityEngine;

namespace VAT.Shared.Data
{
    /// <summary>
    /// Serializable angular limits for a Unity ConfigurableJoint.
    /// </summary>
    [Serializable]
    public struct JointAngularLimits
    {
        /// <summary>
        /// Angular limits with all axes freed.
        /// </summary>
        public static readonly JointAngularLimits Free = new() { AngularXMotion = ConfigurableJointMotion.Free, AngularYMotion = ConfigurableJointMotion.Free, AngularZMotion = ConfigurableJointMotion.Free };

        /// <summary>
        /// Angular limits with all axes locked.
        /// </summary>
        public static readonly JointAngularLimits Locked = new() { AngularXMotion = ConfigurableJointMotion.Locked, AngularYMotion = ConfigurableJointMotion.Locked, AngularZMotion = ConfigurableJointMotion.Locked };

        [Tooltip("The freedom of motion for the angular x axis.")]
        public ConfigurableJointMotion AngularXMotion;

        [Tooltip("The freedom of motion for the angular y axis.")]
        public ConfigurableJointMotion AngularYMotion;

        [Tooltip("The freedom of motion for the angular z axis.")]
        public ConfigurableJointMotion AngularZMotion;

        [Range(-180f, 180f)]
        [Tooltip("The lower angular x limit.")]
        public float LowAngularXLimit;

        [Range(-180f, 180f)]
        [Tooltip("The upper angular x limit.")]
        public float HighAngularXLimit;

        [Range(-180f, 180f)]
        [Tooltip("The angular y limit in both the positive and negative direction.")]
        public float AngularYLimit;

        [Range(-180f, 180f)]
        [Tooltip("The angular z limit in both the positive and negative direction.")]
        public float AngularZLimit;

        public JointAngularLimits(ConfigurableJoint joint)
        {
            AngularXMotion = joint.angularXMotion;
            AngularYMotion = joint.angularYMotion;
            AngularZMotion = joint.angularZMotion;

            LowAngularXLimit = joint.lowAngularXLimit.limit;
            HighAngularXLimit = joint.highAngularXLimit.limit;
            AngularYLimit = joint.angularYLimit.limit;
            AngularZLimit = joint.angularZLimit.limit;
        }

        public JointAngularLimits(float lowAngularXLimit, float highAngularXLimit, float angularYLimit, float angularZLimit)
        {
            AngularXMotion = AngularYMotion = AngularZMotion = ConfigurableJointMotion.Limited;

            LowAngularXLimit = lowAngularXLimit;
            HighAngularXLimit = highAngularXLimit;
            AngularYLimit = angularYLimit;
            AngularZLimit = angularZLimit;
        }

        /// <summary>
        /// Copies the angular limits to a ConfigurableJoint.
        /// </summary>
        /// <param name="joint"></param>
        public readonly void CopyTo(ConfigurableJoint joint)
        {
            joint.lowAngularXLimit = new SoftJointLimit() { limit = LowAngularXLimit };
            joint.highAngularXLimit = new SoftJointLimit() { limit = HighAngularXLimit };
            joint.angularYLimit = new SoftJointLimit() { limit = AngularYLimit };
            joint.angularZLimit = new SoftJointLimit() { limit = AngularZLimit };

            joint.angularXMotion = AngularXMotion;
            joint.angularYMotion = AngularYMotion;
            joint.angularZMotion = AngularZMotion;
        }
    }
}
