
using System;
using Assets.Inventory.Runes;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RuneManaCostDatabase))]
public class RuneManaCostDatabaseEditor : Editor
{
    SerializedProperty runeManaCostCoefficients;

    void OnEnable()
    {
        runeManaCostCoefficients = serializedObject.FindProperty("runeManaCostCoefficients");
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Formula is : (a) * ((b^(rank-1))* (1 + (c*quality / 20))");
        serializedObject.Update();
        int numRuneTypes = Enum.GetNames(typeof(RuneType)).Length;
        runeManaCostCoefficients.arraySize = numRuneTypes;
        for (int i = 0; i < numRuneTypes; i++)
        {
            SerializedProperty runeManaCostCoefficient = runeManaCostCoefficients.GetArrayElementAtIndex(i);
            string typeName = Enum.GetNames(typeof(RuneType))[i];
            EditorGUILayout.PropertyField(runeManaCostCoefficient, new GUIContent(typeName));
        }
        serializedObject.ApplyModifiedProperties();
    }
}
