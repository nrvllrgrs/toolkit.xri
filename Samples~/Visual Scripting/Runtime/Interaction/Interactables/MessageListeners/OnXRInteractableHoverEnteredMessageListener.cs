using UnityEngine;
using Unity.VisualScripting;


namespace ToolkitEngine.XR.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRInteractableHoverEnteredMessageListener : MessageListener
    {
        private void Start() => GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>()?.hoverEntered.AddListener((value) =>
        {
            EventBus.Trigger(EventHooks.OnXRInteractableHoverEntered, gameObject, value);
        });
    }
}