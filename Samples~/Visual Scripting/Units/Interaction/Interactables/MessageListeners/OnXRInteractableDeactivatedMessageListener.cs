using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRInteractableDeactivatedMessageListener : MessageListener
    {
        private void Start() => GetComponent<XRBaseInteractable>()?.deactivated.AddListener((value) =>
        {
            EventBus.Trigger(EventHooks.OnXRInteractableDeactivated, gameObject, value);
        });
    }
}