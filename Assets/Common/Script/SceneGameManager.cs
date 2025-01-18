using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SceneGameManager : MonoBehaviour
{	private async void Start()
	{
		// Stageê∂ê¨
		var stageHandle = Addressables.LoadAssetAsync<GameObject>("Stage_1_1");
		var stage = await stageHandle.Task;
		var stageObj = Instantiate(stage, transform.position, transform.rotation, transform);

		// Playerê∂ê¨
		var playerHandle = Addressables.LoadAssetAsync<GameObject>("Player");
		var player = await playerHandle.Task;
		var playerPostion = new Vector3(-9, -2, 0);
		var playerObj = Instantiate(player, playerPostion, transform.rotation, transform);
	}
}
