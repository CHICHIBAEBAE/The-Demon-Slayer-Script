using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Linq;

public class FastTravelUI : UIBase
{
    public Button buttonPrefab;
    public GameObject buttonGroup;

    private Dictionary<string, Button> buttons = new Dictionary<string, Button>();

    private void Awake()
    {
        InitializeCheckpoint();
    }

    private void OnEnable()
    {
        GameManager.Instance.mapCamera.orthographicSize = 60f;
        UpdateCheckpoint();

        // Ȱ��ȭ �� ù��° ��ư ����
        for(int i = 0; i <buttonGroup.transform.childCount; i++)
        {
            if (buttonGroup.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                buttonGroup.transform.GetChild(i).GetComponent<Button>().Select();
                break;
            }
        }
    }

    private void Start()
    {
    }

    private void Update()
    {        
    }

    private void OnDisable()
    {
        GameManager.Instance.mapCamera.orthographicSize = 25f;

    }

    public void OnClickCheckpointButton(string checkpointName)
    {
        GameManager.Instance.checkpointManager.Teleport(checkpointName);        
        UIManager.Instance.ToggleUI(ref UIManager.Instance.fastTravelUI, 0f, 1f, false, true);
    }

    private void InitializeCheckpoint()
    {
        // ����Ʈ�� �ް� RoomIndex ������� üũ����Ʈ ����

        List<CheckPoint> checkpointList = new List<CheckPoint>(GameManager.Instance.checkpointManager.checkpoints.Values);

        // ���� ����
        for (int i = 0; i < checkpointList.Count - 1; i++)
        {
            for (int j = 0; j < checkpointList.Count - i - 1; j++)
            {
                if (checkpointList[j].checkpointRoom.roomIdx > checkpointList[j + 1].checkpointRoom.roomIdx)
                {
                    CheckPoint tmp = checkpointList[j];
                    checkpointList[j] = checkpointList[j + 1];
                    checkpointList[j + 1] = tmp;
                }
            }
        }

        foreach (var checkpoint in checkpointList)
        {            
            Button button = Instantiate(buttonPrefab, buttonGroup.transform);
            button.transform.SetAsLastSibling();    
            
            button.GetComponentInChildren<TextMeshProUGUI>().text = checkpoint.checkPointName;
            button.onClick.AddListener(() => OnClickCheckpointButton(checkpoint.checkPointName));
            button.gameObject.AddComponent<FastTravelBtn>().Init(this, checkpoint.checkpointIcon.transform.position);
            
            button.gameObject.SetActive(false);
            buttons.Add(checkpoint.checkPointName, button);
        }
    }

    private void UpdateCheckpoint()
    {        
        foreach (var checkpoint in GameManager.Instance.checkpointManager.checkpoints.Values)
        {            
            if (buttons.TryGetValue(checkpoint.checkPointName, out Button button))
            {
                button.gameObject.SetActive(checkpoint.isDiscovered);                
            }
        }
    }

    public void OnCheckpointButton(Vector3 checkpointPosition)
    {
        MoveMapToCheckpoint(checkpointPosition);
    }

    private void MoveMapToCheckpoint(Vector3 checkpointPosition)
    {
        GameManager.Instance.mapCamera.transform.position = checkpointPosition + new Vector3(0, 0, -30f);           
    }
}
