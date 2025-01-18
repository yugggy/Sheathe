using System;
using UnityEngine;

#if (UNITY_EDITOR) && (!_NODEBUG)
[UnityEditor.CustomPropertyDrawer(typeof(LabelAttribute))]
public class LabelDrawer : UnityEditor.PropertyDrawer
{
	/// <summary>OnGUI</summary>
	public override void OnGUI(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label)
	{
		var newLabel = attribute as LabelAttribute;
		UnityEditor.EditorGUI.PropertyField(position, property, new UnityEngine.GUIContent(newLabel.Value), true);
	}

	/// <summary>GetPropertyHeight</summary>
	public override float GetPropertyHeight(UnityEditor.SerializedProperty property, UnityEngine.GUIContent label)
	{
		return UnityEditor.EditorGUI.GetPropertyHeight(property, true);
	}
}
#endif
