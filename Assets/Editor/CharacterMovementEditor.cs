using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CharacterMovement))]
public class CharacterMovementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CharacterMovement characterMovement = (CharacterMovement)target;
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.FloatField("Actual Speed", characterMovement.ActualSpeed);
        EditorGUI.EndDisabledGroup();
    }
}