using System;
using Common.Script;
using UnityEngine;

namespace Stage.Script
{
	/// <summary>
	/// ステージ内の各ゲート
	/// </summary>
	public class Gate : MonoBehaviour
	{
		[SerializeField] private BoxCollider2D _boxCollider2D;
		[SerializeField] private Animator _doorAnimator;
		[SerializeField] private Transform _playerSpawnPoint;
	
		private Action _action;
		public Action GetAction { get { return _action; } set { _action = value; } }
		public Transform PlayerSpawnPoint => _playerSpawnPoint;
	
		private void OnTriggerEnter2D(Collider2D col)
		{
			// プレイヤーがゲートに触れたらアクション
			if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
			{
				if (_action != null)
				{
					_action();
				}
			}
		}

		/// <summary>
		/// 出口開錠
		/// </summary>
		public void ExitOpen()
		{
			if (_doorAnimator  == null)
			{
				return;
			}
		
			Utility.SetAnimationFlg(_doorAnimator, "IsOpen");
			_boxCollider2D.enabled = true;
		}
	}
}
