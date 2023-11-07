using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRSocketPluggedMessageListener : MessageListener
    {
        private void Start() => GetComponent<XRSocketInteractor>()?.selectEntered.AddListener((value) =>
        {
            EventBus.Trigger(EventHooks.OnXRSocketPlugged, gameObject, value);
        });
    }
}