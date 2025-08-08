using System.Threading.Tasks;
using Common.Script;
using Object._2_SlashObject.Enemy.Script;
using Object._2_SlashObject.Enemy.Script.EnemyParam;
using UnityEditor;
using UnityEngine;

namespace Object._2_SlashObject.Script
{
	/// <summary>
	/// オブジェクトのスポナー
	/// </summary>
	public class ObjectSpawner : MonoBehaviour
	{
		[SerializeField] private string _objectID;
		[SerializeField] private ObjectBase.Direction _direction;

		private void Start()
		{
			TaskUtility.FireAndForget(SpawnAsync(),"SpawnAsync");
		
			async Task SpawnAsync()
			{
				var obj = await SceneGameManager.Current.LoadAsync(_objectID);
				if (obj == null)
				{
					return;
				}
				var slashObj = Instantiate(obj, transform.position, transform.rotation, transform.parent);
				if (slashObj.TryGetComponent<SlashBase>(out var slash))
				{
					slash.SetDirection(_direction == ObjectBase.Direction.Right);
					if (slash.IsCanSlash)
					{
						ObjectManager.Current.SetSlashObjectList(slash);
					}
					
					// EnemyParam
					if (slash.TryGetComponent<EnemyBase>(out var enemy) && TryGetComponent<EnemyParamBase>(out var param))
					{
						enemy.SetEnemyParams(param);
					}
				}
				else
				{
					DebugLogger.Log($"{_objectID}プレハブにSlashBaseが付いていません");
				}
		    
				// スポナーの削除
				if (!slash.IsRespawn)
				{
					Destroy(this.gameObject);
				}
				else
				{
					// 削除しないスポナーはImageの非表示だけ行う
					ImageInActive();
			    
					// 生成したオブジェクトが撃破されたときのアクション設定
					slash.DestroyAction = () => TaskUtility.FireAndForget(RespawnAsync(),"RespawnAsync");
				}
			}
	    
			// 削除しないスポナーのImageの非表示
			void ImageInActive()
			{
				var scaleTrans = transform.Find("Scale");
				if (scaleTrans == null)
				{
					DebugLogger.Log($"{name}スポナーにScaleがありません");
					return;
				}
			
				var scaleImage = scaleTrans.Find("Image");
				if (scaleImage == null)
				{
					DebugLogger.Log($"{name}スポナーにImageがありません");
					return;
				}
			
				scaleImage.gameObject.SetActive(false);
			}

			// 一定時間後、再生成
			async Task RespawnAsync()
			{
				// 2秒待つ
				await Task.Delay(2000);
		    
				await SpawnAsync();
			}
		}

		/// <summary>
		/// 非実行時にプレハブと同期
		/// </summary>
		private void OnValidate()
		{
			// 向きの設定
			SetDirection();

			// フォルダ名
			var folderName = "";
			if(transform.name.StartsWith("SPN_EN"))
			{
				folderName = "Enemy";
			}
			else if (transform.name.StartsWith("SPN_SO"))
			{
				folderName = "StageObject";
			}

			var scale = transform.Find("Scale");
			if (scale != null)
			{
				// Scaleの同期
				Transform trans = AssetDatabase.LoadAssetAtPath<Transform>($"Assets/Object/2_SlashObject/{folderName}/Prefab/" + _objectID + ".prefab");
				if (trans != null)
				{
					scale.transform.localScale = trans.Find("Scale").transform.localScale;
				}
				else
				{
					DebugLogger.Log("Scaleの同期が出来ません");
				}
			}
		}

		/// <summary>
		/// 向き変更
		/// </summary>
		private void SetDirection()
		{
			var eulerAngles = transform.eulerAngles;
			eulerAngles.y = (_direction == ObjectBase.Direction.Right) ? 0 : 180;
			transform.eulerAngles = eulerAngles;
		}
	}
}
