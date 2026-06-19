using System.Collections.Generic;
using UnityEngine;

namespace LazyCultivator
{
    // Asset dữ liệu chứa danh sách cảnh giới theo thứ tự (từ thấp tới cao).
    // Tạo asset: chuột phải trong Project > Create > Lazy Cultivator > Realm Database.
    [CreateAssetMenu(fileName = "RealmDatabase", menuName = "Lazy Cultivator/Realm Database")]
    public class RealmDatabase : ScriptableObject
    {
        [SerializeField] private List<Realm> realms = new List<Realm>();

        public IReadOnlyList<Realm> Realms => realms;
    }
}
