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

    [Networked]
    public Team team { get; set; } = 0;

    protected bool isGround = false;

    public ParticleSystem abilityEffect;

     public bool isAbilityPressed { get; private set; } = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        animator = transform.GetChild(0).GetComponent<Animator>();

        AwakeOnChild();
    }
    protected virtual void AwakeOnChild() { }

    public override void FixedUpdateNetwork()
    {
        if (transform.position.y <= -2.5f)
        {
            Revive();
        }

        if(GetInput(out CharacterInputData data))
        {
            Move(data.direction);

            if(data.ability)
            {
                Ability();
            }
        }
        isAbilityPressed = false;
    }

    private void Revive()
    {
        isGround = false;

        float temp = -GameManager.Instance.panManager.blank / 2f;

        float clampX = Mathf.Clamp(transform.position.x, 0.5f + temp, GameManager.Instance.Col * GameManager.Instance.panManager.blank - 0.5f + temp);
        float clampZ = Mathf.Clamp(transform.position.z, 0.5f + temp, GameManager.Instance.Row * GameManager.Instance.panManager.blank - 0.5f + temp);
        
        transform.position = new Vector3(clampX, 4f, clampZ);
        rigid.velocity = Vector3.zero;

        rigid.useGravity = false;

        StartCoroutine(Blink(0, true));
    }

    private IEnumerator Blink(int count, bool makeBlink)
    {
        if (makeBlink)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(0.2f);

        if (count < 7)
        {
            StartCoroutine(Blink(count + 1, !makeBlink));
        }
        else
        {
            rigid.useGravity = true;
        }
    }

    public void AssignUI()
    {
        joystick = InputManager.Instance.joystick;
        actionButton = InputManager.Instance.Button;
        actionButton.onClick.AddListener(() =>
        {
            isAbilityPressed = true;
        });

        actionButton.transform.GetChild(0).GetChild(NetworkRunnerHandler.Instance.SelectedPlayer).gameObject.SetActive(true);
        Camera.main.GetComponent<CameraController>().Target = gameObject;
        Camera.main.GetComponent<CameraController>().SetCameraBoundary();
    }

    protected virtual void Move(Vector3 dir)
    {
        if (!isGround)
            return;

        Vector3 moveVec = dir * (team == Team.blue ? 1 : -1) * speed * Runner.DeltaTime;

        if (moveVec.sqrMagnitude < 0.001f) return;

        transform.Translate(moveVec);
        transform.GetChild(0).rotation = Quaternion.LookRotation(dir * (team == Team.blue ? 1 : -1));
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

public struct CharacterInputData : INetworkInput
{
    public Vector3 direction;
    public bool ability;
}
