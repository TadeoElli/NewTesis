using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FSM_Attack : MonoBehaviour
{
    public enum AttackState { Idle, MeteorRain, ElectricFloor, HorizontalBurst }

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
    [SerializeField] private Transform player;
    [SerializeField] private float minDelayBetweenAttacks = 2f;
    [SerializeField] private float maxDelayBetweenAttacks = 5f;

    [Header("Attacks")]
    [SerializeField] private List<Attack> attacks = new List<Attack>();

    private AttackState currentState = AttackState.Idle;
    private int damageCounter = 0;
    private float attackTimer = 0f;
    private bool isChoosingAttack = false;

    private void Start()
    {
        UnlockAttack(0); // Desbloquea el primer ataque al inicio.
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

        // Configurar el final del ataque después de la duración.
        Invoke(nameof(StartAttack), attack.durationtoStart);
    }
    private void StartAttack(Attack attack)
    {
        Debug.Log($"Iniciando ataque: {attack.name}");

        // Disparar el evento de inicio del ataque.
        attack.onAttackStart?.Invoke();


        // Configurar el final del ataque después de la duración.
        Invoke(nameof(EndAttack), attack.duration);
    }

    private void EndAttack()
    {
        Debug.Log("Ataque finalizado.");

        // Disparar el evento de finalización del ataque.
        attacks[(int)currentState].onAttackEnd?.Invoke();

        // Volver al estado inactivo.
        currentState = AttackState.Idle;

        // Elegir el siguiente ataque.
        ChooseNextAttack();
    }

    public void UnlockAttack(int attackIndex)
    {
        if (attackIndex >= 0 && attackIndex < attacks.Count)
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
