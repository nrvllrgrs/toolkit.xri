using UnityEngine;
using Unity.VisualScripting;


namespace ToolkitEngine.XR.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRInteractableFirstHoverEnteredMessageListener : MessageListener
    {
        private void Start() => GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>()?.firstHoverEntered.AddListener((value) =>
        {
            EventBus.Trigger(EventHooks.OnXRInteractableFirstHoverEntered, gameObject, value);
        });
    }
}