using System;

using UnityEngine;

namespace VAT.Shared.Data
{
    /// <summary>
    /// A general representation of a curve.
    /// </summary>
    public interface ICurve
    {
        /// <summary>
        /// Gets the vertices of the curve in local space around a given angle and writes them to a span.
        /// </summary>
        /// <param name="points">The span that will be written to. The length is equal to the segment count.</param>
        /// <param name="angle">The maximum angle of the curve.</param>
        void GetVertices(Span<Vector3> points, float angle);
    }
}
