using System;
using System.Collections.Generic;

using UnityEngine;

using VAT.Shared.Data;

namespace VAT.Shared.Extensions
{
    /// <summary>
    /// Extension functions for meshes.
    /// </summary>
    public static class MeshExtensions
    {
        /// <summary>
        /// Sets the triangles of a mesh to a span of triangles.
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="triangles"></param>
        public static void SetTriangles(this Mesh mesh, ReadOnlySpan<MeshTriangle> triangles) => mesh.SetTriangles(triangles, 0);

        /// <summary>
        /// Sets the triangles of a mesh to a span of triangles for a given submesh.
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="triangles"></param>
        /// <param name="submesh"></param>
        public static void SetTriangles(this Mesh mesh, ReadOnlySpan<MeshTriangle> triangles, int submesh)
        {
            var indicies = new List<int>();

            foreach (var triangle in triangles)
            {
                indicies.Add(triangle.Vertex1);
                indicies.Add(triangle.Vertex2);
                indicies.Add(triangle.Vertex3);
            }

            mesh.SetTriangles(indicies, submesh);
        }
    }
}
