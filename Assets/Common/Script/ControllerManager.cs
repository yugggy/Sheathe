using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerManager : MonoBehaviour
{
	private Gamepad _gamepad;
	private float _leftStickValue = 0.0f;
	private MoveState _moveState = MoveState.None;
	private JumpState _jumpState = JumpState.None;
	private AttackState _attackState = AttackState.None;

	public static ControllerManager Current;

	public float LeftStickValue => _leftStickValue;
	public MoveState GetMoveState => _moveState;
	public JumpState GetJumpState => _jumpState;

	public AttackState GetAttackState => _attackState;


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
	}

	private void Operate()
	{
		// ������
		_moveState = MoveState.None;
		_leftStickValue = 0;

		// �Q�[���p�b�h����
		GamePadOperate();

		// �L�[�{�[�h����
		KeyboradOperate();

		//Debug.Log($"����F{_state}");
		//Debug.Log($"�ړ��F{_leftStickValue}");
	}

	/// <summary>
	/// �Q�[���p�b�h����
	/// </summary>
	private void GamePadOperate()
	{
		_gamepad = Gamepad.current;
		if (_gamepad == null)
		{
			return;
		}

		// ���E����
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

		// �W�����v
		if (_gamepad.aButton.IsPressed())
		{
			_jumpState = JumpState.Jump;
		}
		else
		{
			_jumpState = JumpState.None;
		}

		// �U��
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
	/// �L�[�{�[�h����
	/// </summary>
	private void KeyboradOperate()
	{
		// ���E����
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

		// �W�����v
		if (Input.GetKey(KeyCode.UpArrow))
		{
			_jumpState = JumpState.Jump;
		}

		// �U��
		if (Input.GetKeyDown(KeyCode.X))
		{
			_attackState = AttackState.Attack;
		}
		else if (Input.GetKeyDown(KeyCode.Z))
		{
			_attackState = AttackState.Sheath;
		}
	}
}
