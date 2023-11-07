using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR
{
	public class XRVelocityInputEvents : XRBaseInputEvents
    {
		#region Enumerators

		[System.Flags]
		public enum DirectionMask
		{
			[InspectorName("+X")]
			PosX = 1 << 1,
			[InspectorName("-X")]
			NegX = 1 << 2,
			[InspectorName("+Y")]
			PosY = 1 << 3,
			[InspectorName("-Y")]
			NegY = 1 << 4,
			[InspectorName("+Z")]
			PosZ = 1 << 5,
			[InspectorName("-Z")]
			NegZ = 1 << 6,
			[InspectorName("Internal Y")]
			IntY = 1 << 7,
			[InspectorName("External Y")]
			ExtY = 1 << 8,
			[InspectorName("Internal Z")]
			IntZ = 1 << 9,
			[InspectorName("External Z")]
			ExtZ = 1 << 10,
		}

		#endregion

		#region Fields

		[SerializeField]
		private bool m_isOmnidirectional = true;

		[SerializeField]
		private DirectionMask m_directionMask = (DirectionMask)~0;

		[SerializeField, VectorLabel("Enter", "Exit")]
		private Vector2 m_threshold = new Vector2(2.5f, 0.5f);

		private bool m_inThreshold;

		#endregion

		#region Properties

		public float enterThreshold => m_threshold.x;
		public float exitThreshold => m_threshold.y;

		#endregion

		#region Methods

		protected override void Interact(BaseInteractionEventArgs e)
		{
			Restart();
			base.Interact(e);
		}

		protected override void Performed(InputAction.CallbackContext ctx)
		{
			if (!canPerform)
				return;

			// Velocity is in local-space
			var velocity = ctx.ReadValue<Vector3>();
			//Debug.LogFormat("Velocity = {0}; {1}", velocity.magnitude.ToString("F4"), velocity.ToString("F4"));

			if (m_isOmnidirectional)
			{
				CheckThreshold(ctx, velocity.magnitude);
			}
			else
			{
				bool changed = false;
				if ((m_directionMask & DirectionMask.PosX) != 0)
				{
					changed = CheckThreshold(ctx, velocity.x);
				}
				if (!changed && (m_directionMask & DirectionMask.NegX) != 0)
				{
					changed = CheckThreshold(ctx, -velocity.x);
				}
				if (!changed && (m_directionMask & DirectionMask.PosY) != 0)
				{
					changed = CheckThreshold(ctx, velocity.y);
				}
				if (!changed && (m_directionMask & DirectionMask.NegY) != 0)
				{
					changed = CheckThreshold(ctx, -velocity.y);
				}
				if (!changed && (m_directionMask & DirectionMask.PosZ) != 0)
				{
					changed = CheckThreshold(ctx, velocity.z);
				}
				if (!changed && (m_directionMask & DirectionMask.NegZ) != 0)
				{
					changed = CheckThreshold(ctx, -velocity.z);
				}
				if (!changed && (m_directionMask & DirectionMask.IntY) != 0)
				{
					changed = CheckThreshold(ctx, m_leftHand ? velocity.y : -velocity.y);
				}
				if (!changed && (m_directionMask & DirectionMask.ExtY) != 0)
				{
					changed = CheckThreshold(ctx, m_leftHand ? -velocity.y : velocity.y);
				}
				if (!changed && (m_directionMask & DirectionMask.IntZ) != 0)
				{
					changed = CheckThreshold(ctx, m_leftHand ? -velocity.z : velocity.z);
				}
				if (!changed && (m_directionMask & DirectionMask.ExtZ) != 0)
				{
					CheckThreshold(ctx, m_leftHand ? velocity.z : -velocity.z);
				}
			}
		}

		private bool CheckThreshold(InputAction.CallbackContext ctx, float speed)
		{
			if (!m_inThreshold)
			{
				m_inThreshold = speed > enterThreshold;
				return true;
			}
			else if (speed < exitThreshold)
			{
				SetPerforming(true, ctx);
				Restart();
				return true;
			}

			return false;
		}

		public void Restart()
		{
			m_inThreshold = false;
		}

		#endregion
	}
}