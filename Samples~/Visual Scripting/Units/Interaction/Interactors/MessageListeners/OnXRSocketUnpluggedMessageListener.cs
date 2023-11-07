using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRSocketUnpluggedMessageListener : MessageListener
    {
        private void Start() => GetComponent<XRSocketInteractor>()?.selectExited.AddListener((value) =>
        {
            EventBus.Trigger(EventHooks.OnXRSocketUnplugged, gameObject, value);
        });
    }
}