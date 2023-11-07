using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR
{
	public class XRInteractionEvents : MonoBehaviour
	{
		#region Enumerators

		public enum InteractionType
		{
			None,
			Touch,
			Untouch,
			FirstTouch,
			LastUntouch,
			Grab,
			Ungrab,
			FirstGrab,
			LastUngrab,
			Use,
			Unuse
		};

		#endregion

		#region Fields

		[SerializeField]
		private XRBaseInteractable m_interactable;

		[SerializeField]
		protected InteractionType m_interactionType;

		[SerializeField]
		protected UnityCondition m_predicate = new UnityCondition(UnityCondition.ConditionType.All);

		protected bool m_interacting;

		#endregion

		#region Events

		[SerializeField]
		protected UnityEvent<BaseInteractionEventArgs> m_onInteracted;

		[SerializeField]
		protected UnityEvent<BaseInteractionEventArgs> m_onUninteracted;

		#endregion

		#region Properties

		public XRBaseInteractable interactable => m_interactable;
		public InteractionType interactionType => m_interactionType;
		public bool interacting => m_interacting;
		public UnityEvent<BaseInteractionEventArgs> onInteracted => m_onInteracted;
		public UnityEvent<BaseInteractionEventArgs> onUninteracted => m_onUninteracted;

		#endregion

		#region Methods

		protected virtual void Awake()
		{
			m_interactable = m_interactable ?? GetComponent<XRBaseInteractable>();
		}

		protected virtual void OnEnable()
		{
			switch (m_interactionType)
			{
				case InteractionType.Touch:
					m_interactable.hoverEntered.AddListener(PositiveInteraction);
					m_interactable.hoverExited.AddListener(NegativeInteraction);

					if (m_interactable.isHovered)
					{
						PositiveInteraction(new HoverEnterEventArgs()
						{
							interactableObject = m_interactable,
							interactorObject = m_interactable.GetOldestInteractorHovering()
						});
					}
					break;

				case InteractionType.Untouch:
					m_interactable.hoverEntered.AddListener(NegativeInteraction);
					m_interactable.hoverExited.AddListener(PositiveInteraction);

					if (!m_interactable.isHovered)
					{
						PositiveInteraction(new HoverExitEventArgs()
						{
							interactableObject = m_interactable,
							interactorObject = null
						});
					}
					break;

				case InteractionType.FirstTouch:
					m_interactable.firstHoverEntered.AddListener(PositiveInteraction);
					m_interactable.lastHoverExited.AddListener(NegativeInteraction);
					break;

				case InteractionType.LastUntouch:
					m_interactable.firstHoverEntered.AddListener(NegativeInteraction);
					m_interactable.lastHoverExited.AddListener(PositiveInteraction);
					break;

				case InteractionType.Grab:
					m_interactable.selectEntered.AddListener(PositiveInteraction);
					m_interactable.selectExited.AddListener(NegativeInteraction);

					if (m_interactable.isSelected)
					{
						PositiveInteraction(new SelectEnterEventArgs()
						{
							interactableObject = m_interactable,
							interactorObject = m_interactable.GetOldestInteractorSelecting()
						});
					}
					break;

				case InteractionType.Ungrab:
					m_interactable.selectEntered.AddListener(NegativeInteraction);
					m_interactable.selectExited.AddListener(PositiveInteraction);

					if (!m_interactable.isSelected)
					{
						PositiveInteraction(new SelectExitEventArgs()
						{
							interactableObject = m_interactable,
							interactorObject = null
						});
					}
					break;

				case InteractionType.FirstGrab:
					m_interactable.firstSelectEntered.AddListener(PositiveInteraction);
					m_interactable.lastSelectExited.AddListener(NegativeInteraction);
					break;

				case InteractionType.LastUngrab:
					m_interactable.firstSelectEntered.AddListener(NegativeInteraction);
					m_interactable.lastSelectExited.AddListener(PositiveInteraction);
					break;

				case InteractionType.Use:
					m_interactable.activated.AddListener(PositiveInteraction);
					m_interactable.deactivated.AddListener(NegativeInteraction);
					break;

				case InteractionType.Unuse:
					m_interactable.activated.AddListener(NegativeInteraction);
					m_interactable.deactivated.AddListener(PositiveInteraction);
					break;
			}
		}

		protected virtual void OnDisable()
		{
			switch (m_interactionType)
			{
				case InteractionType.Touch:
					m_interactable.hoverEntered.RemoveListener(PositiveInteraction);
					m_interactable.hoverExited.RemoveListener(NegativeInteraction);
					break;

				case InteractionType.Untouch:
					m_interactable.hoverEntered.RemoveListener(NegativeInteraction);
					m_interactable.hoverExited.RemoveListener(PositiveInteraction);
					break;

				case InteractionType.FirstTouch:
					m_interactable.firstHoverEntered.AddListener(PositiveInteraction);
					m_interactable.lastHoverExited.AddListener(NegativeInteraction);
					break;

				case InteractionType.LastUntouch:
					m_interactable.firstHoverEntered.AddListener(NegativeInteraction);
					m_interactable.lastHoverExited.AddListener(PositiveInteraction);
					break;

				case InteractionType.Grab:
					m_interactable.selectEntered.RemoveListener(PositiveInteraction);
					m_interactable.selectExited.RemoveListener(NegativeInteraction);
					break;

				case InteractionType.Ungrab:
					m_interactable.selectEntered.RemoveListener(NegativeInteraction);
					m_interactable.selectExited.RemoveListener(PositiveInteraction);
					break;

				case InteractionType.FirstGrab:
					m_interactable.firstSelectEntered.RemoveListener(PositiveInteraction);
					m_interactable.lastSelectExited.RemoveListener(NegativeInteraction);
					break;

				case InteractionType.LastUngrab:
					m_interactable.firstSelectEntered.AddListener(NegativeInteraction);
					m_interactable.lastSelectExited.AddListener(PositiveInteraction);
					break;

				case InteractionType.Use:
					m_interactable.activated.RemoveListener(PositiveInteraction);
					m_interactable.deactivated.RemoveListener(NegativeInteraction);
					break;

				case InteractionType.Unuse:
					m_interactable.activated.RemoveListener(NegativeInteraction);
					m_interactable.deactivated.RemoveListener(PositiveInteraction);
					break;
			}

			m_interacting = false;
		}

		protected virtual void PositiveInteraction(BaseInteractionEventArgs e)
		{
			if (!m_predicate.isTrueAndEnabled)
				return;

			SetInteracting(true, e);
		}

		protected virtual void NegativeInteraction(BaseInteractionEventArgs e)
		{
			SetInteracting(false, e);
		}

		protected void SetInteracting(bool value, BaseInteractionEventArgs e)
		{
			// No change, skip
			if (m_interacting == value)
				return;

			m_interacting = value;

			if (value)
			{
				Interact(e);
				m_onInteracted?.Invoke(e);
			}
			else
			{
				Uninteract(e);
				m_onUninteracted?.Invoke(e);
			}
		}

		protected virtual void Interact(BaseInteractionEventArgs e)
		{ }

		protected virtual void Uninteract(BaseInteractionEventArgs e)
		{ }

		#endregion
	}
}