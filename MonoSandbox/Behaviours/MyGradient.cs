using System.Collections.Generic;
using UnityEngine;

namespace MonoSandbox.Behaviours
{
    // https://forum.unity.com/threads/gradients-number-of-keys-and-alpha-values.1231029/#post-7851633
    public struct MyGradientKey
    {

        public float t { get; set; }
        public Color Color { get; set; } // comes with r, g, b, alpha

        public MyGradientKey(float t, Color color)
        {
            this.t = t;
            this.Color = color;
        }

    }

    public class MyGradient
    {

        readonly List<MyGradientKey> _keys;

        public MyGradient()
        {
            _keys = new List<MyGradientKey>();
        }

        public int Count => _keys.Count;

        public MyGradientKey this[int index]
        {
            get => _keys[index];
            set { _keys[index] = value; sortKeys(); }
        }

        public void AddKey(float t, Color color)
          => AddKey(new MyGradientKey(t, color));

        public void AddKey(MyGradientKey key)
        {
            _keys.Add(key);
            sortKeys();
        }

        public void InsertKey(int index, float t, Color color)
          => InsertKey(index, new MyGradientKey(t, color));

        public void InsertKey(int index, MyGradientKey key)
        {
            _keys.Insert(index, key);
            sortKeys();
        }

        public void RemoveKey(int index)
        {
            _keys.RemoveAt(index);
            sortKeys();
        }

        public void RemoveInRange(float min, float max)
        {
            for (int i = _keys.Count - 1; i >= 0; i--)
                if (_keys[i].t >= min && _keys[i].t <= max) _keys.RemoveAt(i);
            sortKeys();
        }

        public void Clear() => _keys.Clear();

        void sortKeys() => _keys.Sort((a, b) => a.t.CompareTo(b.t));

        (int l, int r) getNeighborKeys(float t)
        {
            var l = Count - 1;

            for (int i = 0; i <= l; i++)
            {
                if (_keys[i].t >= t)
                {
                    if (i == 0) return (-1, i);
                    return (i - 1, i);
                }
            }

            return (l, -1);
        }

        public Color Evaluate(float t)
        {
            if (Count == 0) return new Color(0f, 0f, 0f, 0f);

            var (l, r) = getNeighborKeys(t);

            if (l < 0) return _keys[r].Color;
            else if (r < 0) return _keys[l].Color;

            return Color.Lerp(
              _keys[l].Color,
              _keys[r].Color,
              Mathf.InverseLerp(_keys[l].t, _keys[r].t, t)
            );
        }
    }
}
