using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRInteractableLastHoverExitedMessageListener : MessageListener
    {
        private void Start() => GetComponent<XRBaseInteractable>()?.lastHoverExited.AddListener((value) =>
        {
            EventBus.Trigger(EventHooks.OnXRInteractableLastHoverExited, gameObject, value);
        });
    }
}