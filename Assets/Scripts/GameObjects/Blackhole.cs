using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blackhole : MonoBehaviour
{
    [SerializeField] SpriteRenderer BG;
    [SerializeField] Transform hole;
    List<Virus> viruses;
    Vector3 startScale = new Vector3(1.5f, 1.5f, 1.5f);
    private void Start()
    {
        Init();
    }

    void Init()
    {
        //viruses = new List<Virus>(LevelController.instance.CurrentLevel.Viruses);
        LeanTween.rotateAround(BG.gameObject, Vector3.forward, 360, 5f).setLoopClamp();
        LeanTween.scale(BG.gameObject, Vector3.one, 2.5f).setLoopPingPong();
    }

    private void Update()
    {
        CheckDistance();
    }

    void CheckDistance()
    {
        for (int i = 0; i < LevelController.instance.CurrentLevel.Viruses.Count; i++)
        {
            var dist = Mathf.Sqrt(Mathf.Pow(LevelController.instance.CurrentLevel.Viruses[i].transform.position.x - transform.position.x, 2) + Mathf.Pow(LevelController.instance.CurrentLevel.Viruses[i].transform.position.y - transform.position.y, 2));
            var move_towards_script = LevelController.instance.CurrentLevel.Viruses[i].GetComponent<RandomPatrol>();

            if (dist <= 10)
            {
                move_towards_script.SetTarget(hole);
                Debug.Log($"Move:{move_towards_script.name}");
            }
            else
            {
                move_towards_script.SetTarget(null);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Virus")
        {
            collision.GetComponent<Virus>().DestroyedBlackhole();
        }
    }
}
