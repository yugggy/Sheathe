using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageManager : MonoBehaviour
{
	private float firstAttackTimer = 0;
	private bool isFirstAttack = false;
	private float firstAttackTime = 10;

	public static StageManager Current;


	private void Awake()
	{
		Current = this;
	}

	private void Update()
	{
		if (isFirstAttack)
		{
			firstAttackTimer += Time.deltaTime;
			if (firstAttackTimer >= firstAttackTime)
			{
				Debug.Log("最初の攻撃から10秒");
				firstAttackTimer = 0;
				//GameOver();
			}
		}
	}

	/// <summary>
	/// 結果発表
	/// </summary>
	public void Result()
	{
		// 全滅判定
		if (ObjectManager.Current.GetDestroyCompletely())
		{
			isFirstAttack = false;

			// 斬った敵殲滅
			ObjectManager.Current.DestroySlashObject();

			Debug.Log("ステージクリア");
		}
		else
		{
			isFirstAttack = false;

			// 斬った敵殲滅
			ObjectManager.Current.DestroySlashObject();

			SceneGameManager.Current.ReloadStage();
		}
	}

	public void GameOver()
	{
		SceneGameManager.Current.ReloadStage();
	}

	/// <summary>
	/// 最初の攻撃
	/// </summary>
	public void StartFirstAttack()
	{
		if (isFirstAttack)
		{
			return;
		}
		else
		{
			Debug.Log("最初の攻撃");
			isFirstAttack = true;
		}
	}

	/// <summary>
	/// Gateの取得
	/// </summary>
	public GateController GetGateController()
	{
		var gate = transform.Find("Gate").gameObject;
		if (gate == null)
		{
			Debug.Log("このステージにはGateがありません");
			return null;
		}

		if (gate.TryGetComponent<GateController>(out var gateController))
		{
			return gateController;
		}
		else
		{
			Debug.Log("このステージのGateにGateControllerコンポーネントが付与されていません。");
			return null;
		}
	}

}
