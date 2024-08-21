using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeverCharacter : Character
{
    private bool isActivated = false;

    protected override void CharacterAction()
    {
        StartCoroutine(AcitvateTime(5f));
    }

    public IEnumerator AcitvateTime(float maxTime)
    {
        isActivated = true;
        float activateTime = 0f;

        while (activateTime < maxTime)
        {
            activateTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        isActivated = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        var pan = other.gameObject.GetComponent<Pan>();

        if (pan != null)
        {
            if (isActivated)
            {
                if ((pan.isFlipped && team == Team.red) || (!pan.isFlipped && team == Team.blue))
                    return;
                else
                    pan.Flip();
            }
            else
                pan.Flip();
        }
    }
}
