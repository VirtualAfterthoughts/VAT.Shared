using UnityEngine;

namespace VAT.Shared.Extensions
{
    public static class QuaternionExtensions
    {
        public static Quaternion Shortest(this Quaternion quat)
        {
            if (quat.w < 0f)
            {
                return new Quaternion(-quat.x, -quat.y, -quat.z, -quat.w);
            }

            return quat;
        }
    }
}
