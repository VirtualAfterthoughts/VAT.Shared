using System;

using UnityEngine;

namespace VAT.Shared.Data
{
    [Serializable]
    public struct JointAngularLimits
    {
        public static readonly JointAngularLimits Free = new() { AngularXMotion = ConfigurableJointMotion.Free, AngularYMotion = ConfigurableJointMotion.Free, AngularZMotion = ConfigurableJointMotion.Free };

        public static readonly JointAngularLimits Locked = new() { AngularXMotion = ConfigurableJointMotion.Locked, AngularYMotion = ConfigurableJointMotion.Locked, AngularZMotion = ConfigurableJointMotion.Locked };

        public ConfigurableJointMotion AngularXMotion;

        public ConfigurableJointMotion AngularYMotion;

        public ConfigurableJointMotion AngularZMotion;

        [Range(-180f, 180f)]
        public float LowAngularXLimit;

        [Range(-180f, 180f)]
        public float HighAngularXLimit;

        [Range(-180f, 180f)]
        public float AngularYLimit;

        [Range(-180f, 180f)]
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
