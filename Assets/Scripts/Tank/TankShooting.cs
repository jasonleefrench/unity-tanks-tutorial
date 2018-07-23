﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;       
    public Rigidbody m_Shell;            
    public Transform m_FireTransform;    
    public Slider m_AimSlider;           
    public AudioSource m_ShootingAudio;  
    public AudioClip m_ChargingClip;     
    public AudioClip m_FireClip;         
    public float m_MinLaunchForce = 15f; 
    public float m_MaxLaunchForce = 30f; 
    public float m_MaxChargeTime = 0.75f;
    public float m_CoolOffTime = 0.5f;

    private string m_FireButton;         
    private float m_CurrentLaunchForce;  
    private float m_ChargeSpeed;         
    private bool m_Fired;
    private bool m_ChargeClipPlaying;

    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
        m_ChargeClipPlaying = false;
        m_Fired = false;
    }


    private void Start()
    {
        m_FireButton = "Fire" + m_PlayerNumber;
        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }

    private void Update()
    {
        m_AimSlider.value = m_MinLaunchForce;
        if(m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired) {
            m_CurrentLaunchForce = m_MaxLaunchForce;
            StartCoroutine(Fire());
        } else if(Input.GetButtonDown(m_FireButton) || Input.GetButton(m_FireButton) && !m_ChargeClipPlaying) {
            m_CurrentLaunchForce = m_MinLaunchForce;
            m_ShootingAudio.clip = m_ChargingClip;
            m_ShootingAudio.Play();
            m_ChargeClipPlaying = true;
        } else if (Input.GetButton(m_FireButton) && !m_Fired) {
            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
            m_AimSlider.value = m_CurrentLaunchForce;
        } else if (Input.GetButtonUp (m_FireButton) && !m_Fired) {
            StartCoroutine(Fire());
        }
    }

    private IEnumerator Fire()
    {
        m_Fired = true;
        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();
        m_CurrentLaunchForce = m_MinLaunchForce;
        yield return new WaitForSeconds(m_CoolOffTime);
        m_Fired = false;
        m_ChargeClipPlaying = false;
    }
}