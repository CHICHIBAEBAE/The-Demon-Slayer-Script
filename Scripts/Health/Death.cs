using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    private HealthSystem healthSystem;
    private Rigidbody2D rigidBody;

    private void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        rigidBody = GetComponent<Rigidbody2D>();
        // ���� ���� ��ü�� healthSystem��
        healthSystem.OnDeath += OnDeath;
    }

    // Ŀ���� �ؼ� ��������
    void OnDeath()
    {
        // ���ߵ��� ����
        rigidBody.velocity = Vector3.zero;

        // �ణ �������� �������� ����
        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        // 2�ʵڿ� �ı�
        Destroy(gameObject, 2f);
    }
}
