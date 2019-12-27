﻿using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Droplet : MonoBehaviour
{
    private LiquidBase _color;
    public Rigidbody2D thisRigidBody;

    private SpriteRenderer spriteRen;
    private Coroutine coro;
    private bool _keepUpToDate = false;
    // When color is set, update the sprite and particle system color.
    public LiquidBase color
    {
        set
        {
            _color = value;
            GetComponent<SpriteRenderer>().color = Liquid.BaseToColor(_color);
        }
    }

    void Update()
    {
        if (_keepUpToDate && spriteRen.color != Liquid.instance.CurrentBlendColor)
        {
            spriteRen.color = Liquid.instance.CurrentBlendColor;
        }

        transform.localScale = new Vector3(Random.Range(1f, 1.05f), Random.Range(1f, 1.05f), 1f);

    }

    // As soon as this is enabled, start the countdown for self destruct
    void Start()
    {
        if (spriteRen == null)
        {
            spriteRen = GetComponent<SpriteRenderer>();
        }
        if (thisRigidBody != null)
        {
            thisRigidBody = GetComponent<Rigidbody2D>();
        }
        thisRigidBody.AddForce(Vector2.right * Random.Range(-2f, 2f));
        StartCoroutine(ContributeColor());
    }

    //void OnCollisionEnter2D(Collision2D col)
    //{
    //    if (coro == null && col.collider.GetComponent<Droplet>() == null)
    //        coro = StartCoroutine(ContributeColor());
    //}
 
    // Wait a bit for the droplet to fall, then add to the liquid and destroy self
    IEnumerator ContributeColor()
    {
        yield return new WaitForSeconds(1f);
        Liquid.instance.Add(_color);
        float elapsedTime = 0f;
        float duration = .5f;
        Color initColor = spriteRen.color;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            spriteRen.color = Color.Lerp(initColor, Liquid.instance.CurrentBlendColor, Mathf.InverseLerp(0, duration, elapsedTime));
            yield return new WaitForEndOfFrame();
        }
        _keepUpToDate = true;
    }
}
