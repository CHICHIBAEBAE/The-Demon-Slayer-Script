using System.Collections.Generic;

// �� ���� ������, ����, ���� 3������ ���¸� ������
public enum BTNodeState
{
    Running,
    Success,
    Failure
}

public abstract class BTNode
{
    // Ʈ�� ����
    public List<BTNode> children = new List<BTNode>();

    // �ڽ� ��� �߰� �Լ�
    public void AddChild(BTNode node)
    {
        children.Add(node);
    }
    
    // ����� ���¸� ��ȯ�ϴ� �Լ�
    public virtual BTNodeState Evaluate()
    {
        return BTNodeState.Failure;
    }
}