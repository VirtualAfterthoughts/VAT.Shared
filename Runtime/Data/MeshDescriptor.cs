using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using VAT.Shared.Extensions;

namespace VAT.Shared.Data
{
    [Serializable]
    public struct MeshDescriptor
    {
        public Vector3[] verticies;

        public MeshTriangle[] triangles;

        public MeshDescriptor(Vector3[] verticies, MeshTriangle[] triangles)
        {
            this.verticies = verticies;
            this.triangles = triangles;
        }

        public MeshDescriptor(IList<Vector3> verticies, IList<MeshTriangle> triangles)
        {
            this.verticies = verticies.ToArray();
            this.triangles = triangles.ToArray();
        }

        public static MeshDescriptor Combine(MeshDescriptor x, MeshDescriptor y)
        {
            int xVerticies = x.verticies.Length;
            int yVerticies = y.verticies.Length;

            int xTriangles = x.triangles.Length;
            int yTriangles = y.triangles.Length;

            Vector3[] verticies = new Vector3[xVerticies + yVerticies];
            MeshTriangle[] triangles = new MeshTriangle[xTriangles + yTriangles];

            // Combine vertex arrays
            for (var i = 0; i < xVerticies; i++)
            {
                verticies[i] = x.verticies[i];
            }

            for (var i = 0; i < yVerticies; i++)
            {
                verticies[i + xVerticies] = y.verticies[i];
            }

            // Combine triangle arrays
            for (var i = 0; i < xTriangles; i++)
            {
                triangles[i] = x.triangles[i];
            }

            for (var i = 0; i < yTriangles; i++)
            {
                var triangle = y.triangles[i];
                triangles[i + xTriangles] = MeshTriangle.Offset(triangle, xTriangles + 2);
            }

            // Create the new descriptor
            return new MeshDescriptor(verticies, triangles);
        }

        public readonly Mesh CreateMesh()
        {
            Mesh mesh = new()
            {
                vertices = verticies
            };

            mesh.SetTriangles(triangles);
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}
