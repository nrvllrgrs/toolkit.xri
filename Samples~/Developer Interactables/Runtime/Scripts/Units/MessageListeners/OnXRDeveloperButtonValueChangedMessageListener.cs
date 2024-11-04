using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRDeveloperButtonValueChangedMessageListener : MessageListener
    {
        private void Start() => GetComponent<XRDeveloperButton>()?.onValueChanged.AddListener((value) =>
        {
            EventBus.Trigger(DeveloperEventHooks.OnXRDeveloperButtonValueChanged, gameObject, value);
        });
    }
}