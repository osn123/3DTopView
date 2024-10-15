using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TableData))]
public class TableDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TableData tableData = (TableData)target;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("ID", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Name", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Value", EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();

        for (int i = 0; i < tableData.rows.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();
            tableData.rows[i].id = EditorGUILayout.IntField(tableData.rows[i].id);
            tableData.rows[i].name = EditorGUILayout.TextField(tableData.rows[i].name);
            tableData.rows[i].value = EditorGUILayout.FloatField(tableData.rows[i].value);
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Row"))
        {
            ArrayUtility.Add(ref tableData.rows, CreateInstance<RowData>());
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
