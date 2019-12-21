using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropperBehavior : MonoBehaviour
{
    public DropManager dropManager;
    public LiquidBase baseColor;

    private Coroutine pourRoutine;
    void OnMouseDown()
    {
        pourRoutine = StartCoroutine(Pour());
    }
    void Update()
    {
        if (Input.GetMouseButtonUp(0) && pourRoutine != null)
        {
            StopCoroutine(pourRoutine);
            pourRoutine = null;
        }
    }

    private IEnumerator Pour()
    {
        while (!GameManager.instance.WinState && !GameManager.instance.FailState)
        {
            dropManager.Drop(baseColor, transform.position);
            yield return new WaitForSeconds(.15f);
        }
        pourRoutine = null;
    }
}
