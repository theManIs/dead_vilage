using System;
using UnityEngine;


namespace UnityStandardAssets.SceneUtils
{
    public class PlaceTargetWithMouse : MonoBehaviour
    {
        public float surfaceOffset = 1.5f;
        public GameObject setTargetOn;

        // Update is called once per frame
        private void Update()
        {
            if (!Input.GetMouseButtonDown(0))
            {
                return;
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit))
            {
                return;
            }

            CharacterMainBridge cmb = hit.transform.GetComponent<CharacterMainBridge>();

            if (cmb != null)
            {
                Debug.Log(hit.transform.gameObject.name);

                cmb.HealthKickerContraption.hitMe(cmb.HealthKickerContraption.NormalDamage);
            }
            else
            {
                transform.position = hit.point + hit.normal * surfaceOffset;
                if (setTargetOn != null)
                {
                    setTargetOn.SendMessage("SetTarget", transform);
                }
            }
        }
    }
}
