using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField]
        private Image _bar;

        [SerializeField]
        private Text _text;

        private float _maxValue = 1f;
        private float _curValue = 1f;

        public void SetMaxValue(float maxValue)
        {
            _maxValue = maxValue;
        }

        public void SetCurValue(float curValue)
        {
            _curValue = curValue;
            _text.text = _curValue.ToString();

            var totalRect = GetComponent<RectTransform>();
            var barRect = _bar.GetComponent<RectTransform>();
            barRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, totalRect.rect.width *  curValue / _maxValue);
        }
    }
}
