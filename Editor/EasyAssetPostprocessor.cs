using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EasyAssetPipeline
{
    public class EasyAssetPostprocessor : AssetPostprocessor
    {
        private static List<IEasyAssetPostprocessor> _postprocessors;

        private static EasyAssetPostprocessorsSetup _settings;
        
        private static bool _initialized;

        private static void Initialize()
        {
            var settings = AssetDatabase.FindAssets($"t:{nameof(EasyAssetPostprocessorsSetup)}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<EasyAssetPostprocessorsSetup>)
                .ToList();

            if (settings.Count > 1)
            {
                Debug.LogWarning($"More than one Easy postprocessors settings found in folders!\n{string.Join("\n", settings.Select(AssetDatabase.GetAssetPath))}");
                Debug.LogWarning("Using the first one...");
            }
            
            _settings = settings.FirstOrDefault();

            if (_settings == null)
            {
                Debug.LogWarning("Easy postprocessors settings is not found in any folder!");
                return;
            }
            
            if (_settings.Postprocessors == null)
            {
                Debug.LogWarning("Easy postprocessors settings is null!");
                return;
            }

            _initialized = true;
        }

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            if (!_initialized) Initialize();

            if (_settings is null)
            {
                Debug.LogWarning("Easy postprocessors settings is not found!");
                return;
            }

            if (didDomainReload)
            {
                Debug.Log($"Asset processing during domain reload...");
            }

            foreach (var importedAsset in importedAssets)
            {
                foreach (var easyAssetPostprocessor in _settings.Postprocessors)
                {
                    if (easyAssetPostprocessor.TryProcessAsset(importedAsset))
                    {
                        Debug.Log($"Asset {importedAsset} was processed by {easyAssetPostprocessor.GetType().Name}");
                    }
                }
            }

            foreach (var deletedAsset in deletedAssets)
            {
                foreach (var easyAssetPostprocessor in _settings.Postprocessors)
                {
                    if (easyAssetPostprocessor.TryProcessRemovedAsset(deletedAsset))
                    {
                        Debug.Log($"Removed Asset {deletedAsset} was processed by {easyAssetPostprocessor.GetType().Name}");
                    }
                }
            }

            AssetDatabase.SaveAssets();
        }
    }
}