using UnityEngine;

public class ActiveGame : MonoBehaviour
{
    //Singleton
    private static ActiveGame instance;
    public static ActiveGame Instance { get { return instance; } }

    //This game gives easy access to our active game
    public GameInfo activeGameInfo;

    private void Awake()
    {
        //Singleton setup
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    //We are creatig or loading a game
    public void StartGame(GameInfo gameInfo)
    {
        //Update our active game info so we can share that information
        activeGameInfo = gameInfo;

        //loads the next scene
        GetComponent<LevelManager>().LoadScene(2);
    }
}