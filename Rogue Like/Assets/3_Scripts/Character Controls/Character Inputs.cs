using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CharacterInputs : MonoBehaviour
{
    [Header("---------- INPUTS ----------")]
    public InputActionReference action_Crouch;
    public InputActionReference action_Interact;

    [Header("---------- States ----------")]
    public GameObject characterUP;
    public GameObject characterCrouched;
    public DistanceJoint2D joint;

    [Header("---------- Stats ----------")]
    public int hp = 2;

    private bool isInTaskZone;
    private bool isInHoldZone;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Task"))
        {
            isInTaskZone = true;

            // get task game object

            Debug.Log("is in task zone");
        }
        else if (collision.CompareTag("HoldObject"))
        {
            isInHoldZone = true;
            Debug.Log("is in hold zone");
        }


        else if (collision.CompareTag("KillZone"))
        {
            // Kill
            Debug.Log("is in kill zone");
            hp = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Task"))
        {
            isInTaskZone = false;
            // Zoom in and interact
        }
        else if (collision.CompareTag("HoldObject"))
        {
            isInHoldZone = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Interact
        if (action_Interact.action.WasPressedThisFrame())
        {
            Interact();
        }

        // Crouch
        if (action_Crouch.action.WasPressedThisFrame())
        {
            Crouch(true);
        }
        else if (action_Crouch.action.WasReleasedThisFrame())
        {
            Crouch(false);
        }

        // Death
        if (hp <= 0)
        {
            Death();
        }

        // Zoom
        ZoomCamera();
    }


    void Interact()
    {
        Debug.Log("interacting");

        if (isInTaskZone)
        {
            // play task

        }
        if (isInHoldZone)
        {
            // Hold object
        }
    }

    void Crouch(bool isCrouching)
    {
        Debug.Log("Crouching");

        if (isCrouching)
        {
            characterUP.SetActive(false);
            characterCrouched.SetActive(true);
            characterCrouched.transform.position = new Vector3(characterUP.transform.position.x, characterUP.transform.position.y - 1, characterUP.transform.position.z);
            joint.connectedBody = characterCrouched.GetComponent<Rigidbody2D>();
        }
        else
        {
            characterCrouched.SetActive(false);
            characterUP.SetActive(true);
            characterUP.transform.position = new Vector3(characterCrouched.transform.position.x, characterCrouched.transform.position.y + 1, characterCrouched.transform.position.z); ;
            joint.connectedBody = characterUP.GetComponent<Rigidbody2D>();
        }
        // switch to state crouch
    }

    void Death()
    {
        Debug.Log("Death");
        // Reinitialize game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void ZoomCamera()
    {
        Debug.Log("Zooming");

        if (isInTaskZone || isInHoldZone)
        {
            // zoom camera on GameObject
        }
    }
}
