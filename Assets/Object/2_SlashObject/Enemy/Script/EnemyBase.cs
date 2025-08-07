using Object._2_SlashObject.Script;
using UnityEngine;

namespace Object._2_SlashObject.Enemy.Script
{
    /// <summary>
    /// 敵基底クラス
    /// </summary>
    public class EnemyBase : SlashBase
    {
        protected float ChaseVelocity = 1.0f;
    
        protected override void ObjectUpdate()
        {
            base.ObjectUpdate();
            PlayerChase();
        }

        /// <summary>
        /// 攻撃されたらプレイヤーを追いかける
        /// </summary>
        private void PlayerChase()
        {
            if (!IsSlashed) return;
            if (ObjectManager.Current.Player == null) return;
        
            var vec = ObjectManager.Current.Player.transform.position - transform.position;
            vec.z = 0;
            vec.Normalize();
            transform.position += vec * (ChaseVelocity * Time.deltaTime);
        }
    }
}
