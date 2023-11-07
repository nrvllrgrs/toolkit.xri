using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

using InputActionManager = UnityEngine.XR.Interaction.Toolkit.Inputs.InputActionManager;

namespace ToolkitEngine.XR
{
	public abstract class XRBaseInputEvents : XRInteractionEvents
    {
		#region Fields

		[SerializeField]
		private InputActionProperty m_leftAction;

		[SerializeField]
		private InputActionProperty m_rightAction;

		protected bool m_leftHand;
		private InputActionProperty? m_selectedAction;

		protected bool m_performing;
		protected Dictionary<InputActionProperty, InputAction> m_map = new();

		private static InputActionManager s_inputActionManager;

		#endregion

		#region Events

		[SerializeField]
		private UnityEvent<InputAction.CallbackContext> m_onPerformed;

		[SerializeField]
		private UnityEvent<InputAction.CallbackContext> m_onCanceled;

		#endregion

		#region Properties

		public bool canPerform => m_predicate.isTrueAndEnabled;

		public bool performing => m_performing;

		public UnityEvent<InputAction.CallbackContext> onPerformed => m_onPerformed;
		public UnityEvent<InputAction.CallbackContext> onCanceled => m_onCanceled;

		protected static InputActionManager inputActionManager
		{
			get
			{
				if (s_inputActionManager == null)
				{
					s_inputActionManager = FindObjectOfType<InputActionManager>();
				}
				return s_inputActionManager;
			}
		}

		#endregion

		#region Methods

		protected override void OnEnable()
		{
			if (interactionType != InteractionType.None)
			{
				base.OnEnable();
				return;
			}

			Register(m_leftAction);
			Register(m_rightAction);
		}

		protected override void OnDisable()
		{
			if (interactionType != InteractionType.None)
			{
				base.OnDisable();
				Uninteract(null);
				return;
			}

			Unregister(m_leftAction);
			Unregister(m_rightAction);
		}

		private void Register(InputActionProperty inputAction)
		{
			if (inputAction == null)
				return;

			if (!m_map.TryGetValue(inputAction, out InputAction action))
			{
				if (!inputActionManager.actionAssets.Contains(inputAction.action.actionMap.asset))
				{
					action = inputActionManager.actionAssets.FirstOrDefault(x => Equals(x.name, inputAction.action.actionMap.asset.name))
						?.FindActionMap(inputAction.action.actionMap.id)
						?.FindAction(inputAction.action.id);
				}
				else
				{
					action = inputAction.action;
				}

				m_map.Add(inputAction, action);
			}

			if (action != null)
			{
				action.Enable();
				action.performed += Performed;
				action.canceled += Canceled;
			}
		}

		private void Unregister(InputActionProperty inputAction)
		{
			if (inputAction == null)
				return;

			if (!m_map.TryGetValue(inputAction, out InputAction action))
				return;

			if (action != null)
			{
				action.Disable();
				action.performed -= Performed;
				action.canceled -= Canceled;
			}
		}

		protected override void PositiveInteraction(BaseInteractionEventArgs e)
		{
			SetInteracting(true, e);
		}

		protected override void Interact(BaseInteractionEventArgs e)
		{
			m_selectedAction = null;
			if (XRHandednessContext.IsLeftHand(e.interactorObject))
			{
				m_selectedAction = m_leftAction;
				m_leftHand = true;
			}
			else if (XRHandednessContext.IsRightHand(e.interactorObject))
			{
				m_selectedAction = m_rightAction;
				m_leftHand = false;
			}

			if (m_selectedAction.HasValue)
			{
				Register(m_selectedAction.Value);
			}
		}

		protected override void Uninteract(BaseInteractionEventArgs e)
		{
			if (!m_selectedAction.HasValue)
				return;

			Unregister(m_selectedAction.Value);
		}

		private bool CompareTag(IXRInteractor interactor, string tag)
		{
			return interactor.transform.gameObject.CompareTag(tag);
		}

		protected virtual void Performed(InputAction.CallbackContext obj)
		{
			if (!canPerform)
				return;

			SetPerforming(true, obj);
		}

		protected virtual void Canceled(InputAction.CallbackContext obj)
		{
			SetPerforming(false, obj);
		}

		protected void SetPerforming(bool value, InputAction.CallbackContext obj)
		{
			// No change, skip
			if (m_performing == value)
				return;

			m_performing = value;

			if (value)
			{
				m_onPerformed?.Invoke(obj);
			}
			else
			{
				m_onCanceled?.Invoke(obj);
			}
		}

		public bool IsLeftHand(InputAction.CallbackContext obj)
		{
			return obj.control == m_leftAction.action.activeControl;
		}

		#endregion
	}
}