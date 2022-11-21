using UnityEngine;

using static Unity.Mathematics.math;

namespace VAT.Shared.Extensions {
    using Unity.Mathematics;

    public static partial class Float3Extensions {
        /// <summary>
        /// If the vector is equal to zero and is unable to be normalized, it is replaced with the supplement.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="supplement"></param>
        /// <returns></returns>
        public static float3 forcenormalize(this float3 vector, float3 supplement) => (vector == float3.zero).istrue() ? supplement : normalize(vector);

        /// <summary>
        /// Returns true if each value in the bool3 is true.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool istrue(this bool3 value) => value.x && value.y && value.z;
    }
}