using System;

namespace LazyCultivator
{
    // Dữ liệu một đại cảnh giới (vd: Luyện Khí Kỳ). C# thuần, không phụ thuộc Unity
    // để CultivationSystem test được. RealmDatabase (ScriptableObject) sẽ chứa danh sách các Realm.
    [Serializable]
    public class Realm
    {
        // Tên hiển thị, vd: "Luyện Khí Kỳ"
        public string displayName = "Cảnh Giới";

        // Số tầng (tiểu cảnh giới) trong cảnh giới này, vd 9
        public int subLevels = 9;

        // Qi cần để đột phá từ tầng 1 lên tầng 2 của cảnh giới này
        public double baseQi = 10;

        // Hệ số nhân qi cần cho mỗi tầng kế tiếp trong cùng cảnh giới
        public double qiGrowthPerSubLevel = 1.5;
    }
}
