using UnityEngine;
using VAT.Shared.Data;
using static UnityEngine.GraphicsBuffer;

namespace VAT.Shared.Extensions {
    /// <summary>
    /// Extension methods for drawing gizmos.
    /// </summary>
    public static class GizmosExtensions {
        /// <summary>
        /// Sets the Gizmos color to the desired color, and outputs the original.
        /// </summary>
        /// <param name="color">The new color.</param>
        /// <param name="original">The original color.</param>
        public static void SetTempColor(in Color color, out Color original) {
            original = Gizmos.color;
            Gizmos.color = color;
        }

        /// <summary>
        /// Sets the Gizmos matrix to the desired matrix, and outputs the original.
        /// </summary>
        /// <param name="matrix">The new matrix.</param>
        /// <param name="original">The original matrix.</param>
        public static void SetTempMatrix(in Matrix4x4 matrix, out Matrix4x4 original) {
            original = Gizmos.matrix;
            Gizmos.matrix = matrix;
        }

        /// <summary>
        /// Draws the Gizmos of every mesh on this GameObject.
        /// </summary>
        /// <param name="go">The GameObject to draw.</param>
        /// <param name="transform">The target position and rotations.</param>
        /// <param name="isWireframe">Should this draw as a wireframe?</param>
        public static void DrawGameObject(this GameObject go, Transform transform, bool isWireframe = true)
            => go.DrawGameObject(SimpleTransform.Create(transform), isWireframe);

        /// <summary>
        /// Draws the Gizmos of every mesh on this GameObject.
        /// </summary>
        /// <param name="go">The GameObject to draw.</param>
        /// <param name="transform">The target position and rotations.</param>
        /// <param name="color">The color to draw.</param>
        /// <param name="isWireframe">Should this draw as a wireframe?</param>
        public static void DrawGameObject(this GameObject go, Transform transform, Color color, bool isWireframe = true) 
            => go.DrawGameObject(SimpleTransform.Create(transform), color, isWireframe);

        /// <summary>
        /// Draws the Gizmos of every mesh on this GameObject.
        /// </summary>
        /// <param name="go">The GameObject to draw.</param>
        /// <param name="transform">The target position and rotations.</param>
        /// <param name="isWireframe">Should this draw as a wireframe?</param>
        public static void DrawGameObject(this GameObject go, SimpleTransform transform, bool isWireframe = true) {
            // Draw all of the object's meshes
            Internal_DrawMeshRenderers(go, transform, isWireframe);
            Internal_DrawSkinnedMeshes(go, transform, isWireframe);
        }

        /// <summary>
        /// Draws the Gizmos of every mesh on this GameObject.
        /// </summary>
        /// <param name="go">The GameObject to draw.</param>
        /// <param name="transform">The target position and rotations.</param>
        /// <param name="color">The color to draw.</param>
        /// <param name="isWireframe">Should this draw as a wireframe?</param>
        public static void DrawGameObject(this GameObject go, SimpleTransform transform, Color color, bool isWireframe = true) {
            // Set the color to the target color
            SetTempColor(color, out var original);

            // Draw the meshes
            DrawGameObject(go, transform, isWireframe);

            // Revert the color for the gizmos instance
            Gizmos.color = original;
        }

        private static void Internal_DrawMeshRenderers(GameObject go, SimpleTransform transform, bool isWireframe = true) {
            if (Application.isPlaying)
                return;

            MeshFilter[] meshes = go.GetComponentsInChildren<MeshFilter>();
            Matrix4x4 newMatrix = transform.localToWorldMatrix;
            Matrix4x4 original = Gizmos.matrix;

            // Loop through all mesh filters and draw their meshes
            for (int i = 0; i < meshes.Length; i++) {
                // Make sure this filter has an assigned mesh
                MeshFilter mesh = meshes[i];
                if (!mesh.sharedMesh) 
                    continue;

                // Get offset matrix
                Transform root = mesh.transform.root;
                Vector3 position = mesh.transform.position;
                Quaternion rotation = mesh.transform.rotation;
                Vector3 scale = mesh.transform.lossyScale;

                position -= root.position;
                rotation = Quaternion.Inverse(root.rotation) * rotation;

                Matrix4x4 meshMatrix = Matrix4x4.TRS(position, rotation, scale);

                // Draw the mesh
                Gizmos.matrix = newMatrix * meshMatrix;

                if (isWireframe)
                    Gizmos.DrawWireMesh(mesh.sharedMesh);
                else
                    Gizmos.DrawMesh(mesh.sharedMesh);
            }

            Gizmos.matrix = original;
        }

        private static void Internal_DrawSkinnedMeshes(GameObject go, SimpleTransform transform, bool isWireframe = true) {
            if (Application.isPlaying)
                return;

            SkinnedMeshRenderer[] meshes = go.GetComponentsInChildren<SkinnedMeshRenderer>();
            Matrix4x4 newMatrix = transform.localToWorldMatrix;
            Matrix4x4 original = Gizmos.matrix;

            // Loop through all of the skinned mesh renderers
            for (int i = 0; i < meshes.Length; i++) {
                // Make sure the skinned mesh renderer has an assigned mesh
                SkinnedMeshRenderer mesh = meshes[i];
                if (!mesh.sharedMesh) 
                    continue;

                // Get offset matrix
                Transform root = mesh.transform.root;
                Vector3 position = mesh.transform.position;
                Quaternion rotation = mesh.transform.rotation;
                Vector3 scale = mesh.transform.lossyScale;

                position -= root.position;
                rotation = Quaternion.Inverse(root.rotation) * rotation;

                Matrix4x4 meshMatrix = Matrix4x4.TRS(position, rotation, scale);

                // Draw the mesh
                Gizmos.matrix = newMatrix * meshMatrix;
                if (isWireframe)
                    Gizmos.DrawWireMesh(mesh.sharedMesh);
                else
                    Gizmos.DrawMesh(mesh.sharedMesh);
            }

            Gizmos.matrix = original;
        }
    }
}
