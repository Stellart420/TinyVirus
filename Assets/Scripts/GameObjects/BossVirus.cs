using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossVirus : Virus
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] int collideMaxCount = 3;
    [SerializeField] GameObject destroyedEffect;

    int health;
    GameWindow gameWindow;
    public void Init()
    {
        base.Init();
        health = maxHealth;
    }

    private void Start()
    {
        gameWindow = UIController.Instance.Get<GameWindow>() as GameWindow;
        gameWindow.SetMaxHealth(maxHealth);
    }
    public void Damaged()
    {
        health--;
        gameWindow.ChangeHealth(health);
        if (health == 0)
        {
            Instantiate(destroyedEffect, transform.position, Quaternion.identity);
            LevelController.instance.CurrentLevel.Viruses.Remove(this);
            Destroy(gameObject);
            GameController.Instance.Win();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameController.Instance.GameState != GameState.Play)
            return;

        if (collision.tag == "Virus" && type == VirusType.Boss10)
        {
            collideMaxCount--;
            Instantiate(currentEffect, transform.position, Quaternion.identity);
            if (collideMaxCount == 0)
            {
                avatar.transform.LeanScale(new Vector2(7f, 7f), 1f).setOnComplete(() => GameController.Instance.GameOver());
            }
            else
            {
                avatar.transform.LeanScale(new Vector2(avatar.transform.localScale.x + 1, avatar.transform.localScale.y + 1), 0.5f);
                
            }

            if (collision.tag == "Virus")
            {
                var vir = collision.GetComponent<Virus>();
                vir.Destroyed();
            }


        }
    }
}
