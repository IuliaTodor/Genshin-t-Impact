using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    private Transform cam;

    [SerializeField] private float speed = 0.5f;
    /// <summary>
    /// Controla si debe invertir la rotaci�n o no
    /// </summary>
    [SerializeField] private bool inverted;

    public Vector2 rotation;
    public bool canRotate;



    private void Awake()
    {
        cam = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Activa la rotaci�n del objeto
    /// </summary>
    /// <returns></returns>
    public IEnumerator Rotate()
    {
        canRotate = true;

        while (canRotate)
        {
            rotation *= speed;

            transform.Rotate(Vector3.up, rotation.x, Space.World);
            transform.Rotate(cam.right, -rotation.y, Space.World);

            yield return null;
        }


    }
}
