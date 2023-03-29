using UnityEngine;

namespace Assets.Scripts
{
    public static class BattleData
    {
        public class ShipBattleData
        {
            public int ShipId { get; set; }
            public float Shield { get; set; }
            public float Health { get; set; }
            public float ShieldRechargeRate { get; set; }
            public float RateOfFireMultiplier { get; set; }

            public int[] WeaponIds { get; set; }
        }

        public static ShipBattleData Ship1 { get; set; }
        public static ShipBattleData Ship2 { get; set; }
    }
}
