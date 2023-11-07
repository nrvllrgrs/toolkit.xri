using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using ToolkitEngine;

namespace ToolkitEngine.XR
{
    public class XRHandednessContext : Singleton<XRHandednessContext>
    {
        #region Fields

        [SerializeField]
        private XRBaseInteractor m_left, m_right;

        private XRBaseControllerInteractor m_leftController, m_rightController;

        #endregion

        #region Properties

        public static XRBaseInteractor left => Instance?.m_left;

        public static XRBaseControllerInteractor leftController => GetControllerInteractor(left, ref Instance.m_leftController);

        public static XRBaseInteractor right => Instance?.m_right;

		public static XRBaseControllerInteractor rightController => GetControllerInteractor(right, ref Instance.m_rightController);

		#endregion

		#region Methods

		private static XRBaseControllerInteractor GetControllerInteractor(XRBaseInteractor interactor, ref XRBaseControllerInteractor controller)
        {
            if (controller == null)
            {
                controller = interactor?.GetComponentInParent<XRBaseControllerInteractor>();
            }
            return controller;
        }

        public static bool IsLeftHand(IXRInteractor interactor)
        {
            return Equals(interactor, left);
        }

        public static bool IsRightHand(IXRInteractor interactor)
        {
            return Equals(interactor, right);
        }

        public static bool IsAnyHand(IXRInteractor interactor)
        {
            return IsLeftHand(interactor) || IsRightHand(interactor);
        }

        public static IXRInteractor GetOppositeInteractor(IXRInteractor interactor)
        {
			if (interactor == left as IXRInteractor)
			{
				return right;
			}
			if (interactor == right as IXRInteractor)
			{
				return left;
			}
			return null;
        }

        public static T GetOppositeInteractor<T>(IXRInteractor interactor)
            where T : IXRInteractor
        {
            return (T)GetOppositeInteractor(interactor);
        }

        public static IXRSelectInteractor GetOppositeSelectingInteractor(IXRSelectInteractable interactable)
        {
            if (!interactable.isSelected)
                return null;

            return GetOppositeInteractor(interactable.GetOldestInteractorSelecting()) as IXRSelectInteractor;
        }

        public static T GetOppositeSelectingInteractor<T>(IXRSelectInteractable interactable)
            where T : IXRSelectInteractor
        {
            return (T)GetOppositeSelectingInteractor(interactable);
        }

        public static IXRSelectInteractable GetOppositeSelectedInteractable(IXRSelectInteractable interactable)
        {
            var interactor = GetOppositeSelectingInteractor(interactable);
            if (interactor == null || !interactor.hasSelection)
                return null;

            return interactor.firstInteractableSelected;
        }

        public static T GetOppositeSelectedInteractable<T>(IXRSelectInteractable interactable)
            where T : IXRSelectInteractable
        {
            return (T)GetOppositeSelectedInteractable(interactable);
        }

        public static IXRSelectInteractable[] GetOppositeSelectedInteractables(IXRSelectInteractable interactable)
        {
            var interactor = GetOppositeSelectingInteractor(interactable);
            if (interactor == null || !interactor.hasSelection)
                return null;

            return interactor.interactablesSelected.ToArray();
        }

        public static T[] GetOppositeSelectedInteractables<T>(IXRSelectInteractable interactable)
            where T : IXRSelectInteractable
        {
            var interactables = GetOppositeSelectedInteractables(interactable);
            if (interactables == null)
                return null;

            return interactables.Cast<T>().ToArray();
        }

        #endregion
    }
}