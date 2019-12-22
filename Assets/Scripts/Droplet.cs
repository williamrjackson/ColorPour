using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Droplet : MonoBehaviour
{
    private LiquidBase _color;
    public ParticleSystem particles;
    private ParticleSystem.MainModule _particleMainModule;

    // When color is set, update the sprite and particle system color.
    public LiquidBase color
    {
        set
        {
            _color = value;
            GetComponent<SpriteRenderer>().color = Liquid.BaseToColor(_color);
            if (particles != null)
            {
                _particleMainModule = particles.main;
                _particleMainModule.startColor = Liquid.BaseToColor(_color);
            }
        }
    }

    // As soon as this is enabled, start the countdown for self destruct
    void Start()
    {
        StartCoroutine(ContributeAndDie());
    }

    // Wait a bit for the droplet to fall, then add to the liquid and destroy self
    IEnumerator ContributeAndDie()
    {
        yield return new WaitForSeconds(1.25f);
        Liquid.instance.Add(_color);
        Destroy(gameObject);
    }
}
