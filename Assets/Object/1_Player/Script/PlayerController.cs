using System.Collections;
using UnityEngine;

public class PlayerController : ObjectBase
{
    [SerializeField, Label("移動速度")] float moveSpd;
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
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _velocity.x = moveSpd;
			if (moveSpd == 0)
			{
				Debug.Log("移動速度0");
			}
		}
		else if (Input.GetKey(KeyCode.LeftArrow))
		{
			_velocity.x = -moveSpd;
			if (moveSpd == 0)
			{
				Debug.Log("移動速度0");
			}
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
		_velocity.y = 0;

		if (Input.GetKeyDown(KeyCode.UpArrow))
        {
			if (!_isJump)
			{
				_isJump = true;
				_isGround = false;
				//_velocity.y = jumpPower;
			}
		}

		if (_isJump)
		{
			_velocity.y = jumpPower;
			//_velocity.y -= gravity * Time.deltaTime;
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
		if (Input.GetKeyDown(KeyCode.X))
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
		if (Input.GetKeyDown(KeyCode.A))
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
			//yield return new WaitForSeconds(0.1f);

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
