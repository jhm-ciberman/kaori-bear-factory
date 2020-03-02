using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(OptimizeMesh))]
public class LevelScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        OptimizeMesh myTarget = (OptimizeMesh)target;

        if (!EditorApplication.isPlaying)
        {
            if (GUILayout.Button("Optimize Mesh!"))
            {
                myTarget.DecimateMesh();
            }

            if (GUILayout.Button("Save Mesh!"))
            {
                myTarget.SaveMesh();
            }
        }
    }
}