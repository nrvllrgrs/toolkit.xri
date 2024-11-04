using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace ToolkitEngine.XR
{
	public class XRHapticEvents : XRInteractableEvents
    {
		#region Fields

		[SerializeField]
		private HapticSettings m_haptics;

		[SerializeField, Tooltip("Indicates whether pulse is automatically pulsed on interaction; otherwise, it waits for request after interaction.")]
		private bool m_pulseOnInteract = true;

		private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor m_controllerInteractor = null;

		#endregion

		#region Methods

		protected override void Interact(BaseInteractionEventArgs e)
		{
			m_controllerInteractor = e.interactorObject.transform.GetComponent<XRBaseInputInteractor>();
			if (m_pulseOnInteract)
			{
				m_haptics.SendImpulse(m_controllerInteractor);
			}
		}

		protected override void Uninteract(BaseInteractionEventArgs e)
		{
			m_haptics.CancelImpulse(m_controllerInteractor);
			m_controllerInteractor = null;
		}

		[ContextMenu("Pulse")]
		public void Pulse()
		{
			if (m_controllerInteractor == null || m_pulseOnInteract)
				return;

			m_haptics.SendImpulse(m_controllerInteractor);
		}

		[ContextMenu("Cancel Pulse")]
		public void CancelPulse()
		{
			if (m_controllerInteractor == null || m_pulseOnInteract)
				return;

			m_haptics.CancelImpulse(m_controllerInteractor);
		}

		#endregion
	}
}