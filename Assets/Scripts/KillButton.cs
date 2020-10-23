using MainController.Operator;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MainController
{
    public class KillButton : MonoBehaviour, IPointerDownHandler
    {
        public OperatorController killOperatorObject;

        void Start()
        {

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (killOperatorObject != null)
            {
                MainController.removeDeployedOperator(killOperatorObject.gameObject);
            }
        }
    }
}
