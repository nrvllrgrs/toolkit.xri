using UnityEditor;
using ToolkitEngine.XR;

namespace ToolkitEditor.XR
{
	[CustomEditor(typeof(XRInteractorCollectionEvents))]
    public class XRInteractorCollectionEventsEditor : BaseToolkitEditor
    {
		#region Fields

		protected SerializedProperty m_interactors;
		protected SerializedProperty m_hoverEntered;
		protected SerializedProperty m_hoverExited;
		protected SerializedProperty m_selectEntered;
		protected SerializedProperty m_selectExited;

		#endregion

		#region Methods

		protected virtual void OnEnable()
		{
			m_interactors = serializedObject.FindProperty(nameof(m_interactors));
			m_hoverEntered = serializedObject.FindProperty(nameof(m_hoverEntered));
			m_hoverExited = serializedObject.FindProperty(nameof(m_hoverExited));
			m_selectEntered = serializedObject.FindProperty(nameof(m_selectEntered));
			m_selectExited = serializedObject.FindProperty(nameof(m_selectExited));
		}

		protected override void DrawProperties()
		{
			EditorGUILayout.PropertyField(m_interactors);
		}

		protected override void DrawEvents()
		{
			if (EditorGUILayoutUtility.Foldout(m_hoverEntered, "Interactor Events"))
			{
				EditorGUILayout.LabelField("Hover", EditorStyles.boldLabel);
				EditorGUILayout.PropertyField(m_hoverEntered);
				EditorGUILayout.PropertyField(m_hoverExited);

				EditorGUILayout.LabelField("Select", EditorStyles.boldLabel);
				EditorGUILayout.PropertyField(m_selectEntered);
				EditorGUILayout.PropertyField(m_selectExited);
			}
		}

		#endregion
	}
}