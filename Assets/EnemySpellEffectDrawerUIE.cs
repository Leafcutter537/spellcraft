using UnityEditor;
using Assets.Combat.SpellEffects;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.Diagnostics;

// IngredientDrawerUIE
//[CustomPropertyDrawer(typeof(EnemySpellEffect))]
public class EnemySpellEffectDrawerUIE : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        // Create property container element.
        var container = new VisualElement();




        // Create property fields.
        var spellEffectTypeField = new PropertyField(property.FindPropertyRelative("spellEffectType"));
        var statField = new PropertyField(property.FindPropertyRelative("stat"));
        var elementField = new PropertyField(property.FindPropertyRelative("element"));
        var pathField = new PropertyField(property.FindPropertyRelative("path"));
        var strengthField = new PropertyField(property.FindPropertyRelative("strength"));
        var durationField = new PropertyField(property.FindPropertyRelative("duration"));



        // Add fields to the container.
        property.serializedObject.Update();
        container.Add(spellEffectTypeField);
        property.serializedObject.ApplyModifiedProperties();
        SerializedProperty typeProperty = property.FindPropertyRelative("spellEffectType");
        SpellEffectType type = (SpellEffectType)typeProperty.intValue;
        if (type == SpellEffectType.Buff)
            container.Add(statField);
        container.Add(elementField);
        container.Add(pathField);
        container.Add(strengthField);
        container.Add(durationField);

        return container;
    }
}