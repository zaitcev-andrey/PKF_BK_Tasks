using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        FinishManager finishManager = FindObjectOfType<FinishManager>();
        finishManager.FinishScenario();
    }
}
