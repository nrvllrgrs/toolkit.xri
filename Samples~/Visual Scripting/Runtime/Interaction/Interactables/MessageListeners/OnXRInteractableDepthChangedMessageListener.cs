using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.XR.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRInteractableDepthChangedMessageListener : MessageListener
    {
        private void Start() => GetComponent<XRBasePositionInteractable>()?.onDepthChanged.AddListener((value) =>
        {
            EventBus.Trigger(EventHooks.OnXRInteractableDepthChanged, gameObject, value);
        });
    }
}