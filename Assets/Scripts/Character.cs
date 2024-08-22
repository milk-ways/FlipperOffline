using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Fusion;

public abstract class Character : NetworkBehaviour, IPlayerJoined
{
    [SerializeField]
    protected readonly float speed = 10f;
    
    public VariableJoystick joystick;   
    public Button actionButton;

    protected Rigidbody rigid;
    protected Animator animator;

    private float cooltime;
    protected float maxCooltime = 5f;

    public string description = "";

    [Networked]
    public Team team { get; set; } = 0;

    protected bool isGround = false;

    public ParticleSystem abilityEffect;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        animator = transform.GetChild(0).GetComponent<Animator>();

        AwakeOnChild();
    }
    protected virtual void AwakeOnChild() { }

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
        rigid.velocity = Vector3.zero;
    }

    public void AssignUI()
    {
        joystick = InputManager.Instance.joystick;
        actionButton = InputManager.Instance.Button;
        if (actionButton != null)
        {
            actionButton.onClick.AddListener(Ability);
        }

        actionButton.transform.GetChild(0).GetChild(NetworkRunnerHandler.Instance.SelectedPlayer).gameObject.SetActive(true);
        Camera.main.GetComponent<CameraController>().Target = gameObject;
        Camera.main.GetComponent<CameraController>().SetCameraBoundary();
    }

    protected virtual void Move()
    {
        if (!isGround)
            return;

        float x = team == 0 ? joystick.Horizontal : -joystick.Horizontal;
        float z = team == 0 ? joystick.Vertical : -joystick.Vertical;

        Vector3 moveVec = new Vector3(x, 0, z) * speed * Time.deltaTime;

        transform.Translate(moveVec);

        if (!(x == 0 && z == 0))
        {
            transform.GetChild(0).rotation = Quaternion.LookRotation(new Vector3(x, 0, z));
        }

        if (moveVec.sqrMagnitude == 0)
            return;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RpcAddForce(Vector3 dir)
    {
        rigid.AddForce(dir, ForceMode.Impulse);
    }

    protected abstract void CharacterAction();
    protected virtual void Ability()
    {
        CharacterAction();
        StartCoroutine(CoolTime());
    }

    public IEnumerator CoolTime()
    {
        actionButton.enabled = false;   
        cooltime = 0f;

        while (cooltime < maxCooltime)
        {
            cooltime += Time.deltaTime;
            actionButton.GetComponent<Image>().fillAmount = cooltime / maxCooltime;
            yield return null;
        }

        actionButton.GetComponent<Image>().fillAmount = 1;
        actionButton.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var pan = other.gameObject.GetComponent<Pan>();

        if (pan != null)
        {
            if ((pan.isFlipped && team == Team.red) || (!pan.isFlipped && team == Team.blue))
            {
                return;
            }
            else
            {
                pan.Flip();
            }
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
            team = (Team)(player.PlayerId % 2);
            AssignUI();
        }
    }
}

public enum Team
{
    blue = 0,
    red = 1
};