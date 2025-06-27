using UnityEngine;

namespace VAT.Shared.Math
{
    public static class JointMath
    {
        public static Quaternion GetJointRotation(Quaternion initialRotation, Vector3 axis, Vector3 secondaryAxis)
        {
            Vector3 forward = Vector3.Cross(axis, secondaryAxis);
            Vector3 up = -Vector3.Cross(axis, forward);

            var jointRotation = initialRotation * Quaternion.LookRotation(forward, up);

            return jointRotation;
        }

        public static Quaternion GetTargetRotationWorld(Quaternion jointRotation, Quaternion initialRotation, Quaternion targetRotation, Quaternion initialConnectedRotation, Quaternion connectedRotation)
        {
            var result = Quaternion.Inverse(jointRotation);
            result *= initialRotation * Quaternion.Inverse(targetRotation);
            result *= Quaternion.Inverse(initialConnectedRotation * Quaternion.Inverse(connectedRotation));
            result *= jointRotation;

            return result;
        }

        public static Quaternion GetTargetRotationWorld(Quaternion jointRotation, Quaternion initialRotation, Quaternion targetRotation)
        {
            var result = Quaternion.Inverse(jointRotation);
            result *= initialRotation * Quaternion.Inverse(targetRotation);
            result *= jointRotation;

            return result;
        }
    }
}
