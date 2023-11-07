using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR.VisualScripting
{
	[AddComponentMenu("")]
	public class OnXRInteractableUnpluggedMessageListener : MessageListener
	{
		private void Start()
		{
			GetComponent<XRBaseInteractable>()?.selectExited.AddListener((args) =>
			{
				if (args.interactorObject is XRSocketInteractor)
				{
					EventBus.Trigger(EventHooks.OnXRInteractableUnplugged, gameObject, args);
				}
			});
		}
	}
}