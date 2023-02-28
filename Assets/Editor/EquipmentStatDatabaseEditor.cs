
using System;
using Assets.Inventory.Runes;
using Assets.Equipment;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EquipmentStatDatabase))]
public class EquipmentStatDatabaseEditor : Editor
{
    SerializedProperty equipmentStatData;

    void OnEnable()
    {
        equipmentStatData = serializedObject.FindProperty("equipmentStatData");
    }
    public override void OnInspectorGUI()
    {
        string[] slotNames = Enum.GetNames(typeof(EquipmentSlot));
        string[] setNames = Enum.GetNames(typeof(EquipmentSet));
        int numSlots = slotNames.Length;
        int numSets = setNames.Length;
        serializedObject.Update();
        equipmentStatData.arraySize = numSlots * numSets;
        int i = 0;
        foreach (string setName in setNames)
        {
            EditorGUILayout.LabelField(setName + " Set");
            foreach (string slotName in slotNames)
            {
                SerializedProperty equipmentStatDataEntry = equipmentStatData.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(equipmentStatDataEntry, new GUIContent(slotName));
                i++;
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}
