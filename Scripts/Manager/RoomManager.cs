using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager
{
    public Room[] rooms;
    public int currentRoomIdx { get => data.currentRoomIdx; set => data.currentRoomIdx = value; }

    public Room currentRoom;
    public Vector3 checkPointPosition { get => data.checkPointPosition; set => data.checkPointPosition = value; }

    public Room lastCheckPointRoom;
    public string lastCheckPointName { get => data.lastCheckPointName; set => data.lastCheckPointName = value; }

    public RoomManagerData data;

    public void Init()
    {
        currentRoomIdx = 0;

        GameObject[] roomObjs = GameObject.FindGameObjectsWithTag("Room");

        // roomObjs �迭�� �̸��� �������� ����
        System.Array.Sort(roomObjs, (x, y) => x.name.CompareTo(y.name));

        rooms = new Room[roomObjs.Length];

        for (int i = 0; i < roomObjs.Length; i++)
        {
            rooms[i] = roomObjs[i].GetComponent<Room>();
        }

        for (int i = 0; i < roomObjs.Length; i++)
        {
            rooms[i].roomIdx = i;
        }

        // data �ʱ�ȭ
        data.rooms = new RoomData[rooms.Length];

        for(int i = 0;i < data.rooms.Length; i++)
        {
            if (rooms[i].bossArray.Length > 0)
            {
                data.rooms[i].isBossAlive = true;
            }
            else
            {
                data.rooms[i].isBossAlive = false;
            }
        }

        InitRespawnCheckPoint();
    }


    // �������� üũ����Ʈ�� ���°�� ù��° üũ����Ʈ�� �ʱ�ȭ
    void InitRespawnCheckPoint()
    {
        for (int i = 0; i < rooms.Length; i++) 
        {
            if(rooms[i].checkPoint != null)
            {
                lastCheckPointRoom = rooms[i];
                checkPointPosition = rooms[i].transform.position;
                break;
            }
        }
    }

}
