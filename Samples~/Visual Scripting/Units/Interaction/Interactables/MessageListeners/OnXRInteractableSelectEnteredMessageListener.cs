using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRInteractableSelectEntereddMessageListener : MessageListener
    {
        private void Start() => GetComponent<XRBaseInteractable>()?.selectEntered.AddListener((value) =>
        {
            EventBus.Trigger(EventHooks.OnXRInteractableSelectEntered, gameObject, value);
        });
    }
}