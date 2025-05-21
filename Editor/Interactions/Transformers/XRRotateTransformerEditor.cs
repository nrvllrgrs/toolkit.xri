using UnityEngine;
using UnityEditor;
using ToolkitEngine.XR.Transformers;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace ToolkitEditor.XR
{
    [CustomEditor(typeof(XRRotateTransformer))]
    public class XRRotateTransformerEditor : BaseToolkitEditor
    {
		#region Fields

		protected SerializedProperty m_forward;
		protected SerializedProperty m_upward;
		protected SerializedProperty m_range;
		protected SerializedProperty m_startingAngle;
		protected SerializedProperty m_rotationDirection;

		#endregion

		#region Methods

		private void OnEnable()
		{
			var rotateTransformer = target as XRRotateTransformer;

			// Setup interactable settings
			var grabInteractable = rotateTransformer.GetComponent<XRGrabInteractable>();
			grabInteractable.trackPosition = false;
			grabInteractable.trackRotation = true;
			grabInteractable.trackScale = false;
			grabInteractable.throwOnDetach = false;
			grabInteractable.addDefaultGrabTransformers = false;

			if (!grabInteractable.startingSingleGrabTransformers.Contains(rotateTransformer))
			{
				grabInteractable.startingSingleGrabTransformers.Add(rotateTransformer);
			}

			// Setup rigidbody
			var rigidbody = rotateTransformer.GetComponent<Rigidbody>();
			rigidbody.isKinematic = true;
			rigidbody.useGravity = false;

			m_forward = serializedObject.FindProperty(nameof(m_forward));
			m_upward = serializedObject.FindProperty(nameof(m_upward));
			m_range = serializedObject.FindProperty(nameof(m_range));
			m_startingAngle = serializedObject.FindProperty(nameof(m_startingAngle));
			m_rotationDirection = serializedObject.FindProperty(nameof(m_rotationDirection));
		}

		protected override void DrawProperties()
		{
			EditorGUILayout.PropertyField(m_forward);
			EditorGUILayout.PropertyField(m_upward);
			EditorGUILayout.PropertyField(m_range);

			m_startingAngle.floatValue = EditorGUILayout.Slider(
				m_startingAngle.displayName,
				m_startingAngle.floatValue,
				m_range.vector2Value.x,
				m_range.vector2Value.y);

			EditorGUILayout.PropertyField(m_rotationDirection);
		}

		#endregion
	}
}