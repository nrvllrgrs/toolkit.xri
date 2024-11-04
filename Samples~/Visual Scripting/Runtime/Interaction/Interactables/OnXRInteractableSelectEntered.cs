using Unity.VisualScripting;
using System;

namespace ToolkitEngine.XR.VisualScripting
{
    [UnitTitle("On Select Entered"), UnitSurtitle("XRBaseInteractable")]
    public class OnXRInteractableSelectEntered : XRBaseInteractableEventUnit
    {
        public override Type MessageListenerType => typeof(OnXRInteractableSelectEntereddMessageListener);
    }
}