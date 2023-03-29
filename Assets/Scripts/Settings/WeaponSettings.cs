using System;
using UnityEngine;

namespace Assets.Scripts.Settings
{
    [Serializable]
    public class WeaponSettings : INamedSprite
    {
        [field: SerializeField]
        public string Name { get; private set; }

        [field: SerializeField]
        public int Damage { get; private set; }

        [field: SerializeField]
        public float RateOfFire { get; private set; }

        [field: SerializeField]
        public Sprite Sprite { get; private set; }

        [field: SerializeField]
        public Sprite LaserSprite { get; private set; }
    }
}
