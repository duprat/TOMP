using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LevelGenerator))]
public class MapGeneratorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        
        LevelGenerator levelGen = (LevelGenerator)target;

        if (DrawDefaultInspector())
        {
            levelGen.generate();
        }

        if (GUILayout.Button("Generate"))
        {
            levelGen.generate();
        }

        if (GUILayout.Button("Destroy"))
        {
            levelGen.Destroy();
        }


    }
}