using UnityEngine;

public class SlashBase : ObjectBase
{
	[SerializeField] private GameObject slashAnime;
	[SerializeField] private bool isCanSlash = true; // 斬られるオブジェクトかどうか

	private bool isSlashed = false; // 斬られたフラグ

	public bool IsSlashed => isSlashed;
	public bool IsCanSlash => isCanSlash;

	public virtual void SetSlashed()
	{
		isSlashed = true;
		slashAnime.SetActive(true);
		StageManager.Current.StartFirstAttack();
	}
}
