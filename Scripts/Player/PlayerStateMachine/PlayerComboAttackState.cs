using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerComboAttackState : PlayerAttackState
{
    private bool alreadyAppliedCombo;
    private bool alreadyApplyForce;

    AttackInfoData attackInfoData;

    public PlayerComboAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.ComboAttackParameterHash);

        alreadyAppliedCombo = false;
        alreadyApplyForce = false;

        int comboindex = stateMachine.ComboIndex;
        attackInfoData = stateMachine.Player.Data.AttackData.GetAttackInfoData(comboindex);
        stateMachine.Player.Animator.SetInteger("Combo", comboindex);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.ComboAttackParameterHash);

        if (!alreadyAppliedCombo)
        {
            stateMachine.ComboIndex = 0;
        }
    }

    public override void Update()
    {
        ForceMove();

        float normalizeTime = GetNormalizedTime(stateMachine.Player.Animator, "Attack");
        if (normalizeTime < 1f)
        {
            if (normalizeTime >= attackInfoData.ComboTransitionTime)
            {
                //�޺� �õ�
                TryComboAttack();
            }

            if (normalizeTime >= attackInfoData.ForceTranstionTime)
            {
                //���� �õ�
                TryApplyForce();
            }
        }
        else
        {
            if (alreadyAppliedCombo)
            {
                stateMachine.ComboIndex = attackInfoData.ComboStateIndex;
                stateMachine.ChangeState(stateMachine.ComboAttackState);
            }
            else
            {
                stateMachine.ChangeState(stateMachine.IdleState);
            }
        }
    }

    void TryComboAttack()
    {
        if (alreadyAppliedCombo) return;

        if (attackInfoData.ComboStateIndex == -1) return;

        if (!stateMachine.IsAttacking) return;

        alreadyAppliedCombo = true;
    }

    void TryApplyForce()
    {
        if (alreadyApplyForce) return;
        alreadyApplyForce = true;

        stateMachine.Player.ForceReceiver.Reset();

        Vector2 tmpDir = stateMachine.Player.spriteRenderer.flipX ? Vector2.left : Vector2.right;
        stateMachine.Player.Rigidbody.AddForce(tmpDir * attackInfoData.Force, ForceMode2D.Impulse);
    }
}
