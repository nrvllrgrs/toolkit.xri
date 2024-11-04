using UnityEngine;
using Unity.VisualScripting;


namespace ToolkitEngine.XR.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRSocketPluggedMessageListener : MessageListener
    {
        private void Start() => GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>()?.selectEntered.AddListener((value) =>
        {
            EventBus.Trigger(EventHooks.OnXRSocketPlugged, gameObject, value);
        });
    }
}