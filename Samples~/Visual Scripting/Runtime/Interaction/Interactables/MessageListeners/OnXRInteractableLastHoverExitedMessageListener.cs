using UnityEngine;
using Unity.VisualScripting;


namespace ToolkitEngine.XR.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRInteractableLastHoverExitedMessageListener : MessageListener
    {
        private void Start() => GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>()?.lastHoverExited.AddListener((value) =>
        {
            EventBus.Trigger(EventHooks.OnXRInteractableLastHoverExited, gameObject, value);
        });
    }
}