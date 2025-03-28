#if UNITY_EDITOR
using System;

using UnityEditor;
using UnityEngine;

namespace VAT.Shared.Utilities
{
    public class TempGameObjectScene : IDisposable
    {
        private PreviewRenderUtility _previewScene = null;

        private GameObject _instance = null;

        private TempGameObjectScene(GameObject asset)
        {
            _previewScene = new();

            _instance = GameObject.Instantiate(asset, Vector3.zero, Quaternion.identity);
            _previewScene.AddSingleGO(_instance);
        }

        private bool _disposed = false;

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

        public static TempGameObjectScene Create(GameObject asset, out GameObject instance)
        {
            var scene = new TempGameObjectScene(asset);

            instance = scene._instance;

            return scene;
        }
    }
}
#endif