using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CharacterInputs : MonoBehaviour
{
    [Header("---------- Scripts ----------")]
    public NewController2D newController2D;

    [Header("---------- Inputs ----------")]
    public InputActionReference action_Crouch;
    public InputActionReference action_Interact;
    public InputActionReference action_Jump;
    public InputActionReference action_Move;
    public InputActionReference action_Run;

    [Header("---------- Scenes ----------")]
    public List<string> scenes;

    [Header("---------- States ----------")]
    public GameObject characterUP;
    public GameObject characterCrouched;
    public DistanceJoint2D joint;
    public bool isCrouching;

    [Header("---------- Camera States ----------")]
    public CinemachinePositionComposer cameraPositionComposer;
    public CinemachineCamera c_Camera;

    public Vector3 Base_CameraPos;
    public float Base_CameraZoom;
    public float Base_CameraTimeToSwitch;
    public AnimationCurve Base_CameraSwitchCurve;

    public Vector3 Crouching_CameraPos;
    public float Crouching_CameraZoom;
    public float Crouching_CameraTimeToSwitch;
    public AnimationCurve Crouching_CameraSwitchCurve;

    public Vector3 Falling_CameraPos;
    public float Falling_CameraZoom;
    public float Falling_CameraTimeToSwitch;
    public AnimationCurve Falling_CameraSwitchCurve;

    public Vector3 Running_CameraPos;
    public float Running_CameraZoom;
    public float Running_CameraTimeToSwitch;
    public AnimationCurve Running_CameraSwitchCurve;

    // Private
    private Vector3 initialOffset;
    private float initialOrthographicSize;

    private float timerCameraFall;
    private float timerCameraBase;
    private float timerCameraCrouch;
    private float timerCameraRunning;


    [Header("---------- Stats ----------")]
    public int hp = 2;

    private bool isInTaskZone;
    private bool isInHoldZone;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialOffset = cameraPositionComposer.TargetOffset;
        initialOrthographicSize = c_Camera.Lens.OrthographicSize;

        // Initialize Camera Values
        cameraPositionComposer.TargetOffset = Base_CameraPos;
        c_Camera.Lens.OrthographicSize = Base_CameraZoom;
    }

    void OnEnable()
    {
        timerCameraBase = 0;
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
        else if (collision.CompareTag("NextRoom"))
        {
            EnterNextRoom();
        }
    }

    // Update is called once per frame
    void Update()
    {
        ChangeCameraStates();

        PlaySounds();

        // Death
        if (hp <= 0)
        {
            Death();
        }

        Inputs();
    }
    
    void Inputs()
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


        // Move

        if (action_Move.action.IsInProgress())
        {
            Debug.Log("move Input pressed");
            newController2D.Move(action_Move.action.ReadValue<Vector2>(), action_Run.action.inProgress);
        }
        else
        {
            newController2D.StopMoving();
        }

        // Jump
        if (action_Jump.action.IsInProgress())
        {
            newController2D.Jump(action_Jump.action.WasPressedThisFrame());
        }

        if (action_Jump.action.WasReleasedThisFrame())
        {
            newController2D.ReleaseJump();
        }

    }

    void PlaySounds()
    {

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
            joint.connectedBody = characterCrouched.GetComponent<Rigidbody2D>();
        }
        else
        {
            characterCrouched.SetActive(false);
            characterUP.SetActive(true);
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

    private void EnterNextRoom()
    {
        int sceneNumber = Random.Range(0, scenes.Count);
        int buildIndex = 0;
        while (scenes[sceneNumber] == SceneManager.GetActiveScene().name || buildIndex < 0)
        {
            sceneNumber = Random.Range(0, scenes.Count);
            buildIndex = SceneUtility.GetBuildIndexByScenePath(scenes[sceneNumber]);
        }
        SceneManager.LoadScene(scenes[sceneNumber]);
    }

    void ChangeCameraStates()
    {
        Debug.Log("Zooming");

        // Falling
        if (newController2D.isFalling && timerCameraFall < Falling_CameraTimeToSwitch)
        {
            // Timers
            if (timerCameraFall <= 0)
            {
                initialOffset = cameraPositionComposer.TargetOffset;
                initialOrthographicSize = c_Camera.Lens.OrthographicSize;
            }

            timerCameraFall += Time.deltaTime;
            timerCameraRunning = 0;
            timerCameraCrouch = 0;
            timerCameraBase = 0;

            float t = Mathf.Clamp01(timerCameraFall / Falling_CameraTimeToSwitch);
            float shapedT = Falling_CameraSwitchCurve.Evaluate(t);

            // Zoom
            c_Camera.Lens.OrthographicSize = Mathf.Lerp(initialOrthographicSize, Falling_CameraZoom, shapedT);

            // Target Offset
            cameraPositionComposer.TargetOffset.x = Mathf.Lerp(initialOffset.x, Falling_CameraPos.x, shapedT);
            cameraPositionComposer.TargetOffset.y = Mathf.Lerp(initialOffset.y, Falling_CameraPos.y, shapedT);
            cameraPositionComposer.TargetOffset.z = Mathf.Lerp(initialOffset.z, Falling_CameraPos.z, shapedT);

            Debug.Log("camera fall");
        }

        // Running
        else if (newController2D.isRunning && timerCameraRunning < Running_CameraTimeToSwitch)
        {
            // Timers
            if (timerCameraRunning <= 0)
            {
                initialOffset = cameraPositionComposer.TargetOffset;
                initialOrthographicSize = c_Camera.Lens.OrthographicSize;
            }

            timerCameraRunning += Time.deltaTime;
            timerCameraFall = 0;
            timerCameraCrouch = 0;
            timerCameraBase = 0;

            float t = Mathf.Clamp01(timerCameraRunning / Running_CameraTimeToSwitch);
            float shapedT = Running_CameraSwitchCurve.Evaluate(t);

            // Zoom
            c_Camera.Lens.OrthographicSize = Mathf.Lerp(initialOrthographicSize, Running_CameraZoom, shapedT);

            // Target Offset
            cameraPositionComposer.TargetOffset.x = Mathf.Lerp(initialOffset.x, Running_CameraPos.x, shapedT);
            cameraPositionComposer.TargetOffset.y = Mathf.Lerp(initialOffset.y, Running_CameraPos.y, shapedT);
            cameraPositionComposer.TargetOffset.z = Mathf.Lerp(initialOffset.z, Running_CameraPos.z, shapedT);

            Debug.Log("camera fall");
        }

        // Crouching
        else if (isCrouching && timerCameraCrouch < Crouching_CameraTimeToSwitch)
        {
            // Timers
            if(timerCameraCrouch <= 0)
            {
                initialOffset = cameraPositionComposer.TargetOffset;
                initialOrthographicSize = c_Camera.Lens.OrthographicSize;
            }

            timerCameraCrouch += Time.deltaTime;
            timerCameraRunning = 0;
            timerCameraFall = 0;
            timerCameraBase = 0;

            float t = Mathf.Clamp01(timerCameraCrouch / Crouching_CameraTimeToSwitch);
            float shapedT = Crouching_CameraSwitchCurve.Evaluate(t);

            // Zoom
            c_Camera.Lens.OrthographicSize = Mathf.Lerp(initialOrthographicSize, Crouching_CameraZoom, shapedT);

            // Target Offset
            cameraPositionComposer.TargetOffset.x = Mathf.Lerp(initialOffset.x, Crouching_CameraPos.x, shapedT);
            cameraPositionComposer.TargetOffset.y = Mathf.Lerp(initialOffset.y, Crouching_CameraPos.y, shapedT);
            cameraPositionComposer.TargetOffset.z = Mathf.Lerp(initialOffset.z, Crouching_CameraPos.z, shapedT);

            Debug.Log("camera fall");
        }

        // Base
        else if (timerCameraBase < Base_CameraTimeToSwitch)
        {
            // Timers
            if (timerCameraBase <= 0)
            {
                initialOffset = cameraPositionComposer.TargetOffset;
                initialOrthographicSize = c_Camera.Lens.OrthographicSize;
            }

            timerCameraBase += Time.deltaTime;
            timerCameraFall = 0;
            timerCameraRunning = 0;
            timerCameraCrouch = 0;

            float t = Mathf.Clamp01(timerCameraBase / Base_CameraTimeToSwitch);
            float shapedT = Base_CameraSwitchCurve.Evaluate(t);

            // Zoom
            c_Camera.Lens.OrthographicSize = Mathf.Lerp(initialOrthographicSize, Base_CameraZoom, shapedT);

            // Target Offset
            cameraPositionComposer.TargetOffset.x = Mathf.Lerp(initialOffset.x, Base_CameraPos.x, shapedT);
            cameraPositionComposer.TargetOffset.y = Mathf.Lerp(initialOffset.y, Base_CameraPos.y, shapedT);
            cameraPositionComposer.TargetOffset.z = Mathf.Lerp(initialOffset.z, Base_CameraPos.z, shapedT);

            Debug.Log("camera fall");
        }

        if (isInTaskZone || isInHoldZone)
        {
            // zoom camera on GameObject
        }

    }
}
