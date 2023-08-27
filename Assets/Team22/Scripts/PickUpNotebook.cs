using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;

namespace Team22.Scripts
{
    public class PickUpNotebook : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerCharacterController playerCharacterController))
            {
                EventManager.Broadcast(Events.AllObjectivesCompletedEvent);
                Destroy(gameObject);
            }
        }
    }
}
