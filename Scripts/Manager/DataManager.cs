using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ESaveFile
{
    save1,
    save2,
    save3,
}

public class DataManager : Singleton<DataManager>
{
    private string baseFilePath;
    // private string filePath;
    private const string save1 = "save1.txt";
    private const string save2 = "save2.txt";
    private const string save3 = "save3.txt";

    private string currentSaveFile = "";

    protected override void Awake()
    {
        base.Awake();
        baseFilePath = Application.persistentDataPath + "/";

        // �ӽ÷� ù���� ����
        currentSaveFile = save1;
    }

    public void SaveData()
    {
        Datas datas = new Datas();

        // �÷��̾� ������ ����
        datas.playerData.healthSystemData = GameManager.Instance.Player.healthSystem.data;
        datas.playerData.soulCount = GameManager.Instance.Player.soulCount;

        // �� ���� ����
        datas.roomManagerData = GameManager.Instance.roomManager.data;

        // ���� Ȱ��ȭ ���� ����
        for (int i = 0; i < GameManager.Instance.roomManager.rooms.Length; i++)
        {
            datas.roomManagerData.rooms[i] = GameManager.Instance.roomManager.rooms[i].data;

        }
        // �÷��� Ÿ�� ����
        datas.playTime = GameManager.Instance.timeManager.playTime;

        datas.isPlayIntro = true;

        string json = JsonUtility.ToJson(datas);  

        File.WriteAllText(baseFilePath + currentSaveFile, json);
    }


    public void LoadData()
    {

        if (File.Exists(baseFilePath + currentSaveFile))
        {
            // �ҷ��� �����Ͱ� �ִ�.

            string json = File.ReadAllText(baseFilePath + currentSaveFile);
            Datas datas = JsonUtility.FromJson<Datas>(json);

            // �÷��̾� ������ �ҷ�����
            GameManager.Instance.Player.healthSystem.data = datas.playerData.healthSystemData;
            GameManager.Instance.Player.soulCount = datas.playerData.soulCount;

            // �� ���� �ҷ�����
            GameManager.Instance.roomManager.data = datas.roomManagerData;

            // ���� Ȱ��ȭ ���� �ҷ�����
            for (int i = 0; i < GameManager.Instance.roomManager.data.rooms.Length; i++)
            {
                GameManager.Instance.roomManager.rooms[i].data = datas.roomManagerData.rooms[i];
            }

            // �÷��� Ÿ�� �ҷ�����
            GameManager.Instance.timeManager.loadTime = datas.playTime;

            // ������ üũ����Ʈ ������ �÷��̾� ��ġ����
            GameManager.Instance.Player.transform.position = GameManager.Instance.roomManager.checkPointPosition;
        }
    }

    public Datas[] LoadAllData()
    {
        Datas[] datas = new Datas[3];

        if (File.Exists(baseFilePath + save1))
        {
            string json = File.ReadAllText(baseFilePath + save1);
            datas[0] = JsonUtility.FromJson<Datas>(json);
        }
        if (File.Exists(baseFilePath + save2))
        {
            string json = File.ReadAllText(baseFilePath + save2);
            datas[1] = JsonUtility.FromJson<Datas>(json);
        }
        if (File.Exists(baseFilePath + save3))
        {
            string json = File.ReadAllText(baseFilePath + save3);
            datas[2] = JsonUtility.FromJson<Datas>(json);
        }

        return datas;
    }

    public void SelectSaveData(ESaveFile saveFile)
    {
        // ���� ��ư�� �ش��ϴ� ���̺� ������ ���� ���̺�� 

        switch (saveFile)
        {
            case ESaveFile.save1:
                currentSaveFile = save1;
                break;
            case ESaveFile.save2:
                currentSaveFile = save2;
                break;
            case ESaveFile.save3:
                currentSaveFile = save3;
                break;
            default:
                Debug.Log("���� �ҷ����� ����");
                break;
        }
    }

    public void SelectClearData(ESaveFile saveFile)
    {
        switch (saveFile)
        {
            case ESaveFile.save1:
                currentSaveFile = save1;
                break;
            case ESaveFile.save2:
                currentSaveFile = save2;
                break;
            case ESaveFile.save3:
                currentSaveFile = save3;
                break;
            default:
                Debug.Log("error");
                break;
        }

        if (File.Exists(baseFilePath + currentSaveFile))
        {
            File.Delete(baseFilePath + currentSaveFile);
        }
    }

}
