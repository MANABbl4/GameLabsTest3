using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class LaserBeam : MonoBehaviour
    {
        [SerializeField]
        private Image _image;

        [SerializeField]
        private float _showTime;

        private float _timeLeft = 0f;
        private readonly Color _transparent = new Color(0f, 0f, 0f, 0f);

        public void SetSprite(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        public void Show()
        {
            _timeLeft = _showTime;
            _image.color = Color.white;
        }

        public void Hide()
        {
            _image.color = _transparent;
        }

        private void Start()
        {
            _image.color = _transparent;
        }

        public void Update()
        {
            if(_timeLeft > 0f)
            {
                _timeLeft -= Time.deltaTime;

                if (_timeLeft < 0f)
                {
                    Hide();
                }
            }
        }
    }
}
