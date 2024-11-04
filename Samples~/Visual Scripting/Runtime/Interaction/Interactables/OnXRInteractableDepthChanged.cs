using Unity.VisualScripting;
using System;

namespace ToolkitEngine.XR.VisualScripting
{
    [UnitTitle("On Depth Changed"), UnitSurtitle("XRPositionInteractable")]
    public class OnXRInteractableDepthChanged : XRBaseInteractableEventUnit
    {
        public override Type MessageListenerType => typeof(OnXRInteractableDepthChangedMessageListener);
    }
}