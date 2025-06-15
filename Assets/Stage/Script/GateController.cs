using UnityEngine;

public class GateController : MonoBehaviour
{
	[SerializeField] private Gate _startGate;
	[SerializeField] private Gate _endGate;

	[SerializeField] private string _frontStageName;
	[SerializeField] private string _nextStageName;


	private void Start()
	{
		_startGate.GetAction += frontStage;
		_endGate.GetAction += nextStage;
	}

	public void DoorOpen()
	{
		_endGate.DoorOpen();
	}

	/// <summary>
	/// 前のステージ遷移
	/// </summary>
	private void frontStage()
	{
		if(_frontStageName == "")
		{
			Debug.Log("遷移する前のステージ名が入力されていません。");
			return;
		}

		SceneGameManager.Current.MoveStage(_frontStageName, false);
	}

	/// <summary>
	/// 次のステージ遷移
	/// </summary>
	private void nextStage()
	{
		if (_nextStageName == "")
		{
			Debug.Log("遷移する次のステージ名が入力されていません。");
			return;
		}

		SceneGameManager.Current.MoveStage(_nextStageName, true);
	}

	/// <summary>
	/// プレイヤー生成座標取得
	/// </summary>
	public Transform GetPlayerSpawnPoint(bool isStart)
	{
		var gate = isStart ? _startGate : _endGate;
		var playerSpawnPoint = gate.transform.Find("PlayerSpawnPoint");
		if (playerSpawnPoint == null)
		{
			Debug.Log("プレイヤーが生成される座標であるPlayerSpawnPointオブジェクトがありません。");
			return null;
		}

		return playerSpawnPoint;
	}
}
