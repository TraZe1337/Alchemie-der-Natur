using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		[Header("UI Settings")]
		public bool IsInventoryOpen = false; // Initialize the inventory state

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			if (IsInventoryOpen) return; // Ignore input if inventory is open
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if (IsInventoryOpen) return; // Ignore input if inventory is open
			if (cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			if (IsInventoryOpen) return; // Ignore input if inventory is open
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			if (IsInventoryOpen) return; // Ignore input if inventory is open
			SprintInput(value.isPressed);
		}

		public void OnOpenCloseInventory(InputValue value)
		{
			Debug.Log("Inventory button pressed." + value.isPressed);
			IsInventoryOpen = !IsInventoryOpen; // Toggle the inventory state
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		}

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}

}