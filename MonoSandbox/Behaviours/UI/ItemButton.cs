using GorillaExtensions;
using UnityEngine;

namespace MonoSandbox.Behaviours.UI
{
    public class Button : MonoBehaviour
    {
        public SandboxMenu _list;

        public GameObject _text;
        public int _buttonIndex;

        private Vector3 _btnScale, _txtScale;

        private Color _flipColour;
        private bool _flipping, _active;
        private float _scale = 1f, _sine, _time;

        private float _lastTime;

        public void Start()
        {
            _active = _list._currentPage == 0 && _list.objectButtons[_buttonIndex] || _list._currentPage == 1 && _list.weaponButtons[_buttonIndex] || _list._currentPage == 2 && _list.toolButtons[_buttonIndex] || _list._currentPage == 3 && _list.utilButtons[_buttonIndex] || _list._currentPage == 4 && _list.funButtons[_buttonIndex];
            gameObject.GetComponent<Renderer>().material.color = _active ? new Color32(71, 121, 196, 255) : new Color32(215, 225, 239, 255);

            _btnScale = transform.localScale;
            _txtScale = _text.transform.localScale;
        }

        public void Update()
        {
            bool isActive = (_list._currentPage == 0 && _list.objectButtons[_buttonIndex]) || (_list._currentPage == 1 && _list.weaponButtons[_buttonIndex]) || (_list._currentPage == 2 && _list.toolButtons[_buttonIndex]) || (_list._currentPage == 3 && _list.utilButtons[_buttonIndex]) || (_list._currentPage == 4 && _list.funButtons[_buttonIndex]);

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
            if (other.TryGetComponent(out GorillaTriggerColliderHandIndicator component) && !component.isLeftHand && Time.time > (_lastTime + 0.25f))
            {
                _lastTime = Time.time;
                GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);

                bool[] array = _list.GetArray();
                _list.Clear();
                _list.PlayAudio(true);

                if (array[_buttonIndex]) return;

                array = _list.GetArray();
                array[_buttonIndex] = true;
                _list.SetArray(array);
            }
        }
    }
}
