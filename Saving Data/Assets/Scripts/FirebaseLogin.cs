using Firebase.Auth;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Extensions;

public class FirebaseLogin : MonoBehaviour
{
    public TextMeshProUGUI outputText;
    public Button playButton;
    public InputField emailIn;
    public InputField passwordIn;

    private void Start()
    {
        //Runs in the first scene of the game
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError(task.Exception);
            }
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && emailIn.isFocused)
        {
            passwordIn.Select();
        }
    }

    //Connected to the dropdown menu, on value change.
    public void Login()
    {
        string user = emailIn.text;
        string password = passwordIn.text;

        if (user == "" && password == "")
        {
            user = "oscar@inet.se";
            password = "password";
        }

        //loggs in a test user
        StartCoroutine(SignIn(user, password));
    }

    public void Register()
    {
        string user = emailIn.text;
        string password = passwordIn.text;

        //loggs in a test user
        StartCoroutine(RegUser(user, password));
    }

    private IEnumerator RegUser(string email, string password)
    {
        Debug.Log("Starting Registration");
        var auth = FirebaseAuth.DefaultInstance;
        var regTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => regTask.IsCompleted);

        if (regTask.Exception != null)
            Debug.LogWarning(regTask.Exception);
        else
            Debug.Log("Registration Complete");

        StartCoroutine(SignIn(email, password));
    }

    private IEnumerator SignIn(string email, string password)
    {
        Debug.Log("Atempting to log in");
        var auth = FirebaseAuth.DefaultInstance;
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);

        //TODO: Show loading animation

        yield return new WaitUntil(() => loginTask.IsCompleted);

        //TODO: remove loading animation

        if (loginTask.Exception != null)
            Debug.LogWarning(loginTask.Exception);
        else
            Debug.Log("login completed");

        //Show the email of our logged in user
        outputText.text = loginTask.Result.Email;

        //Activate the play button once we have logged in
        playButton.interactable = true;
    }
}