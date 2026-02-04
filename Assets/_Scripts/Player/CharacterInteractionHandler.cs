using UnityEngine;

namespace CoopPrototype.Player
{
    public class CharacterInteractionHandler : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            HandleInteraction(other.gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            HandleInteraction(collision.gameObject);
        }

        private void HandleInteraction(GameObject target)
        {
            IInteractable interactable = target.GetComponent<IInteractable>();
            if (interactable != null)
                interactable.Interact(gameObject);
        }
    }
}
