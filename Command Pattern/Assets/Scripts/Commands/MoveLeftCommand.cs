using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftCommand : ICommand
{
    private Transform player;
    private float speed;

    public MoveLeftCommand(Transform player, float speed)
    {
        this.player = player;
        this.speed = speed;
    }

    public void Execute()
    {
        player.Translate(Vector3.left * speed * Time.deltaTime);
    }

    public void Undo()
    {
        player.Translate(Vector3.right * speed * Time.deltaTime);
    }
}
