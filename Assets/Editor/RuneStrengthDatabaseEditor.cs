
using System;
using Assets.Inventory.Runes;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RuneStrengthDatabase))]
public class RuneStrengthDatabaseEditor : Editor
{
    SerializedProperty runeStrengthCoefficients;

    void OnEnable()
    {
        runeStrengthCoefficients = serializedObject.FindProperty("runeStrengthCoefficients");
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Formula is : (a) * ((b^(rank-1))* (1 + (c*quality / 100))");
        serializedObject.Update();
        int numRuneTypes = Enum.GetNames(typeof(RuneType)).Length;
        runeStrengthCoefficients.arraySize = numRuneTypes;
        for (int i = 0; i < numRuneTypes; i++)
        {
            SerializedProperty runeStrengthCoefficient = runeStrengthCoefficients.GetArrayElementAtIndex(i);
            string typeName = Enum.GetNames(typeof(RuneType))[i];
            EditorGUILayout.PropertyField(runeStrengthCoefficient, new GUIContent(typeName));
        }
        serializedObject.ApplyModifiedProperties();
    }
}
