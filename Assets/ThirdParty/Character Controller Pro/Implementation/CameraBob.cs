using System;
using UnityEngine;
using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Implementation;

namespace Lightbug.CharacterControllerPro.Implementation
{
	public class CameraBob : MonoBehaviour
	{
		[SerializeField] private Transform cameraTransform;

		[SerializeField] private float walkingBobbingSpeed = 14f;
		[SerializeField] private float runningBobbingSpeed = 20f;
		[SerializeField] private float crouchingBobbingSpeed = 10f;

		[SerializeField] private float bobbingAmount = 0.05f;

		private float timer = 0;

		private NormalMovement _normalMovment;
	
		private void Awake()
		{
			_normalMovment = GetComponentInChildren<NormalMovement>();
		}
		
		public void BobCamera()
		{
			if (!_normalMovment.IsIdle())
			{
				// The player is moving
				if (_normalMovment.IsRunning() && !_normalMovment.IsCrouched())
				{
					timer += Time.deltaTime * runningBobbingSpeed;
				}
				else if (_normalMovment.IsCrouched())
				{
					timer += Time.deltaTime * crouchingBobbingSpeed;
				}
				else
				{
					timer += Time.deltaTime * walkingBobbingSpeed;
				}

				cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, cameraTransform.localPosition.y + Mathf.Sin(timer) * bobbingAmount, cameraTransform.localPosition.z);
			}
			else
			{
				// The player is stationary
				timer = 0;
				cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, Mathf.Lerp(cameraTransform.localPosition.y, cameraTransform.localPosition.y, Time.deltaTime * walkingBobbingSpeed), cameraTransform.localPosition.z);
					
			}
		}
	}
}