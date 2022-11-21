using System;
using System.Collections.Generic;

using UnityEngine;
using static Unity.Mathematics.math;

using VAT.Shared.Extensions;

namespace VAT.Shared.Math {
    /// <summary>
    /// Enum representing every possible face for a cube.
    /// </summary>
    [Flags]
    public enum Faces {
        EVERYTHING = -1,
        NONE = 0,
        PositiveX = 1 << 0,
        NegativeX = 1 << 1,
        PositiveY = 1 << 2,
        NegativeY = 1 << 3,
        PositiveZ = 1 << 4,
        NegativeZ = 1 << 5,
    }

    /// <summary>
    /// Enum representing every possible edge for a cube.
    /// </summary>
    [Flags]
    public enum Edges
    {
        EVERYTHING = -1,
        NONE = 0,
        PositiveXPositiveY = 1 << 0,
        NegativeXPositiveY = 1 << 1,

        PositiveXNegativeY = 1 << 2,
        NegativeXNegativeY = 1 << 3,

        PositiveXPositiveZ = 1 << 4,
        NegativeXPositiveZ = 1 << 5,

        PositiveXNegativeZ = 1 << 6,
        NegativeXNegativeZ = 1 << 7,

        PositiveYPositiveZ = 1 << 8,
        NegativeYPositiveZ = 1 << 9,

        PositiveYNegativeZ = 1 << 10,
        NegativeYNegativeZ = 1 << 11,
    }

    /// <summary>
    /// Enum representing every possible corner for a cube.
    /// </summary>
    [Flags]
    public enum Corners
    {
        EVERYTHING = -1,
        NONE = 0,
        PositiveXPositiveYPositiveZ = 1 << 0,
        NegativeXPositiveYPositiveZ = 1 << 1,

        PositiveXPositiveYNegativeZ = 1 << 2,
        NegativeXPositiveYNegativeZ = 1 << 3,

        PositiveXNegativeYPositiveZ = 1 << 4,
        NegativeXNegativeYPositiveZ = 1 << 5,

        PositiveXNegativeYNegativeZ = 1 << 6,
        NegativeXNegativeYNegativeZ = 1 << 7,
    }

    /// <summary>
    /// Base interface for a box vertex.
    /// </summary>
    internal interface IBoxVertex {
        /// <summary>
        /// Calculates the closest point residing on the surface of the box.
        /// </summary>
        /// <param name="point">The point we are comparing.</param>
        /// <returns>The closest point on the box.</returns>
        Vector3 ClosestPoint(Vector3 point);
    }

    /// <summary>
    /// Structure containing all necessary information about a cube face in local space.
    /// </summary>
    public struct FaceInfo : IBoxVertex {
        public Faces face;
        public Vector3 origin;
        public Vector3 normal;
        public Vector3 extents;

        private Vector3 center;

        public FaceInfo(Vector3 center, Vector3 size, Faces face) {
            this.center = center;
            origin = Geometry.GetFaceCenter(center, size, face);
            this.face = face;
            normal = Geometry.GetFaceNormal(face);
            extents = size * 0.5f;
        }

        public Vector3 ClosestPoint(Vector3 point) {
            // Clamp the point within the bounds of the box
            point -= center;
            point = clamp(point, -extents, extents);
            point += center;

            // Get the closest point on the plane
            var plane = new Plane(normal, origin);
            var result = plane.ClosestPointOnPlane(point);

            return result;
        }
    }

    /// <summary>
    /// Structure containing all necessary information about a cube edge in local space.
    /// </summary>
    public struct EdgeInfo : IBoxVertex {
        public Edges edge;
        public LineData line;
        public Vector3 normal;

        public EdgeInfo(Vector3 center, Vector3 size, Edges edge) {
            line = Geometry.GetEdgeLine(center, size, edge);
            this.edge = edge;
            normal = Geometry.GetEdgeNormal(edge);
        }

        public Vector3 ClosestPoint(Vector3 point) => line.ClosestPointOnLine(point);
    }

    /// <summary>
    /// Structure containing all necessary information about a cube corner in local space.
    /// </summary>
    public struct CornerInfo : IBoxVertex {
        public Corners corner;
        public Vector3 origin;
        public Vector3 normal;

        public CornerInfo(Vector3 center, Vector3 size, Corners corner) {
            origin = Geometry.GetCornerCenter(center, size, corner);
            this.corner = corner;
            normal = Geometry.GetCornerNormal(corner);
        }

        public Vector3 ClosestPoint(Vector3 point) => origin;
    }

    public static partial class Geometry {
        private static readonly List<FaceInfo> _faceInfoBuffer = new();
        private static readonly List<CornerInfo> _cornerInfoBuffer = new();
        private static readonly List<EdgeInfo> _edgeInfoBuffer = new();

        /// <summary>
        /// Calculates a point in local space within the bounds of the box given a direction.
        /// </summary>
        /// <param name="center">The center of the box in local space.</param>
        /// <param name="size">The size of the box in local space. (extents * 2).</param>
        /// <param name="normal">The direction pointing towards the point in local space.</param>
        /// <returns>The point inside the box in local space.</returns>
        public static Vector3 GetConformedPoint(Vector3 center, Vector3 size, Vector3 normal) {
            return center + Vector3.Scale(normal, size * 0.5f);
        }

        /// <summary>
        /// Calculates the outward pointing normal of a box face in local space.
        /// </summary>
        /// <param name="face">The desired face.</param>
        /// <returns>The normal.</returns>
        public static Vector3 GetFaceNormal(Faces face)
        {
            return face switch
            {
                Faces.PositiveX => Vector3.right,
                Faces.PositiveY => Vector3.up,
                Faces.PositiveZ => Vector3.forward,
                Faces.NegativeX => Vector3.left,
                Faces.NegativeY => Vector3.down,
                Faces.NegativeZ => Vector3.back,
                _ => Vector3.right,
            };
        }

        /// <summary>
        /// Calculates the center of a box face in local space.
        /// </summary>
        /// <param name="center">The local space center of the box.</param>
        /// <param name="size">The local space size of the box (extents * 2).</param>
        /// <param name="face">The desired face.</param>
        /// <returns>The center of the face.</returns>
        public static Vector3 GetFaceCenter(Vector3 center, Vector3 size, Faces face) {
            return GetConformedPoint(center, size, GetFaceNormal(face));
        }

        /// <summary>
        /// Calculates the closest point on a box face.
        /// </summary>
        /// /// <param name="point">The point in local space.</param>
        /// <param name="center">The center of the box in local space.</param>
        /// <param name="size">The size of the box in local space.</param>
        /// <param name="face">The desired face.</param>
        /// <returns>The closest point on the face.</returns>
        public static Vector3 ClosestPointOnFace(Vector3 point, Vector3 center, Vector3 size, Faces face)
        {
            var normal = GetFaceNormal(face);
            var extents = size * 0.5f;

            var origin = center + Vector3.Scale(normal, extents);

            var plane = new Plane(normal, origin);
            var result = plane.ClosestPointOnPlane(point);

            result = result.Clamp(-extents, extents);

            return result;
        }

        /// <summary>
        /// Retrieves the face info for all faces contained in the flag.
        /// </summary>
        /// <param name="center">The center of the box in local space.</param>
        /// <param name="size">The size of the box in local space (extents * 2).</param>
        /// <param name="faces">The flag containing all faces.</param>
        /// <returns>The list containing all faces.</returns>
        public static IReadOnlyList<FaceInfo> GetFaceInformation(Vector3 center, Vector3 size, Faces faces) {
            _faceInfoBuffer.Clear();

            for (var i = 0; i < 6; i++) {
                var face = (Faces)(1 << i);

                if ((faces & face) > 0)
                    _faceInfoBuffer.Add(new FaceInfo(center, size, face));
            }

            return _faceInfoBuffer;
        }

        /// <summary>
        /// Calculates the closest face to a point in local space.
        /// </summary>
        /// <param name="point">The point in local space.</param>
        /// <param name="center">The center of the box in local space.</param>
        /// <param name="size">The size of the box in local space (extents * 2).</param>
        /// <param name="faces">The flag containing all faces.</param>
        /// <returns>The closest found face. Returns null if none were found.</returns>
        public static FaceInfo? ClosestFace(Vector3 point, Vector3 center, Vector3 size, Faces faces) {
            float minDistance = float.PositiveInfinity;
            var faceInfos = GetFaceInformation(center, size, faces);

            FaceInfo? result = null;

            foreach (var info in faceInfos) {
                float distance = info.ClosestPoint(point).FastDistance(point);

                if (!result.HasValue || distance < minDistance) {
                    minDistance = distance;
                    result = info;
                }
            }

            return result;
        }

        /// <summary>
        /// Calculates the outward pointing normal of a box corner in local space.
        /// </summary>
        /// <param name="corner">The desired corner.</param>
        /// <returns>The normal.</returns>
        public static Vector3 GetCornerNormal(Corners corner)
        {
            return corner switch
            {
                Corners.PositiveXPositiveYPositiveZ => Vector3.one,
                Corners.NegativeXPositiveYPositiveZ => new Vector3(-1f, 1f, 1f),
                Corners.PositiveXPositiveYNegativeZ => new Vector3(1f, 1f, -1f),
                Corners.NegativeXPositiveYNegativeZ => new Vector3(-1f, 1f, -1f),
                Corners.PositiveXNegativeYPositiveZ => new Vector3(1f, -1f, 1f),
                Corners.NegativeXNegativeYPositiveZ => new Vector3(-1f, -1f, 1f),
                Corners.PositiveXNegativeYNegativeZ => new Vector3(1f, -1f, -1f),
                Corners.NegativeXNegativeYNegativeZ => -Vector3.one,
                _ => Vector3.one,
            };
        }

        /// <summary>
        /// Calculates the center of a corner in local space.
        /// </summary>
        /// <param name="center">The center of the box in local space.</param>
        /// <param name="size">The size of the box in local space (extents * 2).</param>
        /// <param name="corner">The desired corner.</param>
        /// <returns>The center of the corner.</returns>
        public static Vector3 GetCornerCenter(Vector3 center, Vector3 size, Corners corner)
        {
            return GetConformedPoint(center, size, GetCornerNormal(corner));
        }

        /// <summary>
        /// Retrieves the face info for all faces contained in the flag.
        /// </summary>
        /// <param name="center">The center of the box in local space.</param>
        /// <param name="size">The size of the box in local space (extents * 2).</param>
        /// <param name="corners">The flag containing all corners.</param>
        /// <returns>The list containing all corners.</returns>
        public static IReadOnlyList<CornerInfo> GetCornerInformation(Vector3 center, Vector3 size, Corners corners) {
            _cornerInfoBuffer.Clear();

            for (var i = 0; i < 8; i++) {
                var corner = (Corners)(1 << i);

                if ((corners & corner) > 0)
                    _cornerInfoBuffer.Add(new CornerInfo(center, size, corner));
            }

            return _cornerInfoBuffer;
        }

        /// <summary>
        /// Calculates the closest corner to a point in local space.
        /// </summary>
        /// <param name="point">The point in local space.</param>
        /// <param name="center">The center of the box in local space.</param>
        /// <param name="size">The size of the box in local space (extents * 2).</param>
        /// <param name="corners">The flag containing all corners.</param>
        /// <returns>The closest found corner. Returns null if none were found.</returns>
        public static CornerInfo? ClosestCorner(Vector3 point, Vector3 center, Vector3 size, Corners corners)
        {
            float minDistance = float.PositiveInfinity;
            var cornerInfos = GetCornerInformation(center, size, corners);

            CornerInfo? result = null;

            foreach (var info in cornerInfos) {
                float distance = info.ClosestPoint(point).FastDistance(point);

                if (!result.HasValue || distance < minDistance) {
                    minDistance = distance;
                    result = info;
                }
            }

            return result;
        }

        /// <summary>
        /// Calculates the outward pointing normal of a box edge in local space.
        /// </summary>
        /// <param name="edge">The desired edge.</param>
        /// <returns>The normal.</returns>
        public static Vector3 GetEdgeNormal(Edges edge)
        {
            return edge switch
            {
                Edges.PositiveXPositiveY => new Vector3(1f, 1f),
                Edges.NegativeXPositiveY => new Vector3(-1f, 1f),
                Edges.PositiveXNegativeY => new Vector3(1f, -1f),
                Edges.NegativeXNegativeY => new Vector3(-1f, -1f),
                Edges.PositiveXPositiveZ => new Vector3(1f, 0f, 1f),
                Edges.NegativeXPositiveZ => new Vector3(-1f, 0f, 1f),
                Edges.PositiveXNegativeZ => new Vector3(1f, 0f, -1f),
                Edges.NegativeXNegativeZ => new Vector3(-1f, 0f, -1f),
                Edges.PositiveYPositiveZ => new Vector3(0f, 1f, 1f),
                Edges.NegativeYPositiveZ => new Vector3(0f, -1f, 1f),
                Edges.PositiveYNegativeZ => new Vector3(0f, 1f, -1f),
                Edges.NegativeYNegativeZ => new Vector3(0f, -1f, -1f),
                _ => new Vector3(1f, 1f),
            };
        }

        /// <summary>
        /// Calculates the direction the edge's line follows in local space.
        /// </summary>
        /// <param name="edge">The desired edge.</param>
        /// <returns>The direction the edge points.</returns>
        public static Vector3 GetEdgeDirection(Edges edge)
        {
            var normal = GetEdgeNormal(edge);
            return Vector3.one - (Vector3)abs(normal);
        }

        /// <summary>
        /// Calculates the center of the box edge in local space.
        /// </summary>
        /// <param name="center">The center of the box in local space.</param>
        /// <param name="size">The size of the box in local space (extents * 2).</param>
        /// <param name="edge">The desired edge.</param>
        /// <returns>The center of the edge.</returns>
        public static Vector3 GetEdgeCenter(Vector3 center, Vector3 size, Edges edge) {
            return GetConformedPoint(center, size, GetEdgeNormal(edge));
        }

        /// <summary>
        /// Calculates the line data that an edge follows along in local space.
        /// </summary>
        /// <param name="center">The center of the box in local space.</param>
        /// <param name="size">The size of the box in local space.</param>
        /// <param name="edge">The desired edge.</param>
        /// <returns>The line data of the edge.</returns>
        public static LineData GetEdgeLine(Vector3 center, Vector3 size, Edges edge) {
            var lineCenter = GetEdgeCenter(center, size, edge);
            var lineDirection = Vector3.Scale(GetEdgeDirection(edge), size * 0.5f);

            return new LineData(lineCenter - lineDirection, lineCenter + lineDirection);
        }

        /// <summary>
        /// Retrieves the edge info for all edges contained in the flag.
        /// </summary>
        /// <param name="center">The center of the box in local space.</param>
        /// <param name="size">The size of the box in local space (extents * 2).</param>
        /// <param name="edges">The flag containing all edges.</param>
        /// <returns>The list containing all edges.</returns>
        public static IReadOnlyList<EdgeInfo> GetEdgeInformation(Vector3 center, Vector3 size, Edges edges) {
            _edgeInfoBuffer.Clear();

            for (var i = 0; i < 12; i++) {
                var edge = (Edges)(1 << i);

                if ((edges & edge) > 0)
                    _edgeInfoBuffer.Add(new EdgeInfo(center, size, edge));
            }

            return _edgeInfoBuffer;
        }

        /// <summary>
        /// Calculates the closest edge to a point in local space.
        /// </summary>
        /// <param name="point">The point in local space.</param>
        /// <param name="center">The center of the box in local space.</param>
        /// <param name="size">The size of the box in local space (extents * 2).</param>
        /// <param name="edges">The flag containing all edges.</param>
        /// <returns>Returns the found edge. Returns null if none were found.</returns>
        public static EdgeInfo? ClosestEdge(Vector3 point, Vector3 center, Vector3 size, Edges edges) {
            float minDistance = float.PositiveInfinity;
            var edgeInfos = GetEdgeInformation(center, size, edges);

            EdgeInfo? result = null;

            foreach (var info in edgeInfos) {
                float distance = info.ClosestPoint(point).FastDistance(point);

                if (!result.HasValue || distance < minDistance) {
                    minDistance = distance;
                    result = info;
                }
            }

            return result;
        }
    }
}