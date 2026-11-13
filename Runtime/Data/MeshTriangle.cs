using System;

namespace VAT.Shared.Data
{
    /// <summary>
    /// The vertex indices of a triangle on a mesh.
    /// </summary>
    [Serializable]
    public struct MeshTriangle
    {
        /// <summary>
        /// The first vertex.
        /// </summary>
        public int Vertex1;

        /// <summary>
        /// The second vertex.
        /// </summary>
        public int Vertex2;

        /// <summary>
        /// The third vertex.
        /// </summary>
        public int Vertex3;

        /// <summary>
        /// Creates a mesh triangle from three vertex indices.
        /// </summary>
        /// <param name="vertex1"></param>
        /// <param name="vertex2"></param>
        /// <param name="vertex3"></param>
        public MeshTriangle(int vertex1, int vertex2, int vertex3)
        {
            this.Vertex1 = vertex1;
            this.Vertex2 = vertex2;
            this.Vertex3 = vertex3;
        }

        /// <summary>
        /// Creates a new triangle by offsetting the vertex indices of an existing triangle by a given amount.
        /// </summary>
        /// <param name="triangle"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static MeshTriangle Offset(MeshTriangle triangle, int offset)
        {
            return new MeshTriangle(triangle.Vertex1 + offset, triangle.Vertex2 + offset, triangle.Vertex3 + offset);
        }
    }
}
