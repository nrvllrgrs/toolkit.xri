using Unity.VisualScripting;
using System;

namespace ToolkitEngine.XR.VisualScripting
{
    [UnitTitle("On Tipflow"), UnitSurtitle("XRPositionInteractable")]
    public class OnXRInteractableTipflow : XRBaseInteractableEventUnit
    {
        public override Type MessageListenerType => typeof(OnXRInteractableTipflowMessageListener);
    }
}