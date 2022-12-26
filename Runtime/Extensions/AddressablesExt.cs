#if USE_ADDRESSABLES
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
    /// <summary>
    /// Extension methods for AddressableAssets.
    /// </summary>
    public static partial class AddressableExtensions {
#if UNITY_EDITOR
        /// <summary>
        /// Marks this asset as addressable.
        /// </summary>
        /// <param name="asset">The asset.</param>
        /// <returns>The reference to the asset.</returns>
        public static AssetReference MarkAsAddressable(this Object asset) {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            string assetPath = AssetDatabase.GetAssetPath(asset);
            string assetGUID = AssetDatabase.AssetPathToGUID(assetPath);
            var reference = settings.CreateAssetReference(assetGUID);
            
            return reference;
        }

        /// <summary>
        /// Marks this asset as addressable, sets the label and moves it to the desired group.
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="group"></param>
        /// <param name="label"></param>
        /// <param name="isCaseSensitive"></param>
        /// <returns></returns>
        public static AssetReference MarkAsAddressable(this Object asset, string group, string address, string label, bool isCaseSensitive = false) {
            AssetReference assetRef;

            if (!string.IsNullOrWhiteSpace(group))
                assetRef = MarkAsAddressable(asset, group, isCaseSensitive);
            else
                assetRef = MarkAsAddressable(asset);

            // Get info
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            string assetPath = AssetDatabase.GetAssetPath(asset);
            string assetGUID = AssetDatabase.AssetPathToGUID(assetPath);

            var entry = settings.FindAssetEntry(assetGUID);

            if (!string.IsNullOrWhiteSpace(address))
                entry.SetAddress(address);
            if (!string.IsNullOrWhiteSpace(label))
                entry.SetLabel(label, true, true);
            
            return assetRef;
        }

        /// <summary>
        /// Marks this asset as addressable and moves it to the desired group.
        /// </summary>
        /// <param name="asset">The asset.</param>
        /// <param name="group">The group name.</param>
        /// <param name="isCaseSensitive">Whether or not the name is case sensitive.</param>
        /// <returns>The reference to the asset.</returns>
        public static AssetReference MarkAsAddressable(this Object asset, string group, bool isCaseSensitive = false) {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            string assetPath = AssetDatabase.GetAssetPath(asset);
            string assetGUID = AssetDatabase.AssetPathToGUID(assetPath);
            var reference = settings.CreateAssetReference(assetGUID);
            string originalGroupName = group;

            if (!isCaseSensitive)
                group = group.ToLower();

            bool hasGroup = false;

            foreach (var assetGroup in settings.groups) {
                string name = isCaseSensitive ? assetGroup.Name : assetGroup.Name.ToLower();

                // Check the name
                if (name == group) {
                    settings.CreateOrMoveEntry(assetGUID, assetGroup);
                    hasGroup = true;
                    break;
                }
            }

            // Create new group
            if (!hasGroup) {
                var newGroup = settings.CreateGroup(originalGroupName, false, false, false, new List<AddressableAssetGroupSchema>());
                settings.CreateOrMoveEntry(assetGUID, newGroup);
            }

            return reference;
        }
#endif
    }
}
#endif