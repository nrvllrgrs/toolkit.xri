using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.XR.Interaction.Toolkit;
using ToolkitEngine.XR;

namespace ToolkitEditor.XR
{
    [CustomEditor(typeof(XRHolsterInteractor))]
    public class XRHolsterInteractorEditor : XRSocketInteractorEditor
    {
		#region Fields

		protected SerializedProperty m_dropBehavior;
		protected SerializedProperty m_autoholsterDelay;

		protected SerializedProperty m_onHolstered;
		protected SerializedProperty m_onUnholstered;
		protected SerializedProperty m_onChanged;

		#endregion

		#region Methods

		protected override void OnEnable()
		{
			base.OnEnable();

			m_dropBehavior = serializedObject.FindProperty(nameof(m_dropBehavior));
			m_autoholsterDelay = serializedObject.FindProperty(nameof(m_autoholsterDelay));

			m_onHolstered = serializedObject.FindProperty(nameof(m_onHolstered));
			m_onUnholstered = serializedObject.FindProperty(nameof(m_onUnholstered));
			m_onChanged = serializedObject.FindProperty(nameof(m_onChanged));
		}

		protected override void DrawProperties()
		{
			base.DrawProperties();
			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(m_dropBehavior);
			if ((int)XRHolsterInteractor.DropBehavior.Autoholster == m_dropBehavior.intValue)
			{
				++EditorGUI.indentLevel;
				EditorGUILayout.PropertyField(m_autoholsterDelay);
				--EditorGUI.indentLevel;
			}
		}

		protected override void DrawInteractorEventsNested()
		{
			base.DrawInteractorEventsNested();

			EditorGUILayout.LabelField("Holster", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(m_onHolstered);
			EditorGUILayout.PropertyField(m_onUnholstered);
			EditorGUILayout.PropertyField(m_onChanged);
		}

		#endregion
	}
}