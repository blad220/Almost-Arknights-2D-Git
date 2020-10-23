using UnityEngine;
using UnityEngine.EventSystems;

namespace MainController.Ui
{
    public class UISelectRotateCloseUI : MonoBehaviour, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            MainController.mainInterfaceFields.selectRotateUI.unPlaceSelectRotate();
        }

    }
}