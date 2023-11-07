using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR.VisualScripting
{
	[AddComponentMenu("")]
	public class OnXRInteractablePluggedMessageListener : MessageListener
	{
        private void Start()
        {
            GetComponent<XRBaseInteractable>()?.selectEntered.AddListener((args) =>
            {
                if (args.interactorObject is XRSocketInteractor)
                {
                    EventBus.Trigger(EventHooks.OnXRInteractablePlugged, gameObject, args);
                }
            });
        }
	}
}