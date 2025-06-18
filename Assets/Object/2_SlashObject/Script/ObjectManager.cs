using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ObjectManager : MonoBehaviour
{
	private PlayerController _player;
	private GameObject _playerOrigin;
	private List<SlashBase> _slashList = new List<SlashBase>(50);

	public static ObjectManager Current;
	public PlayerController Player => _player;

	private void Awake()
	{
		Current = this;
	}

	/// <summary>
	/// プレイヤー生成
	/// </summary>
	public async Task CreatePlayer(Vector3 position)
	{
		if (_playerOrigin == null)
		{
			var playerHandle = Addressables.LoadAssetAsync<GameObject>("Player");
			_playerOrigin = await playerHandle.Task;
		}

		if (_player == null)
		{
			var playerObj = Instantiate(_playerOrigin, position, transform.rotation, transform);
			if (playerObj.TryGetComponent<PlayerController>(out var playerController))
			{
				_player = playerController;
				_player.SetDirection(true);
			}
		}
		else
		{
			_player.transform.position = position;
			_player.SetDirection(true);
		}
	}

	/// <summary>
	/// プレイヤーダメージ判定
	/// </summary>
	public bool IsPlayerDamage()
	{
		return _player.IsDamage;
	}
	
	/// <summary>
	/// プレイヤー削除
	/// </summary>
	public void PlayerDestroy()
	{
		Destroy(_player.gameObject);
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
		foreach (var slash in _slashList)
		{
			if (slash.IsSlashed)
			{
				slash.Destroy();
			}
		}
	}

	/// <summary>
	/// リスト初期化
	/// </summary>
	public void ClearSlashObjectList()
	{
		_slashList.Clear();
	}
}
