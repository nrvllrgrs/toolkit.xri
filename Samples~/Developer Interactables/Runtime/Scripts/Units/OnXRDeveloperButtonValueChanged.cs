using System;
using Unity.VisualScripting;

namespace ToolkitEngine.VisualScripting
{
    [UnitCategory("Events/Developer")]
    [UnitTitle("On Button Value Changed")]
    public class OnXRDeveloperButtonValueChanged : BaseEventUnit<bool>
    {
        public override Type MessageListenerType => typeof(OnXRDeveloperButtonValueChangedMessageListener);
    }
}