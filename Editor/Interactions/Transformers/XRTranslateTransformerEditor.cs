using UnityEngine;
using UnityEditor;
using ToolkitEngine.XR.Transformers;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace ToolkitEditor.XR
{
	[CustomEditor(typeof(XRTranslateTransformer))]
	public class XRTranslateTransformerEditor : BaseToolkitEditor
	{
		#region Fields

		protected SerializedProperty m_direction;
		protected SerializedProperty m_maxDepth;
		protected SerializedProperty m_startingDepth;

		#endregion

		#region Methods

		private void OnEnable()
		{
			var translateTransformer = target as XRTranslateTransformer;

			// Setup interactable settings
			var grabInteractable = translateTransformer.GetComponent<XRGrabInteractable>();
			grabInteractable.trackPosition = true;
			grabInteractable.trackRotation = false;
			grabInteractable.trackScale = false;
			grabInteractable.throwOnDetach = false;
			grabInteractable.addDefaultGrabTransformers = false;

			if (!grabInteractable.startingSingleGrabTransformers.Contains(translateTransformer))
			{
				grabInteractable.startingSingleGrabTransformers.Add(translateTransformer);
			}

			// Setup rigidbody
			var rigidbody = translateTransformer.GetComponent<Rigidbody>();
			rigidbody.isKinematic = true;
			rigidbody.useGravity = false;

			m_direction = serializedObject.FindProperty(nameof(m_direction));
			m_maxDepth = serializedObject.FindProperty(nameof(m_maxDepth));
			m_startingDepth = serializedObject.FindProperty(nameof(m_startingDepth));
		}

		protected override void DrawProperties()
		{
			EditorGUILayout.PropertyField(m_direction);
			EditorGUILayout.PropertyField(m_maxDepth);

			m_startingDepth.floatValue = EditorGUILayout.Slider(
				m_startingDepth.displayName,
				m_startingDepth.floatValue,
				0f,
				m_maxDepth.floatValue);
		}

		#endregion
	}
}