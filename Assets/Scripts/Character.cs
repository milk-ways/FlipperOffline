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

    private float attackPower = 10f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        //if(actionButton != null)
        //    actionButton.onClick.AddListener(Action);
    }

    private void FixedUpdate()
    {
        if(joystick != null)
            Move();
    }

    public void AssignUI(VariableJoystick joy, Button btn)
    {
        joystick = joy;
        actionButton = btn;

        actionButton.onClick.AddListener(Action);
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

    protected virtual void Action()
    {
        Debug.Log("ACTION");

        Attack();
    }

    private void Attack()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, 3f);

        foreach (var coll in colls)
        {
            if (!coll.gameObject.CompareTag("Player"))
                continue;

            Vector3 dir = (coll.gameObject.transform.position - transform.position).normalized * attackPower; // 값은 characater 마다 다르게

            coll.GetComponent<Rigidbody>().AddForce(dir, ForceMode.Impulse);
        }
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
