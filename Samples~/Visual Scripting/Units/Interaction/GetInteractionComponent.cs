using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.VisualScripting;

namespace ToolkitEngine.XR.VisualScripting
{
	[UnitCategory("XR/Interactions")]
    public class GetInteractionComponent : Unit
    {
		#region Enumerators

		public enum InteractionObject
		{
			Interactable,
			Interactor,
		}

		#endregion

		#region Fields

		[DoNotSerialize, PortLabelHidden]
		public ControlInput inputTrigger { get; private set; }

		[UnitHeaderInspectable]
		public InteractionObject interactionObject;

		[DoNotSerialize]
		public ValueInput interactionEventArgs;

		[DoNotSerialize]
		public ValueInput type;

		[DoNotSerialize]
		public ControlOutput validTrigger { get; private set; }

		[DoNotSerialize]
		public ControlOutput invalidTrigger { get; private set; }

		[DoNotSerialize, PortLabelHidden]
		public ValueOutput component;

		private Component m_component;

		#endregion

		#region Methods

		protected override void Definition()
		{
			inputTrigger = ControlInput(nameof(inputTrigger), Trigger);

			interactionEventArgs = ValueInput<BaseInteractionEventArgs>(nameof(interactionEventArgs));
			type = ValueInput(nameof(type), default(Type));

			component = ValueOutput(nameof(component), (x) => m_component);

			validTrigger = ControlOutput("Not Null");
			invalidTrigger = ControlOutput("Null");

			Requirement(interactionEventArgs, inputTrigger);
			Requirement(type, inputTrigger);

			Succession(inputTrigger, validTrigger);
			Succession(inputTrigger, invalidTrigger);
		}

		private ControlOutput Trigger(Flow flow)
		{
			var _interactionEventArgs = flow.GetValue<BaseInteractionEventArgs>(interactionEventArgs);
			if (_interactionEventArgs != null)
			{
				Transform transform = null;
				switch (interactionObject)
				{
					case InteractionObject.Interactable:
						transform = _interactionEventArgs.interactableObject.transform;
						break;

					case InteractionObject.Interactor:
						transform = _interactionEventArgs.interactorObject.transform;
						break;
				}

				if (transform != null)
				{
					Type _type = flow.GetValue<Type>(type);

					m_component = GetComponent(flow, transform, _type);
					if (m_component != null)
					{
						return validTrigger;
					}
				}
			}

			m_component = null;
			return invalidTrigger;
		}

		protected virtual Component GetComponent(Flow flow, Transform transform, Type type)
		{
			return transform.GetComponent(type);
		}

		#endregion
	}
}