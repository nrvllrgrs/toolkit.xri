using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UnityEngine.XR.Interaction.Toolkit.Filtering;

namespace ToolkitEngine.XR
{
	public class XRPriorityTargetFilter : XRBaseTargetFilter
	{
		#region Fields

		[SerializeField]
		private XRBaseTargetFilter m_targetFilter;

		#endregion

		#region Methods

		public override void Process(UnityEngine.XR.Interaction.Toolkit.Interactors.IXRInteractor interactor, List<UnityEngine.XR.Interaction.Toolkit.Interactables.IXRInteractable> targets, List<UnityEngine.XR.Interaction.Toolkit.Interactables.IXRInteractable> results)
		{
			if (targets == null || targets.Count == 0)
			{
				results = new List<UnityEngine.XR.Interaction.Toolkit.Interactables.IXRInteractable>();
				return;
			}

			var r = from t in targets
					let p = t.transform.GetComponent<XRInteractablePriority>()
					group t by p?.priority ?? 0 into g
					orderby g.Key descending
					select g;

			var list = new List<UnityEngine.XR.Interaction.Toolkit.Interactables.IXRInteractable>(r.First());
			if (m_targetFilter == null)
			{
				results = list;
			}
			else
			{
				m_targetFilter.Process(interactor, list, results);
			}

			// Add the rest of the targets to the processed results
			foreach (var l in r.Skip(1))
			{
				results.AddRange(l);
			}
		}

		#endregion
	}
}