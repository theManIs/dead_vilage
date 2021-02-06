using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneUITrigger : MonoBehaviour
{
    public RectTransform PanelRectTransform;
    public Collider ZoneCollider;

    public void OnTriggerEnter(Collider objCollider)
    {
        if (PanelRectTransform)
        {
            PanelRectTransform.gameObject.SetActive(true);
        }
    }
    public void OnTriggerExit(Collider objCollider)
    {
        if (PanelRectTransform)
        {
            PanelRectTransform.gameObject.SetActive(false);
        }
    }
}
