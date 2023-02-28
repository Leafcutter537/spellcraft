
using System;
using Assets.Inventory.Runes;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RuneValueDatabase))]
public class RuneValueDatabaseEditor : Editor
{
    SerializedProperty runeValueCoefficients;

    void OnEnable()
    {
        runeValueCoefficients = serializedObject.FindProperty("runeValueCoefficients");
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Formula is : (a) * ((b^(rank-1))* (c^(quality-1))");
        serializedObject.Update();
        int numRuneTypes = Enum.GetNames(typeof(RuneType)).Length;
        runeValueCoefficients.arraySize = numRuneTypes;
        for (int i = 0; i < numRuneTypes; i++)
        {
            SerializedProperty runeValueCoefficient = runeValueCoefficients.GetArrayElementAtIndex(i);
            string typeName = Enum.GetNames(typeof(RuneType))[i];
            EditorGUILayout.PropertyField(runeValueCoefficient, new GUIContent(typeName));
        }
        serializedObject.ApplyModifiedProperties();
    }
}
