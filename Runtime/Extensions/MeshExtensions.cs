using System.Collections.Generic;

using UnityEngine;

using VAT.Shared.Data;

namespace VAT.Shared.Extensions
{
    public static class MeshExtensions
    {
        public static void SetTriangles(this Mesh mesh, MeshTriangle[] triangles, int submesh = 0)
        {
            var indicies = new List<int>();

            for (int i = 0; i < triangles.Length; i++)
            {
                indicies.Add(triangles[i].vertex1);
                indicies.Add(triangles[i].vertex2);
                indicies.Add(triangles[i].vertex3);
            }

            mesh.SetTriangles(indicies, submesh);
        }

        public static void SetTriangles(this Mesh mesh, List<MeshTriangle> triangles, int submesh = 0)
        {
            var indicies = new List<int>();

            for (int i = 0; i < triangles.Count; i++)
            {
                indicies.Add(triangles[i].vertex1);
                indicies.Add(triangles[i].vertex2);
                indicies.Add(triangles[i].vertex3);
            }

            mesh.SetTriangles(indicies, submesh);
        }
    }
}
