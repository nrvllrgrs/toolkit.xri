using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRInteractorHoverExitedMessageListener : MessageListener
    {
        private void Start() => GetComponent<XRBaseInteractor>()?.hoverExited.AddListener((value) =>
        {
            EventBus.Trigger(EventHooks.OnXRInteractorHoverExited, gameObject, value);
        });
    }
}