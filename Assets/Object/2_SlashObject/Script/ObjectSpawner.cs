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
			// �X�P�[���̐ݒ�
			Transform trans = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Object/2_SlashObject/StageObject/Prefab/" + ObjectID + ".prefab");
			if (trans != null)
			{
				scale.transform.localScale = trans.Find("Scale").transform.localScale;
			}
			else
			{
				Debug.Log("Prefab������܂���");
			}

			// �摜�̐ݒ�
			if (scale.Find("Image").TryGetComponent<SpriteRenderer>(out var icon))
			{
				Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Object/2_SlashObject/Sprite/Editor/" + ObjectID + ".png");
				if (sprite)
				{
					icon.sprite = sprite;
				}
				else
				{
					Debug.Log("�摜������܂���");
				}
			}
		}
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	private async void Start()
    {
		var objHandle = Addressables.LoadAssetAsync<GameObject>(ObjectID);
		var obj = await objHandle.Task;
		var lashObj = Instantiate(obj, transform.position, transform.rotation, transform.parent);
		var slashObj = lashObj.GetComponent<SlashBase>();
		Destroy(this.gameObject);

		if (slashObj.IsCanSlash)
		{
			ObjectManager.Current.SetSlashObjectList(slashObj);
		}
	}
}
