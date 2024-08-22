using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCharacter : Character
{
    private readonly float NormalizedAttackRange = 3f;

    [SerializeField] private float AttackRange = 1f;
    [SerializeField] private float attackPower = 10f;
    protected override void AwakeOnChild()
    {
        abilityEffect.transform.localScale = new Vector3(AttackRange, 1, AttackRange);
    }

    protected override void CharacterAction()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, NormalizedAttackRange * AttackRange);

        foreach (var coll in colls)
        {
            if (!coll.gameObject.CompareTag("Player"))
                continue;

            Vector3 dir = (coll.gameObject.transform.position - transform.position).normalized * attackPower;   

            Debug.Log(coll.gameObject.name);
            coll.GetComponent<Character>().RpcAddForce(dir);
        }
    }

    [Rpc]
    protected override void RpcSyncAction()
    {
        abilityEffect.Play();
    }
}
