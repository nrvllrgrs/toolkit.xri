using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRDeveloperButtonDisabledMessageListener : MessageListener
    {
        private void Start() => GetComponent<XRDeveloperButton>()?.onDisabled.AddListener(() =>
        {
            EventBus.Trigger(DeveloperEventHooks.OnXRDeveloperButtonDisabled, gameObject);
        });
    }
}