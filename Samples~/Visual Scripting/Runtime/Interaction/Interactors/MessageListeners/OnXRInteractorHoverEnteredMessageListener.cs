using UnityEngine;
using Unity.VisualScripting;


namespace ToolkitEngine.XR.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRInteractorHoverMessageListener : MessageListener
    {
        private void Start() => GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.IXRHoverInteractor>()?.hoverEntered.AddListener((value) =>
        {
            EventBus.Trigger(EventHooks.OnXRInteractorHoverEntered, gameObject, value);
        });
    }
}