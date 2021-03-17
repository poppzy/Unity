using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Get();
    }

    public void Get(string url = "http://www.google.com")
    {
        var request = (HttpWebRequest)WebRequest.Create(url);
        var response = (HttpWebResponse)request.GetResponse();

        //Open a stream to the server so we can read the response data it sent back from our GET request
        using (var stream = response.GetResponseStream())
        {
            using (var reader = new StreamReader(stream))
            {
                // Read the entire body as a string
                var body = reader.ReadToEnd();

                // Display it in the console
                Debug.Log(body);
            }
        }
    }
}
