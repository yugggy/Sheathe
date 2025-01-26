using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageManager : MonoBehaviour
{
	private float firstAttackTimer = 0;
	private bool isFirstAttack = false;
	private float firstAttackTime = 10;

	public static StageManager Current;


	private void Awake()
	{
		Current = this;
	}

	private void Update()
	{
		if (isFirstAttack)
		{
			firstAttackTimer += Time.deltaTime;
			if (firstAttackTimer >= firstAttackTime)
			{
				Debug.Log("�ŏ��̍U������10�b");
				firstAttackTimer = 0;
				GameOver();
			}
		}
	}

	/// <summary>
	/// ���ʔ��\
	/// </summary>
	public void Result()
	{
		// �S�Ŕ���
		if (ObjectManager.Current.GetDestroyCompletely())
		{
			isFirstAttack = false;

			// �a�����G�r��
			ObjectManager.Current.DestroySlashObject();

			Debug.Log("�X�e�[�W�N���A");
		}
		else
		{
			isFirstAttack = false;

			// �a�����G�r��
			ObjectManager.Current.DestroySlashObject();

			SceneGameManager.Current.ReloadStage();
		}
	}

	public void GameOver()
	{
		SceneGameManager.Current.ReloadStage();
	}

	/// <summary>
	/// �ŏ��̍U��
	/// </summary>
	public void StartFirstAttack()
	{
		if (isFirstAttack)
		{
			return;
		}
		else
		{
			Debug.Log("�ŏ��̍U��");
			isFirstAttack = true;
		}
	}
}
