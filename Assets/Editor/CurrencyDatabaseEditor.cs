
using System;
using Assets.Currency;
using Assets.Equipment;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CurrencyDatabase))]
public class CurrencyDatabaseEditor : Editor
{
    SerializedProperty currencyInfo;

    void OnEnable()
    {
        currencyInfo = serializedObject.FindProperty("currencyInfo");
    }
    public override void OnInspectorGUI()
    {
        string[] currencyNames = Enum.GetNames(typeof(CurrencyType));
        serializedObject.Update();
        currencyInfo.arraySize = currencyNames.Length;
        for (int i = 0; i < currencyNames.Length; i++)
        {
            EditorGUILayout.LabelField(currencyNames[i]);
            SerializedProperty currencyInfoEntry = currencyInfo.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(currencyInfoEntry);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
