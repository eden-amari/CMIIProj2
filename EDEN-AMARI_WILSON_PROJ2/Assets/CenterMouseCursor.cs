using UnityEngine;

public class CenterMouseCursor : MonoBehaviour
{
    void Start()
    {
        // Hide the cursor and lock it in the middle of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;  // Hide the cursor
    }

    void Update()
    {
        // Continuously lock the cursor in the center of the screen
        // Note: Unity automatically locks the cursor in the center when `Cursor.lockState` is set to Locked
        // and hides it with `Cursor.visible = false`.

        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
