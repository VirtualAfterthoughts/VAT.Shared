using System.Collections.Generic;

using UnityEngine;

using VAT.Shared.Extensions;

namespace VAT.Shared.Math {
    public static class CrossSection
    {
        public static Dictionary<GameObject, Dictionary<Transform, Vector3[]>> verticesDict = new();

        public static float CrossSectionArea(GameObject go, Vector3 normal)
        {
            if (normal.x == 0f && normal.y == 0f && normal.z == 0f)
            {
                return 0f;
            }

            Dictionary<Transform, Vector3[]> references;

            PutVerticesInDict(go, out references);

            List<Vector3> vertices = new List<Vector3>();
            foreach (KeyValuePair<Transform, Vector3[]> pair in references)
            {
                for (int i = 0; i < pair.Value.Length; i++) {
                    vertices.Add(pair.Key.TransformPoint(pair.Value[i]));
                }
            }

            normal = normal.normalized;

            Vector3 perp1 = normal.Perp().normalized;
            Vector3 perp2 = Vector3.Cross(normal, perp1);


            List<Vector2> inThePlane = new List<Vector2>(vertices.Count);


            for (int i = 0; i < vertices.Count; i++)
            {

                inThePlane.Add(new Vector2(Vector3.Dot(perp1, vertices[i]), Vector3.Dot(perp2, vertices[i])));
            }
            return HullArea(ComputeConvexHull(inThePlane));
        }
        public static void PutVerticesInDict(GameObject go, out Dictionary<Transform, Vector3[]> vertices)
        {
            if (!verticesDict.TryGetValue(go, out vertices))
            {
                MeshFilter[] meshes = go.GetComponentsInChildren<MeshFilter>();

                vertices = new Dictionary<Transform, Vector3[]>();

                foreach (MeshFilter m in meshes) {
                    vertices.Add(m.transform, m.mesh.vertices);
                }

                verticesDict.Add(go, vertices);
            }
        }

        public static void RemoveVertices(GameObject go) {
            verticesDict.Remove(go);
        }

        public static float HullArea(IList<Vector2> hull)
        {

            float sum = 0f;
            for (int i = 1; i < hull.Count - 1; i++)
            {
                sum += Geometry.TriangleArea(hull[0], hull[i], hull[i + 1]);
            }
            return sum;
        }
        //From https://gist.github.com/dLopreiato/7fd142d0b9728518552188794b8a750c
        public static IList<Vector2> ComputeConvexHull(List<Vector2> points, bool sortInPlace = false)
        {
            if (!sortInPlace)
                points = new List<Vector2>(points);
            points.Sort((a, b) =>
                a.x == b.x ? a.y.CompareTo(b.y) : (a.x > b.x ? 1 : -1));

            // Importantly, DList provides O(1) insertion at beginning and end
            CircularList<Vector2> hull = new CircularList<Vector2>();
            int L = 0, U = 0; // size of lower and upper hulls

            // Builds a hull such that the output polygon starts at the leftmost Vector2.
            for (int i = points.Count - 1; i >= 0; i--)
            {
                Vector2 p = points[i], p1;

                // build lower hull (at end of output list)
                while (L >= 2 && ((p1 = hull.Last) - (hull[hull.Count - 2])).Cross(p - p1) >= 0)
                {
                    hull.PopLast();
                    L--;
                }
                hull.PushLast(p);
                L++;

                // build upper hull (at beginning of output list)
                while (U >= 2 && ((p1 = hull.First) - (hull[1])).Cross(p - p1) <= 0)
                {
                    hull.PopFirst();
                    U--;
                }
                if (U != 0) // when U=0, share the Vector2 added above
                    hull.PushFirst(p);
                U++;
                Debug.Assert(U + L == hull.Count + 1);
            }
            hull.PopLast();
            return hull;
        }

        private class CircularList<T> : List<T>
        {
            public T Last
            {
                get
                {
                    return this[Count - 1];
                }
                set
                {
                    this[Count - 1] = value;
                }
            }

            public T First
            {
                get
                {
                    return this[0];
                }
                set
                {
                    this[0] = value;
                }
            }

            public void PushLast(T obj)
            {
                Add(obj);
            }

            public T PopLast()
            {
                T retVal = this[Count - 1];
                RemoveAt(Count - 1);
                return retVal;
            }

            public void PushFirst(T obj)
            {
                Insert(0, obj);
            }

            public T PopFirst()
            {
                T retVal = this[0];
                RemoveAt(0);
                return retVal;
            }
        }
    }
}