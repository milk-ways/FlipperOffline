using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCharacter : Character
{
    private float attackPower = 10f;

    protected override void CharacterAction()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, 3f);

        foreach (var coll in colls)
        {
            if (!coll.gameObject.CompareTag("Player"))
                continue;

            Vector3 dir = (coll.gameObject.transform.position - transform.position).normalized * attackPower; // 값은 characater 마다 다르게

            Debug.Log(coll.gameObject.name);
            coll.GetComponent<Rigidbody>().AddForce(dir, ForceMode.Impulse);
        }
    }
}
