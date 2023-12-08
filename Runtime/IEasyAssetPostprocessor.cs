namespace EasyAssetPipeline
{
    public interface IEasyAssetPostprocessor
    {
        public bool TryProcessAsset(string importedAssetPath);
        public bool TryProcessRemovedAsset(string importedAssetPath);
    }
}