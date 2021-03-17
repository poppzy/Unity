using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        Time.timeScale = 1f;

        //create the grid for all objects to move on
        m_Grid = new Vector2[(int)m_GridSize.x, (int)m_GridSize.y];
        CreateGrid(m_GridSize.x, m_GridSize.y);
    }

    private void Start()
    {
        StartCoroutine(SpawnHumans());

        //spawn the first 3 zombies on the player
        for (int i = 0; i < PlayerController.instance.m_PlayerZombies.Count; i++)
        {
            m_PlayerGridLocations.Add(new GridObject(PlayerController.instance.m_PlayerZombies[i], new Vector2(m_GridSize.x / 2, m_GridSize.y / 2 + i)));
        }
    }

    [Header("Grid")]
    public Vector2 m_GridSize; //the size of the grid
    public Vector2 m_GridOffset; //the offset the grid will be created at
    public Vector2[,] m_Grid; //a 2D array of the grid

    [Header("Movement")]
    public float m_MovementUpdate = 0.2f; //the amount time it takes to make a movementupdate 
    public float m_StepSize = 1f; //the amount of steps taken per movementupdate

    [Header("GridLocations")]
    public List<GridObject> m_PlayerGridLocations = new List<GridObject>(); //the player locations on the grid
    public List<GridObject> m_HumanGridLocations = new List<GridObject>(); //the human locations on the grid

    [Header("Human")]
    public GameObject m_HumanPrefab; //the human prefab
    public float m_HumanSpawnDelay = 7f; //the time in seconds it can take for humans to spawn
    public Vector2 m_HumansSpawnedPerCycle = new Vector2(1, 6); //the min and max amount of humans that can spawn per cycle

    //private
    private int wave = 0; //the current wave

    /// <summary>
    /// Create a grid using the a 2D array.
    /// </summary>
    /// <param name="_width"></param>
    /// <param name="_length"></param>
    private void CreateGrid(float _width, float _length)
    {
        for (int y = 0; y < _length; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                m_Grid[x, y] = new Vector2(m_GridOffset.x + x, m_GridOffset.y - y);
            }
        }
    }

    /// <summary>
    /// Get the player position of the x and y value in the grid array.
    /// </summary>
    /// <param name="xVariable">The width of the grid</param>
    /// <param name="yVariable">The length of the grid</param>
    /// <returns></returns>
    public Vector2 GetPlayerGridPosition(int xVariable, int yVariable)
    {
        IDamagable IDamageble = PlayerController.instance.GetComponent<IDamagable>();

        //check if object is trying to go out of bounds
        if (xVariable < 0 || xVariable >= m_GridSize.x || yVariable < 0 || yVariable >= m_GridSize.y)
            if (IDamageble != null)
            {
                //deal damage if the object is damageble, and return the current position
                IDamageble.ChangeHealth(-IDamageble.healthpoints);
                return m_Grid[(int)m_PlayerGridLocations[0].gridLocation.x, (int)m_PlayerGridLocations[0].gridLocation.y];
            }

        //check if the player hit itself, if it did kill the player
        for (int i = 0; i < m_PlayerGridLocations.Count; i++)
        {
            if (i != 0)
                if (m_PlayerGridLocations[0].gridLocation == m_PlayerGridLocations[i].gridLocation)
                    IDamageble.ChangeHealth(-IDamageble.healthpoints);
        }

        //check if you hit a human, if so eat it and grow larger
        for (int i = 0; i < m_HumanGridLocations.Count; i++)
        {
            if (m_PlayerGridLocations[0].gridLocation == m_HumanGridLocations[i].gridLocation)
            {
                //destroy the human you hit, and remove it from the array
                Destroy(m_HumanGridLocations[i].gridObject);
                m_HumanGridLocations.RemoveAt(i);

                //add +1 to your score
                UI_Manager.instance.AddScore(1);

                //spawn a new zombie and set it to the end of the line
                GameObject zombie = Instantiate(PlayerController.instance.m_ZombiePrefab, PlayerController.instance.gameObject.transform);
                PlayerController.instance.m_PlayerZombies.Add(zombie);
                m_PlayerGridLocations.Add(new GridObject(zombie, m_PlayerGridLocations[m_PlayerGridLocations.Count - 1].gridLocation));
            }
        }

        //return and change the new position
        return m_Grid[xVariable, yVariable];
    }

    /// <summary>
    /// Get the human position of the x and y value in the grid array.
    /// </summary>
    /// <param name="xVariable">The width of the grid</param>
    /// <param name="yVariable">The length of the grid</param>
    /// <returns></returns>
    public Vector2 GetHumanGridPosition(int xVariable, int yVariable, int humanID)
    {
        //check if the grid has this human
        if (!m_HumanGridLocations.Contains(m_HumanGridLocations[humanID]))
            return m_Grid[(int)m_HumanGridLocations[humanID].gridLocation.x, (int)m_HumanGridLocations[humanID].gridLocation.y];

        IDamagable IDamageble = m_HumanGridLocations[humanID].gridObject.GetComponent<IDamagable>();

        //check if object is trying to go out of bounds
        if (xVariable < 0 || xVariable >= m_GridSize.x || yVariable < 0 || yVariable >= m_GridSize.y)
            if (IDamageble != null)
            {
                //deal damage if the object is damageble, and return the current position
                IDamageble.ChangeHealth(-IDamageble.healthpoints);
                return m_Grid[(int)m_HumanGridLocations[humanID].gridLocation.x, (int)m_HumanGridLocations[humanID].gridLocation.y];
            }

        //check if you hit a human, if so dont move this movement update
        for (int i = 0; i < m_HumanGridLocations.Count; i++)
        {
            if (m_HumanGridLocations[humanID].gridLocation == m_HumanGridLocations[i].gridLocation)
            {
                return m_Grid[(int)m_HumanGridLocations[humanID].gridLocation.x, (int)m_HumanGridLocations[humanID].gridLocation.y];
            }
        }

        //check if you hit a zombie, if so dont move this movement update
        for (int i = 0; i < m_PlayerGridLocations.Count; i++)
        {
            if (m_HumanGridLocations[humanID].gridLocation == m_PlayerGridLocations[i].gridLocation)
            {
                return m_Grid[(int)m_HumanGridLocations[humanID].gridLocation.x, (int)m_HumanGridLocations[humanID].gridLocation.y];
            }
        }

        //return and change the new position
        return m_Grid[xVariable, yVariable];
    }

    /// <summary>
    /// Spawn a random amount of humans every {m_HumanSpawnDelay} amount of seconds
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnHumans()
    {
        IDamagable playerIDamageble = PlayerController.instance.GetComponent<IDamagable>();

        while (playerIDamageble.isAlive)
        {
            //wait {m_HumanSpawnDelay} second until you spawn humans
            if (wave == 0)
                yield return new WaitForSeconds(m_HumanSpawnDelay);
            else
                yield return new WaitForSeconds(UnityEngine.Random.Range(m_HumanSpawnDelay - 1f, m_HumanSpawnDelay + 1f));

            //random amount of humans spawned
            int random = UnityEngine.Random.Range((int)m_HumansSpawnedPerCycle.x, (int)m_HumansSpawnedPerCycle.y + 1);

            //spawn the humans
            for (int i = 0; i < random; i++)
            {
                GameObject human = Instantiate(m_HumanPrefab);

                int x = UnityEngine.Random.Range(0, (int)m_GridSize.x);
                int y = UnityEngine.Random.Range(0, (int)m_GridSize.y);

                m_HumanGridLocations.Add(new GridObject(human, new Vector2(x, y)));

                human.transform.position = m_Grid[(int)m_HumanGridLocations[m_HumanGridLocations.Count - 1].gridLocation.x, (int)m_HumanGridLocations[m_HumanGridLocations.Count - 1].gridLocation.y];
            }
        }

        wave++;
    }


    [Serializable]
    public struct GridObject
    {
        public GameObject gridObject;
        public Vector2 gridLocation;

        public GridObject(GameObject _gridObject, Vector2 _gridLocation)
        {
            gridObject = _gridObject;
            gridLocation = _gridLocation;
        }
    }
}
