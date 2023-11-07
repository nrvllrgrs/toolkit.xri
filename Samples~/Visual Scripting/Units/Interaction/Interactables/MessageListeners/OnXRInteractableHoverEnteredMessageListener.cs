using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRInteractableHoverEnteredMessageListener : MessageListener
    {
        private void Start() => GetComponent<XRBaseInteractable>()?.hoverEntered.AddListener((value) =>
        {
            EventBus.Trigger(EventHooks.OnXRInteractableHoverEntered, gameObject, value);
        });
    }
}