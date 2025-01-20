using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageManager : MonoBehaviour
{
	private bool isDestroyCompletely;
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
				GameOver();
			}
		}
	}

	public void SetDestroyCompletely(bool value)
	{
		isDestroyCompletely = value;
	}

	/// <summary>
	/// 結果発表
	/// </summary>
	public void Result()
	{
		// ステージ判定
		if (isDestroyCompletely)
		{
			isFirstAttack = false;
			Debug.Log("ステージクリア");
		}
		else
		{
			SceneGameManager.Current.ReloadStage();
		}

		isDestroyCompletely = false;
	}

	public void GameOver()
	{
		SceneGameManager.Current.ReloadStage();

		isDestroyCompletely = false;
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
}
