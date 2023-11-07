using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRInteractableActivatedMessageListener : MessageListener
    {
        private void Start() => GetComponent<XRBaseInteractable>()?.activated.AddListener((value) =>
        {
            EventBus.Trigger(EventHooks.OnXRInteractableActivated, gameObject, value);
        });
    }
}