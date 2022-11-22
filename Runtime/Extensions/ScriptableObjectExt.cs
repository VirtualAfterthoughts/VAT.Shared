using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VAT.Shared.Extensions {
    /// <summary>
    /// Extension methods for ScriptableObjects.
    /// </summary>
    public static partial class ScriptableObjectExtensions {
#if UNITY_EDITOR
        /// <summary>
        /// Forces Unity to reserialize the ScriptableObject and save it.
        /// <para>Do not call this too many times in succession, as it may pause the Editor for a long time!</para>
        /// </summary>
        /// <param name="obj">The ScriptableObject asset.</param>
        public static void ForceSerialize(this ScriptableObject obj) {
            EditorUtility.SetDirty(obj);
            AssetDatabase.SaveAssets();
        }
#endif
    }
}
