using System;

namespace VAT.Shared.Data
{
    [Serializable]
    public struct MeshTriangle
    {
        public int vertex1;
        public int vertex2;
        public int vertex3;

        public MeshTriangle(int vertex1, int vertex2, int vertex3)
        {
            this.vertex1 = vertex1;
            this.vertex2 = vertex2;
            this.vertex3 = vertex3;
        }

        public static MeshTriangle Offset(MeshTriangle triangle, int offset)
        {
            return new MeshTriangle(triangle.vertex1 + offset, triangle.vertex2 + offset, triangle.vertex3 + offset);
        }
    }
}
