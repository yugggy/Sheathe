using UnityEngine;

/// <summary>
/// ステージ内のゲート
/// </summary>
public class GateController : MonoBehaviour
{
	[SerializeField] private Gate _startGate;
	[SerializeField] private Gate _endGate;

	[SerializeField] private string _frontStageName;
	[SerializeField] private string _nextStageName;
	
	private void Start()
	{
		_startGate.GetAction += FrontStage;
		_endGate.GetAction += NextStage;
	}
	
	/// <summary>
	/// 前のステージ遷移
	/// </summary>
	private void FrontStage()
	{
		if(_frontStageName == "")
		{
			Debug.Log("遷移する前のステージ名が入力されていません。");
			return;
		}

		SceneGameManager.Current.MoveStageAsync(_frontStageName, false);
	}

	/// <summary>
	/// 次のステージ遷移
	/// </summary>
	private void NextStage()
	{
		if (_nextStageName == "")
		{
			Debug.Log("遷移する次のステージ名が入力されていません。");
			return;
		}

		Debug.Log($"{_nextStageName}");
		SceneGameManager.Current.MoveStageAsync(_nextStageName, true);
	}

	/// <summary>
	/// 出口開錠
	/// </summary>
	public void ExitOpen()
	{
		_endGate.ExitOpen();
	}

	/// <summary>
	/// ステージ上のプレイヤー生成座標取得
	/// </summary>
	public Transform GetPlayerSpawnPoint(bool isStart)
	{
		return isStart ? _startGate.PlayerSpawnPoint :  _endGate.PlayerSpawnPoint;
	}
}
