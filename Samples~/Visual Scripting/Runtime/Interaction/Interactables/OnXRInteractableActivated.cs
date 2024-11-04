using Unity.VisualScripting;
using System;

namespace ToolkitEngine.XR.VisualScripting
{
    [UnitTitle("On Activated"), UnitSurtitle("XRBaseInteractable")]
    public class OnXRInteractableActivated : XRBaseInteractableEventUnit
    {
        public override Type MessageListenerType => typeof(OnXRInteractableActivatedMessageListener);
    }
}