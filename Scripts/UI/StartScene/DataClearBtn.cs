using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataClearBtn : MonoBehaviour
{
    public SelectDataPanel selectDataPanel;

    public void ClearData(int idx)
    {
        DataManager.Instance.SelectClearData((ESaveFile)idx);
        // ������ ���� �ĳ� UI ������Ʈ
        selectDataPanel.UpdateData();
    }
}
