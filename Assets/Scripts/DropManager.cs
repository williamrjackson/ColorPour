using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    public Droplet dropletTemplate;
    void Start()
    {
        // disable template object
        dropletTemplate.gameObject.SetActive(false);
    }

    public void Drop(LiquidBase color, Vector3 worldPos)
    {
        Droplet newDroplet = Instantiate(dropletTemplate);
        newDroplet.name = "Droplet";
        newDroplet.transform.position = worldPos;
        newDroplet.transform.Color(Liquid.BaseToColor(color), 0f);
        newDroplet.color = color;
        newDroplet.gameObject.SetActive(true);
    }
}
