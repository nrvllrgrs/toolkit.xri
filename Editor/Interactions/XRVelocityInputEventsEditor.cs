using UnityEditor;
using ToolkitEngine.XR;

namespace ToolkitEditor.XR
{
	[CustomEditor(typeof(XRVelocityInputEvents))]
    public class XRVelocityInputEventsEditor : XRBaseInputEventsEditor
    {
		#region Fields

		protected SerializedProperty m_isOmnidirectional;
		protected SerializedProperty m_directionMask;
		protected SerializedProperty m_threshold;

		#endregion

		#region Methods

		protected override void OnEnable()
		{
			base.OnEnable();
			m_isOmnidirectional = serializedObject.FindProperty(nameof(m_isOmnidirectional));
			m_directionMask = serializedObject.FindProperty(nameof(m_directionMask));
			m_threshold = serializedObject.FindProperty(nameof(m_threshold));
		}

		protected override void DrawProperties()
		{
			base.DrawProperties();

			EditorGUILayout.PropertyField(m_isOmnidirectional);
			if (!m_isOmnidirectional.boolValue)
			{
				EditorGUILayout.PropertyField(m_directionMask);
			}
			EditorGUILayout.PropertyField(m_threshold);
		}

		#endregion
	}
}