using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static UnityEngine.GraphicsBuffer;

public class ObjectSpawner : MonoBehaviour
{
	[SerializeField] private string ObjectID;

	private async void Start()
    {
		var objHandle = Addressables.LoadAssetAsync<GameObject>(ObjectID);
		var obj = await objHandle.Task;

		var lashObj = Instantiate(obj, transform.position, transform.rotation, transform.parent);
		var slashObj = lashObj.GetComponent<SlashBase>();

		if (slashObj.GetLanding() == ObjectBase.Landing.Ground)
		{
			// �n�ʂ����苗���̒n�_���痎��
			var hit = Physics2D.Raycast(transform.position, -transform.up * 10);
			if (hit.collider != null)
			{
				// TODO�F�I�u�W�F�N�g�̃T�C�Y����Z�o
				//var a = slashObj.GetFootPos();
				var gorundPos = hit.point;
				gorundPos.y += 1;
				slashObj.transform.position = gorundPos;
			}
		}

		Destroy(this.gameObject);

		if (slashObj.IsCanSlash)
		{
			ObjectManager.Current.SetSlashObjectList(slashObj);
		}
	}

	// ����s��
	public void OnValidate()
	{
		//Debug.Log("OnValidate");

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
			// �X�P�[���̐ݒ�
			Transform trans = AssetDatabase.LoadAssetAtPath<Transform>($"Assets/Object/2_SlashObject/{folderName}/Prefab/" + ObjectID + ".prefab");
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
}
