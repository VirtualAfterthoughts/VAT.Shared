using System.Collections.Generic;

using UnityEngine;

using static Unity.Mathematics.math;

namespace VAT.Shared.Data
{
    public struct EllipseCylinderMesh
    {
        public EllipseMesh Bottom;

        public EllipseMesh Top;

        public int Segments;

        public MeshDescriptor CreateDescriptor()
        {
            if (Segments <= 0)
            {
                Segments = 32;
            }

            var verticies = new List<Vector3>();
            var triangles = new List<MeshTriangle>();

            foreach (var point in Bottom.Ellipse.GetLocalPoints(Segments))
            {
                verticies.Add(mul(Bottom.Transform.Rotation, point) + Bottom.Transform.Position);
            }

            int offset = verticies.Count;

            foreach (var point in Top.Ellipse.GetLocalPoints(Segments))
            {
                verticies.Add(mul(Top.Transform.Rotation, point) + Top.Transform.Position);
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
