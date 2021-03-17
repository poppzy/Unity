using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRightCommand : ICommand
{
    private Transform player;
    private float speed;

    public MoveRightCommand(Transform player, float speed)
    {
        this.player = player;
        this.speed = speed;
    }
    public void Execute()
    {
        player.Translate(Vector3.right * speed * Time.deltaTime);
    }

    public void Undo()
    {
        player.Translate(Vector3.left * speed * Time.deltaTime);
    }
}
