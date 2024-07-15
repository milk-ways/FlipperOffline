using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Animation anim;
    private bool move;
    private float moveTime = 2.0f;

    public VariableJoystick joy;

    private void Awake()
    {
        anim = GetComponent<Animation>();
    }

    private void FixedUpdate()
    {
        float x = joy.Horizontal;
        float z = joy.Vertical;

        Vector3 dir = new Vector3(x, 0, z);

        if (!move)
        {
            StartCoroutine(Move(dir));
        }
    }

    public IEnumerator Move(Vector3 dir)
    {
        move = true;

        float elapsedTime = 0.0f;

        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = currentPosition + dir * 1.5f;

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(currentPosition, targetPosition, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        move = false;
    }
}
