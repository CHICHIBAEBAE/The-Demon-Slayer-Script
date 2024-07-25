// Selector Node 
// �ڽ� ��带 ���������� ����
// �ڽ� ��尡 Success ��ȯ �� �ߴ� �� Success ��ȯ
// Success ������ �ڽ� ��尡 ������ Failure ��ȯ
public class BTSelector : BTNode
{
    public override BTNodeState Evaluate()
    {
        foreach (BTNode node in children)
        {
            BTNodeState result = node.Evaluate();

            if (result == BTNodeState.Success)
            {
                return BTNodeState.Success;
            }
            else if (result == BTNodeState.Running)
            {
                return BTNodeState.Running;
            }
        }

        return BTNodeState.Failure;
    }
}