using Unity.VisualScripting;
using System;

namespace ToolkitEngine.XR.VisualScripting
{
    [UnitTitle("On Select Exited"), UnitSurtitle("XRBaseInteractable")]
    public class OnXRInteractableSelectExited : XRBaseInteractableEventUnit
    {
        public override Type MessageListenerType => typeof(OnXRInteractableSelectExitedMessageListener);
    }
}