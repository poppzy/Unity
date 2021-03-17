using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpCommand : ICommand
{
    private Transform player;
    private float speed;

    public MoveUpCommand(Transform player, float speed)
    {
        this.player = player;
        this.speed = speed;
    }
    public void Execute()
    {
        player.Translate(Vector3.up * speed * Time.deltaTime);
    }

    public void Undo()
    {
        player.Translate(Vector3.down * speed * Time.deltaTime);
    }
}
