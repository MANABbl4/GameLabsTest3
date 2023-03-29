using System;
using UnityEngine;

namespace Assets.Scripts.Settings
{
    [Serializable]
    public class Slot
    {
        [field: SerializeField]
        public Vector2 Position { get; private set; }

        [field: SerializeField]
        public Vector2 Size { get; private set; }

        [field: SerializeField]
        public Sprite Sprite { get; private set; }
    }
}
