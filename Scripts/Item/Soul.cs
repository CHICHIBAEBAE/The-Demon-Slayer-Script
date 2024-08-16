using System.Collections;
using TMPro;
using UnityEngine;

public class Soul : Item
{
    Player player;
    SoulUI soulUI;

    [SerializeField] protected float explosionForce; // ������ ���� ũ��
    [SerializeField] protected float upForce; // �ʱ� ���� ���� ��

    [SerializeField] private AudioClip obtainClip;
    [SerializeField] private float obtainDistance = 0.1f;

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        ObtainItem();
    //        ObtainSoul();
    //    }
    //}

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        soulUI = UIManager.Instance.soulUI;
        player = GameManager.Instance.Player;
    }

    private void FixedUpdate()
    {
        if (canObtain)
        {
            ChasePlayer();
            ObtainSoul();
        }
    }

    public void ObtainSoul()
    {
        Vector3 targetPosition = GameManager.Instance.Player.transform.position;

        if (Vector3.Distance(targetPosition, transform.position) <= obtainDistance)
        {
            ObtainItem();
            player.soulCount++;
            soulUI.UpdateSoulUI();
            SoundManager.Instance.PlaySFX(obtainClip);
        }
    }

    public override void Drop(Vector3 postion)
    {
        float randomDirX = Random.Range(-1f, 1f);
        float dirY = 1f;
        Vector2 dropDir = new Vector2(randomDirX, dirY).normalized;
        transform.position = new Vector3(postion.x, postion.y + 0.5f, postion.z);
        transform.rotation = Quaternion.identity;

        Vector2 force = dropDir * explosionForce + Vector2.up * upForce;

        rb.AddForce(force, ForceMode2D.Impulse);
    }

}
