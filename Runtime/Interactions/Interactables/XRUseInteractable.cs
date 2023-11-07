using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR
{
	public class XRUseInteractable : XRBaseInteractable
    {
		#region Fields

		[SerializeField]
		private bool m_deactivateOnHoverExit = true;

		#endregion

		#region Methods

		protected override void OnHoverEntered(HoverEnterEventArgs args)
		{
			base.OnHoverEntered(args);

			// Touch (i.e. hover) needs to happen before grab
			Grab(args);
		}

		protected override void OnHoverExiting(HoverExitEventArgs args)
		{
			// Unuse and drop before untouching interactable
			if (m_deactivateOnHoverExit)
			{
				Unuse(args);
			}
			Drop(args);

			base.OnHoverExiting(args);
		}

		protected override void OnSelectExited(SelectExitEventArgs args)
		{
			base.OnSelectExited(args);

			if (!CanDrop(args))
			{
				Grab(args);
			}
		}

		private void Grab(BaseInteractionEventArgs args)
		{
			if (args.interactorObject is IXRSelectInteractor selectInteractor
				&& args.interactableObject is IXRSelectInteractable selectInteractable
				&& !selectInteractor.IsSelecting(selectInteractable))
			{
				interactionManager.SelectEnter(selectInteractor, selectInteractable);
			}
		}

		private bool CanDrop(BaseInteractionEventArgs args)
		{
			return args.interactorObject is IXRHoverInteractor hoverInteractor
				&& args.interactableObject is IXRHoverInteractable hoverInteractable
				&& !hoverInteractor.IsHovering(hoverInteractable);
		}

		private void Drop(BaseInteractionEventArgs args)
		{
			if (args.interactorObject is IXRSelectInteractor selectInteractor
				&& args.interactableObject is IXRSelectInteractable selectInteractable
				&& selectInteractor.IsSelecting(selectInteractable))
			{
				interactionManager.SelectExit(selectInteractor, selectInteractable);
			}
		}

		private void Unuse(BaseInteractionEventArgs args)
		{
			if (args.interactorObject is IXRActivateInteractor activateInteractor
				&& args.interactableObject is IXRActivateInteractable activateInteractable)
			{
				activateInteractable.OnDeactivated(new DeactivateEventArgs()
				{
					interactorObject = activateInteractor,
					interactableObject = activateInteractable
				});
			}
		}

		#endregion
	}
}