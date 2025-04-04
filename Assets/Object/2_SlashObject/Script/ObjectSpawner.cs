using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ObjectSpawner : MonoBehaviour
{
	[SerializeField] private string ObjectID;
	[SerializeField] private ObjectBase.Direnction direnction;


	private async void Start()
    {
		var objHandle = Addressables.LoadAssetAsync<GameObject>(ObjectID);
		var obj = await objHandle.Task;

		var lashObj = Instantiate(obj, transform.position, transform.rotation, transform.parent);
		var slashObj = lashObj.GetComponent<SlashBase>();
		slashObj.SetDirection(direnction == ObjectBase.Direnction.Right);

		// 地面から一定距離の地点から落下
		//if (slashObj.GetLanding() == ObjectBase.Landing.Ground)
		//{
		//	var hit = Physics2D.Raycast(transform.position, -transform.up * 10);
		//	if (hit.collider != null)
		//	{
		//		// TODO：オブジェクトのサイズから算出
		//		//var a = slashObj.GetFootPos();
		//		var gorundPos = hit.point;
		//		gorundPos.y += 1;
		//		slashObj.transform.position = gorundPos;
		//	}
		//}

		Destroy(this.gameObject);

		if (slashObj.IsCanSlash)
		{
			ObjectManager.Current.SetSlashObjectList(slashObj);
		}
	}

	// 非実行時
	public void OnValidate()
	{
		//Debug.Log("OnValidate");

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
			// スケールの設定
			Transform trans = AssetDatabase.LoadAssetAtPath<Transform>($"Assets/Object/2_SlashObject/{folderName}/Prefab/" + ObjectID + ".prefab");
			if (trans != null)
			{
				scale.transform.localScale = trans.Find("Scale").transform.localScale;
			}
			else
			{
				Debug.Log("Prefabがありません");
			}

			// 画像の設定
			if (scale.Find("Image").TryGetComponent<SpriteRenderer>(out var icon))
			{
				Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Object/2_SlashObject/Sprite/Editor/" + ObjectID + ".png");
				if (sprite)
				{
					icon.sprite = sprite;
				}
				else
				{
					Debug.Log("画像がありません");
				}
			}
		}
	}

	private void SetDirection()
	{
		var eulerAngles = transform.eulerAngles;

		switch (direnction)
		{
			case ObjectBase.Direnction.Right:
				eulerAngles.y = 0;
				break;
			case ObjectBase.Direnction.Left:
				eulerAngles.y = 180;
				break;
			default:
				break;
		}

		transform.eulerAngles = eulerAngles;
	}
}
