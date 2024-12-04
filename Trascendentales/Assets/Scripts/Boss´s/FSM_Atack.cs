using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FSM_Attack : MonoBehaviour
{
    [SerializeField] AudioClip bossMusic;
    public enum AttackState { Idle, ElectricFloor, HorizontalBurst }

    [System.Serializable]
    public class Attack
    {
        public AttackState attackState;
        public string name;
        public UnityEvent onAttackActivate;
        public UnityEvent onAttackStart;
        public UnityEvent onAttackEnd;
        public float duration;
        public float durationtoStart;
        public bool isUnlocked;
    }

    [Header("General Settings")]
    [SerializeField] private float minDelayBetweenAttacks = 2f;
    [SerializeField] private float maxDelayBetweenAttacks = 5f;

    [Header("Attacks")]
    [SerializeField] private List<Attack> attacks = new List<Attack>();

    private AttackState currentState = AttackState.Idle;
    private int damageCounter = -1;
    private float attackTimer = 0f;
    private bool isChoosingAttack = false;

    private void Start()
    {
        AudioManager.Instance.PlayMusic(bossMusic);
        ChooseNextAttack();
    }

    private void Update()
    {
        if (currentState == AttackState.Idle && !isChoosingAttack)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                ExecuteRandomAttack();
            }
        }
    }

    private void ChooseNextAttack()
    {
        attackTimer = Random.Range(minDelayBetweenAttacks, maxDelayBetweenAttacks);
        isChoosingAttack = false;
    }

    private void ExecuteRandomAttack()
    {
        isChoosingAttack = true;

        // Obtener los ataques desbloqueados.
        List<Attack> unlockedAttacks = attacks.FindAll(a => a.isUnlocked);

        if (unlockedAttacks.Count == 0)
        {
            Debug.LogWarning("No hay ataques desbloqueados.");
            ChooseNextAttack();
            return;
        }

        // Elegir un ataque aleatorio.
        Attack selectedAttack = unlockedAttacks[Random.Range(0, unlockedAttacks.Count)];
        ActivateAttack(selectedAttack);
    }

    private void ActivateAttack(Attack attack)
    {
        Debug.Log($"Activando ataque: {attack.name}");

        // Disparar el evento de inicio del ataque.
        attack.onAttackActivate?.Invoke();

        // Cambiar al estado del ataque.
        currentState = attack.attackState;

        // Configurar el inicio del ataque después de `durationtoStart`.
        Invoke(nameof(StartAttack), attack.durationtoStart);
    }

    private void StartAttack()
    {
        Debug.Log($"Iniciando ataque: {currentState}");

        // Buscar el ataque correspondiente al estado actual.
        Attack currentAttack = attacks.Find(a => a.attackState == currentState);
        if (currentAttack == null)
        {
            Debug.LogError("No se encontró el ataque correspondiente al estado actual.");
            EndAttack();
            return;
        }

        // Disparar el evento de inicio.
        currentAttack.onAttackStart?.Invoke();

        // Configurar el final del ataque después de `duration`.
        Invoke(nameof(EndAttack), currentAttack.duration);
    }

    private void EndAttack()
    {
        Debug.Log($"Finalizando ataque: {currentState}");

        // Buscar el ataque correspondiente al estado actual.
        Attack currentAttack = attacks.Find(a => a.attackState == currentState);
        if (currentAttack != null)
        {
            // Disparar el evento de finalización.
            currentAttack.onAttackEnd?.Invoke();
        }
        else
        {
            Debug.LogError("No se encontró el ataque correspondiente al estado actual.");
        }

        // Elegir el siguiente ataque y volver al estado inactivo.
        ChooseNextAttack();
        currentState = AttackState.Idle;
    }

    public void UnlockAttack(int attackIndex)
    {
        if (attackIndex >= 0 && attackIndex < 3)
        {
            attacks[attackIndex].isUnlocked = true;
            Debug.Log($"Ataque desbloqueado: {attacks[attackIndex].name}");
        }
    }

    public void IncrementDamageCounter()
    {
        damageCounter++;
        if (damageCounter < attacks.Count)
        {
            UnlockAttack(damageCounter);
        }
    }
}
