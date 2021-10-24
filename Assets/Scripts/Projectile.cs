using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed;

    GameObject[] viruses;
    Vector2 target;

    void Start()
    {
        viruses = GameObject.FindGameObjectsWithTag("Virus");

        int rand = Random.Range(0, viruses.Length);
        target = viruses[rand].transform.position;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position,target) < 0.2f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Virus"))
        {
            GameController.Instance.GameOver();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}


