using System;
using System.Collections.Generic;

// RandomSelector Node 
// �ڽ� ��带 �������� ����
// Selector�� ���� ������� ����
public class BTRandomSelector : BTSelector
{
    private Random random = new Random();

    public override BTNodeState Evaluate()
    {
        Suffle(children);        

        return base.Evaluate();
    }

    private void Suffle(List<BTNode> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int k = random.Next(i + 1);
            BTNode node = list[k];
            list[k] = list[i];
            list[i] = node;
        }
    }
}