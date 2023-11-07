using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.XR.VisualScripting
{
	[AddComponentMenu("")]
	public class OnXRInputInteractionPerformedMessageListener : MessageListener
	{
		private void Start() => GetComponent<XRBaseInputEvents>()?.onPerformed.AddListener((value) =>
		{
			EventBus.Trigger(EventHooks.OnXRInputInteractionPerformed, gameObject, value);
		});
	}
}