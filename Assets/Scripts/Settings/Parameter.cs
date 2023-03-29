using System;
using UnityEngine;

namespace Assets.Scripts.Settings
{
    [Serializable]
    public class Parameter
    {
        [field: SerializeField]
        public ParameterType Type { get; private set; }

        [field: SerializeField]
        public float Value { get; private set; }
    }
}
