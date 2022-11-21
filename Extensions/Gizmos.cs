using UnityEngine;

namespace VAT.Shared.Extensions {
    public static class GizmosExtensions {
        /// <summary>
        /// Sets the gizmos color to the new color and returns the original.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="defaultColor"></param>
        public static void SetTempColor(Color color, out Color defaultColor)
        {
            defaultColor = Gizmos.color;
            Gizmos.color = color;
        }

        /// <summary>
        /// Draws all meshes of a GameObject at a target transform.
        /// </summary>
        /// <param name="go"></param>
        /// <param name="target"></param>
        /// <param name="color"></param>
        /// <param name="wireFrame"></param>
        public static void DrawGameObject(GameObject go, Transform target, Color color, bool wireFrame = true)
        {
            // Set the color to the target color
            SetTempColor(color, out Color defaultColor);
            // Draw all of the object's meshes
            DrawMeshRenderers(go, target, wireFrame);
            DrawSkinnedMeshes(go, target, wireFrame);
            // Revert the color for the gizmos instance
            Gizmos.color = defaultColor;
        }

        /// <summary>
        /// Draws all meshes of a GameObject at a target transform.
        /// </summary>
        /// <param name="go"></param>
        /// <param name="target"></param>
        /// <param name="wireFrame"></param>
        public static void DrawGameObject(GameObject go, Transform target, bool wireFrame = true) => DrawGameObject(go, target, Color.white, wireFrame);

        /// <summary>
        /// Draws only the MeshRenderers of a GameObject at a target transform.
        /// </summary>
        /// <param name="go"></param>
        /// <param name="target"></param>
        /// <param name="wireFrame"></param>
        public static void DrawMeshRenderers(GameObject go, Transform target, bool wireFrame = true)
        {
            if (Application.isPlaying)
                return;

            MeshFilter[] meshes = go.GetComponentsInChildren<MeshFilter>();
            Matrix4x4 self = target.localToWorldMatrix;
            Matrix4x4 old = Gizmos.matrix;

            // Draw all of the mesh filters
            for (int i = 0; i < meshes.Length; i++)
            {
                MeshFilter mesh = meshes[i];
                if (!mesh.sharedMesh) continue;

                // Draw the mesh
                Gizmos.matrix = self * mesh.transform.localToWorldMatrix;

                if (wireFrame)
                    Gizmos.DrawWireMesh(mesh.sharedMesh);
                else
                    Gizmos.DrawMesh(mesh.sharedMesh);
            }

            Gizmos.matrix = old;
        }

        /// <summary>
        /// Draws only the SkinnedMeshRenderers of a GameObject at a target transform.
        /// </summary>
        /// <param name="go"></param>
        /// <param name="target"></param>
        /// <param name="wireFrame"></param>
        public static void DrawSkinnedMeshes(GameObject go, Transform target, bool wireFrame = true)
        {
            if (Application.isPlaying)
                return;

            SkinnedMeshRenderer[] meshes = go.GetComponentsInChildren<SkinnedMeshRenderer>();
            Matrix4x4 self = target.localToWorldMatrix;
            Matrix4x4 old = Gizmos.matrix;

            // Draw all of the mesh filters
            for (int i = 0; i < meshes.Length; i++)
            {
                SkinnedMeshRenderer mesh = meshes[i];
                if (!mesh.sharedMesh) continue;

                // Draw the mesh
                Gizmos.matrix = self * mesh.transform.localToWorldMatrix;
                if (wireFrame)
                    Gizmos.DrawWireMesh(mesh.sharedMesh);
                else
                    Gizmos.DrawMesh(mesh.sharedMesh);
            }

            Gizmos.matrix = old;
        }
    }
}
