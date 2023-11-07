using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.XR.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRInteractableTipflowMessageListener : MessageListener
    {
        private void Start() => GetComponent<XRBasePositionInteractable>()?.onTipflow.AddListener((value) =>
        {
            EventBus.Trigger(EventHooks.OnXRInteractableTipflow, gameObject, value);
        });
    }
}