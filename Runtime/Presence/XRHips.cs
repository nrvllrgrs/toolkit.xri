using UnityEngine;
using NaughtyAttributes;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ToolkitEngine.XR
{
	public class XRHips : MonoBehaviour
	{
		#region Fields

		[SerializeField]
		private bool m_tracking = true;

		[SerializeField, MinMaxSlider(-90f, 90f)]
		private Vector2 m_trackingLimit = new Vector2(-90f, 90f);

		[SerializeField]
		private Transform m_head;

		[SerializeField]
		private float m_headOffset = -0.35f;

		#endregion

		#region Properties

		public bool tracking { get => m_tracking; set => m_tracking = value; }

		#endregion

		#region Methods

		private void LateUpdate()
		{
			// Not tracking, skip
			if (!m_tracking)
				return;

			var angle = Vector3.SignedAngle(Vector3.ProjectOnPlane(m_head.forward, Vector3.up), m_head.forward, m_head.right).WrapEulerAngle();
			if (!angle.Between(m_trackingLimit.x, m_trackingLimit.y))
				return;

			var modForward = m_head.forward;
			modForward.y = 0f;
			modForward.Normalize();

			var modUp = m_head.up;
			if (m_head.forward.y > 0f)
			{
				modUp = -modUp;
			}
			modUp.y = 0f;
			modUp.Normalize();

			var dot = Mathf.Clamp01(Vector3.Dot(modForward, m_head.forward));
			transform.SetPositionAndRotation(
				m_head.position + m_headOffset * Vector3.up,
				Quaternion.LookRotation(Vector3.Lerp(modUp, modForward, dot * dot), Vector3.up));
		}

		#endregion

		#region Edior-Only
#if UNITY_EDITOR

		private void OnDrawGizmosSelected()
		{
			if (m_head == null)
				return;

			var point = m_head.position;
			var from = Quaternion.AngleAxis(m_trackingLimit.x, m_head.right) * m_head.forward;

			Gizmos.color = Handles.color = Color.green;
			Handles.DrawWireArc(point, m_head.right, from, m_trackingLimit.y - m_trackingLimit.x, 0.5f);
			Gizmos.DrawRay(point, from * 0.5f);
			Gizmos.DrawRay(point, Quaternion.AngleAxis(m_trackingLimit.y, m_head.right) * m_head.forward * 0.5f);
		}

#endif
		#endregion
	}
}