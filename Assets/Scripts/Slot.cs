using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Slot : MonoBehaviour, IPointerClickHandler
    {
        public Action<int> OnClicked;

        [SerializeField]
        private Image _image;

        private int _id = 0;

        public void RemoveSprite()
        {
            _image.sprite = null;
            _image.color = new Color(0, 0, 0, 0);
        }

        public void SetSprite(Sprite sprite)
        {
            _image.sprite = sprite;
            _image.color = new Color(1, 1, 1, 1);
        }

        public void SetId(int id)
        {
            _id = id;
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke(_id);
        }
    }
}
