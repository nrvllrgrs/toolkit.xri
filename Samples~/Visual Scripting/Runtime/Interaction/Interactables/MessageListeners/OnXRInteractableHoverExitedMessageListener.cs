using UnityEngine;
using Unity.VisualScripting;


namespace ToolkitEngine.XR.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRInteractableHoverExitedMessageListener : MessageListener
    {
        private void Start() => GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>()?.hoverExited.AddListener((value) =>
        {
            EventBus.Trigger(EventHooks.OnXRInteractableHoverExited, gameObject, value);
        });
    }
}