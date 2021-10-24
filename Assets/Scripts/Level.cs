using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] int index;
    [SerializeField] int winTime;
    [SerializeField] float cameraSize = 15f;
    [SerializeField] bool isTutorial;
    [SerializeField, InspectorName("Префаб Туториала")] GameObject tutorial;
    [SerializeField] LevelType type;
    public int Index => index;
    public int WinTime => winTime;
    public float CameraSize => cameraSize;
    public bool IsTutorial => isTutorial;
    public Tutorial Tutorial => tutorial.GetComponent<Tutorial>();

    public LevelType Type => type;
    List<Virus> viruses;

    public List<Virus> Viruses => viruses;
    private void Awake()
    {
        viruses = new List<Virus>(GetComponentsInChildren<Virus>());
        viruses.ForEach(virus =>
        {
            var boss_virus = virus as BossVirus;
            if (boss_virus != null)
            {
                boss_virus.Init();
            }
            else
            if (virus as VirusShoot)
            {
                ((VirusShoot)virus).Init();
            }
            else
            {
                virus.Init();
            }

        });
    }
}

public enum LevelType
{
    Hold,
    Destroyed,
}
