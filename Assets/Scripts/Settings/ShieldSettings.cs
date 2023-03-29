using System;
using UnityEngine;

namespace Assets.Scripts.Settings
{
    [Serializable]
    public class ShieldSettings
    {
        [field: SerializeField]
        public string Name { get; private set; }

        [field: SerializeField]
        public int Shield { get; private set; }

        [field: SerializeField]
        public float ShieldRechargeRate { get; private set; }

        [field: SerializeField]
        public Sprite Sprite { get; private set; }
    }
}
