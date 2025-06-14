using ToolkitEngine.Scoring;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Filtering;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace ToolkitEngine.XR.Filtering
{
    public class XRUnityEvaluatorFilter : MonoBehaviour, IXRSelectFilter, IXRHoverFilter
    {
		#region Fields

		[SerializeField]
		private UnityEvaluator m_evaluator = new();

		#endregion

		#region Properties

		public bool canProcess => m_evaluator.weight > 0f;

		#endregion

		#region Methods

		public bool Process(IXRHoverInteractor interactor, IXRHoverInteractable interactable)
		{
			return m_evaluator.Evaluate(interactor.transform.gameObject, interactable.transform.gameObject) > 0f;
		}

		public bool Process(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
		{
			return m_evaluator.Evaluate(interactor.transform.gameObject, interactable.transform.gameObject) > 0f;
		}

		#endregion
	}
}