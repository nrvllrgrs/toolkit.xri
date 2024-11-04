using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR.VisualScripting
{
	[AddComponentMenu("")]
	public class OnXRInteractableGazedMessageListener : MessageListener
	{
		private void Start()
		{
			var interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
			if (interactable != null)
			{
				interactable.hoverEntered.AddListener(Trigger);
				interactable.selectEntered.AddListener(Trigger);
			}
		}

		private void Trigger(BaseInteractionEventArgs args)
		{
			EventBus.Trigger(EventHooks.OnXRInteractableGazed, gameObject, args);
		}
	}
}
