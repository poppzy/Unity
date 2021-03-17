using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginButton : MonoBehaviour
{
    public void StartButton()
    {
        ActiveUser.Instance.LoadUserData();
    }
}
