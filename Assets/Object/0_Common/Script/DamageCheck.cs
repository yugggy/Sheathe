using UnityEngine;

public class DamageCheck : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.name != "Attack")
		{
			return;
		}
        var attackChara = collision.transform.parent.parent;
		var damageChara = transform.parent.parent;
		if (attackChara == null || damageChara == null)
		{
			return;
		}
		SlashBase slashObj;
        var isAttackCharaEnemy = attackChara.TryGetComponent<SlashBase>(out var temp);
		var isDamageCharaEnemy = damageChara.TryGetComponent<SlashBase>(out slashObj);

		// �X�L�b�v����
		// �E�������g�������ꍇ
		// �E�G���m�̃t�����h���[�t�@�C���[
		// �E���Ƀ_���[�W���󂯂Ă���ꍇ
		if (attackChara == damageChara ||
			(isAttackCharaEnemy && isDamageCharaEnemy) ||
			slashObj.IsSlashed)
		{
            return;
        }

		// �a��ꂽ
		slashObj.SetSlashed();

		Debug.Log($"�U��:{attackChara.name}, ��_���[�W:{damageChara.name}");
	}
}
