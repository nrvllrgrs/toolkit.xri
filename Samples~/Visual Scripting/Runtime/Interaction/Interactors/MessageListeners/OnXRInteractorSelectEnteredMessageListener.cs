using UnityEngine;
using Unity.VisualScripting;


namespace ToolkitEngine.XR.VisualScripting
{
    [AddComponentMenu("")]
    public class OnXRInteractorSelectEnteredMessageListener : MessageListener
    {
		private void Start() => GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor>()?.selectEntered.AddListener((value) =>
		{
			EventBus.Trigger(nameof(OnXRInteractorSelectEntered), gameObject, value);
		});
	}
}