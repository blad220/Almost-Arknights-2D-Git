using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISelectRotateCloseUI : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        MainController.mainInterfaceFields.selectRotateUI.unPlaceSelectRotate();
    }
    
}
