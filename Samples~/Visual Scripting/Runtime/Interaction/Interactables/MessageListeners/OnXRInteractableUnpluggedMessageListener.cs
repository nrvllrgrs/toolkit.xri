using UnityEngine;
using Unity.VisualScripting;


namespace ToolkitEngine.XR.VisualScripting
{
	[AddComponentMenu("")]
	public class OnXRInteractableUnpluggedMessageListener : MessageListener
	{
		private void Start()
		{
			GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>()?.selectExited.AddListener((args) =>
			{
				if (args.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor)
				{
					EventBus.Trigger(EventHooks.OnXRInteractableUnplugged, gameObject, args);
				}
			});
		}
	}
}