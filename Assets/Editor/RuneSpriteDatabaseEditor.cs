
using System;
using Assets.Inventory.Runes;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RuneSpriteDatabase))]
public class RuneSpriteDatabaseEditor : Editor
{
    SerializedProperty rankShapeSprites;
    SerializedProperty symbolSprites;

    void OnEnable()
    {
        rankShapeSprites = serializedObject.FindProperty("rankShapeSprites");
        symbolSprites = serializedObject.FindProperty("symbolSprites");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(rankShapeSprites);
        int numRuneTypes = Enum.GetNames(typeof(RuneType)).Length;
        symbolSprites.arraySize = numRuneTypes;
        for (int i = 0; i < numRuneTypes; i++)
        {
            SerializedProperty symbolSprite = symbolSprites.GetArrayElementAtIndex(i);
            string typeName = Enum.GetNames(typeof(RuneType))[i];
            EditorGUILayout.PropertyField(symbolSprite, new GUIContent(typeName));
        }
        serializedObject.ApplyModifiedProperties();
    }
}
