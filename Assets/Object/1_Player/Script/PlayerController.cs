using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// プレイヤー
/// </summary>
public class PlayerController : ObjectBase
{
    [SerializeField, Label("移動速度倍率")] float _moveSpd;
    [SerializeField, Label("減速度")] float _deboostSpd;
    [SerializeField, Label("ブレーキ減速度")] float _brakeDeboostSpd;
    [SerializeField, Label("ジャンプ力")] float _jumpPower;

	private Vector3 _velocity;
	private bool _isGround;
	private bool _isJump;
	private float _attackTimer;
	private readonly float _attackTime = 0.1f;
	private bool _isSheath;
    private bool _isUnSheath;
	private bool _isDamage;
	private readonly float _groundCheckRayLength = 0.8f;
	
	public bool IsDamage => _isDamage;


	/// <summary>
	/// 初期化
	/// </summary>
	public async Task InitAsync()
	{
		_isUnSheath = true;
		
		// 基底クラスの初期化処理が終わるまで待機
		while (!IsInit)
		{
			await Task.Delay(100);
		}
		
		_isSheath = false;
		_isDamage = false;
		SetDirection(true);
		
		// 抜刀アニメ
		var isSheathHash = Animator.StringToHash("IsSheath");
		if (ObjAnimator.GetBool(isSheathHash))
		{
			ObjAnimator.SetBool(isSheathHash, false);
			
			// 抜刀終了後、動作可能
			await Task.Delay(1000);
			_isUnSheath = false;
		}
		else
		{
			_isUnSheath = false;
		}
	}

	protected override void Update()
    {
	    base.Update();
	    
	    // 抜刀もしくは納刀中であれば行わない
	    if (_isUnSheath || _isSheath)
	    {
		    _velocity.x = 0;
		    return;
	    }
	    else
	    {
		    // 移動
		    Move();

		    // ジャンプ
		    Jump();
		    
		    // 攻撃
		    Attack();

		    // 納刀
		    Sheath();
	    }
        
        // 着地判定
        GroundCheck();
        
        // 移動値加算
		DebugManager.Current.SetVelocityText(_velocity);
		transform.position += _velocity;
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
		if (_moveSpd == 0)
		{
			Debug.LogError("移動倍率0");
		}
		
		var leftStickValue = ControllerManager.Current.LeftStickValue * _moveSpd * Time.deltaTime;
		
		// ブレーキ
		// // TODO；操作性が微妙なので一旦保留
		// if (IsBrake(leftStickValue))
		// {
		// 	Brake();
		// 	return;
		// }
		
		// 移動
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
			// 慣性
			Inertia();
		}

		// ブレーキ判定
		bool IsBrake(float value)
		{
			if (Math.Abs(_velocity.x) > 0 && Math.Abs(value) > 0)
			{
				if (Math.Sign(_velocity.x) != Math.Sign(value))
				{
					Debug.Log("ブレーキ");
					return true;
				}
			}
			return false;
		}
		
		// ブレーキ
		void Brake()
		{
			_velocity.x -= _velocity.x * _brakeDeboostSpd;
			if (Math.Abs(_velocity.x) < 0.001f)
			{
				_velocity.x = 0;
			}
			SetDirection(leftStickValue >= 0);
		}
		
		// 慣性
		void Inertia()
		{
			_velocity.x -= _velocity.x * _deboostSpd;
			if (Math.Abs(_velocity.x) < 0.001f)
			{
				_velocity.x = 0;
			}			
		}
	}

	/// <summary>
	/// ジャンプ
	/// </summary>
	private void Jump()
    {
		if (_isGround && !_isJump && 
		    (ControllerManager.Current.GetJumpState == ControllerManager.JumpState.Jump || ControllerManager.Current.IsLeadJumpKey))
		{
			_isJump = true;
			var velocity = ObjRigidBody.linearVelocity;
			velocity.y = _jumpPower;
			ObjRigidBody.linearVelocity = velocity;
		}
	}

	/// <summary>
	/// 攻撃
	/// </summary>
	private void Attack()
	{
		if (ControllerManager.Current.GetAttackState == ControllerManager.AttackState.Attack)
		{
			ObjAttackCollider.gameObject.SetActive(true);
			_attackTimer = _attackTime;
		}

		// 0.1秒後に無効
		if (_attackTimer > 0)
		{
			_attackTimer -= Time.deltaTime;

			if (_attackTimer <= 0)
			{
				_attackTimer = 0;
				ObjAttackCollider.gameObject.SetActive(false);
			}
		}
	}

    /// <summary>
    /// 納刀
    /// </summary>
    private void Sheath()
    {
		// 指定のボタン押下で納刀アクション
		if (ControllerManager.Current.GetAttackState == ControllerManager.AttackState.Sheath)
		{
			StartCoroutine(SheathToResult());
		}

		IEnumerator SheathToResult()
		{
			Debug.Log("納刀");

			_isSheath = true;

			// 納刀アニメ
			var isSheathHash = Animator.StringToHash("IsSheath");
			ObjAnimator.SetBool(isSheathHash, true);
			yield return WaitAnimeFinish();

			// 結果発表
			StageManager.Current.ResultAsync();
			
			_isSheath = false;
		}
	}

    /// <summary>
    /// 着地判定
    /// </summary>
    private void GroundCheck()
    {
	    int stageLayer = 1 << LayerMask.NameToLayer("Stage");
	    if (Physics2D.Raycast(transform.position, Vector2.down, _groundCheckRayLength, stageLayer))
	    {
		    _isGround = true;
		    _isJump = false;
	    }
	    else
	    {
		    _isGround = false;
	    }
    }
    
	private void OnTriggerEnter2D(Collider2D collision)
	{
		ExplosionDamage();

		void ExplosionDamage()
		{
			var explosionLayer = 10;
			if (collision.gameObject.layer == explosionLayer)
			{
				_isDamage = true;
				var isDamageHash = Animator.StringToHash("IsDamage");
				ObjAnimator.SetBool(isDamageHash, true);
			}	
		}
	}

	/// <summary>
	/// レイ可視化
	/// </summary>
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position,-transform.up * _groundCheckRayLength);
	}
}
