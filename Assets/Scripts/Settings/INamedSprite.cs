using UnityEngine;

namespace Assets.Scripts.Settings
{
    public interface INamedSprite
    {
        string Name { get; }

        Sprite Sprite { get; }
    }
}
