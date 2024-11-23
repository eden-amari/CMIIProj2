using UnityEngine;

public class FirstPersonMouseControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LockAndHideCursor();
    }

    // Update is called once per frame
    void Update()
    {
        // Optionally, re-lock the cursor every frame in case it gets unlocked
        LockAndHideCursor();
    }

    // Method to lock and hide the cursor
    void LockAndHideCursor()
    {
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;

        // Hide the cursor
        Cursor.visible = false;
    }

    // Optional: You may want to unlock the cursor when the player presses a certain key (like escape)
    void UnlockCursor()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
