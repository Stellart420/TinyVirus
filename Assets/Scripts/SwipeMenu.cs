using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeMenu : MonoBehaviour
{
    [SerializeField] Scrollbar scrollbar;
    [SerializeField] bool vertical = false;
    float scrollPos = 0;
    float[] pos;
    
    private void Update()
    {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }

        if (Input.GetMouseButton(0) || Input.touchCount > 0)
        {
            scrollPos = 1 - scrollbar.value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
                {
                    scrollbar.value = Mathf.Lerp(scrollbar.value, vertical ? 1 - pos[i] : pos[i], 0.1f);
                }
            }
        }

        int select = 0;
        List<int> chekers = new List<int>(); 

        for (int i = 0; i < pos.Length; i++)
        {
            if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
                chekers.Add(i);
                for (int j = 0; j < pos.Length; j++)
                {
                    if (j != i)
                    {
                        //if (ex_lean != lean)
                        //{
                        //    lean.e
                        //}
                        if (i-1 == j || i+1 == j)
                        {

                            transform.GetChild(j).localScale = Vector2.Lerp(transform.GetChild(j).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                        }
                        else
                        transform.GetChild(j).localScale = Vector2.Lerp(transform.GetChild(j).localScale, new Vector2(0.6f, 0.6f), 0.1f);
                    }
                    else
                    {
                        LeanTween.rotateAround(transform.GetChild(i).GetComponent<LevelButton>().BG.gameObject, Vector3.forward, 1, Time.deltaTime);
                    }
                }
            }
        }
    }
}
