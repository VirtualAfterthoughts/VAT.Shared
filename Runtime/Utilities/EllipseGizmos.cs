using UnityEngine;

using VAT.Shared.Data;
using VAT.Shared.Extensions;

namespace VAT.Shared.Utilities
{
    /// <summary>
    /// Functions for drawing <see cref="Ellipse"/> gizmos.
    /// </summary>
    public static class EllipseGizmos
    {
        /// <summary>
        /// Draws an ellipse at a given position and rotation.
        /// </summary>
        /// <param name="ellipse"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        public static void DrawEllipse(Ellipse ellipse, Vector3 position, Quaternion rotation) => DrawEllipse(ellipse, 360f, position, rotation);

        /// <summary>
        /// Draws an ellipse with a maximum angle at a given position and rotation.
        /// </summary>
        /// <param name="ellipse"></param>
        /// <param name="angle"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        public static void DrawEllipse(Ellipse ellipse, float angle, Vector3 position, Quaternion rotation) => CurveGizmos.DrawCurve(ellipse, angle, position, rotation);

        /// <summary>
        /// Draws an ellipse with a specified radius at a given position and rotation.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        public static void DrawEllipse(Vector2 radius, Vector3 position, Quaternion rotation) => DrawEllipse(radius, 360f, position, rotation);

        /// <summary>
        /// Draws an ellipse with a specified radius and maximum angle at a given position and rotation.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="angle"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        public static void DrawEllipse(Vector2 radius, float angle, Vector3 position, Quaternion rotation) => DrawEllipse(new Ellipse(radius), angle, position, rotation);

        /// <summary>
        /// Draws two connected ellipses.
        /// </summary>
        /// <param name="startEllipse"></param>
        /// <param name="startTransform"></param>
        /// <param name="endEllipse"></param>
        /// <param name="endTransform"></param>
        public static void DrawEllipses(Ellipse startEllipse, SimpleTransform startTransform, Ellipse endEllipse, SimpleTransform endTransform) => DrawEllipses(startEllipse, startTransform, endEllipse, endTransform, 360f);

        /// <summary>
        /// Draws two connected ellipses with a maximum angle.
        /// </summary>
        /// <param name="startEllipse"></param>
        /// <param name="startTransform"></param>
        /// <param name="endEllipse"></param>
        /// <param name="endTransform"></param>
        /// <param name="angle"></param>
        public static void DrawEllipses(Ellipse startEllipse, SimpleTransform startTransform, Ellipse endEllipse, SimpleTransform endTransform, float angle) => CurveGizmos.DrawCurves(startEllipse, startTransform, endEllipse, endTransform, angle);
    }
}
