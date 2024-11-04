using UnityEngine;
using Unity.VisualScripting;


namespace ToolkitEngine.XR.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRSocketUnpluggedMessageListener : MessageListener
    {
        private void Start() => GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>()?.selectExited.AddListener((value) =>
        {
            EventBus.Trigger(EventHooks.OnXRSocketUnplugged, gameObject, value);
        });
    }
}