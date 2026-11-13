using System.Collections.Generic;

using UnityEngine;

using VAT.Shared.Extensions;

namespace VAT.Shared.Data
{
    /// <summary>
    /// A descriptor for a mesh with two curve bases and a specified number of segments.
    /// </summary>
    public struct CurveCylinderMesh
    {
        /// <summary>
        /// The bottom base of the curve cylinder.
        /// </summary>
        public CurveMesh Bottom;

        /// <summary>
        /// The top base of the curve cylinder.
        /// </summary>
        public CurveMesh Top;

        /// <summary>
        /// The number of segments that the mesh has on each base.
        /// </summary>
        public int Segments;

        /// <summary>
        /// Creates a mesh descriptor that can then be used to create or write to a mesh.
        /// </summary>
        /// <returns></returns>
        public MeshDescriptor CreateDescriptor()
        {
            if (Segments <= 0)
            {
                Segments = 16;
            }

            var verticies = new List<Vector3>();
            var triangles = new List<MeshTriangle>();

            var points = new Vector3[Segments];

            Bottom.Curve.GetVertices(points);

            foreach (var point in points)
            {
                verticies.Add((Bottom.Transform.Rotation * point) + Bottom.Transform.Position);
            }

            int offset = verticies.Count;

            Top.Curve.GetVertices(points);

            foreach (var point in points)
            {
                verticies.Add((Top.Transform.Rotation * point) + Top.Transform.Position);
            }

            int triangleIndex = 1;

            for (var i = 0; i < offset; i++)
            {
                if (i > 0)
                {
                    triangles.Add(new MeshTriangle(triangleIndex - 1, triangleIndex, triangleIndex + offset));
                    triangles.Add(new MeshTriangle(triangleIndex - 1 + offset, triangleIndex - 1, triangleIndex + offset));
                    triangleIndex += 1;
                }
            }

            if (Bottom.IsFilled)
            {
                verticies.Add(Bottom.Transform.Position);

                int holeIndex = 1;

                for (var i = 0; i < offset - 1; i++)
                {
                    triangles.Add(new MeshTriangle(holeIndex - 1, verticies.Count - 1, holeIndex));
                    holeIndex += 1;
                }
            }

            if (Top.IsFilled)
            {
                verticies.Add(Top.Transform.Position);

                int holeIndex = 1;

                for (var i = 0; i < offset - 1; i++)
                {
                    triangles.Add(new MeshTriangle(verticies.Count - 1, holeIndex + offset - 1, holeIndex + offset));
                    holeIndex += 1;
                }
            }

            return new MeshDescriptor(verticies, triangles);
        }
    }

}
