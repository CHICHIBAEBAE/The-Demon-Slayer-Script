using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoBackGroundScroll : MonoBehaviour
{
    #region Inspector

    private Renderer renderer;
    public float speed = 1f;

    #endregion

    private void Awake()
    {
        renderer = gameObject.GetComponent<Renderer>();
    }

    private void Update()
    {
        float move = Time.deltaTime * speed;
        renderer.material.mainTextureOffset += Vector2.right * move;
    }
}