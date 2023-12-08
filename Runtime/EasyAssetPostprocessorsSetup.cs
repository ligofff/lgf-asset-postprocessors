using System.Collections.Generic;
using UnityEngine;

namespace EasyAssetPipeline
{
    [CreateAssetMenu(fileName = "EasyPostprocessorsSetup", menuName = "Ligofff/Easy Asset Postprocessors Setup")]
    public class EasyAssetPostprocessorsSetup : ScriptableObject
    {
        [SerializeReference, SerializeField]
        private List<IEasyAssetPostprocessor> postprocessors = new List<IEasyAssetPostprocessor>();
        
        public IEnumerable<IEasyAssetPostprocessor> Postprocessors => postprocessors;
        
        public void AddPostprocessor(IEasyAssetPostprocessor postprocessor)
        {
            postprocessors.Add(postprocessor);
        }
    }
}