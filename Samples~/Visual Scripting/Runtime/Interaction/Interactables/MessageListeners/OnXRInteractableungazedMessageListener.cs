using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR.VisualScripting
{
	[AddComponentMenu("")]
	public class OnXRInteractableUngazedMessageListener : MessageListener
	{
		private void Start()
		{
			var interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
			if (interactable != null)
			{
				interactable.hoverExited.AddListener(Trigger);
				interactable.selectExited.AddListener(Trigger);
			}
		}

		private void Trigger(BaseInteractionEventArgs args)
		{
			EventBus.Trigger(EventHooks.OnXRInteractableUngazed, gameObject, args);
		}
	}
}
