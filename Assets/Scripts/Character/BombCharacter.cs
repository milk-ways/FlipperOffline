using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCharacter : Character
{
    [SerializeField] private float BombRange = 6f;
    protected override void CharacterAction()
    {
        RpcSyncAction();
        Bomb();
    }

    [Rpc]
    protected void RpcSyncAction()
    {
        SoundManager.PlayEffect("bomb");
        abilityEffect.Play();
    }

    private void Bomb()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, BombRange);

        foreach (var coll in colls)
        {
            if (!coll.gameObject.CompareTag("Pan"))
                continue;
            var pan = coll.gameObject.GetComponent<Pan>();

            if (pan != null)
            {
                if ((pan.isFlipped && team == Team.red) || (!pan.isFlipped && team == Team.blue))
                {
                    continue;
                }
                else
                {
                    pan.RpcFlip();
                }
            }
        }
    }
}
