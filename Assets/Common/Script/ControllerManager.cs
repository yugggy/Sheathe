using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerManager : MonoBehaviour
{
	private Gamepad _gamepad;
	private float _leftStickValue = 0.0f;
	private MoveState _moveState = MoveState.None;
	private JumpState _jumpState = JumpState.None;
	private AttackState _attackState = AttackState.None;

	private int _leadKeyTimer;
	[SerializeField, Label("先行入力保存フレーム")] private int _leadKeyTime;
	private bool _isLeadJumpKey;

	public static ControllerManager Current;

	public float LeftStickValue => _leftStickValue;
	public MoveState GetMoveState => _moveState;
	public JumpState GetJumpState => _jumpState;
	public AttackState GetAttackState => _attackState;
	
	public bool IsLeadJumpKey => _isLeadJumpKey;
	

	public enum MoveState
	{
		None,
		RightMove,
		LeftMove,
	}

	public enum JumpState
	{
		None,
		Jump,
	}

	public enum AttackState
	{
		None,
		Attack,
		Sheath,
	}

	private void Awake()
	{
		Current = this;
	}

	private void Update()
	{
		Operate();
		
		LeadKey();
	}

	private void Operate()
	{
		// 初期化
		_moveState = MoveState.None;
		_leftStickValue = 0;

		// キーボード操作
		KeyboradOperate();

		// ゲームパッド操作
		GamePadOperate();

		//Debug.Log($"操作：{_state}");
		//Debug.Log($"移動：{_leftStickValue}");
	}

	/// <summary>
	/// キーボード操作
	/// </summary>
	private void KeyboradOperate()
	{
		// 左右操作
		if (Input.GetKey(KeyCode.RightArrow))
		{
			_moveState = MoveState.RightMove;
			_leftStickValue = 1.0f;
		}
		else if (Input.GetKey(KeyCode.LeftArrow))
		{
			_moveState = MoveState.LeftMove;
			_leftStickValue = -1.0f;
		}
		else
		{
			_moveState = MoveState.None;
		}

		// ジャンプ
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			_jumpState = JumpState.Jump;
		}
		else
		{
			_jumpState = JumpState.None;
		}

		// 攻撃
		if (Input.GetKeyDown(KeyCode.X))
		{
			_attackState = AttackState.Attack;
		}
		else if (Input.GetKeyDown(KeyCode.Z))
		{
			_attackState = AttackState.Sheath;
		}
		else
		{
			_attackState = AttackState.None;
		}
	}

	/// <summary>
	/// ゲームパッド操作
	/// </summary>
	private void GamePadOperate()
	{
		_gamepad = Gamepad.current;
		if (_gamepad == null)
		{
			return;
		}

		// 左右操作
		var stickInput = _gamepad.leftStick.ReadValue();
		if (stickInput.x > 0.2f)
		{
			_moveState = MoveState.RightMove;
			_leftStickValue = stickInput.x;
		}
		else if (stickInput.x < -0.2f)
		{
			_moveState = MoveState.LeftMove;
			_leftStickValue = stickInput.x;
		}
		else
		{
			_moveState = MoveState.None;
		}

		// ジャンプ
		if (_gamepad.aButton.IsPressed())
		{
			_jumpState = JumpState.Jump;
		}
		else
		{
			_jumpState = JumpState.None;
		}

		// 攻撃
		if (_gamepad.xButton.IsPressed())
		{
			_attackState = AttackState.Attack;
		}
		else if (_gamepad.yButton.IsPressed())
		{
			_attackState = AttackState.Sheath;
		}
		else
		{
			_attackState = AttackState.None;
		}
	}
	
	/// <summary>
	/// 先行キー保存
	/// 必要に応じてジャンプ以外も対応
	/// </summary>
	private void LeadKey()
	{
		// キー入力
		if (_jumpState == JumpState.Jump)
		{
			_isLeadJumpKey = true;
			_leadKeyTimer = _leadKeyTime;
		}

		// 一定時間経ったら先行キー削除
		if (_isLeadJumpKey)
		{
			_leadKeyTimer--;
			if (_leadKeyTimer <= 0)
			{
				_isLeadJumpKey = false;
			}			
		}
	}
}
