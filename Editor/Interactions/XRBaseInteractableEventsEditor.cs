using UnityEditor;
using ToolkitEngine.XR;

namespace ToolkitEditor.XR
{
	[CustomEditor(typeof(XRInteractableEvents), true)]
	public class XRInteractableEventsEditor : BaseToolkitEditor
	{
		#region Fields

		protected SerializedProperty m_interactable;
		protected SerializedProperty m_interactionType;
		protected SerializedProperty m_predicate;
		protected SerializedProperty m_onInteracted;
		protected SerializedProperty m_onUninteracted;

		#endregion

		#region Methods

		protected virtual void OnEnable()
		{
			m_interactable = serializedObject.FindProperty(nameof(m_interactable));
			m_interactionType = serializedObject.FindProperty(nameof(m_interactionType));
			m_predicate = serializedObject.FindProperty(nameof(m_predicate));
			m_onInteracted = serializedObject.FindProperty(nameof(m_onInteracted));
			m_onUninteracted = serializedObject.FindProperty(nameof(m_onUninteracted));
		}

		protected override void DrawProperties()
		{
			EditorGUILayout.PropertyField(m_interactable);
			EditorGUILayout.PropertyField(m_interactionType);
			EditorGUILayout.PropertyField(m_predicate);
		}

		protected override void DrawEvents()
		{
			if (EditorGUILayoutUtility.Foldout(m_onInteracted, "Interaction Events"))
			{
				EditorGUILayout.PropertyField(m_onInteracted);
				EditorGUILayout.PropertyField(m_onUninteracted);
				DrawNestedEvents();
			}
		}

		#endregion
	}
}