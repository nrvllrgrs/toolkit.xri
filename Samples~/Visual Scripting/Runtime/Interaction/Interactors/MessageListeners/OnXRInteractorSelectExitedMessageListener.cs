using UnityEngine;
using Unity.VisualScripting;


namespace ToolkitEngine.XR.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRInteractorSelectExitedMessageListener : MessageListener
    {
        private void Start() => GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor>()?.selectExited.AddListener((value) =>
        {
            EventBus.Trigger(nameof(OnXRInteractorSelectExited), gameObject, value);
        });
    }
}