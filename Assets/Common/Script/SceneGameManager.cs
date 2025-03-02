using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SceneGameManager : MonoBehaviour
{
	private GameObject stageObj;
	private GameObject playerObj;

	public static SceneGameManager Current;

	private void Awake()
	{
		Current = this;
	}

	private void Start()
	{
		CreateStage();
	}

	private void Update()
	{
		// R�L�[�Ń��g���C
		if (Input.GetKeyDown(KeyCode.R))
		{
			ReloadStage();
		}
	}

	// �X�e�[�W����
	private async void CreateStage()
	{
		// Stage����
		Destroy(stageObj);
		var stageHandle = Addressables.LoadAssetAsync<GameObject>("Stage_1_2");
		var stage = await stageHandle.Task;
		stageObj = Instantiate(stage, transform.position, transform.rotation, transform);
		stageObj.transform.position = new Vector3(stageObj.transform.position.x, -4.0f, stageObj.transform.position.z);

		// Player����
		var playerHandle = Addressables.LoadAssetAsync<GameObject>("Player");
		var player = await playerHandle.Task;
		var playerPostion = new Vector3(-9, -5, 0);
		playerObj = Instantiate(player, playerPostion, transform.rotation, transform);
	}

	// �X�e�[�W�ă��[�h
	public async void ReloadStage()
	{
		Debug.Log("�X�e�[�W�ă��[�h");

		// �I�u�W�F�N�g���X�g������
		ObjectManager.Current.ClearSlashObjectList();

		// �v���C���[�폜
		await Task.Delay(1000);
		Destroy(playerObj);

		// �X�e�[�W����
		await Task.Delay(300);
		CreateStage();
	}
}
