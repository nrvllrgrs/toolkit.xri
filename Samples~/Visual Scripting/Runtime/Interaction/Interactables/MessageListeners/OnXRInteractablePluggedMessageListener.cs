using UnityEngine;
using Unity.VisualScripting;


namespace ToolkitEngine.XR.VisualScripting
{
	[AddComponentMenu("")]
	public class OnXRInteractablePluggedMessageListener : MessageListener
	{
        private void Start()
        {
            GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>()?.selectEntered.AddListener((args) =>
            {
                if (args.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor)
                {
                    EventBus.Trigger(EventHooks.OnXRInteractablePlugged, gameObject, args);
                }
            });
        }
	}
}