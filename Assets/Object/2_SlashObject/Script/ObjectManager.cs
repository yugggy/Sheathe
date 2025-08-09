using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Script;
using Object._1_Player.Script;
using UnityEngine;

namespace Object._2_SlashObject.Script
{
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
		public async Task CreatePlayerAsync(Vector3 position)
		{
			var obj = await SceneGameManager.Current.LoadAsync("Player");
			if (obj == null)
			{
				return;
			}
		
			if (_player == null)
			{
				var playerObj = Instantiate(obj, position, transform.rotation, transform);
				if (playerObj.TryGetComponent<PlayerController>(out var playerController))
				{
					_player = playerController;
				}
			}
			else
			{
				_player.transform.position = position;
			}
		
			await _player.InitAsync();
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
			// 敵が全員死亡しているか
			return _slashList.Any() && _slashList.All(x => x.IsDead);
			
			// 旧全滅判定（敵全員が斬られているか）
			// return _slashList.Any() && _slashList.All(x => x.IsSlashed);
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
					TaskUtility.FireAndForget(slash.DestroyAsync(), "slash.DestroyAsync");
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
}
