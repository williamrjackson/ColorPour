using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droplet : MonoBehaviour
{
    private LiquidBase _color;
    public LiquidBase color
    {
        set
        {
            _color = value;
            GetComponent<SpriteRenderer>().color = Liquid.BaseToColor(_color);
        }
    }
    public ParticleSystem particles;
    private ParticleSystem.MainModule main;
    void Start()
    {
        main = particles.main;
        StartCoroutine(ContributeAndDie());
    }

    // Update is called once per frame
    IEnumerator ContributeAndDie()
    {
        main.startColor = Liquid.BaseToColor(_color);
        yield return new WaitForSeconds(1.25f);
        Liquid.instance.Add(_color);
        Destroy(gameObject);
    }
}
