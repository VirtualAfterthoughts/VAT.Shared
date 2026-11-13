using System;
using System.Collections.Generic;

using UnityEngine;

using VAT.Shared.Extensions;

namespace VAT.Shared.Data
{
    /// <summary>
    /// A descriptor for a mesh's vertices and triangles that can be used to build a mesh.
    /// </summary>
    [Serializable]
    public struct MeshDescriptor
    {
        /// <summary>
        /// The vertices of the mesh.
        /// </summary>
        public Vector3[] Vertices;

        /// <summary>
        /// The triangles of the mesh.
        /// </summary>
        public MeshTriangle[] Triangles;

        /// <summary>
        /// Creates a mesh descriptor with the given vertices and triangles.
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="triangles"></param>
        public MeshDescriptor(Vector3[] vertices, MeshTriangle[] triangles)
        {
            this.Vertices = vertices;
            this.Triangles = triangles;
        }

        /// <summary>
        /// Creates a mesh descriptor with the given vertices and triangles.
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="triangles"></param>
        public MeshDescriptor(List<Vector3> vertices, List<MeshTriangle> triangles)
        {
            this.Vertices = vertices.ToArray();
            this.Triangles = triangles.ToArray();
        }

        /// <summary>
        /// Combines two mesh descriptors into one mesh.
        /// <para>
        /// This will not create any new triangles.
        /// </para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static MeshDescriptor Combine(MeshDescriptor a, MeshDescriptor b)
        {
            int xVerticies = a.Vertices.Length;
            int yVerticies = b.Vertices.Length;

            int xTriangles = a.Triangles.Length;
            int yTriangles = b.Triangles.Length;

            Vector3[] verticies = new Vector3[xVerticies + yVerticies];
            MeshTriangle[] triangles = new MeshTriangle[xTriangles + yTriangles];

            // Combine vertex arrays
            for (var i = 0; i < xVerticies; i++)
            {
                verticies[i] = a.Vertices[i];
            }

            for (var i = 0; i < yVerticies; i++)
            {
                verticies[i + xVerticies] = b.Vertices[i];
            }

            // Combine triangle arrays
            for (var i = 0; i < xTriangles; i++)
            {
                triangles[i] = a.Triangles[i];
            }

            for (var i = 0; i < yTriangles; i++)
            {
                var triangle = b.Triangles[i];
                triangles[i + xTriangles] = MeshTriangle.Offset(triangle, xTriangles + 2);
            }

            // Create the new descriptor
            return new MeshDescriptor(verticies, triangles);
        }

        /// <summary>
        /// Creates a mesh from this descriptor.
        /// </summary>
        /// <returns></returns>
        public readonly Mesh CreateMesh()
        {
            Mesh mesh = new();

            WriteMesh(mesh);

            return mesh;
        }

        /// <summary>
        /// Writes the mesh descriptor to an existing mesh.
        /// </summary>
        /// <param name="mesh"></param>
        public readonly void WriteMesh(Mesh mesh)
        {
            mesh.vertices = Vertices;

            mesh.SetTriangles(Triangles);
            mesh.RecalculateNormals();
        }
    }
}
