using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityStandardAssets.Characters.ThirdPerson;

public class ClickAndMove : MonoBehaviour
{
//    public Camera RealCamera;
//    public ThirdPersonCharacter ThirdPersonCharacter;
//
//    private Vector3 _moveInUpdate = Vector3.back;
//    private bool _moveInUpdateLock = false;
//
//    // Start is called before the first frame update
//    void Start()
//    {
//        
//    }
//
//    // Update is called once per frame
//    void Update()
//    {
//        if (!RealCamera || !ThirdPersonCharacter)
//        {
//            return;
//        }
//
//        if (Input.GetMouseButtonDown(0))
//        {
//            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100))
//            {
//                _moveInUpdateLock = true;
////                ThirdPersonCharacter.Move((hit.point - ThirdPersonCharacter.transform.position).normalized, false, false);
////                _moveInUpdate = (hit.point - ThirdPersonCharacter.transform.position).normalized;
//                _moveInUpdate = hit.point;
//                Debug.Log("Ive got going to " + (hit.point - ThirdPersonCharacter.transform.position).normalized);
//            }
//        }
//
//        if (_moveInUpdateLock)
//        {
//            Vector3 direct = (_moveInUpdate - ThirdPersonCharacter.transform.position).normalized;
//
//            direct.x = direct.x * (1 / direct.x);
//            direct.z = direct.z * (1 / direct.z);
//
//            Debug.Log(direct);
//            ThirdPersonCharacter.Move(direct, false, false);
//
//            if ((_moveInUpdate - ThirdPersonCharacter.transform.position).sqrMagnitude < 1)
//            {
//                _moveInUpdate = Vector3.zero;
//                _moveInUpdateLock = false;
//                Debug.Log("Lock released");
//            }
//        }
//    }
}
