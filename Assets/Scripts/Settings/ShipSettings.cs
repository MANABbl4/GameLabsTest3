using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Settings
{
    [Serializable]
    public class ShipSettings : INamedSprite
    {
        [field: SerializeField]
        public string Name { get; private set; }

        [field: SerializeField]
        public string Description { get; private set; }

        [field: SerializeField]
        public int Health { get; private set; }

        [field: SerializeField]
        public ShieldSettings Shield { get; private set; }

        [field: SerializeField]
        public Slot[] WeaponSlots { get; private set; }

        [field: SerializeField]
        public Slot[] EngineeringModuleSlots { get; private set; }

        [field: SerializeField]
        public Sprite Sprite { get; private set; }
    }
}
