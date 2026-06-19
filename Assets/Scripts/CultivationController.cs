using UnityEngine;
using TMPro;

namespace LazyCultivator
{

    public class CultivationController : MonoBehaviour
    {
        [SerializeField] private RealmDatabase realmDatabase;
        [SerializeField] private TMP_Text realmLabel;
        [SerializeField] private TMP_Text qiLabel;

        private CultivationState _state;
        private CultivationSystem _cultivation;

        void Start()
        {
            if (realmDatabase == null)
            {
                Debug.LogError("CultivationController: chưa gán Realm Database trong Inspector.", this);
                enabled = false;
                return;
            }

            _state = new CultivationState();
            _cultivation = new CultivationSystem(_state, realmDatabase.Realms);
        }

        void Update()
        {
            _cultivation.Tick(Time.deltaTime);

            if (realmLabel != null)
            {
                realmLabel.text =
                    $"{_cultivation.CurrentRealmName} - Tầng {_cultivation.CurrentSubLevelDisplay}";
            }

            if (qiLabel != null)
            {
                if (_cultivation.IsAtPeak)
                {
                    qiLabel.text = $"Tu vi: {_state.qi:F0}   (Viên Mãn)";
                }
                else
                {
                    qiLabel.text =
                        $"Tu vi: {_state.qi:F0} / {_cultivation.CurrentBreakthroughCost:F0}   (+{_cultivation.QiPerSecond}/giây)";
                }
            }
        }
    }
}
