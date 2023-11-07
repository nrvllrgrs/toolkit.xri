using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRInteractableFirstHoverEnteredMessageListener : MessageListener
    {
        private void Start() => GetComponent<XRBaseInteractable>()?.firstHoverEntered.AddListener((value) =>
        {
            EventBus.Trigger(EventHooks.OnXRInteractableFirstHoverEntered, gameObject, value);
        });
    }
}