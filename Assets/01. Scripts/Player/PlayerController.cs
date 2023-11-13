using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviour
{
    private PhotonView pv;

    private float moveX, moveY;

    public float speed = 3f;

    private Camera cam;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        cam = Camera.main.GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        if (pv.IsMine)
        {
            Move();
            cam.transform.SetParent(this.transform);
        }
    }

    public void Move()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        transform.Translate(new Vector2(moveX, moveY).normalized * speed * Time.deltaTime);

        if (moveX < 0f)
            transform.localScale = new Vector3(-1f, 1f, 1f);
        else if (moveX > 0f)
            transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
