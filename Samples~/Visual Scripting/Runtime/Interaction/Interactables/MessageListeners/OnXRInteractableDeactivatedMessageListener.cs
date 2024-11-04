using UnityEngine;
using Unity.VisualScripting;


namespace ToolkitEngine.XR.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRInteractableDeactivatedMessageListener : MessageListener
    {
        private void Start() => GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>()?.deactivated.AddListener((value) =>
        {
            EventBus.Trigger(EventHooks.OnXRInteractableDeactivated, gameObject, value);
        });
    }
}