using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] protected float damageAmount;

    protected void ApplyDamage()
    {
        //�÷��̾�� �������� ����
        GameManager.Instance.Player.healthSystem.ChangeHealth(-damageAmount);
    }
}
