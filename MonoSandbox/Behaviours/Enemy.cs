using UnityEngine;

namespace MonoSandbox.Behaviours
{
    public class Enemy : MonoBehaviour
    {
        public float Health = 20, Defence = 2;
        private bool _isAttacking;

        public void Update()
        {
            transform.LookAt(GorillaTagger.Instance.headCollider.transform.position);
            if (Vector3.Distance(transform.position, GorillaTagger.Instance.headCollider.transform.position) > 1.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, GorillaTagger.Instance.headCollider.transform.position, 4 * Time.deltaTime);
                _isAttacking = false;
            }
            else if (!_isAttacking)
            {
                _isAttacking = true;
                GetComponent<AudioSource>().Play();

                Rigidbody PlayerRigidbody = GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>();
                PlayerRigidbody.AddExplosionForce(1500f * 6f * Mathf.Sqrt(PlayerRigidbody.mass), transform.position, 7.5f + 6f / 1.25f);
            }
            if (Health <= 0)
            {
                Destroy(gameObject);
            }
        }

        public void Damage(float damage, float criticalChance, float criticalMultiplier)
        {
            if (Random.Range(1, 100) < criticalChance)
            {
                Health = Mathf.Clamp(Health - (damage * criticalMultiplier + Random.Range(-2, 2)) / Defence, 0, Mathf.Infinity);
            }
            else
            {
                Health = Mathf.Clamp(Health - (damage + Random.Range(-4, 4)) / Defence, 0, Mathf.Infinity);
            }
        }
    }
}