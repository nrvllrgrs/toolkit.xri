using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace ToolkitEngine.XR
{
	public class XRInteractorCollectionEvents : MonoBehaviour, IXRHoverInteractor, IXRSelectInteractor
	{
		#region Fields

		[SerializeField]
		protected UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor[] m_interactors;

		[SerializeField]
		private HoverEnterEvent m_hoverEntered = new();

		[SerializeField]
		private HoverExitEvent m_hoverExited = new();

		[SerializeField]
		private SelectEnterEvent m_selectEntered = new();

		[SerializeField]
		private SelectExitEvent m_selectExited = new();

		#endregion

		#region Events

		public event Action<InteractorRegisteredEventArgs> registered;
		public event Action<InteractorUnregisteredEventArgs> unregistered;

		public HoverEnterEvent hoverEntered => m_hoverEntered;
		public HoverExitEvent hoverExited => m_hoverExited;
		public SelectEnterEvent selectEntered => m_selectEntered;
		public SelectExitEvent selectExited => m_selectExited;

		#endregion

		#region Properties

		public InteractionLayerMask interactionLayers => throw new NotImplementedException();

		public List<UnityEngine.XR.Interaction.Toolkit.Interactables.IXRHoverInteractable> interactablesHovered => throw new NotImplementedException();

		public bool hasHover => throw new NotImplementedException();

		public bool isHoverActive => throw new NotImplementedException();

		public List<UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable> interactablesSelected => throw new NotImplementedException();

		public UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable firstInteractableSelected => throw new NotImplementedException();

		public bool hasSelection => throw new NotImplementedException();

		public bool isSelectActive => throw new NotImplementedException();

		public bool keepSelectedTargetValid => throw new NotImplementedException();

		public InteractorHandedness handedness => throw new NotImplementedException();

		#endregion

		#region Methods

		private void OnEnable()
		{
			foreach (var interactor in m_interactors)
			{
				interactor.hoverEntered.AddListener(Interactor_HoverEntered);
				interactor.hoverExited.AddListener(Interactor_HoverExited);
				interactor.selectEntered.AddListener(Interactor_SelectEntered);
				interactor.selectExited.AddListener(Interactor_SelectExited);
			}
		}

		private void OnDisable()
		{
			foreach (var interactor in m_interactors)
			{
				interactor.hoverEntered.RemoveListener(Interactor_HoverEntered);
				interactor.hoverExited.RemoveListener(Interactor_HoverExited);
				interactor.selectEntered.RemoveListener(Interactor_SelectEntered);
				interactor.selectExited.RemoveListener(Interactor_SelectExited);
			}
		}

		private void Interactor_HoverEntered(HoverEnterEventArgs e) => m_hoverEntered?.Invoke(e);
		private void Interactor_HoverExited(HoverExitEventArgs e) => m_hoverExited?.Invoke(e);
		private void Interactor_SelectEntered(SelectEnterEventArgs e) => m_selectEntered?.Invoke(e);
		private void Interactor_SelectExited(SelectExitEventArgs e) => m_selectExited?.Invoke(e);

		#endregion

		#region IXRInteractor Methods

		public void OnRegistered(InteractorRegisteredEventArgs args)
		{ }

		public void OnUnregistered(InteractorUnregisteredEventArgs args)
		{ }

		public Transform GetAttachTransform(UnityEngine.XR.Interaction.Toolkit.Interactables.IXRInteractable interactable) => null;

		public void GetValidTargets(List<UnityEngine.XR.Interaction.Toolkit.Interactables.IXRInteractable> targets)
		{ }

		public void PreprocessInteractor(XRInteractionUpdateOrder.UpdatePhase updatePhase)
		{ }

		public void ProcessInteractor(XRInteractionUpdateOrder.UpdatePhase updatePhase)
		{ }

		#endregion

		#region IXRHoverInteractor Methods

		public bool CanHover(UnityEngine.XR.Interaction.Toolkit.Interactables.IXRHoverInteractable interactable)
		{
			return m_interactors.Any(x => x.CanHover(interactable));
		}

		public bool IsHovering(UnityEngine.XR.Interaction.Toolkit.Interactables.IXRHoverInteractable interactable)
		{
			return m_interactors.Any(x => x.IsHovering(interactable));
		}

		public void OnHoverEntering(HoverEnterEventArgs args)
		{ }

		public void OnHoverEntered(HoverEnterEventArgs args)
		{ }

		public void OnHoverExiting(HoverExitEventArgs args)
		{ }

		public void OnHoverExited(HoverExitEventArgs args)
		{ }

		#endregion

		#region IXRSelectInterator Methods

		public bool CanSelect(UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable interactable)
		{
			return m_interactors.Any(x => x.CanSelect(interactable));
		}

		public bool IsSelecting(UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable interactable)
		{
			return m_interactors.Any(x => x.IsSelecting(interactable));
		}

		public Pose GetAttachPoseOnSelect(UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable interactable) => default;

		public Pose GetLocalAttachPoseOnSelect(UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable interactable) => default;

		public void OnSelectEntering(SelectEnterEventArgs args)
		{ }

		public void OnSelectEntered(SelectEnterEventArgs args)
		{ }

		public void OnSelectExiting(SelectExitEventArgs args)
		{ }

		public void OnSelectExited(SelectExitEventArgs args)
		{ }

		#endregion
	}
}