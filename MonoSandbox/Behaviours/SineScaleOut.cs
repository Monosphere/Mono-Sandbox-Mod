using UnityEngine;

namespace MonoSandbox.Behaviours
{
    public class SineScaleOut : MonoBehaviour
    {
        public float Delay = 0f, Speed = 6f;

        private bool _playing;
        private Vector3 _origin;
        private float _time, _scale;

        public void Start()
        {
            if (Delay > 0f)
            {
                Invoke(nameof(Play), Delay);
            }
            else
            {
                Play();
            }
        }

        public void Play()
        {
            _playing = true;
            _time = Mathf.PI * -0.5f;
            _origin = transform.localScale;
        }

        public void Update()
        {
            if (_playing)
            {
                _time += Time.deltaTime * Speed;
                _scale = Mathf.Sin(_time);

                transform.localScale = _origin * Mathf.Abs(_scale);

                if (_time >= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
