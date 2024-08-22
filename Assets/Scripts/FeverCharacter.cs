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
        StartCoroutine(AcitvateTime(5f));
    }

    protected override void Move()
    {
        if (!isGround)
            return;

        float x = team == 0 ? joystick.Horizontal : -joystick.Horizontal;
        float z = team == 0 ? joystick.Vertical : -joystick.Vertical;

        Vector3 moveVec = new Vector3(x, 0, z) * speed * (isActivated ? ActivatedSpeedRate : 1f) * Time.deltaTime;

        transform.Translate(moveVec);

        if (!(x == 0 && z == 0))
        {
            transform.GetChild(0).rotation = Quaternion.LookRotation(new Vector3(x, 0, z));
        }

        if (moveVec.sqrMagnitude == 0)
            return;
    }

    public IEnumerator AcitvateTime(float maxTime)
    {
        isActivated = true;
        float activateTime = 0f;
        while (activateTime < maxTime)
        {
            activateTime += Time.deltaTime;
            yield return null;
        }
        isActivated = false;
    }

    protected override void RpcSyncAction()
    {
        
    }
}
