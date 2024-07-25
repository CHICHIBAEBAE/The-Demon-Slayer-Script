using System;
using System.Diagnostics;

// Add �����ϰ�, Multiple�ϰ�, �������� Override�ϴ� ������
// enum���� ���� ������ ���εǾ��ֱ� ������ ���� (0, 1, 2,...) 
// => ���Ŀ� ���� Ȱ���ϸ� ������������ ����Ȱ���ؼ� ü�������� ����ȿ�� ������� ���� ����
// Q: Override�� �������� ������ ���� ȿ���� �������?
// A: ���ݷ��� �����ؾ��ϴ� Ư�� �����̳� �⺻ ���ݷ� ���뿡 Ȱ�� ����
public enum StatsChangeType
{
    Add,
    Multiple,
    Override,
}

[Serializable]
public class Stats
{
    public StatsChangeType statsChangeType;
    public int maxHealth;
    public int speed;
    public StatsSO statsSO;   
}

