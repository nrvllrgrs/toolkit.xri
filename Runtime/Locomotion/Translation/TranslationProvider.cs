using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR
{
	public class TranslationProvider : ContinuousMoveProviderBase
    {
		#region Fields

		[SerializeField, Min(0f)]
		private float m_arrivalDistance = 1f;

		private Vector3? m_destination;
		private Vector3 m_offset;

		#endregion

		#region Events

		[SerializeField]
		private UnityEvent m_onDeparture;

		[SerializeField]
		private UnityEvent m_onCancelation;

		[SerializeField]
		private UnityEvent m_onArrival;

		#endregion

		#region Properties

		public UnityEvent onDeparture => m_onDeparture;
		public UnityEvent onCancelation => m_onCancelation;
		public UnityEvent onArrival => m_onArrival;

		#endregion

		#region Methods

		protected override void Awake()
		{
			base.Awake();
			endLocomotion += TranslationProvider_EndLocomotion;
		}

		protected virtual void OnDestroy()
		{
			endLocomotion -= TranslationProvider_EndLocomotion;
		}

		private void TranslationProvider_EndLocomotion(LocomotionSystem obj)
		{
			if (!m_destination.HasValue)
				return;

			var xrOrigin = system.xrOrigin;
			if (xrOrigin == null)
				return;

			if ((m_destination.Value + m_offset - xrOrigin.transform.position).sqrMagnitude <= m_arrivalDistance * m_arrivalDistance)
			{
				m_destination = null;
				m_onArrival?.Invoke();
			}
		}

		public bool TryGetDestination(out Vector3 destination)
		{
			if (m_destination.HasValue)
			{
				destination = m_destination.Value;
				return true;
			}

			destination = default;
			return false;
		}

		public void Depart(Vector3 destination)
		{
			Depart(destination, Vector3.zero);
		}

		public void Depart(Vector3 destination, Vector3 offset)
		{
			m_destination = destination;
			m_offset = offset;
			m_onDeparture?.Invoke();
		}

		public void Cancel()
		{
			m_destination = null;
			m_onCancelation?.Invoke();
		}

		protected override Vector2 ReadInput()
        {
            return Vector2.up;
        }

		protected override Vector3 ComputeDesiredMove(Vector2 input)
		{
			if (!m_destination.HasValue)
				return Vector3.zero;

			var xrOrigin = system.xrOrigin;
			if (xrOrigin == null)
				return Vector3.zero;

			var originTransform = xrOrigin.Origin.transform;
			float speedFactor = moveSpeed * Time.deltaTime * originTransform.localScale.x;
			Vector3 forward = (m_destination.Value + m_offset - originTransform.transform.position).normalized;

			return forward * speedFactor;
		}

		#endregion
	}
}