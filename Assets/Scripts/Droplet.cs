using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droplet : MonoBehaviour
{
    public LiquidBase color = LiquidBase.Blue;
    void Start()
    {
        StartCoroutine(ContributeAndDie());
    }

    // Update is called once per frame
    IEnumerator ContributeAndDie()
    {
        yield return new WaitForSeconds(.5f);
        Liquid.instance.Add(color);
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }
}
