using UnityEngine;

public class SelectDataPanel : MonoBehaviour
{
    public LoadBtn[] loadBtns;

    TimeManager timeManager = new TimeManager();

    private void Start()
    {
        UpdateData();
    }

    public void UpdateData()
    {
        Datas[] datas = DataManager.Instance.LoadAllData();

        for (int i = 0; i < datas.Length; i++)
        {
            if (datas[i].playTime > 0)
            {
                // ���� ��ġ
                loadBtns[i].saveLocationText.text = datas[i].roomManagerData.lastCheckPointName;

                // �÷���Ÿ�ӵ� �߰�
                string playTimeText = "�÷��� �ð� : " + timeManager.GetFormattedPlayTime(datas[i].playTime);

                loadBtns[i].playTimeText.text = playTimeText;
                // �ҿ�
                string soulCountText = datas[i].playerData.soulCount.ToString();
                loadBtns[i].soulCountText.text = soulCountText;
                loadBtns[i].soulUI.SetActive(true);

            }
            else
            {
                loadBtns[i].newGameText.gameObject.SetActive(true);
                loadBtns[i].saveLocationText.text = "";
                loadBtns[i].playTimeText.text = "";
                loadBtns[i].soulCountText.text = "";
                loadBtns[i].soulUI.SetActive(false);
            }
        }
    }
}
