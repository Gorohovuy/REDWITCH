using System;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader = default;
	[SerializeField] private TransformAnchor _gameplayCameraTransform = default;

    private Vector2 _inputVector;
	private float _previousSpeed;


    [NonSerialized] public Vector3 movementInput; //Initial input coming from the Protagonist script
	[NonSerialized] public Vector3 movementVector; //Final movement vector, manipulated by the StateMachine actions

    //Adds listeners for events being triggered in the InputReader script
	private void OnEnable()
	{
		_inputReader.MoveEvent += OnMove;
	}

	//Removes all listeners to the events coming from the InputReader script
	private void OnDisable()
	{
		_inputReader.MoveEvent -= OnMove;
	}

	private void Update()
	{
		GetObjectVariables();
		RecalculateMovement();
		SetObjectVariables();
	}

	private void RecalculateMovement()
	{
		float targetSpeed;
		Vector3 adjustedMovement;

		if (_gameplayCameraTransform.isSet)
		{
			//Get the two axes from the camera and flatten them on the XZ plane
			Vector3 cameraForward = _gameplayCameraTransform.Value.forward;
			cameraForward.y = 0f;
			Vector3 cameraRight = _gameplayCameraTransform.Value.right;
			cameraRight.y = 0f;

			//Use the two axes, modulated by the corresponding inputs, and construct the final vector
			adjustedMovement = cameraRight.normalized * _inputVector.x +
				cameraForward.normalized * _inputVector.y;
		}
		else
		{
			//No CameraManager exists in the scene, so the input is just used absolute in world-space
			Debug.LogWarning("No gameplay camera in the scene. Movement orientation will not be correct.");
			adjustedMovement = new Vector3(_inputVector.x, 0f, _inputVector.y);
		}

		//Fix to avoid getting a Vector3.zero vector, which would result in the player turning to x:0, z:0
		if (_inputVector.sqrMagnitude == 0f)
			adjustedMovement = transform.forward * (adjustedMovement.magnitude + .01f);

		//Accelerate/decelerate
		targetSpeed = Mathf.Clamp01(_inputVector.magnitude);
		if (targetSpeed > 0f)
		{
			// This is used to set the speed to the maximum if holding the Shift key,
			// to allow keyboard players to "run"
			// if (isRunning)
				// targetSpeed = 1f;

			// if (attackInput)
			// 	targetSpeed = .05f;
		}
		targetSpeed = Mathf.Lerp(_previousSpeed, targetSpeed, Time.deltaTime * 4f);

		movementInput = adjustedMovement.normalized * targetSpeed;

		_previousSpeed = targetSpeed;
	}

	private void SetObjectVariables()
	{
		Variables.Object(gameObject).Set("movementInput", movementInput);
		Variables.Object(gameObject).Set("movementVector", movementVector);
	}

	private void GetObjectVariables()
	{
		// movementInput = (Vector3)Variables.Object(gameObject).Get("movementInput");
		movementVector = (Vector3)Variables.Object(gameObject).Get("movementVector");
	}

	// ---- EVENT LISTENERS ----

	private void OnMove(Vector2 movement)
	{
		_inputVector = movement;
	}
}