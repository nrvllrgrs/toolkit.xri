using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRInteractorHoverMessageListener : MessageListener
    {
        private void Start() => GetComponent<XRBaseInteractor>()?.hoverEntered.AddListener((value) =>
        {
            EventBus.Trigger(EventHooks.OnXRInteractorHoverEntered, gameObject, value);
        });
    }
}