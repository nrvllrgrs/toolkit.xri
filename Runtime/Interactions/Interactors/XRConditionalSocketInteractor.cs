using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using ToolkitEngine.Scoring;

namespace ToolkitEngine.XR
{
	public class XRConditionalSocketInteractor : UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor
    {
		#region Fields

		[SerializeField]
		protected bool m_selectOnHover = false;

		[SerializeField, Tooltip("Interactable that is selected by the \"dropping\" interactor when it is socketed.")]
		protected UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable m_transferInteractable;

		/// <summary>
		/// Indicates whether socket uses conditionals to when selecting interactables.
		/// </summary>
		[SerializeField, Tooltip("Indicates whether socket uses conditionals to when selecting interactables.")]
		protected bool m_useConditionals;

		[SerializeField]
		protected UnityEvaluator m_interactorFilter = new UnityEvaluator();

		[SerializeField, Tooltip("Interactable filter")]
		protected UnityEvaluator m_interactableFilter = new UnityEvaluator();

		/// <summary>
		/// Indicates whether interactor can select interactables
		/// </summary>
		protected bool m_canSelect;

		#endregion

		#region Properties

		public override bool isSelectActive => m_canSelect && base.isSelectActive;

		#endregion

		#region Methods

		protected override void OnHoverEntering(HoverEnterEventArgs e)
		{
			base.OnHoverEntering(e);

			// Something already in socket, skip
			if (hasSelection)
				return;

			if (m_useConditionals)
			{
				if (e.interactorObject is not UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor selectInteractor
					|| e.interactableObject is not UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable selectInteractable
					|| m_interactorFilter.Evaluate(gameObject, selectInteractor.transform.gameObject) == 0
					|| m_interactableFilter.Evaluate(gameObject, selectInteractable.transform.gameObject) == 0)
				{
					m_canSelect = false;
					return;
				}
			}		

			m_canSelect = true;
		}

		protected override void OnHoverEntered(HoverEnterEventArgs e)
		{
			base.OnHoverEntered(e);

			if (m_selectOnHover
				&& isSelectActive
				&& e.interactableObject is UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable selectInteractable)
			{
				if (selectInteractable.isSelected)
				{
					var interactor = selectInteractable.firstInteractorSelecting;
					e.manager.SelectExit(interactor, selectInteractable);

					if (m_transferInteractable != null)
					{
						e.manager.SelectEnter(interactor, m_transferInteractable);
					}
				}
				e.manager.SelectEnter(this, selectInteractable);
			}
		}

		protected override void OnHoverExiting(HoverExitEventArgs e)
		{
			base.OnHoverExiting(e);

			var selectInteractable = e.interactableObject as UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable;
			if (selectInteractable == null)
				return;

			if (!hasSelection)
			{
				m_canSelect = false;
			}
		}

		public void ForceSelectEnter(UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable interactable)
		{
			if (interactable == null)
				return;

			if (interactable.isSelected)
			{
				if (interactable.IsSelectableBy(this))
					return;

				foreach (var interactor in interactable.interactorsSelecting)
				{
					interactionManager.SelectExit(interactor, interactable);
				}
			}

			m_canSelect = true;
			interactionManager.SelectEnter(this, interactable);
		}

		#endregion
	}
}