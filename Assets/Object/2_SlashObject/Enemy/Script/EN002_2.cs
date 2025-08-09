using Common.Script;
using Object._2_SlashObject.Enemy.Script.EnemyParam;
using UnityEngine;

namespace Object._2_SlashObject.Enemy.Script
{
    public class EN002_2 : EnemyBase
    {
        [SerializeField, Label("移動の速さ")] private float _velocityY;
        [SerializeField, Label("減速度")] private float _decelerationVelocityY;
        [SerializeField, Label("減速停止値")] private float _decelerationStopValue;
        [SerializeField, Label("ポイント到達後の待機時間")] private float _waitTime;
        private Vector2 _targetPosTop;
        private Vector2 _targetPosBottom;
        private Vector2 _relativeTargetPosTop;
        private Vector2 _relativeTargetPosBottom;
        
        private Vector3 _initPos = Vector3.zero;
        private Vector3 _currentPos = Vector3.zero;
        private int _moveUpDownState = 0;
        private int _moveState = 0;
        private float _currentVelocityY = 0;
        private float _waitCurrentTime = 0;
        
        
        /// <summary>
        /// 追加コンポーネントから値を取得
        /// </summary>
        public override void SetEnemyParams(EnemyParamBase enemyParam)
        {
            switch (enemyParam)
            {
                case EnemyParam_E002_2 param:
                    _targetPosTop = param.TargetPosTop;
                    _targetPosBottom = param.TargetPoBottom;
                    break;
                default:
                    base.SetEnemyParams(enemyParam);
                    break;
            }
        }
        
        protected override void Start()
        {
            base.Start();

            Init();
            UpDownMove();
        }

        void Init()
        {
            _initPos = transform.position;
            _currentPos = transform.position;
            _relativeTargetPosTop.y = _initPos.y + _targetPosTop.y;
            _relativeTargetPosBottom.y = _initPos.y + _targetPosBottom.y;
        }

        protected override void ObjectUpdate()
        {
            base.ObjectUpdate();
            UpDownMove();
        }

        /// <summary>
        /// 上下移動
        /// </summary>
        private void UpDownMove()
        {
            switch (_moveUpDownState)
            {
                case 0:
                    Move(true);
                    break;
                case 1:
                    Move(false);
                    break;
            }
            
            void Move(bool isUp)
            {
                switch (_moveState)
                {
                    case 0: // 移動
                        // 移動始めは徐々に早くなっていく
                        if (_currentVelocityY <= _velocityY)
                        {
                            _currentVelocityY += (_velocityY * 0.1f);
                        }
                        else
                        {
                            _currentVelocityY = _velocityY;
                        }

                        // 移動
                        if (isUp)
                        {
                            _currentPos.y += _currentVelocityY;
                            if (transform.position.y >= _relativeTargetPosTop.y)
                            {
                                _moveState = 1;
                            }
                        }
                        else
                        {
                            _currentPos.y -= _currentVelocityY;
                            if (transform.position.y <= _relativeTargetPosBottom.y)
                            {
                                _moveState = 1;
                            }
                        }
                        break;
                    case 1: // ポイント到達後、減速
                        _currentVelocityY *= _decelerationVelocityY;
                        
                        _currentPos.y += (isUp ? 1 : -1) * _currentVelocityY;
                        if (_currentVelocityY <= _decelerationStopValue)
                        {
                            _currentVelocityY = _velocityY;
                            _moveState = 2;
                        }
                        break;
                    case 2: // 待機
                        _waitCurrentTime += Time.deltaTime;
                        if (_waitCurrentTime >= _waitTime)
                        {
                            _waitCurrentTime = 0;
                            _moveState = 0;
                            _moveUpDownState = isUp ? 1 : 0;
                        };
                        break;
                }
            }
            
            transform.position = _currentPos;
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
                Gizmos.DrawRay(_initPos, _targetPosBottom);
                Gizmos.DrawWireSphere(_initPos + (transform.up * _targetPosTop.y),0.2f);
                Gizmos.DrawWireSphere(_initPos + (transform.up * _targetPosBottom.y),0.2f);    
            }
        }
    }
}
