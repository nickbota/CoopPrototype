using UnityEngine;
using CoopPrototype.Player;
using CoopPrototype.Utils;

namespace CoopPrototype.Environment
{
    public class HazardZone : MonoBehaviour, IInteractable
    {
        [Header("Settings")]
        [SerializeField] private bool destroyOnContact = true;
        [SerializeField] private float eliminationDelay = 0f;
        [SerializeField] private LayerMask playerLayer;

        public void Interact(GameObject interactor)
        {
            if (interactor.IsInLayerMask(playerLayer))
            {
                EliminatePlayer(interactor);
            }
        }

        private void EliminatePlayer(GameObject player)
        {
            if (eliminationDelay > 0f)
            {
                Destroy(player, eliminationDelay);
            }
            else if (destroyOnContact)
            {
                Destroy(player);
            }
        }
    }
}
