using UnityEditor;
using ToolkitEngine.XR;

namespace ToolkitEditor.XR
{
	[CustomEditor(typeof(XRBaseInputEvents), true)]
	public class XRBaseInputEventsEditor : XRInteractableEventsEditor
	{
		#region Fields

		protected SerializedProperty m_leftAction;
		protected SerializedProperty m_rightAction;
		protected SerializedProperty m_onPerformed;
		protected SerializedProperty m_onCanceled;

		#endregion

		#region Methods

		protected override void OnEnable()
		{
			base.OnEnable();
			m_leftAction = serializedObject.FindProperty(nameof(m_leftAction));
			m_rightAction = serializedObject.FindProperty(nameof(m_rightAction));
			m_onPerformed = serializedObject.FindProperty(nameof(m_onPerformed));
			m_onCanceled = serializedObject.FindProperty(nameof(m_onCanceled));
		}

		protected override void DrawProperties()
		{
			base.DrawProperties();
			EditorGUILayout.PropertyField(m_leftAction);
			EditorGUILayout.PropertyField(m_rightAction);
		}

		protected override void DrawEvents()
		{
			if (((XRBaseInputEvents)target).interactionType != XRInteractableEvents.InteractionType.None)
			{
				base.DrawEvents();
			}

			if (EditorGUILayoutUtility.Foldout(m_onPerformed, "Input Events"))
			{
				EditorGUILayout.PropertyField(m_onPerformed);
				EditorGUILayout.PropertyField(m_onCanceled);
			}
		}

		#endregion
	}
}