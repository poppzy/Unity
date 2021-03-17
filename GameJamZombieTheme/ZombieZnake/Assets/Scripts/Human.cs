using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Human : MonoBehaviour
{
    public enum Direction : int
    {
        Up = 0,
        Down,
        Left,
        Right
    }

    [Header("Human")]
    public Direction m_Faceing; //the direction you are facing

    //private
    private Health healthScript;
    private GridManager grid;

    void Start()
    {
        healthScript = GetComponent<Health>();
        grid = GridManager.instance;
        StartCoroutine(Movement());
    }

    private IEnumerator Movement()
    {
        while (healthScript.isAlive)
        {
            yield return new WaitForSeconds(GridManager.instance.m_MovementUpdate);

            int humanID = 0;

            for (int i = 0; i < grid.m_HumanGridLocations.Count; i++)
            {
                if (gameObject == grid.m_HumanGridLocations[i].gridObject)
                    humanID = i;
            }

            Vector2 desiredPosition = grid.m_HumanGridLocations[humanID].gridLocation;
            Vector2 previousPosition = Vector2.zero;

            int rad = Random.Range(0, 4);

            //move in a random direction
            switch (rad)
            {
                case 0:
                    m_Faceing = Direction.Up;
                    desiredPosition += new Vector2(0, -1);
                    break;
                case 1:
                    m_Faceing = Direction.Down;
                    desiredPosition += new Vector2(0, 1);
                    break;
                case 2:
                    m_Faceing = Direction.Left;
                    desiredPosition += new Vector2(-1, 0);
                    break;
                case 3:
                    m_Faceing = Direction.Right;
                    desiredPosition += new Vector2(1, 0);
                    break;
                default:
                    Debug.LogError($"!ERROR!: Somehow the {gameObject.name} got a random value of {rad}!");
                    break;
            }

            transform.position = grid.GetHumanGridPosition((int)desiredPosition.x, (int)desiredPosition.y, humanID) * GridManager.instance.m_StepSize;
            previousPosition = grid.m_HumanGridLocations[humanID].gridLocation;
            gameObject.GetComponent<Animator>().SetFloat("X", desiredPosition.x - previousPosition.x);
            gameObject.GetComponent<Animator>().SetFloat("Y", desiredPosition.y - previousPosition.y);
            grid.m_HumanGridLocations[humanID] = new GridManager.GridObject(gameObject, new Vector2(desiredPosition.x, desiredPosition.y));
        }
    }
}
