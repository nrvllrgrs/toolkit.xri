using UnityEngine;
using Unity.VisualScripting;


namespace ToolkitEngine.XR.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRInteractorHoverExitedMessageListener : MessageListener
    {
        private void Start() => GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.IXRHoverInteractor>()?.hoverExited.AddListener((value) =>
        {
            EventBus.Trigger(EventHooks.OnXRInteractorHoverExited, gameObject, value);
        });
    }
}