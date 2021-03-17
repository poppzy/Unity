using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    ICommand moveUp, moveDown, moveLeft, moveRight;

    [SerializeField]
    private float speed = 2.0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            //move up command
            moveUp = new MoveUpCommand(this.transform, speed);
            moveUp.Execute();
            CommandManager.Instance.AddCommand(moveUp);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //move down
            moveDown = new MoveDownCommand(this.transform, speed);
            moveDown.Execute();
            CommandManager.Instance.AddCommand(moveDown);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            //move left
            moveLeft = new MoveLeftCommand(this.transform, speed);
            moveLeft.Execute();
            CommandManager.Instance.AddCommand(moveLeft);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //move right
            moveRight = new MoveRightCommand(this.transform, speed);
            moveRight.Execute();
            CommandManager.Instance.AddCommand(moveRight);
        }
    }
}
