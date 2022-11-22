using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using VAT.Shared.Extensions;

namespace VAT.Shared.Data
{
    /// <summary>
    /// Class for storing initial joint drives, and allowing for multipliers later on.
    /// </summary>
    public class JointDriveMod
    {
        /// <summary>
        /// The joint that is being modified.
        /// </summary>
        public readonly ConfigurableJoint joint;

        /// <summary>
        /// The type of the joint drive.
        /// </summary>
        public readonly JointDriveType type;

        /// <summary>
        /// The initial joint drive.
        /// </summary>
        public JointDrive drive;

        public JointDriveMod(ConfigurableJoint joint, JointDriveType type = JointDriveType.XDRIVE) {
            this.joint = joint;
            this.type = type;
            drive = joint.GetJointDrive(type);
        }

        /// <summary>
        /// Sets the drive of the joint using multipliers.
        /// </summary>
        /// <param name="springMult">The positionSpring multiplier.</param>
        /// <param name="damperMult">The positionDamper multiplier.</param>
        /// <param name="maxForceMult">The maximumForce multiplier.</param>
        public void SetDrive(float springMult, float damperMult, float maxForceMult) {
            joint.SetJointDrive(new JointDrive() { 
                positionSpring = drive.positionSpring * springMult, 
                positionDamper = drive.positionDamper * damperMult, 
                maximumForce = drive.maximumForce * maxForceMult }, 
                type);
        }

        /// <summary>
        /// Sets the drive of the joint using a multiplier.
        /// </summary>
        /// <param name="mult">The multiplier for each parameter.</param>
        public void SetDrive(float mult) => SetDrive(mult, mult, mult);

        /// <summary>
        /// Lerps the drive of the joint using multipliers.
        /// </summary>
        /// <param name="springMult">The positionSpring multiplier.</param>
        /// <param name="damperMult">The positionDamper multiplier.</param>
        /// <param name="maxForceMult">The maximumForce multiplier.</param>
        /// <param name="lerp">The amount to lerp.</param>
        public void SetDriveLerp(float springMult, float damperMult, float maxForceMult, float lerp) {
            JointDrive current = joint.GetJointDrive(type);

            JointDrive newDrive = new() {
                positionSpring = Mathf.Lerp(current.positionSpring, drive.positionSpring * springMult, lerp),
                positionDamper = Mathf.Lerp(current.positionDamper, drive.positionDamper * damperMult, lerp),
                maximumForce = Mathf.Lerp(current.maximumForce, drive.maximumForce * maxForceMult, lerp),
            };

            joint.SetJointDrive(newDrive, type);
        }

        /// <summary>
        /// Sets the base joint drive value. This does not update the joint itself.
        /// </summary>
        /// <param name="spring">The positionSpring.</param>
        /// <param name="damper">The positionDamper.</param>
        /// <param name="maxForce">The maximumForce.</param>
        public void SetDriveBase(float spring, float damper, float maxForce) {
            drive = new JointDrive()
            {
                positionSpring = spring,
                positionDamper = damper,
                maximumForce = maxForce,
            };
        }
    }
}
