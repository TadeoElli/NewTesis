using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class FSM_Atack : MonoBehaviour
{
    private enum State { MeteorRain, ElectricFloor, HorizontalBurst }
    private State currentState = State.MeteorRain;
    public Transform player;
    public VisualEffect vfxMeteor;
    public GameObject electricFloor;
    public ParticleSystem burstParticles;
    public int countAttack = 0;
    public int _random;
    public bool _isActiveFSM;
    public bool _isRandomAttack;
    public UnityEvent onMeteorAttack;
    public UnityEvent offMeteorAttack;

    private void Start()
    {
        vfxMeteor = GetComponent<VisualEffect>();
        electricFloor = GetComponent<GameObject>();
        burstParticles = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (countAttack == 1)
        {
            currentState = State.MeteorRain;
        }
        if (countAttack == 2)
        {
            currentState = State.ElectricFloor;
        }
        if (countAttack == 3)
        {
            currentState = State.HorizontalBurst;
        }

        if (_isRandomAttack)
        {
            countAttack = _random;
        }

        switch (currentState)
        {
            case State.MeteorRain:
                StartCoroutine(PlayAndStopRain());
                break;
            case State.ElectricFloor:
                StartCoroutine(PlayAndStopFloor());
                break;
            case State.HorizontalBurst:
                StartCoroutine(HorizontalAttack());
                break;
            default:
                break;
        }
    }

    public void RandomAttack()
    {
        _random = Random.Range(1, 4);
    }

    public IEnumerator PlayAndStopRain()
    {
        if(vfxMeteor != null)
        {
            vfxMeteor.Play();
        }
        yield return new WaitForSeconds(2f);

        vfxMeteor.Stop();
    }

    public IEnumerator PlayAndStopFloor()
    {
        if (electricFloor != null)
        {
            electricFloor.SetActive(true);
        }

        yield return new WaitForSeconds(2f);

        electricFloor.SetActive(false);
    }

    public IEnumerator HorizontalAttack()
    {
        if (burstParticles != null)
        {
            burstParticles.Play();
        }

        yield return new WaitForSeconds(2f);

        burstParticles.Stop();
    }

    public void ActivateFSM()
    {
        _isActiveFSM = true;
    }

    public void DesactivateFSM()
    {
        _isActiveFSM = false;
    }

    public void ActivateRandomAttack()
    {
        _isRandomAttack = true;
    }

    public void DesactivateRandomAttack()
    {
        _isRandomAttack = false;
    }
}
