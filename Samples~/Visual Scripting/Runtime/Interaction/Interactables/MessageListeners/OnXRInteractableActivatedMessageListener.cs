using UnityEngine;
using Unity.VisualScripting;


namespace ToolkitEngine.XR.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRInteractableActivatedMessageListener : MessageListener
    {
        private void Start() => GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>()?.activated.AddListener((value) =>
        {
            EventBus.Trigger(EventHooks.OnXRInteractableActivated, gameObject, value);
        });
    }
}