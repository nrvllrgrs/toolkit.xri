using UnityEngine;
using Unity.VisualScripting;


namespace ToolkitEngine.XR.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRInteractableSelectEntereddMessageListener : MessageListener
    {
        private void Start() => GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>()?.selectEntered.AddListener((value) =>
        {
            EventBus.Trigger(EventHooks.OnXRInteractableSelectEntered, gameObject, value);
        });
    }
}