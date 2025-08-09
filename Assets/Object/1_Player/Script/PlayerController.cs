using System;
using System.Collections;
using System.Threading.Tasks;
using Common.Script;
using Object._2_SlashObject.Script;
using Stage.Script;
using UnityEngine;

namespace Object._1_Player.Script
{
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
		private float _attackTimer;
		private readonly float _attackTime = 0.1f;
		private bool _isSheathing; // 納刀中
		private bool _isUnSheathing; //抜刀中
		private bool _isAttackable; //攻撃可能（抜刀完了状態）
		private bool _isDamage;
	
		public bool IsDamage => _isDamage;


		/// <summary>
		/// 初期化
		/// </summary>
		public async Task InitAsync()
		{
			// TODO：基底クラスの初期化処理が終わるまで待機
			while (!IsInit)
			{
				await Task.Delay(100);
			}
		
			// ステージ遷移した際にNeutralアニメに戻す
			Utility.SetAnimationFlg(ObjAnimator, "IsSheath", false);
			Utility.SetAnimationFlg(ObjAnimator, "IsUnSheath", false);
			ObjAnimator.Play("Neutral");
		
			_isAttackable = false;
			_isDamage = false;
			SetDirection(true);
		}

		protected override void ObjectUpdate()
		{
			base.ObjectUpdate();
	    
			// 納刀中であれば行わない
			if (_isSheathing)
			{
				_velocity.x = 0;
				return;
			}
	    
			// 移動
			Move();

			// ジャンプ
			Jump();
		    
			// 攻撃
			Attack();

			// ステージをクリアしていないときに行える
			if (!StageController.Current.IsStageClear)
			{
				// 抜刀
				UnSheath();

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
				DebugLogger.LogError("移動倍率0");
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
						DebugLogger.Log("ブレーキ");
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
			if (IsGround && !IsJump && 
			    (ControllerManager.Current.GetJumpState == ControllerManager.JumpState.Jump || ControllerManager.Current.IsLeadJumpKey))
			{
				IsJump = true;
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
			if (_isAttackable && ControllerManager.Current.GetAttackState == ControllerManager.AttackState.Attack)
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
		/// 抜刀
		/// </summary>
		private void UnSheath()
		{
			if (!_isAttackable && ControllerManager.Current.GetAttackState == ControllerManager.AttackState.SheathOrUnSheath)
			{
				if (_isUnSheathing)
				{
					DebugLogger.Log("抜刀中");
					return;				
				}
			
				TaskUtility.FireAndForget(UnSheathAsync(), "UnSheathAsync");
			}
		
			// 抜刀アニメーション
			async Task UnSheathAsync()
			{
				DebugLogger.Log("抜刀");
				_isUnSheathing = true;
			
				// 抜刀アニメ
				Utility.SetAnimationFlg(ObjAnimator, "IsUnSheath");
			
				// TODO：抜刀終了後、動作可能
				await Task.Delay(700);
				// await WaitAnimeFinishAsync();
			
				_isUnSheathing = false;
				_isAttackable = true;
			
				DebugLogger.Log("抜刀終了");
			}
		}

		/// <summary>
		/// 納刀
		/// </summary>
		private void Sheath()
		{
			// 指定のボタン押下で納刀アクション
			if (_isAttackable && !_isSheathing && ControllerManager.Current.GetAttackState == ControllerManager.AttackState.SheathOrUnSheath)
			{
				if (_isSheathing)
				{
					DebugLogger.Log("納刀中");
					return;				
				}
			
				StartCoroutine(SheathToResult());
			}

			IEnumerator SheathToResult()
			{
				// 現状、全ての敵を斬らなくても納刀できる仕様
				// // 全敵斬った判定
				// if (!ObjectManager.Current.GetDestroyCompletely())
				// {
				// 	DebugLogger.Log("納刀失敗");
				// 	Utility.SetAnimationFlg(ObjAnimator, "IsFailureSheathe");
				// 	yield return null;
				// 	Utility.SetAnimationFlg(ObjAnimator, "IsFailureSheathe",false);
				// 	yield break;
				// }
			
				DebugLogger.Log("納刀");
				_isSheathing = true;

				// 納刀アニメ
				Utility.SetAnimationFlg(ObjAnimator, "IsSheath");
				yield return WaitAnimeFinish();

				// 結果発表
				TaskUtility.FireAndForget(StageController.Current.ResultAsync(), "ResultAsync");
			
				_isAttackable = false;
				_isSheathing = false;
				DebugLogger.Log("納刀終了");
			
				Utility.SetAnimationFlg(ObjAnimator, "IsSheath", false);
				Utility.SetAnimationFlg(ObjAnimator, "IsUnSheath", false);
				ObjAnimator.Play("Neutral");
			}
		}

		/// <summary>
		/// 着地判定
		/// </summary>
		private void GroundCheck()
		{
			int stageLayer = 1 << LayerMask.NameToLayer("Stage");
			if (Physics2D.Raycast(transform.position, Vector2.down, GroundCheckRayLength, stageLayer))
			{
				IsGround = true;
				IsJump = false;
			}
			else
			{
				IsGround = false;
			}
		}
    
    
		private void OnTriggerEnter2D(Collider2D collision)
		{
			// ダメージ判定
			int explosionLayer = LayerMask.NameToLayer("Explosion");
			if (collision.gameObject.layer == explosionLayer && !_isDamage)
			{
				StartCoroutine(Damage());
			}

			IEnumerator Damage()
			{
				_isDamage = true;
				Utility.SetAnimationFlg(ObjAnimator, "IsDamage");
				yield return WaitAnimeFinish();
				
				// TODO：納刀せずにダメージを受けることがあれば復活
				// 納刀時の結果発表と被らないよう対応必須
				// TaskUtility.FireAndForget(SceneGameManager.Current.ReloadStageAsync(), "ReloadStageAsync");
			}
		}

		/// <summary>
		/// レイ可視化
		/// </summary>
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawRay(transform.position,-transform.up * GroundCheckRayLength);
		}
	}
}
