using UnityEngine;

public class GravityManipulation : MonoBehaviour
{
    [SerializeField] private GameObject[] hologram;
    public Vector3 keyDirection;
    private int currentHologramIndex = -1;

    [SerializeField] private float gravityChange = 9.81f;

    

    void Start()
    {
        // Ensure all holograms are initially inactive
        foreach (var holo in hologram)
        {
            holo.SetActive(false);
        }
    }

    void Update()
    {
        HandleDirection();
    }

    private void HandleDirection()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ActivateHologram(2);
            keyDirection = Vector3.up;
            ChangeGravity(Vector3.up);
            
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ActivateHologram(3);
            keyDirection = Vector3.forward;
            ChangeGravity(Vector3.forward);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ActivateHologram(1);
            keyDirection = Vector3.right;
            ChangeGravity(Vector3.right);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ActivateHologram(0);
            keyDirection = Vector3.left;
            ChangeGravity(Vector3.left);
        }
    }

    private void ActivateHologram(int index)
    {
        if (currentHologramIndex >= 0 && currentHologramIndex < hologram.Length)
        {
            hologram[currentHologramIndex].SetActive(false);
        }

        if (index >= 0 && index < hologram.Length)
        {
            hologram[index].SetActive(true);
            currentHologramIndex = index;
        }
    }

    public void ChangeGravity(Vector3 direction)
    {
        // Update the gravity based on the direction
        Physics.gravity = direction * gravityChange;
    }
}
