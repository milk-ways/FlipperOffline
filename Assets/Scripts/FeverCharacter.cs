using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class FeverCharacter : Character
{
    [Networked]
    private bool isActivated { get; set; } = false;

    [SerializeField]
    private float ActivatedSpeedRate = 2f;

    protected override void CharacterAction()
    {
        RpcSyncAction();
        StartCoroutine(AcitvateTime(5f));
    }

    protected override void Move(Vector3 dir)
    {
        if (!isGround)
            return;

        Vector3 moveVec = dir * (team == Team.blue ? 1 : -1) * speed * (isActivated ? ActivatedSpeedRate : 1f) * Runner.DeltaTime;
        transform.Translate(moveVec);

        transform.GetChild(0).rotation = Quaternion.LookRotation(dir * (team == Team.blue ? 1 : -1));
    }

    public IEnumerator AcitvateTime(float maxTime)
    {
        isActivated = true;
        abilityEffect.Play();
        float activateTime = 0f;
        while (activateTime < maxTime)
        {
            activateTime += Time.deltaTime;
            yield return null;
        }
        abilityEffect.Stop();
        isActivated = false;
    }

    [Rpc]
    protected void RpcSyncAction()
    {
        
    }
}
