using UnityEngine;

namespace VAT.Shared.Data
{
    /// <summary>
    /// A general representation of a closed 2D curve centered around an origin.
    /// </summary>
    public interface ICurve
    {
        /// <summary>
        /// Gets a point on the curve in local space at a specific angle.
        /// </summary>
        /// <param name="angle">The angle in degrees from the +x axis.</param>
        /// <returns></returns>
        Vector3 GetPointOnCurve(float angle);
    }
}
