using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace UnityEditor.XR.Interaction.Toolkit
{
	[CustomEditor(typeof(XRPocketInteractor))]
    public class XRPocketInteractorEditor : XRSocketInteractorEditor
    {
		#region Fields

		protected SerializedProperty m_size;
		protected SerializedProperty m_autoCenter;
		protected SerializedProperty m_usePocketLayer;
		protected SerializedProperty m_pocketLayer;

		#endregion

		#region Methods

		protected override void OnEnable()
		{
			base.OnEnable();
			m_size = serializedObject.FindProperty(nameof(m_size));
			m_autoCenter = serializedObject.FindProperty(nameof(m_autoCenter));
			m_usePocketLayer = serializedObject.FindProperty(nameof(m_usePocketLayer));
			m_pocketLayer = serializedObject.FindProperty(nameof(m_pocketLayer));
		}

		protected override void DrawProperties()
		{
			base.DrawProperties();
			
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

		#endregion
	}
}