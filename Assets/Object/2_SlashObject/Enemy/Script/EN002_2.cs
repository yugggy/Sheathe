using Common.Script;
using Object._2_SlashObject.Enemy.Script.EnemyParam;
using Unity.VisualScripting;
using UnityEngine;

namespace Object._2_SlashObject.Enemy.Script
{
    public class EN002_2 : EnemyBase
    {
        [SerializeField, Label("移動の速さ")] private float _velocityY;
        private Vector2 _targetPosTop;
        private Vector2 _targetPoBottom;
        
        private Transform _point1;
        private Transform _point2;
        private Vector3 _initPos = Vector3.zero;
        private Vector3 _currentPos = Vector3.zero;
        
        protected override void Start()
        {
            base.Start();

            Init();
            UpDownMove();
        }
        
        /// <summary>
        /// 追加コンポーネントから値を取得
        /// </summary>
        public override void SetEnemyParams(EnemyParamBase enemyParam)
        {
            switch (enemyParam)
            {
                case EnemyParam_E002_2 param:
                    _targetPosTop = param.TargetPosTop;
                    _targetPoBottom = param.TargetPoBottom;
                    // DebugLogger.Log($"{_targetPosTop}");
                    // DebugLogger.Log($"{_targetPoBottom}");
                    break;
                default:
                    base.SetEnemyParams(enemyParam);
                    break;
            }
        }

        void Init()
        {
            // _targetPosTop
            _initPos = transform.position;
            _currentPos = transform.position;
        }

        protected override void ObjectUpdate()
        {
            base.ObjectUpdate();
            UpDownMove();
        }

        private void UpDownMove()
        {
            _currentPos.y += _velocityY;
            transform.position = _currentPos;
            
            // if (_point1.position.y >= transform.position.y)
            // {
            //     DebugLogger.Log("aa");
            // }
        }
        
        /// <summary>
        /// レイ可視化
        /// </summary>
        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(_initPos, _targetPosTop);
                Gizmos.DrawRay(_initPos, _targetPoBottom);
                Gizmos.DrawWireSphere(_initPos + (transform.up * _targetPosTop.y),0.2f);
                Gizmos.DrawWireSphere(_initPos + (transform.up * _targetPoBottom.y),0.2f);    
            }
        }
    }
}
