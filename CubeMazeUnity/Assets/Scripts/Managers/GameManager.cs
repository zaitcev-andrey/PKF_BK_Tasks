using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Заполняем в инспекторе
    public GameObject[] ForwardTriggers = new GameObject[5];
    public GameObject[] BackTriggers = new GameObject[5];

    void Start()
    {
        for (int i = 0; i < ForwardTriggers.Length; i++)
        {
            ForwardTriggers[i].SetActive(false);
            BackTriggers[i].SetActive(false);
        }
        ForwardTriggers[0].SetActive(true);
    }
}
