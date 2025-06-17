using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
	private List<SlashBase> _slashList = new List<SlashBase>(50);
	private PlayerController _player;

	public static ObjectManager Current;
	public List<SlashBase> SlashList => _slashList;

	private void Awake()
	{
		Current = this;
	}
	
	/// <summary>
	/// プレイヤー生成時
	/// </summary>
	public void SetPlayer(PlayerController player)
	{
		_player = player;
	}

	/// <summary>
	/// プレイヤーダメージ判定
	/// </summary>
	public bool IsPlayerDamage()
	{
		return _player.IsDamage;
	}
	
	/// <summary>
	/// プレイヤークリア
	/// </summary>
	public void ClearPlayer()
	{
		_player = null;
	}

	/// <summary>
	/// 敵生成時
	/// </summary>
	public void SetSlashObjectList(SlashBase slashObj)
	{
		_slashList.Add(slashObj);
	}

	/// <summary>
	/// 全滅判定
	/// </summary>
	public bool GetDestroyCompletely()
	{
		return !_slashList.Any(x => !x.IsSlashed);
	}

	/// <summary>
	/// 斬った敵殲滅
	/// </summary>
	public void DestroySlashObject()
	{
		foreach (var slash in SlashList)
		{
			if (slash.IsSlashed)
			{
				slash.Destroy();
			}
		}

		ClearSlashObjectList();
	}

	/// <summary>
	/// リスト初期化
	/// </summary>
	public void ClearSlashObjectList()
	{
		_slashList.Clear();
	}
}
