using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class KingSlime : Boss
{
    // public Transform player;
    // public GameObject QueenSlime;

    [Header("Stat Data")]
    public MonsterStatsSO data;

    // TODO :: �ν����Ϳ��� �߰����� ���� �ٸ� ��� �����ϱ�
    // ex) �̸��̳� �±׷� ã��
    [Header("Queen Slime Data")]
    public QueenSlime queenSlime;

    [HideInInspector] public bool isActing = false;
    [HideInInspector] public bool isDie = false;
    [HideInInspector] public bool isInvincibility = false;
    [HideInInspector] public bool onPhase2 = false;
    [HideInInspector] public bool onPhase3 = false;

    [HideInInspector] public HealthSystem healthSystem;    
    public Animator Animator { get; private set; }
    private Rigidbody2D _rigidbody;
    private SpriteRenderer spriteRenderer;
    private KingSlimeSkills skills;
    private PaintWhite paintWhite;
    private SoundManager soundManager;
    private BTSelector root;

    private float fixedY;

    [Header("Sound")]
    public AudioClip bgmClip;
    public AudioClip damageClip;
    public AudioClip groundSlamClip;
    public AudioClip smashClip;

    protected override void Awake()
    {        
        base.Awake();
        //queenSlime = GameObject.Find("Queen").GetComponent<QueenSlime>();
        _rigidbody = GetComponent<Rigidbody2D>();
        healthSystem = GetComponent<HealthSystem>();
        skills = GetComponentInChildren<KingSlimeSkills>();
        Animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        paintWhite = GetComponentInChildren<PaintWhite>();
        soundManager = SoundManager.Instance;
    }

    private void Start()
    {
        fixedY = transform.position.y;

        healthSystem.OnDamage += OnDamage;
        healthSystem.OnDeath += OnDeath;
        healthSystem.OnDeath += queenSlime.OnDeath;
        healthSystem.OnHealthChanged += bossUI.UpdateBossHp;

        root = new BTSelector();

        BTSequence skillSequence = new BTSequence();
        BTSequence meleeAttackSequence = new BTSequence();
        BTSequence movementSequence = new BTSequence();
        BTSequence phaseChangeSequence = new BTSequence();

        BTCondition inAttackRange = new BTCondition(IsInAttackRange);
        BTCondition canAct = new BTCondition(CanAct);
        BTCondition canChangePhase = new BTCondition(CanChangePhase);

        BTRandomSelector skillRandomSelector = new BTRandomSelector();
        
        BTAction smash = new BTAction(skills.SmashAction);
        BTAction groundSlam = new BTAction(skills.GroundSlamAction);       
        BTAction meleeAttack = new BTAction(skills.MeleeAttackAction);
        BTAction moveToPlayer = new BTAction(MoveToPlayer);
        BTAction changePhase = new BTAction(skills.SetPhaseAction);

        // TODO :: Ʈ�� ���ÿ� �޼��� �����? �ʹ� ������
        root.AddChild(phaseChangeSequence);
        {
            phaseChangeSequence.AddChild(canChangePhase);
            phaseChangeSequence.AddChild(changePhase);
        }
        root.AddChild(skillSequence);
        {
            skillSequence.AddChild(canAct);
            skillSequence.AddChild(inAttackRange);
            skillSequence.AddChild(skillRandomSelector);
            {
                skillRandomSelector.AddChild(smash);
                skillRandomSelector.AddChild(groundSlam);
            }
        }
        root.AddChild(meleeAttackSequence);
        {
            meleeAttackSequence.AddChild(canAct);
            meleeAttackSequence.AddChild(inAttackRange);
            meleeAttackSequence.AddChild(meleeAttack);
        }
        root.AddChild(movementSequence);
        {
            movementSequence.AddChild(canAct);
            movementSequence.AddChild(moveToPlayer);
        }

        root.Evaluate();
    }

    private void HealthSystem_OnDeath()
    {
        throw new NotImplementedException();
    }

    private void Update()
    {
        root.Evaluate();
    }   

    private bool CanAct()
    {
        return !isActing;
    }

    private bool IsInAttackRange()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, GameManager.Instance.Player.transform.position);
        return distanceToPlayer <= data.range;
      
    }

    private bool CanChangePhase()
    {       
        return (healthSystem.CurrentHealth / healthSystem.MaxHealth) * 100f <= 70f ? true : false;
    }

    // TODO :: �̵��� ���� ���� �ܼ� ��� �߻�ȭ
    private BTNodeState MoveToPlayer()
    {
        // TODO :: ����
        if (GameManager.Instance.roomManager.rooms[6].isPlayerInRoom)
        {
            if (!IsInAttackRange())
            {
                Vector3 targetPosition = GameManager.Instance.Player.transform.position;
                targetPosition.y = fixedY;

                transform.position = Vector2.MoveTowards(transform.position, targetPosition, data.speed * Time.deltaTime);                
            }
            Flip(GameManager.Instance.Player.transform.position - transform.position);
            return BTNodeState.Running;
        }
        return BTNodeState.Failure;
    }

    private void Flip(Vector2 dir)
    {
        if (dir.x < 0f)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }    

    private void OnDamage()
    {       
        paintWhite.FlashWhite();
        soundManager.PlaySFX(damageClip);
    }

    private void OnDeath()
    {
        SoundManager.Instance.PlayBGM(bgmClip); // �⺻ ������� ����
        myRoom.IsBossAlive = false; // ����濡�� ���� ��Ȱ��ȭ
        myRoom.portal.gameObject.SetActive(true);

        isDie = true;
        isActing = true;
        Animator.SetTrigger("Death");
        skills.groundSlamEffect.SetActive(false);
        skills.smashEffect.SetActive(false);
        skills.smashRange.SetActive(false);
        Destroy(gameObject, 2f);
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 attackBoxSize = new Vector2(4f, 2f);
        float yOffset = 1f;

        Gizmos.color = Color.red;
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        

        Vector2 startPosition = new Vector2(transform.position.x - 2f, transform.position.y + 1f);
        Vector2 flipDirection = new Vector2(spriteRenderer.flipX ? data.range : -data.range, yOffset);
        Vector2 attackOrigin = startPosition;
    }
}
