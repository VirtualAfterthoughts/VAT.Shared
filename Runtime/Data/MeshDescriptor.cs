using System;
using System.Collections.Generic;

using UnityEngine;

using VAT.Shared.Extensions;

namespace VAT.Shared.Data
{
    [Serializable]
    public struct MeshDescriptor
    {
        public Vector3[] Vertices;

        public MeshTriangle[] Triangles;

        public MeshDescriptor(Vector3[] vertices, MeshTriangle[] triangles)
        {
            this.Vertices = vertices;
            this.Triangles = triangles;
        }

        public MeshDescriptor(List<Vector3> vertices, List<MeshTriangle> triangles)
        {
            this.Vertices = vertices.ToArray();
            this.Triangles = triangles.ToArray();
        }

        public static MeshDescriptor Combine(MeshDescriptor x, MeshDescriptor y)
        {
            int xVerticies = x.Vertices.Length;
            int yVerticies = y.Vertices.Length;

            int xTriangles = x.Triangles.Length;
            int yTriangles = y.Triangles.Length;

            Vector3[] verticies = new Vector3[xVerticies + yVerticies];
            MeshTriangle[] triangles = new MeshTriangle[xTriangles + yTriangles];

            // Combine vertex arrays
            for (var i = 0; i < xVerticies; i++)
            {
                verticies[i] = x.Vertices[i];
            }

            for (var i = 0; i < yVerticies; i++)
            {
                verticies[i + xVerticies] = y.Vertices[i];
            }

            // Combine triangle arrays
            for (var i = 0; i < xTriangles; i++)
            {
                triangles[i] = x.Triangles[i];
            }

            for (var i = 0; i < yTriangles; i++)
            {
                var triangle = y.Triangles[i];
                triangles[i + xTriangles] = MeshTriangle.Offset(triangle, xTriangles + 2);
            }

            // Create the new descriptor
            return new MeshDescriptor(verticies, triangles);
        }

        public readonly Mesh CreateMesh()
        {
            Mesh mesh = new()
            {
                vertices = Vertices
            };

            mesh.SetTriangles(Triangles);
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}
