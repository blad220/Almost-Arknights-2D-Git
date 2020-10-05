using UnityEngine;
using UnityEngine.EventSystems;

public class UISelectRotateCloseUI : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Time.timeScale = 1f;
        MainController.mainInterfaceFields.selectRotateUI.SelectRotateOpen(false);
    }

    // Start is called before the first frame update
    private void Start()
    {
    }
}