using Unity.VisualScripting;
using System;

namespace ToolkitEngine.XR.VisualScripting
{
	[UnitTitle("On Plugged"), UnitSurtitle("XRBaseInteractable")]
	public class OnXRInteractablePlugged : XRBaseInteractableEventUnit
	{
		public override Type MessageListenerType => typeof(OnXRInteractablePluggedMessageListener);
	}
}
