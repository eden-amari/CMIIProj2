using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    //public List<TreasureBox> collectedTreasureBoxes = new List<TreasureBox>();
    public List<Weapon> collectedWeapons = new List<Weapon>(); // List to store weapons
    public Weapon currentWeapon; // Track the currently equipped weapon
    public int lives = 3; // Player lives

    private int currentWeaponIndex = -1; // Track the index of the currently equipped weapon

    private void Update()
    {
        HandleWeaponSwitch();
    }

    //public void CollectTreasureBox(TreasureBox treasureBox)
    //{
    //    //collectedTreasureBoxes.Add(treasureBox);
    //    //Debug.Log("Collected treasure box: " + treasureBox.name);

    //    // Check win condition
    //    if (collectedTreasureBoxes.Count == 3)
    //    {
    //        Debug.Log("Player wins! All treasure boxes collected.");
    //        // Implement win logic here
    //    }
    //}

    public void AddWeapon(Weapon weapon)
    {
        if (weapon == null)
        {
            Debug.LogError("Attempted to add a null weapon to the inventory!");
            return; // Exit if the weapon is null
        }

        collectedWeapons.Add(weapon);
        Debug.Log("Weapon added: " + weapon.name);
        // Automatically equip the weapon if it's the first one added
        if (collectedWeapons.Count == 1)
        {
            EquipWeapon(weapon);
        }
    }

    public void EquipWeapon(Weapon weapon)
    {
        // Deactivate all weapons first
        foreach (Weapon w in collectedWeapons)
        {
            w.gameObject.SetActive(false); // Hide all weapons
        }

        currentWeapon = weapon; // Set the new weapon as the current weapon
        currentWeapon.gameObject.SetActive(true); // Activate the current weapon
        currentWeapon.transform.SetParent(transform); // Attach it to the player

        Debug.Log("Equipped weapon: " + weapon.name);
    }

    public void RemoveCurrentWeapon()
    {
        if (currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(false); // Deactivate the weapon
            currentWeapon = null; // Set the current weapon to null
            Debug.Log("Weapon removed from inventory.");
        }
    }

    private void HandleWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWeapon(-1); // Switch to previous weapon
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchWeapon(1); // Switch to next weapon
        }
    }

    private void SwitchWeapon(int direction)
    {
        if (collectedWeapons.Count == 0) return; // No weapons to switch

        currentWeaponIndex += direction;

        // Wrap around if the index goes out of bounds
        if (currentWeaponIndex < 0)
        {
            currentWeaponIndex = collectedWeapons.Count - 1; // Go to last weapon
        }
        else if (currentWeaponIndex >= collectedWeapons.Count)
        {
            currentWeaponIndex = 0; // Go to first weapon
        }

        EquipWeapon(collectedWeapons[currentWeaponIndex]);
    }

    public void LoseLife()
    {
        lives--;
        if (lives <= 0)
        {
            Debug.Log("Game Over!");
            // Implement game over logic here
        }
    }
}


//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerInventory : MonoBehaviour
//{
//    public List<TreasureBox> collectedTreasureBoxes = new List<TreasureBox>();
//    public List<Weapon> collectedWeapons = new List<Weapon>(); // List to store weapons
//    public Weapon currentWeapon; // Track the currently equipped weapon
//    public int lives = 3; // Player lives

//    private int currentWeaponIndex = -1; // Track the index of the currently equipped weapon

//    private void Update()
//    {
//        HandleWeaponSwitch();
//    }

//    public void CollectTreasureBox(TreasureBox treasureBox)
//    {
//        collectedTreasureBoxes.Add(treasureBox);
//        Debug.Log("Collected treasure box: " + treasureBox.name);

//        // Check win condition
//        if (collectedTreasureBoxes.Count == 3)
//        {
//            Debug.Log("Player wins! All treasure boxes collected.");
//            // Implement win logic here
//        }
//    }

//    public void AddWeapon(Weapon weapon)
//    {
//        if (weapon == null)
//        {
//            Debug.LogError("Attempted to add a null weapon to the inventory!");
//            return; // Exit if the weapon is null
//        }

//        collectedWeapons.Add(weapon);
//        Debug.Log("Weapon added: " + weapon.name);
//        // Automatically equip the weapon if it's the first one added
//        if (collectedWeapons.Count == 1)
//        {
//            EquipWeapon(weapon);
//        }
//    }

//    public void EquipWeapon(Weapon weapon)
//    {
//        // Deactivate all weapons first
//        foreach (Weapon w in collectedWeapons)
//        {
//            w.gameObject.SetActive(false); // Hide all weapons
//        }

//        currentWeapon = weapon; // Set the new weapon as the current weapon
//        currentWeapon.gameObject.SetActive(true); // Activate the current weapon
//        currentWeapon.transform.SetParent(transform); // Attach it to the player

//        Debug.Log("Equipped weapon: " + weapon.name);
//    }

//    private void HandleWeaponSwitch()
//    {
//        if (Input.GetKeyDown(KeyCode.Q))
//        {
//            SwitchWeapon(-1); // Switch to previous weapon
//        }
//        else if (Input.GetKeyDown(KeyCode.E))
//        {
//            SwitchWeapon(1); // Switch to next weapon
//        }
//    }

//    private void SwitchWeapon(int direction)
//    {
//        if (collectedWeapons.Count == 0) return; // No weapons to switch

//        currentWeaponIndex += direction;

//        // Wrap around if the index goes out of bounds
//        if (currentWeaponIndex < 0)
//        {
//            currentWeaponIndex = collectedWeapons.Count - 1; // Go to last weapon
//        }
//        else if (currentWeaponIndex >= collectedWeapons.Count)
//        {
//            currentWeaponIndex = 0; // Go to first weapon
//        }

//        EquipWeapon(collectedWeapons[currentWeaponIndex]);
//    }

//    public void LoseLife()
//    {
//        lives--;
//        if (lives <= 0)
//        {
//            Debug.Log("Game Over!");
//            // Implement game over logic here
//        }
//    }
//}
