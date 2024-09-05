using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Unity.VisualScripting;

public class FeverCharacter : Character
{
    [Networked]
    private bool isActivated { get; set; } = false;

    [SerializeField]
    private float ActivatedSpeedRate = 2f;
    [SerializeField]
    private float duration = 4f;

    protected override void CharacterAction()
    {
        RpcSyncAction();
        StartCoroutine(AcitvateTime(duration));
    }

    protected override void Move(Vector3 dir)
    {
        if (!isGround)
            return;

        Vector3 moveVec = dir * (team == Team.blue ? 1 : -1) * speed * (isActivated ? ActivatedSpeedRate : 1f) * Runner.DeltaTime;

        if (moveVec.sqrMagnitude < 0.001f) return;

        transform.Translate(moveVec);
        RpcSyncRot(moveVec);
    }

    public IEnumerator AcitvateTime(float maxTime)
    {
        isActivated = true;
        RpcSyncParticle(true);
        float activateTime = 0f;
        while (activateTime < maxTime)
        {
            activateTime += Time.deltaTime;
            yield return null;
        }
        RpcSyncParticle(false);
        isActivated = false;
    }

    [Rpc]
    protected void RpcSyncAction()
    {
        SoundManager.PlayEffect("fever");
    }

    [Rpc]
    public void RpcSyncParticle(bool value)
    {
        if(value)
        {
            abilityEffect.Play();
        }
        else
        {
            abilityEffect.Stop();
        }
    }
}
