using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VirusShoot : Virus
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] int collideMaxCount = 3;
    [SerializeField] GameObject destroyedEffect;
    [SerializeField] GameObject shootPrefab;
    [SerializeField] Transform Gun;

    bool canShoot;
    int health;
    GameWindow gameWindow;

    [SerializeField] List<Sprite> StagesSprites;

    public void Init()
    {
        base.Init();
        health = maxHealth;
    }

    private void Start()
    {
        gameWindow = UIController.Instance.Get<GameWindow>() as GameWindow;
        gameWindow.SetMaxHealth(maxHealth);
        StartCoroutine(CheckShoot());
    }

    int checker = -1;
    bool canRecharge = true;
    public void Damaged()
    {
        health--;
        gameWindow.ChangeHealth(health);
        if (health <= maxHealth * 0.35f)
        {
            if (checker == 0 || checker == 1)
            {
                ActivateGun(3);
            }
            checker = 2;
        }
        else if (health > maxHealth * 0.35f && health <= maxHealth * 0.75f)
        {
            if (checker == 0)
            {
                ActivateGun(2);
            }
            checker = 1;
        }
        else
        {
            if (checker == -1)
            {
                ActivateGun(1);
            }
            checker = 0;
        }

        if (health == 0)
        {
            Instantiate(destroyedEffect, transform.position, Quaternion.identity);
            LevelController.instance.CurrentLevel.Viruses.Remove(this);
            Destroy(gameObject);
            GameController.Instance.Win();
        }
    }

    void ActivateGun(int lvl = 1)
    {
        Instantiate(destroyedEffect, transform.position, Quaternion.identity);
        switch (lvl)
        {
            case 1:
                avatar.sprite = StagesSprites[0];
                break;
            case 2:
                avatar.sprite = StagesSprites[1];
                break;
            case 3:
                avatar.sprite = StagesSprites[2];
                break;
        }
    }

    void Shoot(float speed = 4)
    {
        var shoot = Instantiate(shootPrefab, Gun).GetComponent<Shoot>();
        shoot.Init(avatar.transform.parent, speed);
    }

    void DoubleShoot()
    {
        Shoot();
        Shoot();
    }

    void TripleShoot()
    {
        Shoot(4);
        Shoot(4);
        Shoot(4);
    }

    IEnumerator CheckShoot()
    {
        while (true)
        {
            if (GameController.Instance.GameState != GameState.Play || checker == -1 || !canRecharge)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }

            canRecharge = false;

            if (health <= maxHealth * 0.35f)
            {
                LeanTween.color(avatar.gameObject, Color.red, 2f).setOnComplete(()=>
                {
                    TripleShoot();
                    LeanTween.color(avatar.gameObject, Color.white, 0.1f).setOnComplete(() =>
                    {
                        canRecharge = true;
                    });
                });
            }
            else if (health > maxHealth * 0.35f && health <= maxHealth * 0.75f)
            {
                LeanTween.color(avatar.gameObject, Color.red, 3f).setOnComplete(() =>
                {
                    DoubleShoot();
                    
                    LeanTween.color(avatar.gameObject, Color.white, 0.1f).setOnComplete(()=>
                    {
                        canRecharge = true;
                    });
                });
            }
            else
            {
                LeanTween.color(avatar.gameObject, Color.red, 5f).setOnComplete(() =>
                {
                    Shoot();
                    LeanTween.color(avatar.gameObject, Color.white, 0.1f).setOnComplete(() =>
                    {
                        canRecharge = true;
                    });
                });
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameController.Instance.GameState != GameState.Play)
            return;

        if (collision.tag == "Virus" && type == VirusType.Boss20)
        {
            var vir = collision.GetComponent<Virus>();
            if (vir.type == VirusType.Normal)
                GameController.Instance.GameOver();

            vir.Destroyed();
        }
    }
}
