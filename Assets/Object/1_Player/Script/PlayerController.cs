using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : ObjectBase
{
    [SerializeField, Label("移動速度")] float moveSpd;
    [SerializeField, Label("減速速度")] float deboostSpd;
    [SerializeField, Label("ジャンプ力")] float jumpPower;
    [SerializeField, Label("重力")] float gravity;
    private Vector3 _velocity;
    //private bool _isJump;
    private float _attackTimer;
	private float AttackTimer = 0.1f;


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
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
			_velocity.y += jumpPower;
			//_isJump = true;
        }

        //if (_isJump)
        //{

        //    //重力
        //    if (_velocity.y > 0)
        //    {
        //        _velocity.y -= gravity;
        //    }
        //}


        //if (_velocity.y > 0)
        //{
        //    _velocity.y -= _gravity;
        //}
        //else
        //{
        //    _velocity.y = 0;
        //}
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    private void Attack()
	{
		if (Input.GetKeyDown(KeyCode.Y))
		{
			attackCollider.gameObject.SetActive(true);
			_attackTimer = AttackTimer;
		}

		// 0.1秒後に無効
		if (_attackTimer > 0)
		{
			_attackTimer -= Time.deltaTime;

			if (_attackTimer <= 0)
			{
				_attackTimer = 0;
				attackCollider.gameObject.SetActive(false);
			}
		}
	}

    /// <summary>
    /// 納刀
    /// </summary>
    private void Sheath()
    {
		if (Input.GetKeyDown(KeyCode.A))
		{
			Debug.Log("納刀");

            // TODO：納刀アニメ

            // 全滅判定保存
            StageManager.Current.SetDestroyCompletely(ObjectManager.Current.GetDestroyCompletely());

			// 斬った敵殲滅
			DestroySlashObject();

            // 結果発表
			StageManager.Current.Result();
		}
	}

	/// <summary>
	/// 斬った敵殲滅
	/// </summary>
	public void DestroySlashObject()
	{
		foreach (var slash in ObjectManager.Current.SlashList)
		{
			if (slash.IsSlashed)
			{
				// TODO：殲滅アニメ再生

				Destroy(slash.gameObject);
			}
		}

		ObjectManager.Current.ClearSlashObjectList();
	}


	private void OnCollisionEnter2D(Collision2D collision)
	{
		//_isJump = true;
		_velocity.y = 0;
	}
}
