#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using CustomMath;

[CustomEditor(typeof(Vec3))]
public class VecEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    { 
        SerializedProperty xProp = property.FindPropertyRelative("x");
        SerializedProperty yProp = property.FindPropertyRelative("y");
        SerializedProperty zProp = property.FindPropertyRelative("z");

        EditorGUILayout.LabelField("Test");

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(xProp, new GUIContent("X"));
        EditorGUILayout.PropertyField(yProp, new GUIContent("Y"));
        EditorGUILayout.PropertyField(zProp, new GUIContent("Z"));
        EditorGUILayout.EndHorizontal();

        property.serializedObject.ApplyModifiedProperties();
    }
}

#endif
