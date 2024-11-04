using UnityEngine;
using UnityEditor;
using ToolkitEngine.XR;

namespace ToolkitEditor.XR
{
	[CustomEditor(typeof(XRPocketInteractor))]
    public class XRPocketInteractorEditor : XRConditionalSocketInteractorEditor
	{
		#region Fields

		protected SerializedProperty m_size;
		protected SerializedProperty m_autoCenter;
		protected SerializedProperty m_usePocketLayer;
		protected SerializedProperty m_pocketLayer;
		protected SerializedProperty m_onPocketed;
		protected SerializedProperty m_onUnpocketed;

		#endregion

		#region Methods

		protected override void OnEnable()
		{
			base.OnEnable();
			m_size = serializedObject.FindProperty(nameof(m_size));
			m_autoCenter = serializedObject.FindProperty(nameof(m_autoCenter));
			m_usePocketLayer = serializedObject.FindProperty(nameof(m_usePocketLayer));
			m_pocketLayer = serializedObject.FindProperty(nameof(m_pocketLayer));
			m_onPocketed = serializedObject.FindProperty(nameof(m_onPocketed));
			m_onUnpocketed = serializedObject.FindProperty(nameof(m_onUnpocketed));
		}

		protected override void DrawProperties()
		{
			base.DrawProperties();
			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(m_size);
			EditorGUILayout.PropertyField(m_autoCenter);

			EditorGUILayout.PropertyField(m_usePocketLayer);
			if (m_usePocketLayer.boolValue)
			{
				++EditorGUI.indentLevel;
				EditorGUILayout.PropertyField(m_pocketLayer, new GUIContent("Layer"));
				--EditorGUI.indentLevel;
			}
		}

		protected override void DrawInteractorEventsNested()
		{
			base.DrawInteractorEventsNested();

			EditorGUILayout.LabelField("Pocket", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(m_onPocketed);
			EditorGUILayout.PropertyField(m_onUnpocketed);
		}

		#endregion
	}
}