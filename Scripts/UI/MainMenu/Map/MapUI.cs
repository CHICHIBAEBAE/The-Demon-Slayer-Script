using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : UIBase
{
    public MapController mapController;
    public Camera mapCamera;

    private void Awake()
    {
        //mapController = GetComponent<MapController>();
        mapCamera = GameObject.FindWithTag("MapCamera").GetComponent<Camera>();
    }

    private void OnEnable()
    {
        // �̴ϸ� ���� ���� �ѱ�
    }

    private void OnDisable()
    {
        // ���� ���� �̴ϸ� �ѱ�
    }
}
