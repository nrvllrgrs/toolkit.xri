using UnityEditor;

using ToolkitEngine.XR;

namespace ToolkitEditor.XR
{
	[CustomEditor(typeof(XRConditionalSocketInteractor))]
	public class XRConditionalSocketInteractorEditor : UnityEditor.XR.Interaction.Toolkit.Interactors.XRSocketInteractorEditor
    {
		#region Fields

		protected SerializedProperty m_selectOnHover;
		protected SerializedProperty m_transferInteractable;
		protected SerializedProperty m_useConditionals; 
		protected SerializedProperty m_interactorFilter;
		protected SerializedProperty m_interactableFilter;

		#endregion

		#region Methods

		protected override void OnEnable()
		{
			base.OnEnable();

			m_selectOnHover = serializedObject.FindProperty(nameof(m_selectOnHover));
			m_transferInteractable = serializedObject.FindProperty(nameof(m_transferInteractable));
			m_useConditionals = serializedObject.FindProperty(nameof(m_useConditionals));
			m_interactorFilter = serializedObject.FindProperty(nameof(m_interactorFilter));
			m_interactableFilter = serializedObject.FindProperty(nameof(m_interactableFilter));
		}

		protected override void DrawProperties()
		{
			base.DrawProperties();

			EditorGUILayout.PropertyField(m_selectOnHover);
			if (m_selectOnHover.boolValue)
			{
				++EditorGUI.indentLevel;
				EditorGUILayout.PropertyField(m_transferInteractable);
				--EditorGUI.indentLevel;
			}

			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(m_useConditionals);
			if (m_useConditionals.boolValue)
			{
				++EditorGUI.indentLevel;
				EditorGUILayout.PropertyField(m_interactorFilter);
				EditorGUILayout.PropertyField(m_interactableFilter);
				--EditorGUI.indentLevel;
			}
		}

		#endregion
	}
}