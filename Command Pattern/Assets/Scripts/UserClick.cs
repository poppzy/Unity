using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //left click
        if (Input.GetMouseButton(0))
        {
            //cast a ray
            Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(rayOrigin, out hitInfo))
            {
                //detect a cube
                if (hitInfo.collider.tag == "Cube")
                {
                    //assign random color
                    //hitInfo.collider.GetComponent<MeshRenderer>().material.color = new Color(Random.value, Random.value, Random.value);
                    //execute click command
                    ICommand click = new ClickCommand(hitInfo.collider.gameObject, new Color(Random.value, Random.value, Random.value));
                    click.Execute();
                    CommandManager.Instance.AddCommand(click);
                }
            }
        }
    }
}
