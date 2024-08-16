using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public float healthChangeDelay = 1f;
    [SerializeField] private int DamageClipIdx;

    private DamageTextUI damageTextUI;
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
    public float damagefontSize = 1f;

    [SerializeField] public float CurrentHealth { get => data.CurrentHealth; set => data.CurrentHealth = value; }
    [SerializeField] public float CurrentMana { get => data.CurrentMana; set => data.CurrentMana = value; }
    [SerializeField] public float CurrentStamina { get => data.CurrentStamina; set => data.CurrentStamina = value; }

    // get�� ������ ��ó�� ������Ƽ�� ����ϴ� ��
    public float MaxHealth { get => data.MaxHealth; set => data.MaxHealth = value; }
    public float MaxMana { get => data.MaxMana; set => data.MaxMana = value; }
    public float MaxStamina { get => data.MaxStamina; set => data.MaxStamina = value; }

    public HealthSystemData data;
    public float playerTextYNum = .5f;

    public bool isShieldBuff = false;

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

        if(isShieldBuff && transform.CompareTag("Player") && change < 0)
        {
            change *= (GameManager.Instance.Player.PlayerSkill.shiledValue);
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
            DamageTextMethod(change);
            return true;
        }

        if (change >= 0)
        {
            OnHeal?.Invoke();
            CreateDamageText(new Vector3
                    (this.gameObject.transform.position.x,
                     this.gameObject.transform.position.y + playerTextYNum,
                     this.gameObject.transform.position.z), change, Color.green);
        }
        else
        {
            OnDamage?.Invoke();
            DamageTextMethod(change);
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

        if (statsHandler == null) return;

        if (statsHandler.CurrentStat.statsSO is PlayerStatsSO)
        {
            CurrentMana = MaxMana;
            CurrentStamina = MaxStamina;
        }
    }

    public void DamageTextMethod(float change)
    {
        if (this.tag == "Player")
        {
            CreateDamageText(new Vector3
                (this.gameObject.transform.position.x,
                 this.gameObject.transform.position.y + playerTextYNum,
                 this.gameObject.transform.position.z), change, Color.red);
        }
        else
        {
            CreateDamageText(new Vector3
                (this.gameObject.transform.position.x + UnityEngine.Random.Range(-.5f, .5f),
                 this.gameObject.transform.position.y + UnityEngine.Random.Range(2, 4),
                 this.gameObject.transform.position.z), change, Color.red);
        }
    }

    public void CreateDamageText(Vector3 pos, float damage, Color color)
    {
        Text dmgTxt = GameManager.Instance.damageTextPool.GetFromPool(UnityEngine.Random.Range(0, 4));
        damageTextUI = dmgTxt.GetComponent<DamageTextUI>();
        damageTextUI.Init(dmgTxt, pos, damage, color, damagefontSize);
    }
}