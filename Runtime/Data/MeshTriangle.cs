using System;

namespace VAT.Shared.Data
{
    [Serializable]
    public struct MeshTriangle
    {
        public int Vertex1;

        public int Vertex2;

        public int Vertex3;

        public MeshTriangle(int vertex1, int vertex2, int vertex3)
        {
            this.Vertex1 = vertex1;
            this.Vertex2 = vertex2;
            this.Vertex3 = vertex3;
        }

        public static MeshTriangle Offset(MeshTriangle triangle, int offset)
        {
            return new MeshTriangle(triangle.Vertex1 + offset, triangle.Vertex2 + offset, triangle.Vertex3 + offset);
        }
    }
}
