using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class Enemy : MonoBehaviour
{
    private float baseHealth;
    public float Health = 20;
    public float Defence = 2;
    private void Awake()
    {
        baseHealth = Health;
    }
    private void Update()
    {
        transform.LookAt(GorillaTagger.Instance.transform.position);
        if (Vector3.Distance(transform.position, GorillaTagger.Instance.transform.position) > 2.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, GorillaTagger.Instance.transform.position, 4 * Time.deltaTime);
        }
        if (Health == 0)
        {
            Destroy(gameObject);
        }
    }
    public void Damage(float damage, float criticalChance, float criticalMultiplier)
    {
        if (UnityEngine.Random.Range(1, 100) < criticalChance)
        {
            Health = Mathf.Clamp(Health - (damage * criticalMultiplier + UnityEngine.Random.Range(-2, 2)) / Defence, 0, Mathf.Infinity);
        }
        else
        {
            Health = Mathf.Clamp(Health - (damage + UnityEngine.Random.Range(-4, 4)) / Defence, 0, Mathf.Infinity);
        }
    }
}