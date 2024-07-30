using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public GameObject miner;
    private void OnTriggerEnter(Collider other)
    {
        if (miner == null || miner == other.gameObject)
            return;

        Vector3 dir = (other.gameObject.transform.position - transform.position).normalized * 5f; // ���� characater ���� �ٸ���

        other.GetComponent<Rigidbody>().AddForce(dir, ForceMode.Impulse);

        gameObject.SetActive(false);
    }
}
