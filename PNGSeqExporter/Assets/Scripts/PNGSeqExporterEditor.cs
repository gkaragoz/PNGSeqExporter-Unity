using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PNGSeqExporter))]
public class PNGSeqExporterEditor : Editor {

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        PNGSeqExporter exporter = (PNGSeqExporter)target;

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Record")) {
            #if UNITY_EDITOR
                if (EditorApplication.isPlaying)
            #endif
                    exporter.Action();
        }
    }

}