using System;
using System.Collections;
using UnityEngine;

namespace LazyCultivator
{
    // Đọc WaveDatabase và spawn quái ra scene theo từng đợt.
    // Dọn sạch quái của đợt hiện tại thì tự chuyển sang đợt kế tiếp.
    public class WaveSpawner : MonoBehaviour
    {
        [SerializeField] private WaveDatabase waveDatabase;

        [Tooltip("Nơi quái xuất hiện. Để trống = dùng vị trí của spawner.")]
        [SerializeField] private Transform spawnPoint;

        [Tooltip("Hero/mục tiêu để quái tiến tới. Để trống = tự tìm object có tag 'Player'.")]
        [SerializeField] private Transform target;

        [Tooltip("Lặp lại từ đợt đầu khi đã hết tất cả các đợt.")]
        [SerializeField] private bool loop = false;

        private int _aliveCount;
        private int _currentWaveIndex = -1;

        // Số đợt hiện tại (1-based) để UI hiển thị.
        public int CurrentWaveNumber => _currentWaveIndex + 1;

        // Báo cho UI khi đổi đợt (truyền số đợt 1-based).
        public event Action<int> OnWaveChanged;

        // Báo khi đã clear toàn bộ các đợt (không bật loop).
        public event Action OnAllWavesCleared;

        void Start()
        {
            if (waveDatabase == null)
            {
                Debug.LogError("WaveSpawner: chưa gán Wave Database trong Inspector.", this);
                enabled = false;
                return;
            }

            // Chưa gán target thì tự tìm hero theo tag "Player".
            if (target == null)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                if (player != null) target = player.transform;
                else Debug.LogWarning("WaveSpawner: không tìm thấy object tag 'Player'. Quái sẽ đi ngang vô hạn.", this);
            }

            StartCoroutine(RunWaves());
        }

        private IEnumerator RunWaves()
        {
            var waves = waveDatabase.Waves;
            if (waves.Count == 0) yield break;

            do
            {
                for (int i = 0; i < waves.Count; i++)
                {
                    _currentWaveIndex = i;
                    OnWaveChanged?.Invoke(CurrentWaveNumber);

                    yield return StartCoroutine(SpawnWave(waves[i]));

                    // Chờ tới khi dọn sạch quái của đợt này mới qua đợt sau.
                    while (_aliveCount > 0) yield return null;
                }
            } while (loop);

            OnAllWavesCleared?.Invoke();
        }

        private IEnumerator SpawnWave(Wave wave)
        {
            if (wave.startDelay > 0f)
                yield return new WaitForSeconds(wave.startDelay);

            foreach (var entry in wave.spawns)
            {
                if (entry.enemyPrefab == null)
                {
                    Debug.LogWarning($"WaveSpawner: bỏ qua entry thiếu prefab ở đợt '{wave.waveName}'.", this);
                    continue;
                }

                for (int n = 0; n < entry.count; n++)
                {
                    SpawnOne(entry.enemyPrefab);
                    if (entry.interval > 0f)
                        yield return new WaitForSeconds(entry.interval);
                }
            }
        }

        private void SpawnOne(GameObject prefab)
        {
            Vector3 pos = spawnPoint != null ? spawnPoint.position : transform.position;
            GameObject go = Instantiate(prefab, pos, Quaternion.identity);

            var enemy = go.GetComponent<Enemy>();
            if (enemy == null) enemy = go.AddComponent<Enemy>();
            enemy.Init(OnEnemyDied);
            if (target != null) enemy.SetTargetX(target.position.x);

            _aliveCount++;
        }

        private void OnEnemyDied(Enemy enemy)
        {
            _aliveCount = Mathf.Max(0, _aliveCount - 1);
        }
    }
}
