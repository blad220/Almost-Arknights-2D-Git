using Operator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeTile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        //AtackEnemiesList.add
    }
    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out OperatorController operatorController))
        {
      

        }
 
    }
    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out OperatorController operatorController))
        {
            //AtackEnemiesList.remove
        }
    }
}
