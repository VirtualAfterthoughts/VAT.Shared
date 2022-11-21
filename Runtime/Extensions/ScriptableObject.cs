using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VAT.Shared.Extensions {
    public static partial class ScriptableObjectExtensions {
#if UNITY_EDITOR
        public static void ForceSerialize(this ScriptableObject obj) {
            EditorUtility.SetDirty(obj);
            AssetDatabase.SaveAssets();
        }
#endif
    }
}
