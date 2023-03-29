using System;

namespace Assets.Scripts.Settings
{
    [Serializable]
    public enum ParameterType : int
    {
        Health = 0,
        Shield = 1,
        RateOfFireMultiplier = 2,
        ShieldRegenMultiplier = 3,
    }
}
