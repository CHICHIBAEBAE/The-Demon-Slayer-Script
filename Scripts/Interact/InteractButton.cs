using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractButton : InteractableObject
{
    public GameObject moverObj;
    private float moverObjHeight;
    public float duration = 3.0f; // �̵��� �ɸ��� �ð�
    public bool isUp = false;

    private CinemachineVirtualCamera virtualCamera;
    private Coroutine moveCor;

    public AudioClip clip;

    private Animator animator;

    private WaitForSeconds waitForSeconds;


    private void Awake()
    {
        virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        Vector3 virtualCameraPos = new Vector3 (moverObj.transform.position.x, moverObj.transform.position.y, -10);
        virtualCamera.transform.position = virtualCameraPos;

        moverObjHeight = moverObj.GetComponent<Renderer>().bounds.size.y;

        animator = GetComponent<Animator>();

        waitForSeconds = new WaitForSeconds(1f);
    }

    public override void Interact()
    {
        Execute();
    }

    private void Execute()
    {
        animator.SetTrigger("Press");

        moveCor = StartCoroutine(MoveObject());
    }

    private IEnumerator MoveObject()
    {
        yield return waitForSeconds;

        // ī�޶� �̵�
        virtualCamera.Priority = 20;

        yield return waitForSeconds;

        // ����
        SoundManager.Instance.PlaySFX(clip);

        // ���� ��ġ�� ��ǥ ��ġ ����
        Vector3 startPosition = moverObj.transform.position;

        float targetPositionY = isUp ? moverObj.transform.position.y + moverObjHeight : moverObj.transform.position.y - moverObjHeight;
        Vector3 targetPosition = new Vector3(moverObj.transform.position.x, targetPositionY, moverObj.transform.position.z);

        // ������ �̵�
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            moverObj.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���� ��ġ ����
        moverObj.transform.position = targetPosition;

        // ī�޶� ����ġ
        virtualCamera.Priority = 0;
        
        // ��ȣ�ۿ� ����
        canInteract = false;

        HideInteractText();
    }
}
