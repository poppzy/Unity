using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bullet;
    public GameObject gun;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject newBullet = Instantiate(bullet, gun.transform.position, gun.transform.rotation);

            newBullet.GetComponent<Rigidbody2D>().velocity = gun.transform.right * 10;
        }
    }
}
