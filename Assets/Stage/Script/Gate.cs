using System.Collections.Generic;
using System;
using UnityEngine;

public class Gate : MonoBehaviour
{
	[SerializeField] private BoxCollider2D _boxCollider2D;
	[SerializeField] private Animator _doorAnimator;
	
	public Action _action;

	public Action GetAction { get { return _action; } set { _action = value; } }


	private void OnTriggerEnter2D(Collider2D collider)
	{
		var playerLayer = 6;
		if (collider.gameObject.layer == playerLayer)
		{
			if (_action != null)
			{
				_action();
			}
		}
	}
	
	public void DoorOpen()
	{
		// ドア開錠
		_doorAnimator.SetBool("IsOpen", true);
		_boxCollider2D.enabled = true;
	}
}
