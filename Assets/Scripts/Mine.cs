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

        Vector3 dir = (other.gameObject.transform.position - transform.position).normalized * 5f; // 값은 characater 마다 다르게

        other.GetComponent<Rigidbody>().AddForce(dir, ForceMode.Impulse);

        gameObject.SetActive(false);
    }
}
