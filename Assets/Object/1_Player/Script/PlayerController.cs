using System.Collections;
using UnityEngine;

public class PlayerController : ObjectBase
{
    [SerializeField, Label("移動速度倍率")] float moveSpd;
    [SerializeField, Label("減速速度")] float deboostSpd;
    [SerializeField, Label("ジャンプ力")] float jumpPower;
    [SerializeField, Label("重力")] float gravity;
	[SerializeField] Rigidbody2D rigidBody2d;
	private Vector3 _velocity;
	private bool _isJump = false;
	private bool _isDamage = false;
	private bool _isGround = false;
	private float _attackTimer;
	private float AttackTimer = 0.1f;
	private float timer = 0f;
	private bool _isSheath = false;
	private float _groundCheckRayLength = 0.8f;
	
	public bool IsDamage => _isDamage;

	void Update()
    {
        // 移動
        Move();

        // ジャンプ
        Jump();
        
        // 着地判定
        GroundCheck();
        
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
		if (_isSheath)
		{
			_velocity.x = 0;
			return;
		}

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
		if (_isSheath)
		{
			return;
		}
		
		if (_isGround && !_isJump && ControllerManager.Current.GetJumpState == ControllerManager.JumpState.Jump)
		{
			Debug.Log("ジャンプ");
			_isJump = true;
			var velocity = rigidBody2d.linearVelocity;
			velocity.y = jumpPower;
			rigidBody2d.linearVelocity = velocity;
		}
	}

	/// <summary>
	/// 着地判定
	/// </summary>
	private void GroundCheck()
	{
		int stageLayer = 1 << LayerMask.NameToLayer("Stage");
		if (Physics2D.Raycast(transform.position, -transform.up, _groundCheckRayLength, stageLayer))
		{
			_isGround = true;
			_isJump = false;
		}
		else
		{
			_isGround = false;
		}
	}

	/// <summary>
	/// 攻撃
	/// </summary>
	private void Attack()
	{
		// 納刀中であれば行わない
		if (_isSheath)
		{
			return;
		}

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
			_animator.SetBool("IsSheath", false);

			// 結果発表
			StageManager.Current.Result();
			
			_isSheath = false;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var explosionLayer = 10;
		if (collision.gameObject.layer == explosionLayer)
		{
			_isDamage = true;
			_animator.SetBool("IsDamage", true);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position,-transform.up * _groundCheckRayLength);
	}
}
