using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR
{
	public class XRSpawnerInteractable : UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable
    {
		#region Fields

		[SerializeField]
		protected Spawner m_spawner;

		#endregion

		#region Methods

		protected override void OnSelectEntering(SelectEnterEventArgs e)
		{
			base.OnSelectEntering(e);
			e.manager.SelectCancel(e.interactorObject, e.interactableObject);

			m_spawner.Instantiate(Spawned, e);
		}

		protected override void OnSelectEntered(SelectEnterEventArgs args)
		{
			// Intentionally left blank
		}

		private void Spawned(GameObject obj, params object[] args)
		{
			var interactable = obj.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
			if (interactable == null)
				return;

			var e = args[0] as SelectEnterEventArgs;
			if (e == null)
				return;

			obj.transform.SetPositionAndRotation(e.interactorObject.transform.position, e.interactorObject.transform.rotation);

			if (e.interactorObject.hasSelection)
			{
				// Already selecting interactable, skip
				if (e.interactorObject.IsSelecting(interactable))
					return;

				e.manager.SelectExit(e.interactorObject, e.interactorObject.firstInteractableSelected);
			}

			e.manager.SelectEnter(e.interactorObject, interactable);
		}

		#endregion
	}
}