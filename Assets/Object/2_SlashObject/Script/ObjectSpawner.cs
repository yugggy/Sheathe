using UnityEngine;
using UnityEngine.AddressableAssets;

public class ObjectSpawner : MonoBehaviour
{
	[SerializeField] private string ObjectID;

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
