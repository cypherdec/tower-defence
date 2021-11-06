using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHitPoints = 5;
    [SerializeField] ParticleSystem hitParticles;

    [Tooltip("adds amount to max hit points when enemy dies")]
    [SerializeField] int difficultyRamp = 1;
    int currentHitPoints = 0;

    Enemy enemy;

    public int CurrentHitPoints { get { return currentHitPoints; } }

    void Start()
    {
        enemy = GetComponent<Enemy>();
    }
    void OnEnable()
    {
        currentHitPoints = maxHitPoints;
    }

    void OnParticleCollision(GameObject other)
    {
        ProcessHit();
    }

    void ProcessHit()
    {
        currentHitPoints--;

        if(!hitParticles.isPlaying){
            hitParticles.Play();
        }

        if (currentHitPoints <= 0)
        {
            gameObject.SetActive(false);
            enemy.Die();
            maxHitPoints += difficultyRamp;
        }
    }
}
