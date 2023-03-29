using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Settings
{
    public class Settings : MonoBehaviour
    {
        [field: SerializeField]
        public ShipSettings[] ShipSettings { get; private set; }

        [field: SerializeField]
        public WeaponSettings[] WeaponSettings { get; private set; }

        [field: SerializeField]
        public ModuleSettings[] ModuleSettings { get; private set; }
    }
}
