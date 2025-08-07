using System.Collections.Generic;
using System.Threading.Tasks;
using Object._2_SlashObject.Script;
using Stage.Script;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Common.Script
{
	public class SceneGameManager : MonoBehaviour
	{
		private GameObject _stageObj;
		private string _stageName = "";
	
		private bool _isMoveStage;
		private bool _isReloadStage;
	
		// Addressableのキャッシュ
		// TODO：今後別のクラスに移行する
		private Dictionary<string, AsyncOperationHandle<GameObject>> _cache = new();

		public Dictionary<string, AsyncOperationHandle<GameObject>> AddressableCache => _cache;

		public static SceneGameManager Current;

		// TODO：今後別のクラスに移行する
		[SerializeField] public int HitStopTime;
	
		private void Awake()
		{
			Current = this;
		}

		private void Start()
		{
			Application.targetFrameRate = 60;
		
			// TODO：ステージ名をセーブデータから読み込む
			_stageName = "2_1";
			TaskUtility.FireAndForget(MoveStageAsync(_stageName, true), "MoveStageAsync");
		}

		private void Update()
		{
			// Rキーでリトライ
			if (Input.GetKeyDown(KeyCode.R) && !_isReloadStage && !_isMoveStage)
			{
				TaskUtility.FireAndForget(ReloadStageAsync(), "ReloadStageAsync");
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
					// DebugLogger.Log($"{key}:既にロード済み");
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
				DebugLogger.LogError($"{key}がロード出来ませんでした。");
				return null;
			}
		}

		/// <summary>
		/// ステージ再生成
		/// </summary>
		public async Task ReloadStageAsync()
		{
			if (_isReloadStage)
			{
				DebugLogger.Log("ステージ再ロード キャンセル");	
			}
		
			_isReloadStage = true;
		
			DebugLogger.Log("ステージ再ロード");

			// オブジェクトリスト初期化
			ObjectManager.Current.ClearSlashObjectList();
	
			// プレイヤー削除
			ObjectManager.Current.PlayerDestroy();

			// ステージ生成
			await MoveStageAsync(_stageName, true, true);
		
			_isReloadStage = false;
		}

		/// <summary>
		/// ステージ遷移
		/// </summary>
		public async Task MoveStageAsync(string stageName, bool isStartPoint, bool isReloadStage = false)
		{
			if (_isMoveStage)
			{
				DebugLogger.Log("ステージ遷移 キャンセル");	
			}
		
			_isMoveStage = true;
		
			if (isReloadStage)
			{
				// TODO：プレイヤー削除してから一定時間後ステージ再ロード
				await Task.Delay(1000);			
			}
		
			// オブジェクトリスト初期化
			ObjectManager.Current.ClearSlashObjectList();
		
			// Stage生成
			Destroy(_stageObj);
			_stageName = stageName;
		
			var obj = await LoadAsync($"Stage_{stageName}");
			if (obj == null)
			{
				return;
			}
			_stageObj = Instantiate(obj, transform.position, transform.rotation, transform);
		
			// ステージに設定されている生成ポイント取得
			if (!_stageObj.TryGetComponent<StageController>(out var stageManager))
			{
				DebugLogger.Log($"Stage_{stageName}にStageManagerが付いていません");
				return;
			}
			DebugLogger.Log($"Stage_{stageName}に遷移");

			var gateController = stageManager.GateController;
			if (gateController == null)
			{
				DebugLogger.Log($"Stage_{stageName}のGateにGateControllerが付いていません");
				return;
			}
		
			var playerSpawnPoint = gateController.GetPlayerSpawnPoint(isStartPoint);
			if (playerSpawnPoint == null)
			{
				DebugLogger.Log($"Stage_{stageName}の{(isStartPoint ? "Start" : "End")}GateのplayerSpawnPointがありません");
				return;
			}
		
			// Player生成
			await ObjectManager.Current.CreatePlayerAsync(playerSpawnPoint.position);
		
			_isMoveStage = false;
		}
	}
}
