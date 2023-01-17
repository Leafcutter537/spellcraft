
using System;
using Assets.Inventory.Runes;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RuneDescriptionDatabase))]
public class RuneDescriptionDatabaseEditor : Editor
{
    SerializedProperty runeDescriptions;

    void OnEnable()
    {
        runeDescriptions = serializedObject.FindProperty("runeDescriptions");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        int numRuneTypes = Enum.GetNames(typeof(RuneType)).Length;
        runeDescriptions.arraySize = numRuneTypes;
        for (int i = 0; i < numRuneTypes; i++)
        {
            SerializedProperty runeDescription = runeDescriptions.GetArrayElementAtIndex(i);
            string typeName = Enum.GetNames(typeof(RuneType))[i];
            EditorGUILayout.PropertyField(runeDescription, new GUIContent(typeName));
        }
        serializedObject.ApplyModifiedProperties();
    }
}
