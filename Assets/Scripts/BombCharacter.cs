using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCharacter : Character
{
    protected override void CharacterAction()
    {
        Bomb();
    }

    protected override void RpcSyncAction()
    {
        
    }

    private void Bomb()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, 3f);

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
