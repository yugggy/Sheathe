using System.Collections;
using UnityEngine;

public class PlayerController : ObjectBase
{
    [SerializeField, Label("移動速度倍率")] float moveSpd;
    [SerializeField, Label("減速速度")] float deboostSpd;
    [SerializeField, Label("ジャンプ力")] float jumpPower;
    [SerializeField, Label("重力")] float gravity;
    private Vector3 _velocity;
	private bool _isJump = false;
	private bool _isGround = false;
	private float _attackTimer;
	private float AttackTimer = 0.1f;
	private float timer = 0f;
	private bool _isSheath = false;

	void Update()
    {
        // 移動
        Move();

        // ジャンプ
        Jump();
        
        // 攻撃
        Attack();

        // 納刀
        Sheath();

        // 移動値加算
		DebugManager.Current.SetVelocityText(_velocity);
		transform.position += _velocity;
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
		// 納刀中であれば行わない
		//if (_isSheath)
		//{
		//	_velocity.x = 0;
		//	return;
		//}

		if (moveSpd == 0)
		{
			Debug.LogError("移動倍率0");
		}

		var leftStickValue = ControllerManager.Current.LeftStickValue * moveSpd * Time.deltaTime;
		if (ControllerManager.Current.GetMoveState == ControllerManager.MoveState.RightMove)
		{
			_velocity.x = leftStickValue;
			SetDirection(true);
		}
		else if (ControllerManager.Current.GetMoveState == ControllerManager.MoveState.LeftMove)
		{
			_velocity.x = leftStickValue;
			SetDirection(false);
		}
		else
		{
			_velocity.x = 0;
		}
	}

	/// <summary>
	/// ジャンプ
	/// </summary>
	private void Jump()
    {
		// 納刀中であれば行わない
		//if (_isSheath)
		//{
		//	return;
		//}

		//_velocity.y = 0;

		if (ControllerManager.Current.GetJumpState == ControllerManager.JumpState.Jump)
		{
			if (!_isJump)
			{
				_isJump = true;
				_isGround = false;
				_velocity.y = jumpPower * Time.deltaTime;
			}
		}

		if (_isJump)
		{
			//_velocity.y = jumpPower;
			//_velocity.y -= gravity * Time.deltaTime;
			_velocity.y -= gravity * Time.deltaTime;
		}
		else
		{
			// 空中にいる
			if (!_isGround)
			{
				//_velocity.y -= gravity;
			}
		}

		//_velocity.y -= gravity;
	}

	/// <summary>
	/// 攻撃
	/// </summary>
	private void Attack()
	{
		// 納刀中であれば行わない
		//if (_isSheath)
		//{
		//	return;
		//}

		if (ControllerManager.Current.GetAttackState == ControllerManager.AttackState.Attack)
		{
			_attackCollider.gameObject.SetActive(true);
			_attackTimer = AttackTimer;
		}

		// 0.1秒後に無効
		if (_attackTimer > 0)
		{
			_attackTimer -= Time.deltaTime;

			if (_attackTimer <= 0)
			{
				_attackTimer = 0;
				_attackCollider.gameObject.SetActive(false);
			}
		}
	}

    /// <summary>
    /// 納刀
    /// </summary>
    private void Sheath()
    {
		// 納刀は一度のみ
		if (_isSheath)
		{
			return;
		}

		// 指定のボタン押下で納刀アクション
		if (ControllerManager.Current.GetAttackState == ControllerManager.AttackState.Sheath)
		{
			StartCoroutine(Sheath());
		}

		IEnumerator Sheath()
		{
			Debug.Log("納刀");

			_isSheath = true;

			// 納刀アニメ
			_animator.SetBool("IsSheath", true);
			yield return WaitAnimeFinish();

			// 結果発表
			StageManager.Current.Result();
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		var stageLayer = 8;
		if (collision.gameObject.layer == stageLayer)
		{
			_isGround = true;
			_isJump = false;
			_velocity.y = 0;
		}
	}
}
