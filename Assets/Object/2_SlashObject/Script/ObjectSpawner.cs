using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// オブジェクトのスポナー
/// </summary>
public class ObjectSpawner : MonoBehaviour
{
	[SerializeField] private string _objectID;
	[SerializeField] private ObjectBase.Direction _direction;

	private async void Start()
    {
	    var obj = await SceneGameManager.Current.LoadAsync(_objectID);
		var slashObj = Instantiate(obj, transform.position, transform.rotation, transform.parent);
		if (slashObj.TryGetComponent<SlashBase>(out var slash))
		{
			slash.SetDirection(_direction == ObjectBase.Direction.Right);
			if (slash.IsCanSlash)
			{
				ObjectManager.Current.SetSlashObjectList(slash);
			}
		}
		else
		{
			Debug.Log($"{_objectID}プレハブにSlashBaseが付いていません");
		}
		
		Destroy(this.gameObject);
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
				Debug.Log("Scaleの同期が出来ません");
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
