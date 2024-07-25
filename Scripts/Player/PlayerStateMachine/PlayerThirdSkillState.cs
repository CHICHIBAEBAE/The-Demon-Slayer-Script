using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThirdSkillState : PlayerSkillState
{
    public PlayerThirdSkillState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }
    public int SkillHash { get; set; }
    int index = 2;
    public override void Enter()
    {
        stateMachine.Player.isSkillActive = true;
        base.Enter();
        if (stateMachine.Player.firstSkillSlot == true)
        {
            index = 2;
        }
        else
        {
            index = 5;
        }
        if (stateMachine.Player.healthSystem.CurrentMana < stateMachine.Player.playerEquipSkill[index].MPCost)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
        SkillAnimationSelect(index);
    }

    public override void Exit()
    {
        stateMachine.Player.isSkillActive = false;
        base.Exit();
        StopSkillState(index);
    }

    public override void Update()
    {
        if (stateMachine.Player.IsAnimationFinished())
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public void SkillAnimationSelect(int index)
    {
        string skillString = stateMachine.Player.playerEquipSkill[index].AnimationName;
        SkillHash = Animator.StringToHash(skillString);
        StartTriggerAnimation(SkillHash);
    }

    public void StopSkillState(int index)
    {
        string skillString = stateMachine.Player.playerEquipSkill[index].AnimationName;
        SkillHash = Animator.StringToHash(skillString);
        StopTriggerAnimation(SkillHash);
    }
}
