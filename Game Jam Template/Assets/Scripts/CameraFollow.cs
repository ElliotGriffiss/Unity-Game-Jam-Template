using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[Header("Camera Settings")]
	[SerializeField] public Camera Camera;
	[SerializeField] private float FollowSpeed;
	[SerializeField] private Vector3 CameraOffset;

	[SerializeField] private float ScreenSpaceModifier = 4;
	[SerializeField] private float MaxDistanceFromPlayer;
	public bool MiamiCam = false;

	[Header("CameraShake Curve")]
	[SerializeField] private AnimationCurve ShakeCurve;

	private Transform FollowTransform;
	private float ShakeDuration = 0.0f;
	private float CurrentShakeDuration = 0.0f;
	private float ShakeAmount = 0.0f;

	public void UpdateFollowTarget(Transform target, bool miamiCam)
	{
		FollowTransform = target;
		MiamiCam = miamiCam;
	}

	public void SetCameraPositionImmediate(Vector3 position, bool miamiCam)
	{
		transform.position = position;
		MiamiCam = miamiCam;
	}

	public void TriggerShakeCamera(float shackDuration, float shakeAmount)
	{
		if (CurrentShakeDuration < shackDuration)
		{
			ShakeDuration = shackDuration;
			CurrentShakeDuration = shackDuration;
		}

		if (ShakeAmount < shakeAmount)
		{
			ShakeAmount = shakeAmount;
		}
	}

	private void FixedUpdate()
	{
		if (FollowTransform != null)
		{
			Vector3 Target = CameraOffset;

			if (MiamiCam)
			{
				Target += Vector3.Lerp(transform.position, Vector3.ClampMagnitude((Camera.ScreenToWorldPoint(Input.mousePosition) - FollowTransform.position) * ScreenSpaceModifier, MaxDistanceFromPlayer) + FollowTransform.position, FollowSpeed);
			}
			else
			{
				Target += FollowTransform.position;
			}

			Vector3 smoothedPosition = Vector3.Lerp(transform.position, Target, FollowSpeed);

			if (CurrentShakeDuration > 0)
			{
				CurrentShakeDuration -= Time.deltaTime;
				transform.position = smoothedPosition + Random.insideUnitSphere * -ShakeCurve.Evaluate(CurrentShakeDuration / ShakeDuration) * ShakeAmount;
			}
			else
			{
				transform.position = smoothedPosition;
				ShakeAmount = 0;
			}
		}
	}
}
