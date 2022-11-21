using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AddressableAssets;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
#endif

namespace VAT.Shared.Extensions {
    public static partial class AddressableExtensions {
#if UNITY_EDITOR
        /// <summary>
        /// Marks this asset as addressable.
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public static AssetReference MarkAsAddressable(this Object asset) {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            string assetPath = AssetDatabase.GetAssetPath(asset);
            string assetGUID = AssetDatabase.AssetPathToGUID(assetPath);
            var reference = settings.CreateAssetReference(assetGUID);
            
            return reference;
        }

        /// <summary>
        /// Marks this asset as addressable and moves it to the desired group.
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="group"></param>
        /// <param name="isCaseSensitive"></param>
        /// <returns></returns>
        public static AssetReference MarkAsAddressable(this Object asset, string group, bool isCaseSensitive = false) {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            string assetPath = AssetDatabase.GetAssetPath(asset);
            string assetGUID = AssetDatabase.AssetPathToGUID(assetPath);
            var reference = settings.CreateAssetReference(assetGUID);

            if (!isCaseSensitive)
                group = group.ToLower();

            foreach (var assetGroup in settings.groups) {
                string name = isCaseSensitive ? assetGroup.Name : assetGroup.Name.ToLower();

                // Check the name
                if (name == group) {
                    settings.CreateOrMoveEntry(assetGUID, assetGroup);
                }
            }

            return reference;
        }
#endif
    }
}