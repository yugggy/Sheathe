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
	public GameObject Player => playerObj;

	private void Awake()
	{
		Current = this;
	}

	private void Start()
	{
		// TODO：セーブデータから読み込んだステージ名
		_stageName = "2_1";

		MoveStage(_stageName, true, 4);
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
		await Task.Delay(1000);
		Destroy(playerObj);

		// ステージ生成
		await Task.Delay(300);
		MoveStage(_stageName, true);
	}

	/// <summary>
	/// ステージ遷移
	/// </summary>
	public async void MoveStage(string stageName, bool isStart, float playerPosX = 0)
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

		// Player生成
		if (playerObj == null)
		{
			var playerHandle = Addressables.LoadAssetAsync<GameObject>("Player");
			var player = await playerHandle.Task;
			playerObj = Instantiate(player, Vector3.zero, transform.rotation, transform);
		}
		// ステージに設定されている生成ポイントを元に生成する
		var gateController = stageObj.GetComponent<StageManager>().GetGateController();
		if (gateController != null)
		{
			var playerSpawnPoint = isStart ? gateController.GetPlayerSpawnPoint(true) : gateController.GetPlayerSpawnPoint(false);
			if (playerSpawnPoint != null)
			{
				playerObj.transform.position = playerSpawnPoint.position;
				var hit = Physics2D.Raycast(playerSpawnPoint.position, Vector2.down * 10);
				if (hit.collider != null)
				{
					// TODO：地面に到達したところからプレイヤーの大きさ分上に生成する
					var pos = hit.point + new Vector2(0, 0.6f);
					pos = pos + (isStart ? 1 : -1) * new Vector2(playerPosX, 0);
					playerObj.transform.position = pos;
					Debug.Log($"Stage_{stageName}に遷移");
				}
				else
				{
					Debug.Log($"レイが当たっていない");
				}
			}
		}
	}
}
