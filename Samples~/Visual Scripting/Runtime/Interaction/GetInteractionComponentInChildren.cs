using System;
using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.XR.VisualScripting
{
	[UnitCategory("XR/Interactions")]
	public class GetInteractionComponentInChildren : GetInteractionComponent
	{
		#region Fields

		[DoNotSerialize]
		public ValueInput includeInactive;

		#endregion

		#region Methods

		protected override void Definition()
		{
			base.Definition();
			includeInactive = ValueInput(nameof(includeInactive), false);
		}

		protected override Component GetComponent(Flow flow, Transform transform, Type type)
		{
			bool _includeInactive = flow.GetValue<bool>(includeInactive);
			return transform.GetComponentInChildren(type, _includeInactive);
		}

		#endregion
	}
}