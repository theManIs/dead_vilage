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
            if (!Input.GetMouseButtonDown(0))
            {
                return;
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int ignoreNavigationLayer = 1 << 9;
            ignoreNavigationLayer = ~ignoreNavigationLayer;
            CharacterMainBridge cmb = null;

            if (Physics.Raycast(ray, out hit, 100, ignoreNavigationLayer))
            {
                return;
            } 
            else
            {
                if (!UnityTools.IsPointerOverUI())
                {
                    bool isHit = Physics.Raycast(ray, out hit, 100);
                    if (isHit && hit.collider.gameObject.layer == 9)
                    {
                        Debug.Log(UnityTools.IsPointerOverUI());
                    
                        cmb = hit.transform.GetComponent<CharacterMainBridge>();
                    }
                }
            }

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
