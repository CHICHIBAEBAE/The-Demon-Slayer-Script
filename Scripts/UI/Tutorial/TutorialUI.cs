using DG.Tweening;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : UIBase
{
    public GameObject whiteImg;
    private Image whiteImage;
    public Text text;
    public GameObject TextPanel;
    public Transform moveTutorialTarget;
    public TextMeshProUGUI skipText;

    // Ʃ�丮�� �ؽ�Ʈ �׷�
    private string[][] tutorialTexts;
    private WaitForSeconds waitTextDelay = new WaitForSeconds(4f);
    private WaitForSeconds Delay = new WaitForSeconds(2f);
    Player player;
    // UI ����
    public CheckPointUI checkPointUI;
    [SerializeField] PauseUI pauseUI;
    [SerializeField] MainMenuUI mainMenuUI;
    public BoxCollider2D ChackpointCollider;
    public BoxCollider2D TutorialCollider;
    public BoxCollider2D MoveCollider;
    public PracticeMonster ScareCrow;
    public GameObject SkipBtn;
    public Button firstBtn;

    private void Awake()
    {
        whiteImage = whiteImg.GetComponent<Image>();
        ChackpointCollider.enabled = true;
        UIManager.Instance.tutorialUI = this;
    }
    private void Start()
    {
        player = GameManager.Instance.Player;
        pauseUI = UIManager.Instance.pauseUI;
        mainMenuUI = UIManager.Instance.mainMenuUI;
        player.Input.TutorialActions.Enable();
        // Ʃ�丮�� �ؽ�Ʈ �ʱ�ȭ
        tutorialTexts = new string[][]
        {
            new string[] // ������
            {
                "�¿� ����Ű�� ������ �� �ֽ��ϴ�.",
                "�տ� ������ ������ �̵��غ�����",
                "�������� �����ó׿�."
            },
            new string[] // üũ����Ʈ
            {
                "���鿡 �ִ� ������ üũ����Ʈ �Դϴ�.",
                "���� �ٰ����� 'E' �� ����������.",
                "üũ����Ʈ�� ��ȣ�ۿ��ϸ� ������ ������ �˴ϴ�.",
                "üũ����Ʈ ������ �پ��� ��ȣ�ۿ��� �� �� �ֽ��ϴ�.",
                "���� �̵���, üũ����Ʈ�� ������ ������ �����̵� �� �� �ֽ��ϴ�.",
                "������ ���͸� ��� �ҿ��� ȹ���ؼ�, ��ų�� ������ �� �ֽ��ϴ�.",
                "��ų�� ���ܿ��� ������ ��ų�� ���� �� �� �ֽ��ϴ�.",
                "��ų ��� ����Ű�� 'A','S','D' �Դϴ�. 'F'Ű�� ��ų ���� ��ȯ�� �����մϴ�.",
                "�������� �� ������ �����ִ� ������ Ȥ�� ������ ��Ƽ� ȹ���ϴ� �������� ���� �� �� �ֽ��ϴ�.",
                "ESC ��ư�� ���� â�� ���� �� �ֽ��ϴ�."
            },
            new string[] // ����
            {
                "ESC ��ư�� ���� ���� â�� �� �� �ֽ��ϴ�.",
                "��� ��ư�� ���� �������� ���� �� �� �ֽ��ϴ�.",
                "üũ����Ʈ�� ���ư����, ������ �����ص״� üũ����Ʈ�� �̵��� �� �ֽ��ϴ�.",
                "���� ������ ������� �ػ󵵸� ���� �� �� �ֽ��ϴ�.",
                "Ÿ��Ʋ�� ���ư���� ����ȭ������ ���ư��ϴ�.",
                "Ÿ��Ʋ�� ���ư��ñ� ���� �� üũ����Ʈ ������ �ϰ� �����Ͻñ� �ٶ��ϴ�.",
                "�������� ��ư�Դϴ�. �����Ͻñ����� üũ����Ʈ ������ �ϰ� �����Ͻñ� �ٶ��ϴ�.",
                "ESC ��ư�� ���� â�� ���� �� �ֽ��ϴ�."
            },
            new string[] // ĳ����
            {
                "TABŰ�� ���� ĳ������ ������ �� �� �ֽ��ϴ�.",
                "TABŰ�� ���� ������",
                "���¿��� ĳ������ ������ Ȯ�� �� �� �ֽ��ϴ�. 'P'Ű�� ����ص� Ȯ�� �� �� �ֽ��ϴ�.",
                "�Ʒ��� �׸�ĭ�� �нú� ������ �������� �������� ȹ���ϸ� �ڵ����� ������ �˴ϴ�.",
                "��ų���� ��ų�� ������ ���� �� �ֽ��ϴ�. ������ üũ����Ʈ ������ �����մϴ�.'K'Ű�� ����ص� Ȯ�� �� �� �ֽ��ϴ�.",
                "���������� ȹ���� �������� Ȯ���Ͻ� �� �ֽ��ϴ�. ������ üũ����Ʈ ������ �����մϴ�. 'I'Ű�� ����ص� Ȯ�� �� �� �ֽ��ϴ�.",
                "���������� Ž���� ���� Ȯ���� �� �ֽ��ϴ�. 'M'Ű�� ����ص� Ȯ�� �� �� �ֽ��ϴ�.",
                "ESC ��ư�� ���� â�� ���� �� �ֽ��ϴ�."
            },
            new string[] // ����
            {
                "���鿡 �ִ� ����ƺ� �����غ�����. ������ 'X'Ű �Դϴ�.",
                "�����ϴ�. ������ �� 3������ ����� �����ϸ�, �� ��°,�� ��° ������ �� ���� �������� �� �� �ֽ��ϴ�.",
                "'Z'Ű�� �����⸦ ��� �� �� �ֽ��ϴ�. �����⸦ ����ϴ� �߿��� ������ ������ ���� �ʽ��ϴ�.",
                "'C'Ű�� ������ �� �� �ֽ��ϴ�. ���� ���� �ö� �� ����غ�����",
                "�⺻���� ���۹��� ������� �Դϴ�. �����Ͻô��� ����ϼ̽��ϴ�.",
                "����� ���迡 ����� ���⸦ �ٶ��ϴ�."
            }
        };

        Execute();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SkipBtn.SetActive(true);
            firstBtn.Select();
        }
    }

    public void Execute()
    {
        Datas datas = DataManager.Instance.GetData();

        if (datas.isPlayIntro)
        {
            ChackpointCollider.gameObject.SetActive(false);
            TutorialCollider.gameObject.SetActive(false);
            MoveCollider.gameObject.SetActive(false);
            player.Input.TutorialActions.Disable();
            PlayerInputEnable();
            gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(RunTutorial());
        }
    }

    public void SkipTutorial()
    {
        ChackpointCollider.gameObject.SetActive(false);
        TutorialCollider.gameObject.SetActive(false);
        MoveCollider.gameObject.SetActive(false); ;
        StopCoroutine(RunTutorial());
        player.Input.TutorialActions.Disable();
        PlayerInputEnable();
        gameObject.SetActive(false);
    }

    public void CloseSkip()
    {
        SkipBtn.SetActive(false);
    }

    void PlayerInputEnable()
    {
        player.Input.PlayerActions.Enable();
        player.Input.UIActions.Enable();
        TextPanel.SetActive(false);
    }
    void PlayerInputDisable()
    {
        player.Input.PlayerActions.Disable();
        player.Input.UIActions.Disable();
        TextPanel.SetActive(true);
    }
    IEnumerator RunTutorial()
    {
        yield return StartCoroutine(RunMovementTutorial());
        yield return StartCoroutine(RunCheckpointTutorial());
        yield return StartCoroutine(RunPauseTutorial());
        yield return StartCoroutine(RunCharacterTutorial());
        yield return StartCoroutine(RunBattleTutorial());
        player.Input.TutorialActions.Disable();
        PlayerInputEnable();
        gameObject.SetActive(false);
    }

    // 1. ������ Ʃ�丮��
    IEnumerator RunMovementTutorial()
    {
        PlayerInputDisable();
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "��,�� ����Ű�� ������ �� �ֽ��ϴ�.",
            "���鿡 ������ ������ �̵��غ�����. "
        }));
        PlayerInputEnable();

        // ����: �÷��̾ Ư�� ��ġ�� ������ ��

        yield return new WaitUntil(() => PlayerHasReachedTargetPosition());
        PlayerInputDisable();
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "�������� �����ó׿�."
        }));
    }

    // 2. üũ����Ʈ Ʃ�丮��
    IEnumerator RunCheckpointTutorial()
    {
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "���鿡 �ִ� ������ üũ����Ʈ �Դϴ�.",
            "���� �ٰ����� 'E' �� ����������."
        }));
        PlayerInputEnable();

        // ����: üũ����Ʈ UI Ȱ��ȭ ���
        yield return new WaitUntil(() => CheckpointOn());
        PlayerInputDisable();
        checkPointUI.BtnUnActive();
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "üũ����Ʈ�� ��ȣ�ۿ��ϸ� ������ ������ �˴ϴ�.",
            "üũ����Ʈ������ �پ��� ��ȣ�ۿ��� �� �� �ֽ��ϴ�.",
        }));
        checkPointUI.BtnActive();
        checkPointUI.OnClickFastTravelBtn();
        Time.timeScale = 1f;
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "���� �̵���, üũ����Ʈ�� ������ ������ �����̵� �� �� �ֽ��ϴ�."

        }));
        checkPointUI.OnSecondBtn();
        Time.timeScale = 1f;
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "������ ���͸� ��� �ҿ��� ȹ���ؼ�, ��ų�� ������ �� �ֽ��ϴ�.",
        }));
        checkPointUI.OnThirdBtn();
        Time.timeScale = 1f;
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "��ų�� ���ܿ��� ������ ��ų�� ���� �� �� �ֽ��ϴ�.",
            "��ų ��� ����Ű�� 'A','S','D' �Դϴ�. 'F'Ű�� ��ų ���� ��ȯ�� �����մϴ�.",
        }));
        checkPointUI.OnClickfourthBtn();
        Time.timeScale = 1f;
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "�������� �� ������ �����ִ� ������ Ȥ�� ������ ��Ƽ� ȹ���ϴ� �������� ���� �� �� �ֽ��ϴ�.",
            "ESC ��ư�� ���� â�� ���� �� �ֽ��ϴ�."
        }));
        mainMenuUI.gameObject.SetActive(false);
        UIManager.Instance.SetOnUI(false);
        ChackpointCollider.enabled = false;
        PlayerInputEnable();
    }

    bool CheckpointOn()
    {
        checkPointUI = GameManager.Instance.TutorialCheckPoint;
        bool CheckpointOn = false;

        if (checkPointUI != null)
        {
            if (checkPointUI.gameObject.activeInHierarchy)
            {
                CheckpointOn = true;
            }
        }
        return CheckpointOn;
    }

    // 3. ���� Ʃ�丮��
    IEnumerator RunPauseTutorial()
    {
        yield return Delay;
        PlayerInputDisable();
        yield return StartCoroutine(ShowTexts(new string[]
        {
                "ESC ��ư�� ���� ���� â�� �� �� �ֽ��ϴ�.",
                "ESC ��ư�� ����������."
        }));

        TextPanel.SetActive(false);
        player.Input.PlayerActions.Disable();
        player.Input.UIActions.Enable();

        // ����: ESC Ű �Է� �� ����â ����
        yield return new WaitUntil(() => pauseUI.gameObject.activeInHierarchy);
        pauseUI.BtnUnActive();
        Time.timeScale = 1f;
        PlayerInputDisable();
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "��� ��ư�� ���� �������� ���� �� �� �ֽ��ϴ�.",
            "üũ����Ʈ�� ���ư����, ������ �����ص״� üũ����Ʈ�� �̵��� �� �ֽ��ϴ�.",
            "���� ������ ����� �ػ󵵸� ���� �� �� �ֽ��ϴ�.",
            "Ÿ��Ʋ�� ���ư���� ����ȭ������ ���ư��ϴ�.",
            "Ÿ��Ʋ�� ���ư��ñ� ���� �� üũ����Ʈ ������ �ϰ� �����Ͻñ� �ٶ��ϴ�.",
            "�������� ��ư�Դϴ�. �����Ͻñ����� üũ����Ʈ ������ �ϰ� �����Ͻñ� �ٶ��ϴ�.",
            "ESC ��ư�� ���� â�� ���� �� �ֽ��ϴ�."
        }));
        pauseUI.BtnActive();
        pauseUI.gameObject.SetActive(false);
        UIManager.Instance.SetOnUI(false);
    }

    // 4. ĳ���� Ʃ�丮��
    IEnumerator RunCharacterTutorial()
    {
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "TABŰ�� ���� ĳ������ ������ �� �� �ֽ��ϴ�.",
            "TABŰ�� ���� ������."
        }));
        player.Input.UIActions.Enable();
        TextPanel.SetActive(false);
        // ����: TAB Ű �Է� �� ĳ���� ���� â ����
        yield return new WaitUntil(() => MainMenuUIOn());
        PlayerInputDisable();
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "���¿��� ĳ������ ������ Ȯ�� �� �� �ֽ��ϴ�. 'P'Ű�� ����ص� Ȯ�� �� �� �ֽ��ϴ�.",
            "�Ʒ��� �׸�ĭ�� �нú� ������ �������� �������� ȹ���ϸ� �ڵ����� ������ �˴ϴ�."
        }));

        mainMenuUI.SwapMainMenu(1);
        Time.timeScale = 1f;
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "��ų���� ���� Ȥ�� ȹ���� ��ų�� ������ ���� �� �ֽ��ϴ�. ������ üũ����Ʈ ������ �����մϴ�.'K'Ű�� ����ص� Ȯ�� �� �� �ֽ��ϴ�.",
        }));
        mainMenuUI.SwapMainMenu(2);
        Time.timeScale = 1f;
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "���������� ȹ�� Ȥ�� ������ �������� Ȯ���Ͻ� �� �ֽ��ϴ�. ������ üũ����Ʈ ������ �����մϴ�. 'I'Ű�� ����ص� Ȯ�� �� �� �ֽ��ϴ�.",
        }));
        mainMenuUI.SwapMainMenu(3);
        Time.timeScale = 1f;
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "���������� Ž���� ���� Ȯ���� �� �ֽ��ϴ�. 'M'Ű�� ����ص� Ȯ�� �� �� �ֽ��ϴ�.",
            "������ ���콺 �巡�׷� ������ �� �ֽ��ϴ�.",
            "�Ʒ� ��ư�� ������ ������ ���� ��ġ�� �ǵ����ϴ�.",
            "ESC ��ư�� ���� â�� ���� �� �ֽ��ϴ�."
        }));
        mainMenuUI.gameObject.SetActive(false);
        UIManager.Instance.SetOnUI(false);
    }

    bool MainMenuUIOn()
    {
        bool mainMeunOn = false;

        if (mainMenuUI != null)
        {
            if (mainMenuUI.gameObject.activeInHierarchy)
            {
                mainMeunOn = true;
                mainMenuUI.SwapMainMenu(0);
            }
        }
        return mainMeunOn;
    }

    // 5. ���� Ʃ�丮��
    IEnumerator RunBattleTutorial()
    {
        TextPanel.SetActive(true);
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "���鿡 �ִ� ����ƺ� �����غ�����. ������ 'X'Ű �Դϴ�."
        }));
        PlayerInputEnable();
        // ����: Ư�� �� �Ǵ� ������Ʈ(����ƺ�) ���� ����
        yield return new WaitUntil(() => MonsterDead());

        TextPanel.SetActive(true);
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "�����ϴ�. ������ �� 3������ ����� �����ϸ�, �� ��°,�� ��° ������ �� ���� �������� �� �� �ֽ��ϴ�.",
            "'Z'Ű�� �����⸦ ��� �� �� �ֽ��ϴ�. �����⸦ ����ϴ� �߿��� ������ ������ ���� �ʽ��ϴ�.",
            "'C'Ű�� ������ �� �� �ֽ��ϴ�. ���� ���� �ö� �� ����غ�����.",
            "�⺻���� ���۹��� ������� �Դϴ�. �����Ͻô��� ����ϼ̽��ϴ�.",
            "����� ���迡 ����� ���⸦ �ٶ��ϴ�."
        }));
        TutorialCollider.enabled = false;
    }

    bool MonsterDead()
    {
        bool MonsterHit = false;
        if (ScareCrow.HitCount >= 3)
        {
            PlayerInputDisable();
            MonsterHit = true;
        }
        return MonsterHit;
    }


    IEnumerator ShowTexts(string[] texts)
    {
        foreach (var line in texts)
        {
            bool isSkipped = false; 

            Tween tween = null;
            tween = text.DOText(line, 3f).OnUpdate(() =>
            {
                // VŰ �Է� ����
                if (Input.GetKeyDown(KeyCode.V))
                {
                    tween.Complete(); 
                    isSkipped = true;
                }
            });
            if (isSkipped)
            {
                yield return Delay; 
            }
            else
            {
                yield return waitTextDelay; 
            }
            ClearText();
        }
    }


    private void ClearText()
    {
        text.text = "";
    }

    // �÷��̾ ��ǥ ��ġ�� �����ߴ��� Ȯ���ϴ� ����
    private bool PlayerHasReachedTargetPosition()
    {
        float distance = Vector3.Distance(player.transform.position, moveTutorialTarget.transform.position);

        return distance <= 5.45f;
    }

}
