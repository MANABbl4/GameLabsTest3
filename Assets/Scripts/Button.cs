using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class Button : MonoBehaviour, IPointerClickHandler
    {
        public Action OnClicked;

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke();
        }
    }
}
