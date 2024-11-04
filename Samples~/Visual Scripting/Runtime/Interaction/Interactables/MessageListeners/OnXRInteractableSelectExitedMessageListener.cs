using UnityEngine;
using Unity.VisualScripting;


namespace ToolkitEngine.XR.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRInteractableSelectExitedMessageListener : MessageListener
    {
        private void Start() => GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>()?.selectExited.AddListener((value) =>
        {
            EventBus.Trigger(EventHooks.OnXRInteractableSelectExited, gameObject, value);
        });
    }
}