using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions
{
    // Assign delegate{} to events to initialise them with an empty delegate
	// so we can skip the null check when we use them

    // Gameplay
    public event UnityAction<Vector2> moveEvent = delegate { };

    private GameInput gameInput;

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
        moveEvent.Invoke(context.ReadValue<Vector2>());
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
