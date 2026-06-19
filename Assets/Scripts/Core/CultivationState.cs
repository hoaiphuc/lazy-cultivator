using System;

namespace LazyCultivator
{
    [Serializable]
    public class CultivationState
    {
        // Lượng qi (linh khí) hiện đang tích lũy, bị tiêu hao mỗi lần đột phá
        public double qi;

        // Chỉ số đại cảnh giới hiện tại (0 = cảnh giới đầu tiên trong RealmDatabase)
        public int realmIndex;

        // Tầng hiện tại trong cảnh giới (0-based: 0 = Tầng 1)
        public int subLevel;
    }
}
