using System;
using DevionGames;
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
            if (!Input.GetMouseButtonDown(0)) return;
            if (setTargetOn == null) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            bool isHit = Physics.Raycast(ray, out hit, 100);
//            int ignoreNavigationLayer = 1 << 9;
//            ignoreNavigationLayer = ~ignoreNavigationLayer;
            CharacterMainBridge cmb = null;
//            Debug.Log(isHit + " "  + UnityTools.IsPointerOverUI() + " " + hit.collider.gameObject.layer);

            if (isHit && hit.collider.gameObject.layer == 7 && !UnityTools.IsPointerOverUI())
            {
                transform.position = Vector3.Lerp(setTargetOn.transform.position, hit.point + hit.normal * surfaceOffset, 1 - 2 / Vector3.Distance(setTargetOn.transform.position, hit.point));

                hit.collider.gameObject.SendMessage("MarkItem");
                setTargetOn.SendMessage("SetTarget", transform);
            } 
            else if (isHit && hit.collider.gameObject.layer == 9 && !UnityTools.IsPointerOverUI())
            {
                cmb = hit.transform.GetComponent<CharacterMainBridge>();

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
}
