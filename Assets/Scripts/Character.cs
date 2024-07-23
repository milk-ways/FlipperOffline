using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [SerializeField]
    private float speed;
    
    public VariableJoystick joystick;
    public Button actionButton;

    private Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(joystick != null)
            Move();
    }

    private void Move()
    {
        float x = joystick.Horizontal;
        float z = joystick.Vertical;

        Vector3 moveVec = new Vector3(x, 0, z) * speed * Time.deltaTime;
        rigid.MovePosition(rigid.position + moveVec);

        if (moveVec.sqrMagnitude == 0)
            return;
    }

    private void Action()
    {
        Debug.Log("ACTION");
    }

    private void OnTriggerEnter(Collider other)
    {
        var pan = other.gameObject.GetComponent<Pan>();

        if (pan != null)
        {
            pan.Flip();
        }
    }
}
