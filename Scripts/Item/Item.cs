using System.Collections;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public ItemSO itemSO;

    [SerializeField] float chaseSpeed;
    [SerializeField] float maxTargetDistance = 10;

    protected Rigidbody2D rb;
    [SerializeField] protected float obtainDelayTime = 0.5f;

    protected bool canObtain = false;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        StartCoroutine(SetCanObtainAfterDelay(obtainDelayTime));
    }

    public void ObtainItem()
    {
        gameObject.SetActive(false);
    }

    //private void FixedUpdate()
    //{
    //    if(canObtain)
    //    {
    //        ChasePlayer();
    //    }
    //}


    public void ChasePlayer()
    {
        Vector3 targetPosition = GameManager.Instance.Player.transform.position;

        if (Vector3.Distance(targetPosition, transform.position) <= maxTargetDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, chaseSpeed * Time.deltaTime);
        }

    }

    public abstract void Drop(Vector3 postion);


    // �־��� �ð� �Ŀ� canObtain�� true�� ����
    private IEnumerator SetCanObtainAfterDelay(float delay)
    {
        canObtain = false;
        // ������ �ð���ŭ ���
        yield return new WaitForSeconds(delay);

        // canObtain�� true�� ����
        canObtain = true;
    }

}
