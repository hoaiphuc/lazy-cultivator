using System;
using UnityEngine;

namespace LazyCultivator
{
    // Quái thật trên scene. Stats set thẳng trên prefab qua Inspector.
    // Tự đi ngang về phía hero và dừng lại ở tầm đánh; báo về spawner khi chết.
    public class Enemy : MonoBehaviour
    {
        [Header("Thông tin")]
        [SerializeField] private string displayName = "Yêu Thú";

        [Header("Chỉ số")]
        // Dùng double vì số trong game idle có thể rất lớn
        [SerializeField] private double maxHp = 10;
        [SerializeField] private double attack = 1;

        [Header("Di chuyển")]
        [SerializeField] private float moveSpeed = 2f;

        [Tooltip("Hướng đi ngang: -1 = sang trái (về phía hero bên trái), +1 = sang phải.")]
        [SerializeField] private float moveDirection = -1f;

        [Tooltip("Dừng cách mục tiêu (hero) bao xa để áp sát tấn công.")]
        [SerializeField] private float attackRange = 1f;

        private Action<Enemy> _onDied;
        private double _hp;

        private bool _hasTarget;
        private float _stopX;

        public string DisplayName => displayName;
        public double MaxHp => maxHp;
        public double Attack => attack;
        public double Hp => _hp;
        public bool IsDead => _hp <= 0;

        // True khi quái đã áp sát hero → sẵn sàng tấn công (nối phần combat sau).
        public bool HasReachedTarget { get; private set; }

        // Spawner gọi ngay sau khi Instantiate để nạp callback chết và reset máu.
        public void Init(Action<Enemy> onDied)
        {
            _onDied = onDied;
            _hp = maxHp;
        }

        // Spawner gọi (nếu có hero) để báo vị trí X của hero.
        // Quái sẽ dừng ở điểm cách hero attackRange về phía nó đi tới.
        public void SetTargetX(float heroX)
        {
            _stopX = heroX - moveDirection * attackRange;
            _hasTarget = true;
        }

        void Update()
        {
            if (IsDead || HasReachedTarget) return;

            Vector3 pos = transform.position;
            pos.x += moveDirection * moveSpeed * Time.deltaTime;

            // Tới mốc dừng (theo hướng đang đi) thì kẹp lại và đánh dấu đã áp sát.
            if (_hasTarget &&
                ((moveDirection < 0f && pos.x <= _stopX) ||
                 (moveDirection > 0f && pos.x >= _stopX)))
            {
                pos.x = _stopX;
                HasReachedTarget = true;
            }

            transform.position = pos;
        }

        // Nguồn sát thương (đòn của người chơi) gọi hàm này để trừ máu.
        public void TakeDamage(double amount)
        {
            if (IsDead) return;
            _hp -= amount;
            if (_hp <= 0) Die();
        }

        private void Die()
        {
            _hp = 0;
            _onDied?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
