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
				Debug.Log("Å‰‚ÌUŒ‚‚©‚ç10•b");
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
	/// Œ‹‰Ê”­•\
	/// </summary>
	public async void Result()
	{
		// ƒXƒe[ƒW”»’è
		if (isDestroyCompletely)
		{
			isFirstAttack = false;

			// a‚Á‚½“GŸr–Å
			await ObjectManager.Current.DestroySlashObject();

			Debug.Log("ƒXƒe[ƒWƒNƒŠƒA");
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
	/// Å‰‚ÌUŒ‚
	/// </summary>
	public void StartFirstAttack()
	{
		if (isFirstAttack)
		{
			return;
		}
		else
		{
			Debug.Log("Å‰‚ÌUŒ‚");
			isFirstAttack = true;
		}
	}
}
