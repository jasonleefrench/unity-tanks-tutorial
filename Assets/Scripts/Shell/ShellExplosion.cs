﻿using UnityEngine;
using System.Collections;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask m_TankMask;
    public ParticleSystem m_ExplosionParticles;       
    public AudioSource m_ExplosionAudio;              
    public float m_MaxDamage = 100f;                  
    public float m_ExplosionForce = 1000f;            
    public float m_MaxLifeTime = 2f;                  
    public float m_ExplosionRadius = 5f;              

    private void OnTriggerEnter(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);
        for(int i = 0; i < colliders.Length; i++) {
            Rigidbody target = colliders[i].GetComponent<Rigidbody>();
            if(!target) { continue; }
            target.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);
            TankHealth targetHealth = target.GetComponent<TankHealth>();
            if(!targetHealth) { continue; }
            float damage = CalculateDamage(target.position);
            targetHealth.TakeDamage(damage);
        }
        m_ExplosionParticles.transform.parent = null;
        m_ExplosionParticles.transform.position = gameObject.transform.position;
        m_ExplosionParticles.transform.rotation = gameObject.transform.rotation;
        m_ExplosionParticles.Play();
        m_ExplosionAudio.Play();
        StartCoroutine(Remove());
    }


    private float CalculateDamage(Vector3 targetPosition)
    {
        Vector3 explosionToTarget = targetPosition - transform.position;
        float explosionDistance = explosionToTarget.magnitude;
        float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;
        float damage = relativeDistance * m_MaxDamage;
        damage = Mathf.Max(0f, damage); 
        return damage;
    }

    private IEnumerator Remove() {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(m_ExplosionParticles.duration);
        m_ExplosionParticles.Stop();
    }

}