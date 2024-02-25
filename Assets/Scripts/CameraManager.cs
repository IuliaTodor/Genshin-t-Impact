using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager : MonoBehaviour
{
    InputManager inputManager;
    public Transform target;
    /// <summary>
    /// Object camera uses to pivot
    /// </summary>
    public Transform cameraPivot;
    private Transform cameraTransform;
    /// <summary>
    /// Las capas con las que queremos que colisione la cámara
    /// </summary>
    public LayerMask colisionLayers; 
    private float defaultPosition;

    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraVectorPosition;
    /// <summary>
    /// Cantidad en que la cámara se aleja al colisionar
    /// </summary>
    public float cameraColisionOffset = 0.2f;
    public float minimumColisionOffset = 0.2f;
    public float cameraColisionRadius = 0.2f;

    public float cameraFollowSpeed = 0.2f;
    public float cameraLookSpeed = 0.1f;
    public float cameraPivotSpeed = 0.1f;

    /// <summary>
    /// Camara mira arriba-abajo
    /// </summary>
    public float lookAngle;
    /// <summary>
    /// Camara mira derecha-izquierda
    /// </summary>
    public float pivotAngle;

    public float minPivotAngle = -15;
    public float maxPivotAngle = 35;

    public bool isThirdPerson;
    public float thirdPersonPosition;
    public float firstPersonPosition;
    public int zoom;

    public Transform cameraManager;
    public Transform playerContainer;

    private Coroutine corroutine = null;

    private void Awake()
    {
        target = FindObjectOfType<PlayerManager>().transform;
        inputManager = FindObjectOfType<InputManager>();

        cameraManager = transform;
        playerContainer = FindObjectOfType<InputManager>().transform;

        isThirdPerson = true;
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;

    }

    public void HandleCameraMovement()
    {
        //FollowTarget();
        RotateCamera();
        //ChangeCameraMode();
        //HandleCameraColisions();
    }

    private void FollowTarget()
    {
        //SmoothDamp mueve algo de forma smooth entre un punto y otro
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, target.position, ref cameraFollowVelocity, cameraFollowSpeed);
        transform.position = targetPosition;
    }

    private void RotateCamera()
    {
        Vector3 rotation;
        Quaternion targetRotation;

        //Rotación en los ejes vertical y horizontal de la cámara
        lookAngle = lookAngle + (inputManager.cameraInputX * cameraLookSpeed);
        pivotAngle = pivotAngle - (inputManager.cameraInputY * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, minPivotAngle, maxPivotAngle);

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;

        inputManager.cameraInputX = inputManager.cameraInput.x;
        inputManager.cameraInputY = inputManager.cameraInput.y;

        //transform.Rotate(sensibility * inputManager.cameraInputX * Time.deltaTime * Vector3.up);


        //Debug.Log(cameraPivot.localRotation);
    }
    private void ChangeCameraMode()
    {
        if (inputManager.changeCameraInput && corroutine == null)
        {
            corroutine = StartCoroutine(ChangeCamera());
        }

    }

    private IEnumerator ChangeCamera()
    {
        Debug.Log("corutina");

        if (isThirdPerson == true)
        {
            //firstPersonPosition = thirdPersonPosition + zoom;
            //cameraPivot.transform.position = new Vector3(cameraPivot.transform.position.x, cameraPivot.transform.position.y, firstPersonPosition);
            cameraManager.SetParent(playerContainer);
        }

        else
        {
            //    thirdPersonPosition = firstPersonPosition - zoom;
            //    cameraPivot.transform.position = new Vector3(cameraPivot.transform.position.x, cameraPivot.transform.position.y, thirdPersonPosition);
            //}
            cameraManager.SetParent(null);
        }

        yield return new WaitForSeconds(0.5f);


        isThirdPerson = !isThirdPerson;
        corroutine = null;
    }

    /// <summary>
    /// Mueve la cámara si colisiona con algún objeto
    /// </summary>
    private void HandleCameraColisions()
    {
        float targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        //Crea un raycast de radio pequeño alrededor de la cámara en una dirección
        //Si golpea algo esto su información se guarda en el hit
        if (Physics.SphereCast(cameraPivot.transform.position, cameraColisionRadius, direction, out hit, Mathf.Abs(targetPosition), colisionLayers))
        {
            //Distancia entre oivot y lo que golpeamos
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition =- (distance - cameraColisionOffset);
        }

        if(Mathf.Abs(targetPosition) < minimumColisionOffset)
        {
            targetPosition = targetPosition - minimumColisionOffset;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }
}
