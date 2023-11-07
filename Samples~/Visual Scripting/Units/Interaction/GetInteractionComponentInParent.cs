using System;
using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.XR.VisualScripting
{
	[UnitCategory("XR/Interactions")]
	public class GetInteractionComponentInParent : GetInteractionComponent
	{
		#region Methods

		protected override Component GetComponent(Flow flow, Transform transform, Type type)
		{
			return transform.GetComponentInParent(type);
		}

		#endregion
	}
}