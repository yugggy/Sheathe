using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ステージ
/// </summary>
public class StageController : MonoBehaviour
{
	[SerializeField] private GateController _gateController;
	public static StageController Current;
	public GateController GateController => _gateController;
	private bool _isStageClear; // ステージクリアフラグ
	public bool IsStageClear => _isStageClear;
	
	
	private void Awake()
	{
		Current = this;
	}

	private void Start()
	{
		// SetGateController();
		//
		// // ステージ内のGate取得
		// void SetGateController()
		// {
		// 	var gateTrans = transform.Find("Gate");
		// 	if (gateTrans == null)
		// 	{
		// 		DebugLogger.Log($"{name}プレハブ内にGateがありません");
		// 	}
		// 	else
		// 	{
		// 		if (gateTrans.TryGetComponent<GateController>(out var gateController))
		// 		{
		// 			_gateController = gateController;
		// 		}
		// 		else
		// 		{
		// 			DebugLogger.Log($"{name}プレハブのGateにGateControllerが付いていません");
		// 		}
		// 	}
		// }
	}

	/// <summary>
	/// ステージクリア判定
	/// </summary>
	public async Task ResultAsync()
	{
		// 斬った敵殲滅
		ObjectManager.Current.DestroySlashObject();
		
		await Task.Delay(500);
		
		// 全滅判定
		if (ObjectManager.Current.GetDestroyCompletely() && !ObjectManager.Current.IsPlayerDamage())
		{
			// 扉開錠
			_gateController.ExitOpen();
			
			_isStageClear = true;
			
			DebugLogger.Log("ステージクリア");
		}
		// ダメージ受けていたら再ロード
		else if(ObjectManager.Current.IsPlayerDamage())
		{
			await SceneGameManager.Current.ReloadStageAsync();
		}
		// 全滅できなかったら
		else
		{
			// TODO：納刀一回制限、無制限で決めかねている
			// await SceneGameManager.Current.ReloadStageAsync();
		}
	}

	/// <summary>
	/// ステージクリアフラグのリセット
	/// </summary>
	public void ResetStageClearFlg()
	{
		_isStageClear = false;
	}
}
