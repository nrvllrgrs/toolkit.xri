using System;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using Locomotion = UnityEngine.XR.Interaction.Toolkit.Locomotion;
using UnityEditor;
using ToolkitEngine.XR;

namespace ToolkitEditor.XR.VisualScripting
{
	[InitializeOnLoad]
	public static class Setup
	{
		static Setup()
		{
			var types = new List<Type>()
			{
				typeof(XROrigin),
				typeof(XRBaseInteractor),
				typeof(XRBaseInteractorExt),
				typeof(XRDirectInteractor),
				typeof(XRRayInteractor),
				typeof(XRPokeInteractor),
				typeof(XRSocketInteractor),
				typeof(XRBaseInteractable),
				typeof(XRSimpleInteractable),
				typeof(XRGrabInteractable),
				typeof(BaseInteractionEventArgs),
				typeof(IXRInteractor),
				typeof(IXRInteractable),
				typeof(LocomotionSystem),
				typeof(HapticSettings),
				typeof(XRHapticEvents),
				typeof(XRButtonInputEvents),
				typeof(XRInteractableEvents),
				typeof(XRVelocityInputEvents),
				typeof(Locomotion.LocomotionProvider),
				typeof(Locomotion.Teleportation.TeleportationProvider),
				typeof(Locomotion.Teleportation.TeleportRequest),
			};

			ToolkitEditor.VisualScripting.Setup.Initialize(new[]
			{
				"Unity.XR.CoreUtils",
				"Unity.XR.Interaction.Toolkit",
				"ToolkitEngine.XR"
			},
			types);
		}
	}
}