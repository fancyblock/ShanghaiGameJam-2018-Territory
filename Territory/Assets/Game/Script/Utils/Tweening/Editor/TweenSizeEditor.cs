//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2016 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TweenSize))]
public class TweenSizeEditor : UITweenerEditor
{
	public override void OnInspectorGUI ()
	{
		GUILayout.Space(6f);
		NGUIEditorTools.SetLabelWidth(120f);

        TweenSize tw = target as TweenSize;
		GUI.changed = false;

		Vector2 from = EditorGUILayout.Vector3Field("From", tw.from);
		Vector2 to = EditorGUILayout.Vector3Field("To", tw.to);

		if (GUI.changed)
		{
			NGUIEditorTools.RegisterUndo("Tween Change", tw);
			tw.from = from;
			tw.to = to;
		}

		DrawCommonProperties();
	}
}
