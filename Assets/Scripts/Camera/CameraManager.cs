using System;
using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
	public InputReader inputReader;
	public Camera mainCamera;
	public CinemachineFreeLook freeLookVCam;
	// public CinemachineImpulseSource impulseSource;
	// private bool _isRMBPressed;

	[SerializeField][Range(.5f, 3f)] private float _speedMultiplier = 1f; //TODO: make this modifiable in the game settings											
	[SerializeField] private TransformAnchor _cameraTransformAnchor = default;
	// [SerializeField] private TransformAnchor _protagonistTransformAnchor = default;

	// [Header("Listening on channels")]
	// [Tooltip("The CameraManager listens to this event, fired by protagonist GettingHit state, to shake camera")]
	// [SerializeField] private VoidEventChannelSO _camShakeEvent = default;

	private bool _cameraMovementLock = false;

	private void OnEnable()
	{
		inputReader.CameraMoveEvent += OnCameraMove;
		// inputReader.EnableMouseControlCameraEvent += OnEnableMouseControlCamera;
		// inputReader.DisableMouseControlCameraEvent += OnDisableMouseControlCamera;

		// _protagonistTransformAnchor.OnAnchorProvided += SetupProtagonistVirtualCamera;
		// _camShakeEvent.OnEventRaised += impulseSource.GenerateImpulse;

		_cameraTransformAnchor.Provide(mainCamera.transform);
	}

	private void OnDisable()
	{
		inputReader.CameraMoveEvent -= OnCameraMove;
		// inputReader.EnableMouseControlCameraEvent -= OnEnableMouseControlCamera;
		// inputReader.DisableMouseControlCameraEvent -= OnDisableMouseControlCamera;

		// _protagonistTransformAnchor.OnAnchorProvided -= SetupProtagonistVirtualCamera;
		// _camShakeEvent.OnEventRaised -= impulseSource.GenerateImpulse;

		_cameraTransformAnchor.Unset();
	}



	private void OnCameraMove(Vector2 cameraMovement, bool isDeviceMouse)
	{
		if (_cameraMovementLock)
			return;

		// if (isDeviceMouse && !_isRMBPressed)
		// 	return;

		//Using a "fixed delta time" if the device is mouse,
		//since for the mouse we don't have to account for frame duration
		// float deviceMultiplier = isDeviceMouse ? 0.02f : Time.deltaTime;
        float deviceMultiplier = Time.deltaTime;

		freeLookVCam.m_XAxis.m_InputAxisValue = cameraMovement.x * deviceMultiplier * _speedMultiplier;
		freeLookVCam.m_YAxis.m_InputAxisValue = cameraMovement.y * deviceMultiplier * _speedMultiplier;
	}

	/// <summary>
	/// Provides Cinemachine with its target, taken from the TransformAnchor SO containing a reference to the player's Transform component.
	/// This method is called every time the player is reinstantiated.
	/// </summary>
	// public void SetupProtagonistVirtualCamera()
	// {
	// 	Transform target = _protagonistTransformAnchor.Value;

	// 	freeLookVCam.Follow = target;
	// 	freeLookVCam.LookAt = target;
	// 	freeLookVCam.OnTargetObjectWarped(target, target.position - freeLookVCam.transform.position - Vector3.forward);
	// }
}
