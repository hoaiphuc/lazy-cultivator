using System;
using System.Collections.Generic;

namespace LazyCultivator
{
    public class CultivationSystem
    {
        private readonly IReadOnlyList<Realm> _realms;

        public CultivationState State { get; }

        public double QiPerSecond { get; set; } = 1.0;

        public CultivationSystem(CultivationState state, IReadOnlyList<Realm> realms)
        {
            State = state;
            _realms = realms ?? Array.Empty<Realm>();
        }

        public void Tick(double deltaTime)
        {
            State.qi += QiPerSecond * deltaTime;
            TryBreakthrough();
        }

        // Đột phá tự động: đủ qi thì tiêu hao qi và lên tầng. Lặp để xử lý
        // trường hợp một tick đủ qi để lên nhiều tầng cùng lúc.
        private void TryBreakthrough()
        {
            while (!IsAtPeak)
            {
                double cost = CurrentBreakthroughCost;
                if (State.qi < cost) return;

                State.qi -= cost;
                Advance();
            }
        }

        private void Advance()
        {
            var realm = _realms[State.realmIndex];
            State.subLevel++;
            if (State.subLevel >= realm.subLevels)
            {
                State.subLevel = 0;
                State.realmIndex++;
            }
        }

        // Đã đạt đỉnh: cảnh giới cuối + tầng cuối, không còn gì để đột phá
        public bool IsAtPeak
        {
            get
            {
                if (_realms.Count == 0) return true;
                int last = _realms.Count - 1;
                return State.realmIndex >= last
                    && State.subLevel >= _realms[last].subLevels - 1;
            }
        }

        // Qi cần để đột phá từ tầng hiện tại lên tầng kế tiếp
        public double CurrentBreakthroughCost
        {
            get
            {
                if (IsAtPeak) return double.PositiveInfinity;
                var realm = _realms[State.realmIndex];
                return realm.baseQi * Math.Pow(realm.qiGrowthPerSubLevel, State.subLevel);
            }
        }

        // Tên cảnh giới hiện tại để hiển thị
        public string CurrentRealmName =>
            _realms.Count == 0 ? "—" : _realms[State.realmIndex].displayName;

        // Số tầng hiển thị (1-based)
        public int CurrentSubLevelDisplay => State.subLevel + 1;
    }
}
