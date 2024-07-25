using System;

// Leaf Node
// ��������Ʈ�� �Լ��� �޾Ƽ� ��� ����
// ���� �� ����� ���¸� ��ȯ
public class BTAction : BTNode
{
    Func<BTNodeState> action = null;

    public BTAction(Func<BTNodeState> action)
    {
        this.action = action;
    }

    public override BTNodeState Evaluate()
    {        
        return action();
    }
}