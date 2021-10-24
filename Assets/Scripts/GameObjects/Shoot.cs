using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] float speed;

    bool canFlying;
    bool turnFront = true;
    ShootType shootType;

    public void Init(Transform parent, float _speed)
    {
        speed = _speed;
        canFlying = true;
        transform.SetParent(parent);
    }

    private void Update()
    {
        if (GameController.Instance.GameState != GameState.Play && !canFlying)
            return;

        switch (shootType)
        {
            case ShootType.Double:
                {
                    transform.localPosition += Vector3.right * Time.deltaTime * speed;
                    break;
                }
            case ShootType.Triple:
                {
                    transform.localPosition += new Vector3(-1,-1,0) * Time.deltaTime * speed;
                    break;
                }
            default:
                {
                    transform.Translate(Vector3.right * Time.deltaTime * speed);
                    //transform.position += Vector3.left * Time.deltaTime * speed;
                    break;
                }
        }
        
        //transform.position += new Vector3(sp, tripleStage ? 0 : sp, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameController.Instance.GameState != GameState.Play || collision.tag != "Virus")
            return;

        collision.GetComponent<Virus>().Destroyed();
        Destroy(gameObject);
    }
}

public enum ShootType
{
    Once,
    Double,
    Triple,
}
