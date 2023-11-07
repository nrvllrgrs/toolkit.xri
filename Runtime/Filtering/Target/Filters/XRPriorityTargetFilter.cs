using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
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

		public override void Process(IXRInteractor interactor, List<IXRInteractable> targets, List<IXRInteractable> results)
		{
			if (targets == null || targets.Count == 0)
			{
				results = new List<IXRInteractable>();
				return;
			}

			var r = from t in targets
					let p = t.transform.GetComponent<XRInteractablePriority>()
					group t by p?.priority ?? 0 into g
					orderby g.Key descending
					select g;

			var list = new List<IXRInteractable>(r.First());
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