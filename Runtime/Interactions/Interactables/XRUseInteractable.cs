using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR
{
	public class XRUseInteractable : UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable
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
			if (args.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor selectInteractor
				&& args.interactableObject is UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable selectInteractable
				&& !selectInteractor.IsSelecting(selectInteractable))
			{
				interactionManager.SelectEnter(selectInteractor, selectInteractable);
			}
		}

		private bool CanDrop(BaseInteractionEventArgs args)
		{
			return args.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.IXRHoverInteractor hoverInteractor
				&& args.interactableObject is UnityEngine.XR.Interaction.Toolkit.Interactables.IXRHoverInteractable hoverInteractable
				&& !hoverInteractor.IsHovering(hoverInteractable);
		}

		private void Drop(BaseInteractionEventArgs args)
		{
			if (args.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor selectInteractor
				&& args.interactableObject is UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable selectInteractable
				&& selectInteractor.IsSelecting(selectInteractable))
			{
				interactionManager.SelectExit(selectInteractor, selectInteractable);
			}
		}

		private void Unuse(BaseInteractionEventArgs args)
		{
			if (args.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.IXRActivateInteractor activateInteractor
				&& args.interactableObject is UnityEngine.XR.Interaction.Toolkit.Interactables.IXRActivateInteractable activateInteractable)
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