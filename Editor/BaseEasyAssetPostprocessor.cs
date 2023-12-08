using System;
using UnityEditor;
using Object = UnityEngine.Object;

namespace EasyAssetPipeline
{
    [Serializable]
    public abstract class BaseEasyAssetPostprocessor : IEasyAssetPostprocessor
    {
        public bool isEnabled = true;
        
        public bool onlyForPath = false;
        public string path = "path not set";

        protected abstract Type AssetType { get; }
        
        private bool Filter(Object asset, string assetPath)
        {
            if (!isEnabled) return false;

            if (!PathOnlyFilter(assetPath)) return false;

            if (!AssetType.IsAssignableFrom(asset.GetType())) return false;

            return InternalFilter(asset, assetPath);
        }

        private bool PathOnlyFilter(string assetPath)
        {
            if (onlyForPath && !assetPath.Contains(path)) return false;

            return true;
        }

        public bool TryProcessAsset(string importedAssetPath)
        {
            var asset = GetObjectFromPath(importedAssetPath);
            
            if (!Filter(asset, importedAssetPath)) return false;

            return InternalProcessAsset(asset, importedAssetPath);
        }

        public bool TryProcessRemovedAsset(string removedAssetPath)
        {
            if (!PathOnlyFilter(removedAssetPath)) return false;

            return InternalAssetRemoved(removedAssetPath);
        }

        protected Object GetObjectFromPath(string path)
        {
            return AssetDatabase.LoadAssetAtPath<Object>(path);
        }

        protected abstract bool InternalProcessAsset(Object asset, string assetPath);

        protected virtual bool InternalAssetRemoved(string assetPath)
        {
            return true;
        }

        protected abstract bool InternalFilter(Object asset, string assetPath);
    }
}