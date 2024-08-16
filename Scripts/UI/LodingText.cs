using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class LodingText : MonoBehaviour
{
    public TextMeshProUGUI lodingText;
    private string[] tips;

    public float blinkSpeed = 1.0f;

    private WaitForSeconds blinkDurantion = new WaitForSeconds(0.1f);

    public void Start()
    {
        tips = new string[]
        {
            "TIP: ��ŵ�� 'Speac bar'Ű�� \r\n���� �� �� �ֽ��ϴ�.",
            "TIP: �� ������ ������ �ִ� ��������\r\n ���� �մϴ�.",
            "TIP: �нú� �������� ĳ���Ϳ��� \r\n���������� �ɷ�ġ�� �� �� �ֽ��ϴ�.",
            "TIP: ŷ�������� óġ�ϸ� ���� �� �� �ִ� \r\n�������� ����Ѵٰ� �մϴ�."
        };

        SetRandomTip();
    }

    private void Update()
    {
        Color color = lodingText.color;
        color.a = Mathf.PingPong(Time.time * blinkSpeed, 1.0f);
        lodingText.color = color;
    }

    private void SetRandomTip()
    {
        int randomIndex = Random.Range(0, tips.Length);

        lodingText.text = tips[randomIndex];
    }
}
