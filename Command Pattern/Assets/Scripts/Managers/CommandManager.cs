using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CommandManager : MonoBehaviour
{
    private static CommandManager _instance;
    public static CommandManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("The CommandManager is NULL");
            }
            return _instance;
        }
    }

    private List<ICommand> _commandBuffer = new List<ICommand>();

    private void Awake()
    {
        _instance = this;
    }

    //create a method to "add" commands to the command buffer

    public void AddCommand(ICommand command)
    {
        _commandBuffer.Add(command);
    }


    //create a play routine triggered by a play method that's going to play back all the commands
    //1 second delay
    public void Play()
    {
        StartCoroutine(PlayRoutine());
    }

    IEnumerator PlayRoutine()
    {
        Debug.Log("Playing...");

        foreach (var command in _commandBuffer)
        {
            command.Execute();
            yield return new WaitForSeconds(1.0f);
        }

        Debug.Log("Finished!");
    }

    //create a rewind routine triggered by a rewind method that's going to play in reverse, with a 1 second delay
    public void Rewind()
    {
        StartCoroutine(RewindRoutine());
    }

    IEnumerator RewindRoutine()
    {
        Debug.Log("Rewinding...");
        foreach (var command in Enumerable.Reverse(_commandBuffer))
        {
            command.Undo();
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Finished rewinding!");
    }

    public void Replay()
    {
        StartCoroutine(ReplayRoutine());
    }

    IEnumerator ReplayRoutine()
    {
        Debug.Log("Replaying...");
        foreach (var command in _commandBuffer)
        {
            command.Execute();
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Finished replaying!");
    }

    //Done = Finished with changing colors. Turn them all white
    public void Done()
    {
        var cubes = GameObject.FindGameObjectsWithTag("Cube");

        foreach (var cube in cubes)
        {
            cube.GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }

    //Reset - Clear the command buffer
    public void Reset()
    {
        _commandBuffer.Clear();
    }
}
