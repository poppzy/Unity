using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWASDMovement : MonoBehaviour
{
    PlayerMovement pm;

    void Start()
    {
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        var move = new Vector3(x, y, 0);

        pm.MovePlayer(move);
    }
}
