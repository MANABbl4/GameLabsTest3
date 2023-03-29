using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ScrollItem : MonoBehaviour, IPointerClickHandler
    {
        public Action<int> OnSelected;

        [SerializeField]
        private Image _image;

        [SerializeField]
        private Text _name;

        [SerializeField]
        private Text _description;

        private int _id = -1;

        public void SetSprite(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        public void SetName(string name)
        {
            _name.text = name;
        }

        public void SetDescription(string description)
        {
            _description.text = description;
        }

        public void SetId(int id)
        {
            _id = id;
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            OnSelected?.Invoke(_id);
        }
    }
}
