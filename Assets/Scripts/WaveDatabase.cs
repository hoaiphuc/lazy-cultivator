using System.Collections.Generic;
using UnityEngine;

namespace LazyCultivator
{
    // Asset dữ liệu chứa danh sách các đợt quái (wave) theo thứ tự.
    // Tạo asset: chuột phải trong Project > Create > Lazy Cultivator > Wave Database.
    [CreateAssetMenu(fileName = "WaveDatabase", menuName = "Lazy Cultivator/Wave Database")]
    public class WaveDatabase : ScriptableObject
    {
        [SerializeField] private List<Wave> waves = new List<Wave>();

        public IReadOnlyList<Wave> Waves => waves;
    }
}
