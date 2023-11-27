using GorillaExtensions;
using UnityEngine;

namespace MonoSandbox.Behaviours.UI
{
    public class PageButton : MonoBehaviour
    {
        public SandboxMenu _list;

        public GameObject _text;
        public int _pageIndex;

        private Vector3 _btnScale, _txtScale;

        private Color _flipColour;
        private bool _flipping, _active;
        private float _scale = 1f, _sine, _time;

        private float _lastTime;

        public void Start()
        {
            _active = _list._currentPage == _pageIndex;
            gameObject.GetComponent<Renderer>().material.color = _active ? new Color32(71, 121, 196, 255) : new Color32(215, 225, 239, 255);

            _btnScale = transform.localScale;
            _txtScale = _text.transform.localScale;
        }

        public void Update()
        {
            bool isActive = _list._currentPage == _pageIndex;

            if (_active != isActive)
            {
                _active = isActive;

                _flipping = true;
                _time = Mathf.PI * -0.5f;
                _flipColour = isActive ? new Color32(71, 121, 196, 255) : new Color32(215, 225, 239, 255);
            }

            if (_flipping)
            {
                _time += Time.deltaTime * 18f;
                _sine = Mathf.Sin(_time);
                _scale = Mathf.Abs(_sine);

                if (_time > 0)
                {
                    gameObject.GetComponent<Renderer>().material.color = _flipColour;
                }

                if (_time >= Mathf.PI * 0.5f)
                {
                    _scale = 1f;
                    _flipping = false;
                }
            }

            transform.localScale = _btnScale.WithY(_btnScale.y * _scale);
            _text.transform.localScale = _txtScale.WithY(_txtScale.y * _scale);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out GorillaTriggerColliderHandIndicator component) && !component.isLeftHand && Time.time > (_lastTime + 0.4f))
            {
                _lastTime = Time.time;
                GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);

                if (_list._currentPage != _pageIndex)
                {
                    _list._currentPage = _pageIndex;

                    _list.Clear();
                    _list.PlayAudio(false);
                }
            }
        }
    }
}
