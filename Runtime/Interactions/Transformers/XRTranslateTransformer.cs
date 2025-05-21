using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ToolkitEngine.XR.Transformers
{
	[RequireComponent(typeof(XRGrabInteractable))]
	public class XRTranslateTransformer : XRBaseGrabTransformer
	{
		#region Fields

		[SerializeField]
		protected Axis m_direction;

		[SerializeField, Min(0f)]
		protected float m_maxDepth = 1f;

		[SerializeField]
		protected float m_startingDepth;

		private Transform m_defaultParent;
		private Vector3 m_defaultLocalPosition;
		private Quaternion m_defaultLocalRotation;

#if UNITY_EDITOR
		private const float SCREEN_SPACE_SIZE = 2f;
#endif
		#endregion

		#region Events

		#endregion

		#region Properties

		public Vector3 direction => GetDirection(m_direction);

		#endregion

		#region Methods

		private void Awake()
		{
			m_defaultParent = transform.parent;
			m_defaultLocalPosition = transform.localPosition;
			m_defaultLocalRotation = transform.localRotation;

			transform.position = GetPosition(m_startingDepth);
		}

		public override void Process(XRGrabInteractable grabInteractable, XRInteractionUpdateOrder.UpdatePhase updatePhase, ref Pose targetPose, ref Vector3 localScale)
		{
			var interactor = grabInteractable.firstInteractorSelecting;
			var direction = interactor.transform.position - GetDefaultPosition();
			var normal = this.direction;

			direction = Vector3.Project(direction, normal);
			float depth = Vector3.Dot(normal, direction) > 0f
				? direction.magnitude
				: 0f;

			targetPose.position = GetPosition(depth);
		}

		private Vector3 GetPosition(float depth)
		{
			return GetDefaultPosition() + direction * Mathf.Clamp(depth, 0f, m_maxDepth);
		}

		private Vector3 GetDirection(Axis axis)
		{
			if (!Application.isPlaying)
				return transform.rotation * AxisUtil.GetDirection(axis);

			return GetDefaultRotation() * AxisUtil.GetDirection(axis);
		}

		/// <summary>
		/// Get default position in world-space
		/// </summary>
		/// <returns>Default world-space position</returns>
		private Vector3 GetDefaultPosition()
		{
			return m_defaultParent == null
				? m_defaultLocalPosition
				: m_defaultParent.position + m_defaultParent.rotation * m_defaultLocalPosition;
		}

		/// <summary>
		/// Get default rotation is world-space
		/// </summary>
		/// <returns>Default world-space rotation</returns>
		private Quaternion GetDefaultRotation()
		{
			return m_defaultParent == null
				? m_defaultLocalRotation
				: m_defaultParent.rotation * m_defaultLocalRotation;
		}

		#endregion

		#region Editor-Only
#if UNITY_EDITOR

		private void OnDrawGizmosSelected()
		{
			if (m_maxDepth <= 0f)
				return;

			var startPoint = Application.isPlaying
				? GetDefaultPosition()
				: transform.position;

			var direction = GetDirection(m_direction) * m_maxDepth;
			var endPoint = startPoint + direction;
			var scale = HandleUtility.GetHandleSize(startPoint);

			Gizmos.DrawLine(startPoint, endPoint);

			var rotation = Quaternion.LookRotation(direction);
			Handles.ArrowHandleCap(0, endPoint - direction * scale, rotation, scale, EventType.Repaint);

			Handles.color = Color.green;
			Handles.DrawWireDisc(startPoint, endPoint - startPoint, 0.5f * scale, SCREEN_SPACE_SIZE);
			Handles.color = Color.red;
			Handles.DrawWireDisc(endPoint, endPoint - startPoint, 0.5f * scale, SCREEN_SPACE_SIZE);
		}

#endif
		#endregion
	}
}