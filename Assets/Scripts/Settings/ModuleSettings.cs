using System;
using UnityEngine;

namespace Assets.Scripts.Settings
{
    [Serializable]
    public class ModuleSettings : INamedSprite
    {
        [field: SerializeField]
        public string Name { get; private set; }

        [field: SerializeField]
        public Parameter[] Parameters { get; private set; }

        [field: SerializeField]
        public Sprite Sprite { get; private set; }
    }
}
