using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusController : MonoBehaviour
{
    [SerializeField] Virus virus;
    [SerializeField] float maxTimeForSpawn = 7;

    private void Start()
    {
        if (LevelController.instance.CurrentLevel.Index >= 3 && LevelController.instance.CurrentLevel.Index != 10)
            StartCoroutine(SpawnVirus());
    }

    IEnumerator SpawnVirus()
    {
        while (true)
        {
            var count = Random.Range(1, 4);
            yield return new WaitForSeconds(Random.Range(0f, maxTimeForSpawn));
            if (GameController.Instance.GameState == GameState.Play)
            {
                for (int i = 0; i < count; i++)
                {
                    CreateVirus(VirusType.Small);
                }
            }
        }
    }

    public void CreateVirus(VirusType type)
    {
        var can = true;
        float randomX = Random.Range(GameController.Instance.minX, GameController.Instance.maxX);
        float randomY = Random.Range(GameController.Instance.minY, GameController.Instance.maxY);

        LevelController.instance.CurrentLevel.Viruses.ForEach(v =>
        {
            if (v == null)
                return;

            var dist = Vector3.Distance(v.transform.position, new Vector2(randomX, randomY));
            if (dist < 10f)
            {
                can = false;

            }
        });

        if (!can)
            return;

        var create_virus = Instantiate(virus, new Vector2(randomX, randomY), Quaternion.identity, LevelController.instance.levelContainer.transform);
        create_virus.Init(type);
        LevelController.instance.CurrentLevel.Viruses.Add(create_virus);
    }
}
