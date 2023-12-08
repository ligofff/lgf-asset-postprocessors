using UnityEditor;
using UnityEngine;
using EasyAssetPipeline;
using System;
using System.Linq;
using System.Collections.Generic;

[CustomEditor(typeof(EasyAssetPostprocessorsSetup))]
public class EasyAssetPostprocessorsSetupEditor : Editor
{
    private List<Type> _postprocessorTypes;
    private string[] _postprocessorNames;
    private int _selectedIndex = 0;

    private void OnEnable()
    {
        _postprocessorTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IEasyAssetPostprocessor).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract && type.IsSerializable)
            .ToList();

        _postprocessorNames = _postprocessorTypes.Select(type => type.Name).ToArray();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EasyAssetPostprocessorsSetup setup = (EasyAssetPostprocessorsSetup)target;

        _selectedIndex = EditorGUILayout.Popup("Add Postprocessor", _selectedIndex, _postprocessorNames);
        
        if (GUILayout.Button("Add Postprocessor"))
        {
            var postprocessor = (IEasyAssetPostprocessor)Activator.CreateInstance(_postprocessorTypes[_selectedIndex]);
            setup.AddPostprocessor(postprocessor);
        }
    }
}