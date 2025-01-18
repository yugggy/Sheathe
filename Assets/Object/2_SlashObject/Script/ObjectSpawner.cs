using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static UnityEngine.GraphicsBuffer;

public class ObjectSpawner : MonoBehaviour
{
	[SerializeField] private string ObjectID;

	public void OnValidate()
	{
		//Debug.Log("OnValidate");

		var scale = transform.Find("Scale");
		if (scale != null)
		{
			// スケールの設定
			Transform trans = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Object/2_SlashObject/StageObject/Prefab/" + ObjectID + ".prefab");
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

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	private async void Start()
    {
		var objHandle = Addressables.LoadAssetAsync<GameObject>(ObjectID);
		var obj = await objHandle.Task;
		// TODO：地面から一定距離の地点から落下

		var lashObj = Instantiate(obj, transform.position, transform.rotation, transform.parent);
		var slashObj = lashObj.GetComponent<SlashBase>();
		Destroy(this.gameObject);

		if (slashObj.IsCanSlash)
		{
			ObjectManager.Current.SetSlashObjectList(slashObj);
		}
	}
}
