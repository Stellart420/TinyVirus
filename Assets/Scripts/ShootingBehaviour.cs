using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBehaviour : MonoBehaviour
{
    [SerializeField] Transform shotPos;
    [SerializeField] GameObject projective;
    [SerializeField] float startTimeBtwShot;

    float timeBtwnShot;

    private void Update()
    {
        if (timeBtwnShot == 0)
        {
            Instantiate(projective, shotPos.position, Quaternion.identity);
            timeBtwnShot = startTimeBtwShot;
        }
        else
        {
            timeBtwnShot -= Time.deltaTime;
        }
    }
}
