using System;

namespace VAT.Shared.Math
{
    /// <summary>
    /// Functions for calculating values using angles.
    /// </summary>
    public static class AngleMath
    {
        /// <summary>
        /// Gets the quadrant of an angle in degrees as an index from 0 (quadrant 1) to 3 (quadrant 4).
        /// </summary>
        /// <param name="angle">The angle in degrees.</param>
        /// <returns></returns>
        public static int GetQuadrantFromDegrees(float angle)
        {
            return (int)MathF.Floor(angle / 90f) % 4;
        }

        /// <summary>
        /// Returns the given x and y values based on their sign in a given quadrant index from 0 to 3 (quadrant 1 to 4).
        /// <para>Quadrant 1 will return (positiveX, positiveY).</para>
        /// <para>Quadrant 2 will return (negativeX, positiveY).</para>
        /// <para>Quadrant 3 will return (negativeX, negativeY).</para>
        /// <para>Quadrant 4 will return (positiveX, negativeY).</para>
        /// </summary>
        /// <param name="quadrant">The quadrant index from 0 to 3 representing quadrants 1 through 4. If the index is not between 0 and 3 inclusive, the quadrant will wrap back around.</param>
        /// <param name="positiveX">The x-value in quadrants 1 and 4.</param>
        /// <param name="positiveY">The y-value in quadrants 1 and 2.</param>
        /// <param name="negativeX">The x-value in quadrants 2 and 3.</param>
        /// <param name="negativeY">The y-value in quadrants 3 and 4.</param>
        /// <returns></returns>
        public static (float x, float y) GetValuesFromQuadrant(int quadrant, float positiveX, float positiveY, float negativeX, float negativeY)
        {
            int normalizedQuadrant = quadrant % 4;

            return normalizedQuadrant switch
            {
                // Quadrant 2 (-x, +y)
                1 => (negativeX, positiveY),
                // Quadrant 3 (-x, -y)
                2 => (negativeX, negativeY),
                // Quadrant 4 (+x, -y)
                3 => (positiveX, negativeY),
                // Quadrant 1 (+x, +y)
                _ => (positiveX, positiveY),
            };
        }
    }
}
