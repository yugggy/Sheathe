using UnityEngine;

public class SlashBase : ObjectBase
{
	[SerializeField] private GameObject slashAnime;
	[SerializeField] private bool isCanSlash = true; // �a����I�u�W�F�N�g���ǂ���

	private bool isSlashed = false; // �a��ꂽ�t���O

	public bool IsSlashed => isSlashed;
	public bool IsCanSlash => isCanSlash;

	public virtual void SetSlashed()
	{
		isSlashed = true;
		slashAnime.SetActive(true);
		StageManager.Current.StartFirstAttack();
	}
}
