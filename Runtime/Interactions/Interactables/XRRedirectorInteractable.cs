using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR
{
	public class XRRedirectorInteractable : UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable
    {
		#region Fields

		[SerializeField]
		private UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable m_startingTargetInteractable;

		private UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable m_targetInteractable;

		#endregion

		#region Properties

		public bool hasTarget => m_targetInteractable != null;

		public UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable targetInteractable
		{
			get => m_targetInteractable;
			set
			{
				// No change, skip
				if (m_targetInteractable == value)
					return;

				m_targetInteractable = value;
			}
		}

		#endregion

		#region Methods

		protected override void Awake()
		{
			base.Awake();
			m_targetInteractable = m_startingTargetInteractable;
		}

		protected override void OnSelectEntering(SelectEnterEventArgs e)
		{
			base.OnSelectEntering(e);
			e.manager.SelectCancel(e.interactorObject, e.interactableObject);

			if (hasTarget)
			{
				e.manager.SelectEnter(e.interactorObject, m_targetInteractable);
			}
		}

		protected override void OnSelectEntered(SelectEnterEventArgs e)
		{
			// Intentionally left blank
		}

		#endregion
	}
}