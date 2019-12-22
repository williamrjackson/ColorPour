using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    [Tooltip("Droplet prefab for spawning")]
    public Droplet dropletTemplate;
    void Start()
    {
        // Disable template object in the scene hierarchy
        dropletTemplate.gameObject.SetActive(false);
    }

    // Spawn a new droplet of the requested color, in the requested position
    public void Drop(LiquidBase color, Vector3 worldPos)
    {
        Droplet newDroplet = Instantiate(dropletTemplate);
        newDroplet.name = "Droplet";
        newDroplet.transform.position = worldPos;
        newDroplet.color = color;
        newDroplet.gameObject.SetActive(true);
    }
}
