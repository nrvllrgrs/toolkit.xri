using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRInteractableSelectExitedMessageListener : MessageListener
    {
        private void Start() => GetComponent<XRBaseInteractable>()?.selectExited.AddListener((value) =>
        {
            EventBus.Trigger(EventHooks.OnXRInteractableSelectExited, gameObject, value);
        });
    }
}