using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SceneGameManager : MonoBehaviour
{
	private GameObject _stageObj;
	private string _stageName = "";

	public static SceneGameManager Current;

	private void Awake()
	{
		Current = this;
	}

	private void Start()
	{
		// TODO：セーブデータから読み込んだステージ
		_stageName = "2_1";
		MoveStage(_stageName, true, 0);
	}

	private void Update()
	{
		// Rキーでリトライ
		if (Input.GetKeyDown(KeyCode.R))
		{
			ReloadStage();
		}
	}

	/// <summary>
	/// ステージ再ロード
	/// </summary>
	public async void ReloadStage()
	{
		Debug.Log("ステージ再ロード");

		// オブジェクトリスト初期化
		ObjectManager.Current.ClearSlashObjectList();

		// プレイヤー削除
		ObjectManager.Current.PlayerDestroy();

		// ステージ生成
		await Task.Delay(1000);
		MoveStage(_stageName, true);
	}

	/// <summary>
	/// ステージ遷移
	/// </summary>
	public async void MoveStage(string stageName, bool isStart, float playerPosX = 0)
	{
		// オブジェクトリスト初期化
		ObjectManager.Current.ClearSlashObjectList();
		
		// Stage生成
		var stageHandle = Addressables.LoadAssetAsync<GameObject>($"Stage_{stageName}");
		var stage = await stageHandle.Task;
		if (stage == null)
		{
			Debug.Log($"指定のステージ：Stage_{stageName}が存在しないため遷移できません。");
		}
		_stageName = stageName;
		Destroy(_stageObj);
		_stageObj = Instantiate(stage, transform.position, transform.rotation, transform);
		
		// ステージに設定されている生成ポイント取得
		if (!_stageObj.TryGetComponent<StageManager>(out var stageManager))
		{
			Debug.Log($"Stage_{stageName}にStageManagerが付いていません");
			return;
		}

		var gateController = stageManager.GateController;
		if (gateController == null)
		{
			Debug.Log($"Stage_{stageName}のGateにGateControllerが付いていません");
			return;
		}
		
		var playerSpawnPoint = gateController.GetPlayerSpawnPoint(isStart);
		if (playerSpawnPoint == null)
		{
			Debug.Log($"Stage_{stageName}の{(isStart ? "Start" : "End")}GateのplayerSpawnPointがありません");
			return;
		}
		
		// Player生成
		await ObjectManager.Current.CreatePlayer(playerSpawnPoint.position);
		
		Debug.Log($"Stage_{stageName}に遷移");
	}
}
