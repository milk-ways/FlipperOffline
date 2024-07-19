using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DirectionJoystick))]
public class DirectionJoystickEditor : JoystickEditor
{
    private SerializedProperty inputThreshold;
    private SerializedProperty moveRestriction;

    protected override void OnEnable()
    {
        base.OnEnable();
        inputThreshold = serializedObject.FindProperty("inputThreshold");
        moveRestriction = serializedObject.FindProperty("moveRestriction");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (background != null)
        {
            RectTransform backgroundRect = (RectTransform)background.objectReferenceValue;
            backgroundRect.anchorMax = Vector2.zero;
            backgroundRect.anchorMin = Vector2.zero;
            backgroundRect.pivot = center;
        }
    }

    protected override void DrawValues()
    {
        base.DrawValues();
        EditorGUILayout.PropertyField(inputThreshold, new GUIContent("Input Threshold", "The restriction."));
    }

    protected override void DrawComponents()
    {
        base.DrawComponents();
        EditorGUILayout.ObjectField(moveRestriction, new GUIContent("Move Restriction", "The restriction."));
    }
}