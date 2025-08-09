using Common.Script;
using UnityEngine;

namespace Object._2_SlashObject.Enemy.Script.EnemyParam
{
    public class EnemyParam_E001 : EnemyParamBase
    {
        [SerializeField, Label("右崖で待機（false:左崖で待機）")] private bool _isRightCliffWait;
        
        public bool IsRightCliffWait => _isRightCliffWait;
    }
}
