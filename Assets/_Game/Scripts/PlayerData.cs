using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayer Data", menuName = "Player Data")]
public class PlayerData : ScriptableObject
{
	public bool isIdle;
	public bool isWalking;
	public bool isRunning;
	public bool isCrouching;
	public bool isGrounded;
	public bool wasGrounded;
	public bool hasBecomeNotGrounded;
	public bool hasBecomeGrounded;
}
