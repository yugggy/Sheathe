using Common.Script;
using UnityEngine;

namespace Object._2_SlashObject.Enemy.Script.EnemyParam
{
    public class EnemyParam_E002_2 : EnemyParamBase
    {
        [SerializeField, Label("目標点_上")] private Vector2 _targetPosTop;
        [SerializeField, Label("目標点_下")] private Vector2 _targetPoBottom;
        
        public Vector2 TargetPosTop => _targetPosTop;
        public Vector2 TargetPoBottom => _targetPoBottom;
    
        /// <summary>
        /// レイ可視化
        /// </summary>
        #if(UNITY_EDITOR)&&(!_NODEBUG)
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(transform.position, _targetPosTop);
                Gizmos.DrawRay(transform.position, _targetPoBottom);
                Gizmos.DrawWireSphere(transform.position + (transform.up * _targetPosTop.y),0.2f);
                Gizmos.DrawWireSphere(transform.position + (transform.up * _targetPoBottom.y),0.2f);            
            }
        }
#endif
    }
}
