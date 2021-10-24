using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Virus : MonoBehaviour
{
    [SerializeField] internal VirusType type;
    
    [SerializeField] GameObject smallEffect;
    [SerializeField] GameObject normalEffect;
    [SerializeField] Sprite small;
    [SerializeField] internal SpriteRenderer avatar;
    public Vector3 Size => avatar.bounds.size;

    [SerializeField] internal bool destroyable;
    [SerializeField] internal bool moveable = true;
    [SerializeField] List<Sprite> normalSprites;
    [SerializeField] Collider2D collider2D;
    public bool IsDestroyable => destroyable;
    public bool CanMove => moveable;

    internal GameObject currentEffect;

    private void Awake()
    {

    }

    public void Init(VirusType virus_type)
    {
        type = virus_type;
        Init();
    }

    public void Init()
    {
        float radius = 1f;
        switch (type)
        {
            case VirusType.Small:
                transform.localScale = new Vector2(0, 0);
                LeanTween.scale(gameObject, new Vector2(1f, 1f), 1f);
                LeanTween.scale(avatar.gameObject, new Vector2(1f, 1f), 1f);
                avatar.sprite = small;
                radius = 1.75f;
                destroyable = true;
                moveable = false;
                LeanTween.rotateAround(avatar.gameObject, Vector3.forward, 360, 2.5f).setLoopClamp();
                currentEffect = smallEffect;
                //gameObject.SetActive(false);
                //Invoke("Activate", 2f);
                break;

            case VirusType.Normal:
                transform.localScale = new Vector2(1f, 1f);
                avatar.transform.localScale = new Vector2(1f, 1f);
                avatar.sprite = normalSprites[Random.Range(0,normalSprites.Count)];
                radius = 2f;
                destroyable = false;
                moveable = true;
                LeanTween.rotateAround(avatar.gameObject, Vector3.forward, 360, 5f).setLoopClamp();
                currentEffect = normalEffect;
                break;
            case VirusType.Boss10:
                radius = 5f;
                destroyable = false;
                moveable = false;
                LeanTween.rotateAround(avatar.gameObject, Vector3.forward, 360, 20f).setLoopClamp();
                currentEffect = normalEffect;
                break;
            case VirusType.Boss20:
                radius = 1.5f;
                destroyable = false;
                moveable = false;
                LeanTween.rotateAround(avatar.gameObject, Vector3.forward, 360, 15).setLoopClamp();
                currentEffect = normalEffect;
                break;
            default: break;
        }
        if (collider2D == null)
        {
            var col = gameObject.AddComponent<CircleCollider2D>();
            col.isTrigger = true;
            col.radius = radius;
        }
    }

    public void Destroyed()
    {
        Instantiate(currentEffect, transform.position, Quaternion.identity);
        LevelController.instance.CurrentLevel.Viruses.Remove(this);
        Destroy(gameObject);
    }

    public void DestroyedBlackhole()
    {
        transform.LeanScale(Vector3.zero, 1f).setOnComplete(()=>
        {
            LevelController.instance.CurrentLevel.Viruses.Remove(this);
            Destroy(gameObject);
            if (type == VirusType.Normal)
                GameController.Instance.GameOver();
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Virus")
        {
            Instantiate(currentEffect, transform.position, Quaternion.identity);
            LevelController.instance.CurrentLevel.Viruses.Remove(this);
            Destroy(gameObject);

            if (collision.GetComponent<Virus>().type == VirusType.Normal)
                GameController.Instance.GameOver();
        }
    }
}

public enum VirusType
{
    Normal,
    Small,
    Boss10,
    Boss20,
}
