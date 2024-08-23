using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Target;

    private float offsetX = 0.0f;
    private float offsetY = 4.0f;
    private float offsetZ = -6.0f;

    [SerializeField]
    private float cameraSpeed = 10.0f;

    private Vector3 targetPos;
    [SerializeField]
    private Vector3 minCameraBoundary;
    [SerializeField]
    private Vector3 maxCameraBoundary;

    public void SetCameraBoundary()
    {
        if (Target.GetComponent<Character>().team == 0)
        {
            offsetZ = -6.0f;
            transform.rotation = Quaternion.Euler(30f, 0f, 0f);
        }
        else
        {
            offsetZ = 6.0f;
            transform.rotation = Quaternion.Euler(30f, 180f, 0f);
        }


        float height = Camera.main.orthographicSize;
        float width = height * Screen.width / Screen.height;


        minCameraBoundary = new Vector3(
            -width * GameManager.Instance.Col + offsetX,
            offsetY,
            -height * (GameManager.Instance.Row - 1) + offsetZ
            );
        maxCameraBoundary = new Vector3(
            width * GameManager.Instance.Col + offsetX,
            offsetY,
            height * (GameManager.Instance.Row - 1) + offsetZ
            );
    }

    private void FixedUpdate()
    {
        if (Target == null)
            return;

        targetPos = new Vector3(
            Target.transform.position.x + offsetX,
            Target.transform.position.y + offsetY,
            Target.transform.position.z + offsetZ);

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * cameraSpeed);

        float clampX = Mathf.Clamp(transform.position.x, minCameraBoundary.x, maxCameraBoundary.x);
        float clampZ = Mathf.Clamp(transform.position.z, minCameraBoundary.z, maxCameraBoundary.z);

        if (minCameraBoundary != null)
        {
            transform.position = new Vector3(clampX, transform.position.y, clampZ);
        }
    }
}
