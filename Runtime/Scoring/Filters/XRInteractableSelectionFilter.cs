using UnityEngine;
using ToolkitEngine.Scoring;

namespace ToolkitEngine.XR
{
	[EvaluableCategory("XR")]
	public class XRInteractableSelectionFilter : BaseFilter
	{
		#region Fields

		[SerializeField]
		private bool m_isSelected;

		#endregion

		#region Methods

		protected override bool IsIncluded(GameObject actor, GameObject target, Vector3 position)
		{
			var selectInteractable = target.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable>();
			if (selectInteractable == null)
				return false;

			return m_isSelected == selectInteractable.isSelected;
		}

		#endregion
	}
}