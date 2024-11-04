using System;
using UnityEngine;

using UnityEngine.XR.Interaction.Toolkit.Filtering;

namespace ToolkitEngine.XR
{
    [Serializable]
	public class XRPriorityEvaluator : XRTargetEvaluator
	{
        protected override float CalculateNormalizedScore(UnityEngine.XR.Interaction.Toolkit.Interactors.IXRInteractor interactor, UnityEngine.XR.Interaction.Toolkit.Interactables.IXRInteractable target)
        {
            return target.transform.TryGetComponent(out XRInteractablePriority interactablePriority)
                ? MathUtil.GetPercent(interactablePriority.priority, short.MinValue, short.MaxValue)
				: 0.5f;
        }
    }
}