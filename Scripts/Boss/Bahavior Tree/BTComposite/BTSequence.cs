// Sequence Node
// ��� �ڽ� ��带 ���������� ����
// �ڽ� ��尡 Failure ��ȯ �� �ߴ� �� Failure ��ȯ
public class BTSequence : BTNode
{
    public override BTNodeState Evaluate()
    {
        bool isChildInProgress = false;

        foreach (BTNode node in children)
        {
            BTNodeState result = node.Evaluate();

            if (result == BTNodeState.Failure)
            {
                return BTNodeState.Failure;
            }
            else if (result == BTNodeState.Running)
            {
                isChildInProgress = true;
            }
        }
        return isChildInProgress ? BTNodeState.Running : BTNodeState.Success;
    }
}