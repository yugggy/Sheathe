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
				Debug.Log("Å‰‚ÌUŒ‚‚©‚ç10•b");
				firstAttackTimer = 0;
				GameOver();
			}
		}
	}

	/// <summary>
	/// Œ‹‰Ê”­•\
	/// </summary>
	public void Result()
	{
		// ‘S–Å”»’è
		if (ObjectManager.Current.GetDestroyCompletely())
		{
			isFirstAttack = false;

			// a‚Á‚½“GŸr–Å
			ObjectManager.Current.DestroySlashObject();

			Debug.Log("ƒXƒe[ƒWƒNƒŠƒA");
		}
		else
		{
			isFirstAttack = false;

			// a‚Á‚½“GŸr–Å
			ObjectManager.Current.DestroySlashObject();

			SceneGameManager.Current.ReloadStage();
		}
	}

	public void GameOver()
	{
		SceneGameManager.Current.ReloadStage();
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
