using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR
{
	[RequireComponent(typeof(XRBaseInteractable))]
    public class XRInteractablePriority : MonoBehaviour
    {
		#region Fields

		[SerializeField]
		private short m_priority = 0;

		#endregion

		#region Properties

		public int priority => m_priority;

		#endregion
	}
}