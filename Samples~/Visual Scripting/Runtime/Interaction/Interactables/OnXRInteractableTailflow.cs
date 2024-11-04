using Unity.VisualScripting;
using System;

namespace ToolkitEngine.XR.VisualScripting
{
    [UnitTitle("On Tailflow"), UnitSurtitle("XRPositionInteractable")]
    public class OnXRInteractableTailflow : XRBaseInteractableEventUnit
    {
        public override Type MessageListenerType => typeof(OnXRInteractableTailflowMessageListener);
    }
}