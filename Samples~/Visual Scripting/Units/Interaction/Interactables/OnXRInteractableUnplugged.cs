using Unity.VisualScripting;
using System;

namespace ToolkitEngine.XR.VisualScripting
{
	[UnitTitle("On Unplugged"), UnitSurtitle("XRBaseInteractable")]
	public class OnXRInteractableUnplugged : XRBaseInteractableEventUnit
	{
		public override Type MessageListenerType => typeof(OnXRInteractableUnpluggedMessageListener);
	}
}
