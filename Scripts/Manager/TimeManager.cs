using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager
{
    private float startTime;

    public float loadTime;
    public float playTime { get => data.playTime; set => data.playTime = value; }

    public Datas data;

    public void ResetTime()
    {
        startTime = Time.time;
        playTime = 0;
    }

    public void CheckTime()
    {
        playTime = loadTime + Time.time - startTime;
    }

    // �÷��� Ÿ���� "00�� 00�� 00��" ������ ���ڿ��� ��ȯ�ϴ� �޼���
    public string GetFormattedPlayTime(float time)
    {
        int hours = (int)(time / 3600);
        int minutes = (int)((time % 3600) / 60);
        int seconds = (int)(time % 60);

        return $"{hours:D2}�� {minutes:D2}�� {seconds:D2}��";
    }
}
