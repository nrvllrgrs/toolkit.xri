using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR
{
	public class HolsterEventArgs : BaseInteractionEventArgs
	{
		private XRHolsterInteractor m_holsterInteractor;
		public XRHolsterInteractor holsterInteractor => interactableObject.transform.GetComponent(ref m_holsterInteractor);
	}

	[RequireComponent(typeof(XRRedirectorInteractable))]
	public class XRHolsterInteractor : XRConditionalSocketInteractor
    {
		#region Enumerators

		public enum DropBehavior
		{
			None,
			Unassign,
			Autoholster,
		}

		#endregion

		#region Fields

		[SerializeField]
		private DropBehavior m_dropBehavior = DropBehavior.Autoholster;

		[SerializeField, Min(0f)]
		private float m_autoholsterDelay = 0f;

		/// <summary>
		/// Reference to required XRSimpleInteractable
		/// </summary>
		private XRRedirectorInteractable m_redirector;

		private UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable m_holstered;
		private CancellationTokenSource m_reholsterCancellationTokenSource;

		#endregion

		#region Events

		[SerializeField]
		private UnityEvent<HolsterEventArgs> m_onHolstered;

		[SerializeField]
		private UnityEvent<HolsterEventArgs> m_onUnholstered;

		[SerializeField]
		private UnityEvent<HolsterEventArgs> m_onChanged;

		#endregion

		#region Properties

		public UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable holstered
		{
			get => m_holstered;
			set
			{
				// No change, skip
				if (m_holstered == value)
					return;

				UnregisterHolstered();
				m_holstered = value;
				RegisterHolstered();

				m_redirector.targetInteractable = value;
				UpdateRedirector();
			}
		}

		public bool hasHolstered => m_holstered != null;

		public override bool isSelectActive => m_canSelect && base.isSelectActive;

		#endregion

		#region Methods

		protected override void Awake()
		{
			base.Awake();
			m_redirector = GetComponent<XRRedirectorInteractable>();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			UpdateRedirector();
		}

		protected override void OnSelectEntered(SelectEnterEventArgs args)
		{
			base.OnSelectEntered(args);

			holstered = args.interactableObject;
			m_onHolstered?.Invoke(GetHolsterEventArgs(args));

			// UpdateRedirector needs to called (even though it may have been called in 'holstered')
			// If there was no change then it wasn't invoked
			UpdateRedirector();
		}

		protected override void OnSelectExited(SelectExitEventArgs args)
		{
			base.OnSelectExited(args);

			m_onUnholstered?.Invoke(GetHolsterEventArgs(args));
			UpdateRedirector();
		}

		private HolsterEventArgs GetHolsterEventArgs(BaseInteractionEventArgs args)
		{
			return new HolsterEventArgs()
			{
				interactorObject = args.interactorObject,
				interactableObject = args.interactableObject
			};
		}

		private void RegisterHolstered()
		{
			if (!hasHolstered)
				return;

			m_holstered.selectEntered.AddListener(Holstered_Grabbed);
			m_holstered.selectExited.AddListener(Holstered_Dropped);
		}

		private void UnregisterHolstered()
		{
			if (!hasHolstered)
				return;

			m_holstered.selectEntered.RemoveListener(Holstered_Grabbed);
			m_holstered.selectExited.RemoveListener(Holstered_Dropped);
		}

		private void UpdateRedirector()
		{
			m_redirector.enabled = !hasSelection && hasHolstered;
		}

		#endregion

		#region Item Methods

		private void Holstered_Grabbed(SelectEnterEventArgs args)
		{
			// Stop auto-reholster timer
			m_reholsterCancellationTokenSource?.Cancel();
		}

		private async void Holstered_Dropped(SelectExitEventArgs args)
		{
			// Holstered object exiting holster, skip
			if (Equals(args.interactorObject, this) && Equals(args.interactableObject, m_holstered))
				return;

			// Dropping over holster, skip
			if (args.interactableObject is UnityEngine.XR.Interaction.Toolkit.Interactables.IXRHoverInteractable hoverInteractable && IsHovering(hoverInteractable))
				return;

			switch (m_dropBehavior)
			{
				case DropBehavior.Unassign:
					holstered = null;
					break;

				case DropBehavior.Autoholster:
					// Wait for delay before reholstering
					try
					{
						m_reholsterCancellationTokenSource = new CancellationTokenSource();
						await Task.Delay(TimeSpan.FromSeconds(m_autoholsterDelay), m_reholsterCancellationTokenSource.Token);
					}
					// Item was grabbed, canceling timer, skip holstering
					catch { return; }

					HolsterItem();
					break;
			}
		}

		private void HolsterItem()
		{
			if (!hasHolstered)
				return;

			// Currently grabbed, skip
			if (m_holstered.isSelected)
				return;

			// Grab holstered interactable
			m_canSelect = true;
			m_holstered.transform.SetPositionAndRotation(attachTransform.position, attachTransform.rotation);
			interactionManager.SelectEnter(this, m_holstered);
		}

		#endregion
	}
}