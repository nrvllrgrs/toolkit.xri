using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

namespace ToolkitEngine.XR.Transformers
{
	[AddComponentMenu("XR/Transformers/XR Socket-Attach Point Transformer")]
	public class XRSocketAttachPointTransformer : XRSingleGrabFreeTransformer
	{
		#region Fields

		[SerializeField]
		private SocketAttachPointMap m_socketAttachPoints = new();

		#endregion

		#region Methods

		public override void Process(XRGrabInteractable grabInteractable, XRInteractionUpdateOrder.UpdatePhase updatePhase, ref Pose targetPose, ref Vector3 localScale)
		{
			if (grabInteractable.firstInteractorSelecting is XRSocketInteractor socketInteractor
				&& m_socketAttachPoints.TryGetValue(socketInteractor, out var attachPoint))
			{
				var interactorAttachPose = socketInteractor.GetAttachTransform(grabInteractable).GetWorldPose();
				var thisTransformPose = grabInteractable.transform.GetWorldPose();
				var thisAttachTransform = attachPoint.transform;

				// Calculate offset of the grab interactable's position relative to its attach transform
				var attachOffset = thisTransformPose.position - thisAttachTransform.position;
					
				var positionOffset = thisAttachTransform.InverseTransformDirection(attachOffset);
				var rotationOffset = Quaternion.Inverse(Quaternion.Inverse(thisTransformPose.rotation) * thisAttachTransform.rotation);
				targetPose.position = (interactorAttachPose.rotation * positionOffset) + interactorAttachPose.position;
				targetPose.rotation = (interactorAttachPose.rotation * rotationOffset);
			}
			else
			{
				base.Process(grabInteractable, updatePhase, ref targetPose, ref localScale);
			}
		}

		public Transform GetAttachTranform(XRGrabInteractable grabInteractable)
		{
			if (grabInteractable.firstInteractorSelecting is not XRSocketInteractor socketInteractor)
				return null;
				
			if (m_socketAttachPoints.TryGetValue(socketInteractor, out var attachPoint))
			{
				return attachPoint.transform;
			}
			return grabInteractable.GetAttachTransform(socketInteractor);
		}

		#endregion

		#region Structures

		[System.Serializable]
		public class SocketAttachPointMap : SerializableDictionary<XRSocketInteractor, GameObject>
		{ }
		
		#endregion
	}
}