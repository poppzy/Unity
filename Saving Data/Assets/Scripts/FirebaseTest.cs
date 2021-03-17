using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Database;
using System.Collections;
using Firebase.Auth;

using UnityEngine.UI;

public class FirebaseTest : MonoBehaviour
{

    public string email;
    public string password;

    public InputField emailIn;
    public InputField passwordIn;

    void StartSignIn(string email, string password, bool registerNewUser)
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError(task.Exception);
            }

            if (registerNewUser)
            {
/*                StartCoroutine(RegUser(email, password));*/
            }
            else
            {
                /*StartCoroutine(SignIn(email, password));*/
            }


            //StartCoroutine(RegUser("test@test.test", "password1"));

            //StartCoroutine(SignIn("test@test.test", "password1"));
        });
    }

    public void OnClickRegisterNewUser()
    {
        StartSignIn(email, password, true);
    }

    public void OnClickSignIn()
    {
        StartSignIn(email, password, false);
    }

    public void ChangeEmail(string inputEmail)
    {
        email = emailIn.text;
    }

    public void ChangePassword(string inputPassword)
    {
        password = passwordIn.text;
    }

    /*private IEnumerator RegUser(string email, string password)
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
    }*/


    private IEnumerator SignIn(string email, string password)
    {
        Debug.Log("Attempting to log in");
        var auth = FirebaseAuth.DefaultInstance;
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);

        //Show loading animation

        yield return new WaitUntil(() => loginTask.IsCompleted);

        //remove loading animation

        if (loginTask.Exception != null)
        {
            Debug.LogWarning(loginTask.Exception);
            FirebaseException error = (FirebaseException)(loginTask.Exception.InnerException.InnerException);
            Debug.Log(error.ErrorCode);
            Debug.LogWarning(loginTask.Exception);
            if (error.ErrorCode == 14)
            {
                Debug.Log("Account does not exist");
            }
        }
        else
            Debug.Log("login completed");

        //StartCoroutine(DataTest(FirebaseAuth.DefaultInstance.CurrentUser.UserId, "TestWrite1"));
    }

    private IEnumerator DataTest(string userID, string data)
    {
        Debug.Log("Trying to write data");
        var db = FirebaseDatabase.DefaultInstance;
        var dataTask = db.RootReference.Child("users").Child(userID).SetValueAsync(data);

        yield return new WaitUntil(() => dataTask.IsCompleted);

        if (dataTask.Exception != null)
            Debug.LogWarning(dataTask.Exception);
        else
            Debug.Log("DataTestWrite: Complete");
    }
}