using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerManager : MonoBehaviour
{
	private Gamepad _gamepad;
	private MoveState _moveState = MoveState.None;
	private float _leftStickValue = 0.0f;

	public static ControllerManager Current;

	public MoveState GetMoveState => _moveState;
	public float LeftStickValue => _leftStickValue;

	public enum MoveState
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
		// 初期化
		_moveState = MoveState.None;
		_leftStickValue = 0;

		// ゲームパッド操作
		GamePadOperate();

		// キーボード操作
		KeyboradOperate();

		//Debug.Log($"操作：{_state}");
		//Debug.Log($"移動：{_leftStickValue}");
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

		// TODO：ジャンプは基本同時操作なので並行で行う必要がある
		if (Input.GetKey(KeyCode.UpArrow))
		{

		}
	}

}
