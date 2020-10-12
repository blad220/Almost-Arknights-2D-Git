using Operator;
using UnityEngine;
using UnityEngine.EventSystems;

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
            MainController.mainInterfaceFields.selectOperatorUI.displaySelectedPanel.SetActive(false);
        }
    }
}
