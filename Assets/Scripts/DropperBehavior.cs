using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropperBehavior : MonoBehaviour
{
    public DropManager dropManager;
    [Tooltip("Which color this button should drop")]
    public LiquidBase baseColor;

    private Coroutine _pourRoutine;

    // If this button is clicked, start pouring
    void OnMouseDown()
    {
        _pourRoutine = StartCoroutine(PourRoutine());
    }
    void Update()
    {
        // If a mouse up (anywhere) is received while pouring, stop pouring
        if (Input.GetMouseButtonUp(0) && _pourRoutine != null)
        {
            StopCoroutine(_pourRoutine);
            _pourRoutine = null;
        }
    }

    private IEnumerator PourRoutine()
    {
        // Drop droplets repeatedly until win, lose, or mouse up (routine is killed in Update()).
        while (!GameManager.instance.WinState && !GameManager.instance.FailState)
        {
            dropManager.Drop(baseColor, transform.position);
            yield return new WaitForSeconds(.15f);
        }
        _pourRoutine = null;
    }
}
