using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR
{
	public static class XRBaseInteractableExt
	{
		public static void InvokeActivate(this UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable)
		{
			if (!interactable.isSelected)
				return;

			interactable.activated.Invoke(new ActivateEventArgs()
			{
				interactorObject = interactable.firstInteractorSelecting as UnityEngine.XR.Interaction.Toolkit.Interactors.IXRActivateInteractor,
				interactableObject = interactable,
			});
		}

		public static void InvokeDeactivate(this UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable)
		{
			if (!interactable.isSelected)
				return;

			interactable.deactivated.Invoke(new DeactivateEventArgs()
			{
				interactorObject = interactable.firstInteractorSelecting as UnityEngine.XR.Interaction.Toolkit.Interactors.IXRActivateInteractor,
				interactableObject = interactable,
			});
		}
	}
}