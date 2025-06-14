using UnityEngine;

public class EnemyBase : SlashBase
{
    protected float _chaseVelocity = 0.004f;
    
    private void Update()
    {
        // 攻撃されたらプレイヤーを追いかける
        if (IsSlashed)
        {
            var playerPos = SceneGameManager.Current.Player.transform.position;
            var vec = playerPos - transform.position;
            vec.z = 0;
            vec.Normalize();
            transform.position += vec * _chaseVelocity;
        }
    }
}
