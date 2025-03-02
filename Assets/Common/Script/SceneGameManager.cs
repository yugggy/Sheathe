using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SceneGameManager : MonoBehaviour
{
	private GameObject stageObj;
	private GameObject playerObj;

	public static SceneGameManager Current;

	private void Awake()
	{
		Current = this;
	}

	private void Start()
	{
		CreateStage();
	}

	private void Update()
	{
		// Rキーでリトライ
		if (Input.GetKeyDown(KeyCode.R))
		{
			ReloadStage();
		}
	}

	// ステージ生成
	private async void CreateStage()
	{
		// Stage生成
		Destroy(stageObj);
		var stageHandle = Addressables.LoadAssetAsync<GameObject>("Stage_1_2");
		var stage = await stageHandle.Task;
		stageObj = Instantiate(stage, transform.position, transform.rotation, transform);
		stageObj.transform.position = new Vector3(stageObj.transform.position.x, -4.0f, stageObj.transform.position.z);

		// Player生成
		var playerHandle = Addressables.LoadAssetAsync<GameObject>("Player");
		var player = await playerHandle.Task;
		var playerPostion = new Vector3(-9, -5, 0);
		playerObj = Instantiate(player, playerPostion, transform.rotation, transform);
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
		CreateStage();
	}
}
