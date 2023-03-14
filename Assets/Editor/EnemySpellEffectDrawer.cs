using Assets.Combat.SpellEffects;
using UnityEditor;
using UnityEngine;
using static PlasticPipe.PlasticProtocol.Messages.NegotiationCommand;

// IngredientDrawer
[CustomPropertyDrawer(typeof(EnemySpellEffect))]
public class EnemySpellEffectDrawer : PropertyDrawer
{

    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.height *= 2;
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var pathRect = new Rect(position.x, position.y, position.width, position.height/2f);
        var strengthRect = new Rect(position.x, position.y + position.height / 2f, position.width, position.height/2f);
        // var unitRect = new Rect(position.x + 35, position.y, 50, position.height);
        // var nameRect = new Rect(position.x + 90, position.y, position.width - 90, position.height);

        // Draw fields - pass GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(pathRect, property.FindPropertyRelative("path"), new GUIContent("Path"));
        EditorGUI.PropertyField(strengthRect, property.FindPropertyRelative("strength"), new GUIContent("Strength"));

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}