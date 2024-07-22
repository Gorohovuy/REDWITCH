using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions
{
    // Assign delegate{} to events to initialise them with an empty delegate
	// so we can skip the null check when we use them

    // Gameplay
    public event UnityAction<Vector2> MoveEvent = delegate { };
    public event UnityAction<Vector2, bool> CameraMoveEvent = delegate { };
    public event UnityAction<bool> IsJump = delegate { };

    private GameInput gameInput;

    private bool IsDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";

    private void OnEnable()
	{
		if (gameInput == null)
		{
			gameInput = new GameInput();
			// gameInput.Menus.SetCallbacks(this);
			gameInput.Gameplay.SetCallbacks(this);
			// gameInput.Dialogues.SetCallbacks(this);
		}

		EnableGameplayInput();
	}

    private void OnDisable()
	{
		DisableAllInput();
	}

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnMoveCamera(InputAction.CallbackContext context)
    {
        CameraMoveEvent.Invoke(context.ReadValue<Vector2>(), IsDeviceMouse(context));
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump");
    }

    public void EnableGameplayInput()
    {
        gameInput.Gameplay.Enable();
    }

    public void DisableAllInput()
    {
        gameInput.Gameplay.Disable();
    }
}
