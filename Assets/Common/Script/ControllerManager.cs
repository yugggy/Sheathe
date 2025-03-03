using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerManager : MonoBehaviour
{
	private Gamepad _gamepad;
	private State _state = State.None;
	private float _leftStickValue = 0.0f;

	public static ControllerManager Current;

	public State GetState => _state;
	public float LeftStickValue => _leftStickValue;

	public enum State
	{
		None,
		RightMove,
		LeftMove,
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
		_state = State.None;
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
			_state = State.RightMove;
			_leftStickValue = stickInput.x;
		}
		else if (stickInput.x < -0.2f)
		{
			_state = State.LeftMove;
			_leftStickValue = stickInput.x;
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
			_state = State.RightMove;
			_leftStickValue = 1.0f;
		}
		else if (Input.GetKey(KeyCode.LeftArrow))
		{
			_state = State.LeftMove;
			_leftStickValue = -1.0f;
		}

		// TODO�F�W�����v�͊�{��������Ȃ̂ŕ��s�ōs���K�v������
		if (Input.GetKey(KeyCode.UpArrow))
		{

		}
	}

}
