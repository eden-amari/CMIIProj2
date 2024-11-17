using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    public Weapon weaponPrefab; // Reference to the weapon prefab

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
            if (playerInventory != null)
            {
                // Instantiate a new weapon from the prefab and add it to the inventory
                Weapon newWeapon = Instantiate(weaponPrefab);
                playerInventory.AddWeapon(newWeapon); // Add the new weapon to the player's inventory

                Destroy(gameObject); // Destroy the pickup object
            }
        }
    }
}
