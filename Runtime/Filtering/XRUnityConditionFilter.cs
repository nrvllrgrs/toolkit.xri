using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Filtering;

namespace ToolkitEngine.XR
{
	public class XRUnityConditionFilter : MonoBehaviour, IXRHoverFilter, IXRSelectFilter
	{
		#region Fields

		[SerializeField, Tooltip("If ALL conditions are true, then interaction can occur. If empty, return true.")]
		private UnityCondition m_predicate = new UnityCondition(UnityCondition.ConditionType.All);

		#endregion

		#region Properties

		public bool canProcess => true;

		#endregion

		#region Methods

		public bool Process(IXRHoverInteractor interactor, IXRHoverInteractable interactable)
		{
			return m_predicate.isTrueAndEnabled;
		}

		public bool Process(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
		{
			return m_predicate.isTrueAndEnabled;
		}

		#endregion
	}
}
