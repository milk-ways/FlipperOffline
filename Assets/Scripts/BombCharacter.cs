using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCharacter : Character
{
    protected override void Action()
    {
        Debug.Log("BOMB!");

        Bomb();
    }

    private void Bomb()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, 3f);

        foreach (var coll in colls)
        {
            if (!coll.gameObject.CompareTag("Pan"))
                continue;

            coll.GetComponent<Pan>().Flip();
        }
    }
}
