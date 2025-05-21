using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Transformers;
using NaughtyAttributes;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ToolkitEngine.XR.Transformers
{
	[RequireComponent(typeof(XRGrabInteractable))]
    public class XRRotateTransformer : XRBaseGrabTransformer
	{
		#region Enumerators

		[Flags]
		public enum RotationMode
		{
			Clockwise = 1 << 1,
			Counterclockwise = 1 << 2,
		}

		public enum SnapMode
		{
			None,
			Angle,
			Step,
		}

		#endregion

		#region Fields

		[SerializeField]
		protected Axis m_forward = Axis.Forward;

		[SerializeField]
		protected Axis m_upward = Axis.Up;

		[SerializeField, MinMaxSlider(-180f, 180f)]
		protected Vector2 m_range = new Vector2(-180f, 180f);

		[SerializeField]
		protected float m_startingAngle;

		[SerializeField]
		protected RotationMode m_rotationDirection = RotationMode.Clockwise | RotationMode.Counterclockwise;

		//[SerializeField]
		//protected bool m_useSteps = false;

		private Transform m_defaultParent;
		private Vector3 m_defaultPosition;
		private Quaternion m_defaultLocalRotation;

#if UNITY_EDITOR
		private const float SCREEN_SPACE_SIZE = 2f;
#endif
		#endregion

		#region Events

		//[SerializeField]
		//private UnityEvent<RotationInteractionEvent> m_onValueChanged;

		//[SerializeField]
		//private UnityEvent<RotationInteractionEvent> m_onIndexChanged;

		#endregion

		#region Properties

		/// <summary>
		/// Indicates whether object rotatation wraps at its limits.
		/// </summary>
		protected bool wrapping => minAngle == -180f && maxAngle == 180f;

		/// <summary>
		/// Minimum angle of interactable
		/// </summary>
		public float minAngle => m_range.x;

		/// <summary>
		/// Maximum angle of interactable
		/// </summary>
		public float maxAngle => m_range.y;

		public float range => m_range.y - m_range.x;

		/// <summary>
		/// Forward direction of axis in world-space
		/// </summary>
		protected Vector3 forward => GetDirection(m_forward);

		/// <summary>
		/// Upward direction of axis in world-space
		/// </summary>
		protected Vector3 upward => GetDirection(m_upward);

		#endregion

		#region Methods

		private void Awake()
		{
			m_defaultParent = transform.parent;
			m_defaultPosition = transform.position;
			m_defaultLocalRotation = transform.localRotation;

			transform.rotation = GetRotation(m_startingAngle);
		}

		public override void Process(XRGrabInteractable grabInteractable, XRInteractionUpdateOrder.UpdatePhase updatePhase, ref Pose targetPose, ref Vector3 localScale)
		{
			var interactor = grabInteractable.firstInteractorSelecting;
			var direction = Vector3.ProjectOnPlane(
				interactor.transform.position - grabInteractable.transform.position,
				upward).normalized;

			targetPose.position = m_defaultPosition;
			targetPose.rotation = GetRotation(
				Vector3.SignedAngle(forward, direction, upward).WrapEulerAngle());
		}

		private Vector3 GetDirection(Axis axis)
		{
			if (!Application.isPlaying)
				return transform.rotation * AxisUtil.GetDirection(axis);

			return GetDefaultRotation() * AxisUtil.GetDirection(axis);
		}

		private Quaternion GetRotation(Axis axis)
		{
			return Quaternion.LookRotation(GetDirection(axis));
		}

		private Quaternion GetRotation(float angle)
		{
			angle = Mathf.Clamp(angle, minAngle, maxAngle);
			return Quaternion.LookRotation(
				GetRotation(m_forward) * Quaternion.AngleAxis(angle, upward) * forward,
				upward);
		}

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
			var point = transform.position;
			//if (m_useSteps)
			//{
			//	float theta = range / m_stepCount;
			//	int count = m_stepCount * 2;

			//	if (!wrapping)
			//	{
			//		theta = range / (m_stepCount - 1);
			//		--count;
			//	}

			//	// Cut theta in half to draw step boundaries
			//	theta *= 0.5f;

			//	for (int i = 0; i < count; ++i)
			//	{
			//		if (i % 2 == 0)
			//		{
			//			if (m_snap || m_snapSpeed > 0f)
			//			{
			//				Handles.color = Color.white;
			//				Handles.DrawLine(point, point + Quaternion.AngleAxis(minAngle + (theta * i), forward) * upward * 0.5f, SCREEN_SPACE_SIZE);
			//			}
			//			else if (!wrapping && (i == 0 || i == count - 1))
			//			{
			//				Handles.color = Color.green;
			//				Handles.DrawLine(point, point + Quaternion.AngleAxis(minAngle + (theta * i), forward) * upward * 0.5f, SCREEN_SPACE_SIZE);
			//			}
			//		}
			//		else
			//		{
			//			Handles.color = Color.green;
			//			Handles.DrawDottedLine(point, point + Quaternion.AngleAxis(minAngle + (theta * i), forward) * upward * 0.5f, SCREEN_SPACE_SIZE * 2f);
			//		}
			//	}
			//}
			//else
			if (!wrapping)
			{
				Handles.color = Color.green;
				Handles.DrawLine(point, point + Quaternion.AngleAxis(minAngle, upward) * forward * 0.5f, SCREEN_SPACE_SIZE);
				Handles.DrawLine(point, point + Quaternion.AngleAxis(maxAngle, upward) * forward * 0.5f, SCREEN_SPACE_SIZE);
			}

			var scale = HandleUtility.GetHandleSize(point);

			Handles.color = Color.green;
			Handles.DrawWireArc(point, upward, Quaternion.AngleAxis(minAngle, upward) * forward, range, 0.5f * scale, SCREEN_SPACE_SIZE);

			Gizmos.color = Color.blue;
			Gizmos.DrawRay(transform.position, forward * 0.5f);
		}

#endif
		#endregion
	}
}