using Common.Script;
using Object._2_SlashObject.Enemy.Script.EnemyParam;
using UnityEngine;

namespace Object._2_SlashObject.Enemy.Script
{
    public class EN001 : EnemyBase
    {
        [SerializeField, Label("移動の速さ")] private float _velocityY;
        [SerializeField, Label("左崖到達後の待機時間")] private float _waitTime;
        private bool _isRightCliffWait;
        
        private bool _isRightMove = true;
        private Vector2 _currentPos = Vector3.zero;
        private int _moveState = 0;
        private float _waitCurrentTime = 0;
        private Vector2 _shortForwardPosition;
        private int _arriveCenterCliffCount = 0; // 中心の崖に到達した回数 

        
        /// <summary>
        /// 追加コンポーネントから値を取得
        /// </summary>
        public override void SetEnemyParams(EnemyParamBase enemyParam)
        {
            switch (enemyParam)
            {
                case EnemyParam_E001 param:
                    _isRightCliffWait = param.IsRightCliffWait;
                    break;
                default:
                    base.SetEnemyParams(enemyParam);
                    break;
            }
        }
        
        protected override void Start()
        {
            base.Start();
            _isRightMove = IsInitDirectionRight();
            _currentPos = transform.position;
        }
        
        protected override void ObjectUpdate()
        {
            base.ObjectUpdate();
            SideMove();
        }

        /// <summary>
        /// 横移動
        /// </summary>
        private void SideMove()
        {
            // 着地前はスキップ
            if (!IsGround)
            {
                return;
            }
            
            switch (_moveState)
            {
                case 0: // 移動
                    // 少し前方から下に向けて、崖判定
                    int stageLayer = 1 << LayerMask.NameToLayer("Stage");
                    _shortForwardPosition = transform.position + (transform.right * 0.5f);
                    if (Physics2D.Raycast(_shortForwardPosition, Vector2.down, GroundCheckRayLength, stageLayer))
                    {
                        _currentPos.x += (_isRightMove ? 1 : -1) * _velocityY;
                    }
                    else
                    {
                        // 端の崖なら反転
                        if (_isRightCliffWait ^ _isRightMove)
                        {
                            _isRightMove = !_isRightMove;
                            SetDirection(_isRightMove);
                        }
                        // 中心の崖なら待機
                        else
                        {
                            _moveState = 1;
                            
                            // 中心の崖二回目到達時に待機
                            // _arriveCenterCliffCount++;
                            // if (_arriveCenterCliffCount >= 2)
                            // {
                            //     _arriveCenterCliffCount = 0;
                            //     _moveState = 1;
                            // }
                            // else
                            // {
                            //     _isRightMove = !_isRightMove;
                            //     SetDirection(_isRightMove);
                            // }
                        }
                    }
                    break;
                case 1: // 待機
                    _waitCurrentTime += Time.deltaTime;
                    if (_waitCurrentTime >= _waitTime)
                    {
                        _waitCurrentTime = 0;
                        _moveState = 0;
                        _isRightMove = !_isRightMove;
                        SetDirection(_isRightMove);
                    };
                    break;
            }
            
            transform.position = new Vector3(_currentPos.x, transform.position.y, transform.position.y);
        }
        
        /// <summary>
        /// レイ可視化
        /// </summary>
        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(_shortForwardPosition, Vector2.down);
            }
        }
    }
}
