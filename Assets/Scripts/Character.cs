using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Fusion;

public class Character : NetworkBehaviour, IPlayerJoined
{
    [SerializeField]
    private float speed;
    
    public VariableJoystick joystick;   
    public Button actionButton;

    private Rigidbody rigid;

    private float attackPower = 10f;
    private float cooltime;
    protected float maxCooltime = 5f;

    [Networked]
    public Team team { get; set; } = 0;

    private bool isGround = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        //if(actionButton != null)
        //    actionButton.onClick.AddListener(Action);
    }

    public override void FixedUpdateNetwork()
    {
        if (transform.position.y <= -1f)
        {
            Revive();
        }

        if(joystick != null)
            Move();
    }

    private void Revive()
    {
        isGround = false;

        float temp = -GameManager.Instance.panManager.blank / 2f;

        float clampX = Mathf.Clamp(transform.position.x, 0.5f + temp, GameManager.Instance.Col * GameManager.Instance.panManager.blank - 0.5f + temp);
        float clampZ = Mathf.Clamp(transform.position.z, 0.5f + temp, GameManager.Instance.Row * GameManager.Instance.panManager.blank - 0.5f + temp);
        
        transform.position = new Vector3(clampX, 4f, clampZ);
    }

    public void AssignUI(VariableJoystick joy, Button btn)
    {
        if (Runner.GetPlayerObject(Runner.LocalPlayer) != this) return;

        Debug.Log("Assign");
    }

    public void AssignUI()
    {
        joystick = FindObjectOfType<VariableJoystick>();
        Camera.main.GetComponent<CameraController>().Target = gameObject;
        Camera.main.GetComponent<CameraController>().SetCameraBoundary();
    }

    private void Move()
    {
        if (!isGround)
            return;

        float x = team == 0 ? joystick.Horizontal : -joystick.Horizontal;
        float z = team == 0 ? joystick.Vertical : -joystick.Vertical;

        Vector3 moveVec = new Vector3(x, 0, z) * speed * Time.deltaTime;

        transform.Translate(moveVec);

        if (moveVec.sqrMagnitude == 0)
            return;
    }

    protected virtual void Action()
    {
        Debug.Log("ACTION");

        Attack();

        StartCoroutine(CoolTime());
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

    public IEnumerator CoolTime()
    {
        actionButton.enabled = false;
        cooltime = 0f;

        while (cooltime < maxCooltime)
        {
            cooltime += Time.deltaTime;
            actionButton.GetComponent<Image>().fillAmount = cooltime / maxCooltime;
            yield return new WaitForFixedUpdate();
        }

        actionButton.GetComponent<Image>().fillAmount = 1;
        actionButton.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var pan = other.gameObject.GetComponent<Pan>();

        if (pan != null)
        {
            pan.Flip();
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    }

    public void PlayerJoined(PlayerRef player)
    {
        if(Runner.LocalPlayer == player)
        {
            AssignUI();
            team = (Team)(player.PlayerId % 2);
        }
    }
}

public enum Team
{
    blue = 0,
    red = 1
};