using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyCultivator
{
    // Một dòng spawn trong wave: spawn prefab nào, mấy con, cách nhau bao lâu.
    [Serializable]
    public class SpawnEntry
    {
        // Prefab quái sẽ spawn (nên có gắn sẵn component Enemy để set stats trong prefab)
        public GameObject enemyPrefab;

        // Số lượng con spawn cho entry này
        public int count = 1;

        // Khoảng cách thời gian (giây) giữa mỗi con trong cùng entry
        public float interval = 0.5f;
    }

    // Dữ liệu một đợt quái, được tay-thiết-kế trong Inspector (qua WaveDatabase).
    [Serializable]
    public class Wave
    {
        // Tên đợt để dễ quản lý, vd: "Đợt 1 - Sói núi"
        public string waveName = "Đợt mới";

        // Chờ bao lâu (giây) trước khi đợt này bắt đầu spawn
        public float startDelay = 1f;

        // Các dòng spawn của đợt; spawn lần lượt từ trên xuống
        public List<SpawnEntry> spawns = new List<SpawnEntry>();
    }
}
