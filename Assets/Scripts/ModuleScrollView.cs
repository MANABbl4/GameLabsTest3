using Assets.Scripts.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class ModuleScrollView : MonoBehaviour
    {
        public Action<int> OnItemSelected;

        [SerializeField]
        private ScrollItem _scollItemPrefab;

        [SerializeField]
        private Sprite _removeSprite;

        private List<ScrollItem> _moduleItems = new List<ScrollItem>();
        private CanvasGroup _moduleScrollViewCanvasGroup = null;
        private INamedSprite[] _items = null;

        public void SetItems(INamedSprite[] items, bool addRemoveItem)
        {
            if (_items != null)
            {
                foreach (var item in _moduleItems)
                {
                    item.OnSelected -= OnSelected;
                    Destroy(item.gameObject);
                }

                _moduleItems.Clear();
            }

            _items = items;

            _moduleScrollViewCanvasGroup = gameObject.GetComponent<CanvasGroup>();
            var transforms = gameObject.GetComponentsInChildren<RectTransform>();
            var modulesScrollViewContent = transforms.FirstOrDefault(x => x.name == "Content");

            if (addRemoveItem)
            {
                var scrollItem = CreateItem(_scollItemPrefab, modulesScrollViewContent, "Remove", _removeSprite, -1);
                _moduleItems.Add(scrollItem);
            }

            for (int i = 0; i < _items.Length; ++i)
            {
                var item = _items[i];
                var scrollItem = CreateItem(_scollItemPrefab, modulesScrollViewContent, item.Name, item.Sprite, i);
                _moduleItems.Add(scrollItem);
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _moduleScrollViewCanvasGroup.alpha = 1;
            _moduleScrollViewCanvasGroup.interactable = true;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _moduleScrollViewCanvasGroup.alpha = 0;
            _moduleScrollViewCanvasGroup.interactable = false;
        }

        private void OnSelected(int id)
        {
            OnItemSelected?.Invoke(id);
        }

        private ScrollItem CreateItem(ScrollItem scollItemPrefab, RectTransform shipsScrollViewContent, string name, Sprite sprite, int id)
        {
            var scrollItem = Instantiate(scollItemPrefab, shipsScrollViewContent);
            scrollItem.SetName(name);
            scrollItem.SetSprite(sprite);
            scrollItem.SetId(id);
            scrollItem.OnSelected += OnSelected;

            return scrollItem;
        }
    }
}
