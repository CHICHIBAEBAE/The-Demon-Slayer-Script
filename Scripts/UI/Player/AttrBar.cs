using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttrBar : MonoBehaviour
{
    [SerializeField]
    private AttrType attrType;
    // [SerializeField]
    // private PlayerStats playerStats;
    [SerializeField]
    private Image bar;
    [SerializeField]
    private Image secondaryBar;
    [SerializeField]
    private Image barBackground;
    [SerializeField]
    private Image barBorder;
    [SerializeField]
    private Image useBar;
    [SerializeField]
    private Image usedBar;

    public AttrType AttrType1 { get => attrType; set => attrType = value; }

    //public PlayerStats PlayerStats
    //{
    //    get => playerStats;
    //    set => playerStats = value;
    //}
    public Image Bar { get => bar; set => bar = value; }
    public Image SecondaryBar { get => secondaryBar; set => secondaryBar = value; }
    public Image BarBackground { get => barBackground; set => barBackground = value; }
    public Image BarBorder { get => barBorder; set => barBorder = value; }
    public Image UseBar { get => useBar; set => useBar = value; }
    public Image UsedBar { get => usedBar; set => usedBar = value; }


    //private void Start()
    //{
    //    if (!playerStats)
    //    {
    //        playerStats = FindObjectOfType<PlayerStats>();
    //    }
    //}

    [SerializeField] private HealthSystem playerHS;

    // Start is called before the first frame update
    void Start()
    {
        if (!playerHS)
        {
            playerHS = GameManager.Instance.Player.healthSystem;
        }
    }

    private void Update()
    {
        UpdateStats(playerHS);
    }

    public void UpdateStats(HealthSystem playerHS)
    {
        if (!playerHS)
        {
            return;
        }

        float percent = GetPercentByAttr(AttrType1);

        if (bar)
        {
            bar.fillAmount = percent;

            if (secondaryBar)
            {
                secondaryBar.fillAmount = Mathf.Lerp(secondaryBar.fillAmount, bar.fillAmount, 5 * Time.deltaTime);
            }
        }
    } 

    public float GetAttrValueByAttrType(AttrType attr)
    {
        switch (attr)
        {
            default:
                return 0;
            case AttrType.Health:
                return playerHS.CurrentHealth;
            case AttrType.Mana:
                return playerHS.CurrentMana;
        }
    }

    public float GetMaxAttrByAttrType(AttrType attr)
    {
        if(playerHS != null)
        {
            switch (attr)
            {
                default:
                    return 0;
                case AttrType.Health:
                    return playerHS.MaxHealth;
                case AttrType.Mana:
                    return playerHS.MaxMana;
            }
        }
        else
        {
            return 0;
        }
        
    }

    public float GetPercentByAttr(AttrType attr)
    {
        switch (attr)
        {
            default:
                return 0;
            case AttrType.Health:
                return (float)playerHS.CurrentHealth / (float)playerHS.MaxHealth;
            case AttrType.Mana:
                return (float)playerHS.CurrentMana / (float)playerHS.MaxMana;
            case AttrType.Stamina:
                return (float)playerHS.CurrentStamina / (float)playerHS.MaxStamina;
        }
    }

    public enum AttrType
    {
        Health = 0,
        Mana = 1,
        Stamina = 2
    }
}
