using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageManager : MonoBehaviour
{
	private bool isDestroyCompletely;
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

	public void SetDestroyCompletely(bool value)
	{
		isDestroyCompletely = value;
	}

	/// <summary>
	/// ���ʔ��\
	/// </summary>
	public void Result()
	{
		// �X�e�[�W����
		if (isDestroyCompletely)
		{
			isFirstAttack = false;
			Debug.Log("�X�e�[�W�N���A");
		}
		else
		{
			SceneGameManager.Current.ReloadStage();
		}

		isDestroyCompletely = false;
	}

	public void GameOver()
	{
		SceneGameManager.Current.ReloadStage();

		isDestroyCompletely = false;
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
