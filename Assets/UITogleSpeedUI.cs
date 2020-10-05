using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITogleSpeedUI : MonoBehaviour, IPointerClickHandler
{
    public bool isFast;
    public Text textField;

    private void Start()
    {
        textField.text = "1X";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isFast = !isFast;
        if (isFast)
        {
            Time.timeScale = 2f;
            textField.text = "2X";
        }
        else
        {
            Time.timeScale = 1f;
            textField.text = "1X";
        }
    }
}