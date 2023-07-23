using UnityEngine;

public class DeathHoleTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        RestartGameAfterFallingInHole restartGame = FindObjectOfType<RestartGameAfterFallingInHole>();
        restartGame.RestartScenarioAfterFallingInHole();
    }
}
