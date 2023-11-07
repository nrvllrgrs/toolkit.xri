using Unity.VisualScripting;
using System;

namespace ToolkitEngine.XR.VisualScripting
{
    [UnitTitle("On Deactivated"), UnitSurtitle("XRBaseInteractable")]
    public class OnXRInteractableDeactivated : XRBaseInteractableEventUnit
    {
        public override Type MessageListenerType => typeof(OnXRInteractableDeactivatedMessageListener);
    }
}