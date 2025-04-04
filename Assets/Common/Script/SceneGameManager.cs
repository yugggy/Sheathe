using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SceneGameManager : MonoBehaviour
{
	private GameObject stageObj = null;
	private GameObject playerObj = null;
	private string _stageName = "";

	public static SceneGameManager Current;

	private void Awake()
	{
		Current = this;
	}

	private void Start()
	{
		// TODO：セーブデータから読み込んだステージ名
		_stageName = "2_1";

		MoveStage(_stageName, true);
	}

	private void Update()
	{
		// Rキーでリトライ
		if (Input.GetKeyDown(KeyCode.R))
		{
			ReloadStage();
		}
	}

	// ステージ遷移
	public async void MoveStage(string stageName, bool isStart)
	{
		// Stage生成
		var stageHandle = Addressables.LoadAssetAsync<GameObject>($"Stage_{stageName}");
		var stage = await stageHandle.Task;
		if (stage == null)
		{
			Debug.Log($"指定のステージ：Stage_{stageName}が存在しないため遷移できません。");
		}
		_stageName = stageName;
		Destroy(stageObj);
		stageObj = Instantiate(stage, transform.position, transform.rotation, transform);
		stageObj.transform.position = new Vector3(stageObj.transform.position.x, -4.0f, stageObj.transform.position.z);

		// Player生成
		if (playerObj == null)
		{
			var playerHandle = Addressables.LoadAssetAsync<GameObject>("Player");
			var player = await playerHandle.Task;
			playerObj = Instantiate(player, Vector3.zero, transform.rotation, transform);
		}
		var gateController = stageObj.GetComponent<StageManager>().GetGateController();
		if (gateController != null)
		{
			var playerSpawnPoint = isStart ? gateController.GetPlayerSpawnPoint(true) : gateController.GetPlayerSpawnPoint(false);
			if (playerSpawnPoint != null)
			{
				playerObj.transform.position = playerSpawnPoint.position;
				Debug.Log($"Stage_{stageName}に遷移");
			}
		}
	}

	// ステージ再ロード
	public async void ReloadStage()
	{
		Debug.Log("ステージ再ロード");

		// オブジェクトリスト初期化
		ObjectManager.Current.ClearSlashObjectList();

		// プレイヤー削除
		await Task.Delay(1000);
		Destroy(playerObj);

		// ステージ生成
		await Task.Delay(300);
		MoveStage(_stageName, true);
	}
}
