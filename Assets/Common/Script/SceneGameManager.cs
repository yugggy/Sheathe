using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SceneGameManager : MonoBehaviour
{
	private GameObject _stageObj;
	private string _stageName = "";
	
	// Addressableのキャッシュ
	// TODO：今後別のクラスに移行する
	private Dictionary<string, AsyncOperationHandle<GameObject>> _cache = new();

	public Dictionary<string, AsyncOperationHandle<GameObject>> AddressableCache => _cache;

	public static SceneGameManager Current;
	
	private void Awake()
	{
		Current = this;
	}

	private void Start()
	{
		// TODO：セーブデータから読み込んだステージ
		_stageName = "2_1";
		MoveStageAsync(_stageName, true, 0);
		Application.targetFrameRate = 60;
	}

	private void Update()
	{
		// Rキーでリトライ
		if (Input.GetKeyDown(KeyCode.R))
		{
			ReloadStageAsync();
		}
	}

	/// <summary>
	/// Addressableロード
	/// TODO：今後別のクラスに移行する
	/// </summary>
	public async Task<GameObject> LoadAsync(string key)
	{
		if (AddressableCache.TryGetValue(key, out var cachedHandle))
		{
			// 既にロードしていたら返す
			if (cachedHandle.Status == AsyncOperationStatus.Succeeded)
			{
				// Debug.Log($"{key}:既にロード済み");
				return cachedHandle.Result;
			}
			
			// 失敗していた場合破棄
			Addressables.Release(cachedHandle);
			AddressableCache.Remove(key);
		}
		
		// ロード
		var handle = Addressables.LoadAssetAsync<GameObject>(key);
		await handle.Task;
		
		if (handle.Status == AsyncOperationStatus.Succeeded)
		{
			AddressableCache[key] = handle;
			return handle.Result;
		}
		else
		{
			Debug.LogError($"{key}がロード出来ませんでした。");
			return null;
		}
	}

	/// <summary>
	/// ステージ再ロード
	/// </summary>
	public async Task ReloadStageAsync()
	{
		Debug.Log("ステージ再ロード");

		// オブジェクトリスト初期化
		ObjectManager.Current.ClearSlashObjectList();

		// プレイヤー削除
		ObjectManager.Current.PlayerDestroy();

		// ステージ生成
		await Task.Delay(1000);
		await MoveStageAsync(_stageName, true);
	}

	/// <summary>
	/// ステージ遷移
	/// </summary>
	public async Task MoveStageAsync(string stageName, bool isStart, float playerPosX = 0)
	{
		// オブジェクトリスト初期化
		ObjectManager.Current.ClearSlashObjectList();
		
		// Stage生成
		Destroy(_stageObj);
		_stageName = stageName;
		var obj = await LoadAsync($"Stage_{stageName}");
		_stageObj = Instantiate(obj, transform.position, transform.rotation, transform);
		
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
		await ObjectManager.Current.CreatePlayerAsync(playerSpawnPoint.position);
		
		Debug.Log($"Stage_{stageName}に遷移");
	}
}
