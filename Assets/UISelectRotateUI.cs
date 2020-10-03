using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelectRotateUI : MonoBehaviour
{
    public Color32 hoverColor;

    public GameObject RotateSelectUI;

    public Image topObjectImage;
    public Image leftObjectImage;
    public Image rightObjectImage;
    public Image bottomObjectImage;

    public GameObject target;
    public TileDescription cubeTile;
    void Start()
    {
        UISelectRotateUI link = MainController.mainInterfaceFields.selectRotateUI;
        if (link == null) MainController.mainInterfaceFields.selectRotateUI = gameObject.GetComponent<UISelectRotateUI>();
    }
    public void setPosition(GameObject CubeObject) {

        Vector3 pos = Camera.main.WorldToScreenPoint(CubeObject.GetComponent<TileDescription>().standByPosition.position);

        transform.position = pos;
    }
    public GameObject getTarget()
    {
        return target;
    }
    public TileDescription getCubeTile()
    {
        return cubeTile;
    }
    public void placeSelectRotate(GameObject operatorObject, TileDescription cube, bool isActive)
    {
        RotateSelectUI.SetActive(isActive);
        target = operatorObject;
        cubeTile = cube;
        //operatorObject.GetComponent<OperatorController>().setupDirection = OperatorController.SetupDirection.
    }

}
