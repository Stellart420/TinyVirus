using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomPatrol : MonoBehaviour
{

    [SerializeField] Transform target;

    Vector3 targetPosition;

    internal float minSpeed = 1f;
    internal float maxSpeed = 7f;
    
    internal float speed;
    float secondsToMaxDifficulty = 300f;
    
    private void Start()
    {
        targetPosition = GetRandomPosition();
    }

    internal void Update()
    {
        if (GameController.Instance.GameState != GameState.Play)
            return;
        
        if (target == null)
        {
            if (transform.position != targetPosition)
            {
                speed = Mathf.Lerp(minSpeed, maxSpeed, GetDifficultyPercent());
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            }
            else
            {
                targetPosition = GetRandomPosition();
            }
            return;
        }

        speed = Mathf.Lerp(minSpeed, maxSpeed, GetDifficultyPercent());
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(GameController.Instance.minX, GameController.Instance.maxX);
        float randomY = Random.Range(GameController.Instance.minY, GameController.Instance.maxY);

        return new Vector2(randomX, randomY);
    }

    internal float GetDifficultyPercent()
    {
        return Mathf.Clamp01(Time.timeSinceLevelLoad / secondsToMaxDifficulty);
    }
}
