using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.XR.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRInteractableTailflowMessageListener : MessageListener
    {
        private void Start() => GetComponent<XRBasePositionInteractable>()?.onTailflow.AddListener((value) =>
        {
            EventBus.Trigger(EventHooks.OnXRInteractableTailflow, gameObject, value);
        });
    }
}