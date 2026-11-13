#if UNITY_EDITOR
using System;

using UnityEditor;

using UnityEngine;

namespace VAT.Shared.Utilities
{
    /// <summary>
    /// A disposable GameObject preview scene. This class only exists in the editor.
    /// </summary>
    public class TempGameObjectScene : IDisposable
    {
        private PreviewRenderUtility _previewScene = null;

        private GameObject _instance = null;

        private bool _disposed = false;

        private TempGameObjectScene(GameObject asset)
        {
            _previewScene = new();

            _instance = GameObject.Instantiate(asset, Vector3.zero, Quaternion.identity);
            _previewScene.AddSingleGO(_instance);
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            GameObject.DestroyImmediate(_instance);
            _previewScene.Cleanup();

            _instance = null;
            _previewScene = null;

            _disposed = true;
        }

        /// <summary>
        /// Instantiates a GameObject into a temporary preview scene.
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static TempGameObjectScene Create(GameObject asset, out GameObject instance)
        {
            var scene = new TempGameObjectScene(asset);

            instance = scene._instance;

            return scene;
        }
    }
}
#endif