using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace ToolkitEngine.XR
{
	public class XRHandednessContext : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private List<InteractorPair> m_pairs;

        private Dictionary<IXRInteractor, IXRInteractor> m_opposites = new();

		#endregion

		#region Properties

		public IList<InteractorPair> pairs => m_pairs;

		#endregion

		#region Methods

		private void Awake()
		{
			foreach (var p in m_pairs)
            {
                if (p.left == null || p.right == null)
                    continue;

				Assert.AreEqual(p.left.handedness, InteractorHandedness.Left);
				Assert.AreEqual(p.right.handedness, InteractorHandedness.Right);

                m_opposites.Add(p.left, p.right);
                m_opposites.Add(p.right, p.left);
            }
		}

		public IXRInteractor GetOppositeInteractor(IXRInteractor interactor)
		{
			return m_opposites.TryGetValue(interactor, out var opposite)
				? opposite
				: null;
		}

		public T GetOppositeInteractor<T>(IXRInteractor interactor)
			where T : IXRInteractor
		{
			return (T)GetOppositeInteractor(interactor);
		}

		public IXRSelectInteractor GetOppositeSelectingInteractor(IXRSelectInteractable interactable)
		{
			if (!interactable.isSelected)
				return null;

			return GetOppositeInteractor(interactor: XRSelectInteractableExtensions.GetOldestInteractorSelecting(interactable)) as IXRSelectInteractor;
		}

		public T GetOppositeSelectingInteractor<T>(IXRSelectInteractable interactable)
			where T : IXRSelectInteractor
		{
			return (T)GetOppositeSelectingInteractor(interactable);
		}

		public IXRSelectInteractable GetOppositeSelectedInteractable(IXRSelectInteractable interactable)
		{
			var interactor = GetOppositeSelectingInteractor(interactable);
			if (interactor == null || !interactor.hasSelection)
				return null;

			return interactor.firstInteractableSelected;
		}

		public T GetOppositeSelectedInteractable<T>(IXRSelectInteractable interactable)
			where T : IXRSelectInteractable
		{
			return (T)GetOppositeSelectedInteractable(interactable);
		}

		public IXRSelectInteractable[] GetOppositeSelectedInteractables(IXRSelectInteractable interactable)
		{
			var interactor = GetOppositeSelectingInteractor(interactable);
			if (interactor == null || !interactor.hasSelection)
				return null;

			return interactor.interactablesSelected.ToArray();
		}

		public T[] GetOppositeSelectedInteractables<T>(IXRSelectInteractable interactable)
			where T : IXRSelectInteractable
		{
			var interactables = GetOppositeSelectedInteractables(interactable);
			if (interactables == null)
				return null;

			return interactables.Cast<T>().ToArray();
		}

		#endregion

		#region Static Methods

		private static XRBaseInputInteractor GetControllerInteractor(XRBaseInteractor interactor, ref XRBaseInputInteractor controller)
        {
            if (controller == null)
            {
                controller = interactor?.GetComponentInParent<XRBaseInputInteractor>();
            }
            return controller;
        }

        public static bool IsLeftHand(IXRInteractor interactor)
        {
            return interactor.handedness == InteractorHandedness.Left;
        }

        public static bool IsRightHand(IXRInteractor interactor)
        {
            return interactor.handedness == InteractorHandedness.Right;
        }

        public static bool IsAnyHand(IXRInteractor interactor)
        {
            return IsLeftHand(interactor) || IsRightHand(interactor);
        }

        #endregion

        #region Structures

        [System.Serializable]
        public struct InteractorPair
		{
            public XRBaseInteractor left;
            public XRBaseInteractor right;
        }

		#endregion
	}
}