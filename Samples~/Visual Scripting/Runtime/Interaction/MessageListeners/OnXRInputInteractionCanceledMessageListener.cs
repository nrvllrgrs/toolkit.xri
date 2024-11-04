using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.XR.VisualScripting
{
	[AddComponentMenu("")]
	public class OnXRInputInteractionCanceledMessageListener : MessageListener
	{
		private void Start() => GetComponent<XRBaseInputEvents>()?.onCanceled.AddListener((value) =>
		{
			EventBus.Trigger(EventHooks.OnXRInputInteractionCanceled, gameObject, value);
		});
	}
}