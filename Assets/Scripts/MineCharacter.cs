using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineCharacter : Character
{
    public GameObject mine;

    protected override void Action()
    {
        Debug.Log("Mine");

        PlantMine();
    }

    private void PlantMine()
    {
        var temp = Instantiate(mine, transform.position, Quaternion.identity);

        temp.GetComponent<Mine>().miner = gameObject;
    }
}
