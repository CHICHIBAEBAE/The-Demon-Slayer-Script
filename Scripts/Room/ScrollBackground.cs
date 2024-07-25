using UnityEngine;

public class ScrollBackground : MonoBehaviour
{
    private Vector3 previousPosition; // ���� �������� ī�޶� ��ġ
    private Vector3 cameraMoveDir;   // �̵� ����
    private float cameraMoveSpeed;   // �̵� ����
    private Camera mainCamera;

    // [SerializeField]
	// private	Transform	target;				// ���� ���� �̾����� ���
	// [SerializeField]
	// private	float		scrollAmount;		// �̾����� �� ��� ������ �Ÿ�
	[SerializeField]
	private	float moveSpeed;			// ��� �̵� �ӵ�
	private	float baseSpeed = 0.001f;			// ��� �̵� �ӵ�


    private void Awake()
    {
        mainCamera = Camera.main;
    }

    void Start()
    {
        // �ʱ� ��ġ ����
        previousPosition = mainCamera.transform.position;
    }

    private void CalculateCameraDirAndSpeed()
    {
        // ���� ��ġ�� ���� ��ġ�� ���̸� �̵� ���ͷ� ���
        cameraMoveDir = mainCamera.transform.position - previousPosition;
        cameraMoveDir.y = 0;
        cameraMoveDir = cameraMoveDir.normalized;

        // �̵� �ӵ� ��� (����: ����/��)
        cameraMoveSpeed = cameraMoveDir.magnitude / Time.deltaTime;

        // ���� ��ġ�� ���� ��ġ�� ������Ʈ
        previousPosition = mainCamera.transform.position;
    }

    private void FixedUpdate()
    {
        CalculateCameraDirAndSpeed();
        // ����� moveDirection �������� moveSpeed�� �ӵ��� �̵�
        transform.position += -cameraMoveDir * cameraMoveSpeed * moveSpeed * Time.deltaTime * baseSpeed;

        // ����� ������ ������ ����� ��ġ �缳��
        //if ( transform.position.x <= -scrollAmount )
        //{
        //	transform.position = target.position - cameraMoveDir * scrollAmount;
        //}
    }
}

