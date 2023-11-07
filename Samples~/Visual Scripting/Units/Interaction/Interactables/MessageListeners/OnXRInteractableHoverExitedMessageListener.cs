using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRInteractableHoverExitedMessageListener : MessageListener
    {
        private void Start() => GetComponent<XRBaseInteractable>()?.hoverExited.AddListener((value) =>
        {
            EventBus.Trigger(EventHooks.OnXRInteractableHoverExited, gameObject, value);
        });
    }
}