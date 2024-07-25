using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float healthChangeDelay = .5f;
    [SerializeField] private int DamageClipIdx;

    private StatHandler statsHandler;
    public float timeSinceLastChange = float.MaxValue;
    [HideInInspector] public bool isInvincibility = false;

    // ü���� ������ �� �� �ൿ���� �����ϰ� ���� ����
    public event Action OnDamage;
    public event Action OnHeal;
    public event Action OnDeath;
    // public event Action OnInvincibilityEnd;
    public event Action<float> OnHealthChanged;

    // ���� 
    public event Action OnRefillMana;
    public event Action OnConsumeMana;
    public event Action OnManaZero;

    public LayerMask layerMask;

    [SerializeField] public float CurrentHealth { get => data.CurrentHealth; set => data.CurrentHealth = value; }
    [SerializeField] public float CurrentMana { get => data.CurrentMana; set => data.CurrentMana = value; }
    [SerializeField] public float CurrentStamina { get => data.CurrentStamina; set => data.CurrentStamina = value; }

    // get�� ������ ��ó�� ������Ƽ�� ����ϴ� ��
    public float MaxHealth { get => data.MaxHealth; set => data.MaxHealth = value; }
    public float MaxMana { get => data.MaxMana; set => data.MaxMana = value; }
    public float MaxStamina { get => data.MaxStamina; set => data.MaxStamina = value; }

    public HealthSystemData data;

    private void Awake()
    {
        SetStats();
    }

    public void SetStats()
    {
        statsHandler = GetComponent<StatHandler>();
        CurrentHealth = statsHandler.CurrentStat.maxHealth;

        UpdateStats();
    }
    private void Start()
    {
        statsHandler.StatChange += UpdateStats;
    }

    public void UpdateStats()
    {
        MaxHealth = statsHandler.CurrentStat.maxHealth;

        if (statsHandler.CurrentStat.statsSO is PlayerStatsSO)
        {
            //CurrentHealth = statsHandler.CurrentStat.statsSO.hp;
            PlayerStatsSO playerStatsSO = (PlayerStatsSO)statsHandler.CurrentStat.statsSO;

            if (playerStatsSO != null)
            {
                MaxMana = playerStatsSO.mana;
                MaxStamina = playerStatsSO.stamina;

                CurrentMana = playerStatsSO.mana;
                CurrentStamina = playerStatsSO.stamina;
            }
        }
        
    }


    private void Update()
    {
        if (timeSinceLastChange < healthChangeDelay)
        {
            timeSinceLastChange += Time.deltaTime;
            //if (timeSinceLastChange >= healthChangeDelay)
            //{
            //    OnInvincibilityEnd?.Invoke();
            //}
        }
        ChangeMana(0.01f);
    }

    public bool ChangeHealth(float change)
    {
        if (isInvincibility && change <= 0)
        {
            return false;
        }

        if (timeSinceLastChange < healthChangeDelay && transform.CompareTag("Player"))
        {
            return false;
        }

        timeSinceLastChange = 0f;
        CurrentHealth += change;
        // �ּڰ��� 0, �ִ��� MaxHealth�� �ϴ� ����.
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        // CurrentHealth = CurrentHealth > MaxHealth ? MaxHealth : CurrentHealth;
        // CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth; �� ����


        if (CurrentHealth <= 0f)
        {
            CallHealthChanged();
            CallDeath();
            return true;
        }

        if (change >= 0)
        {
            OnHeal?.Invoke();
        }
        else
        {
            OnDamage?.Invoke();
        }

        CallHealthChanged();

        return true;
    }

    private void CallDeath()
    {
        OnDeath?.Invoke();
    }

    public void CallHealthChanged()
    {
        OnHealthChanged?.Invoke(CurrentHealth);
    }

    public bool ChangeMana(float change)
    {
        CurrentMana += change;
        CurrentMana = Mathf.Clamp(CurrentMana, 0, MaxMana);

        if (CurrentMana <= 0f)
        {
            OnManaZero?.Invoke();
            return true;
        }

        if (change >= 0)
        {
            OnRefillMana?.Invoke();
        }
        else
        {
            OnConsumeMana?.Invoke();
        }

        return true;
    }

    public void ResetHealth()
    {
        CurrentHealth = MaxHealth;

        if (statsHandler.CurrentStat.statsSO is PlayerStatsSO)
        {
            CurrentMana = MaxMana;
            CurrentStamina = MaxStamina;
        }
    }
}