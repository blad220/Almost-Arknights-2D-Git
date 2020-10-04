using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class uiSelectAvatar : MonoBehaviour, IPointerDownHandler, IPointerClickHandler,
    IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject operatorObject;
    public Canvas parentCanvasOfImageToMove;
    public GameObject Avatar;
    public Text DpCost;

    private Vector3 hoverOn;
    private Vector3 hoverOff;
    private OperatorData _operatorData;
    private bool isCanPlace;

    private Vector3 screenPosition;
    private Vector3 offset;
    private float timeScaleBefore;
    public bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().color = new Color32(118, 118, 118, 139);
        hoverOff = transform.localPosition;
        hoverOn = transform.localPosition + new Vector3(0f, 10f, 0f);
        if(operatorObject.TryGetComponent(out OperatorController operatorController))
        {
            _operatorData = operatorController._operatorData;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isActive)
        {
            Debug.Log("Drag Begin");
            screenPosition = Camera.main.WorldToScreenPoint(operatorObject.transform.position);
            offset = operatorObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));
            timeScaleBefore = Time.timeScale;
            Time.timeScale = 0.5f;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isActive)
        {
            if (_operatorData.isLowGrounded) MainController.mainObject.GetComponent<LevelControllScript>().showLowGround = true;
            if (_operatorData.isHighGrounded) MainController.mainObject.GetComponent<LevelControllScript>().showHighGround = true;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
            {
                if (hit.collider.gameObject.TryGetComponent<TileDescription>(out TileDescription cube))
                {

                    TileDescription.TypeTile LowType = TileDescription.TypeTile.NoneStandableTyle;
                    TileDescription.TypeTile HighType = TileDescription.TypeTile.NoneStandableTyle;
                    if (_operatorData.isLowGrounded) { LowType = TileDescription.TypeTile.LowStandableTyle; }
                    if (_operatorData.isHighGrounded) { HighType = TileDescription.TypeTile.HighStandableTyle; }

                    if (cube.typeTile != TileDescription.TypeTile.NoneStandableTyle)
                    {
                        if (cube.typeTile == LowType) onFieldPosition(cube);
                        else if (cube.typeTile == HighType) onFieldPosition(cube);
                        else
                        {
                            noOnFieldPosition();
                        }
                    }
                    else
                    {
                        noOnFieldPosition();
                    }
                }
            }
        }
    }
    private TileDescription targetCube;
    void onFieldPosition(TileDescription cube)
    {
        operatorObject.transform.position = cube.standByPosition.position;
        isCanPlace = true;
        targetCube = cube;
    }
    void noOnFieldPosition()
    {
        //Vector3 transformOperator = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));
        //operatorObject.transform.position = new Vector3(transformOperator.x, 1f, transformOperator.z);
        operatorObject.transform.position = new Vector3(9999f, 9999f, 9999f);
        isCanPlace = false;
    }
 
    public void OnEndDrag(PointerEventData eventData)
    {
        if (isActive)
        {
            MainController.mainObject.GetComponent<LevelControllScript>().showLowGround = false;
            MainController.mainObject.GetComponent<LevelControllScript>().showHighGround = false;

            if (isCanPlace)
            {
                //MainController.mainInterfaceFields.selectRotateUI.gameObject.SetActive(true);
                MainController.mainInterfaceFields.selectRotateUI.placeSelectRotate(operatorObject, targetCube, true);

                //MainController.mainInterfaceFields.selectRotateUI.setPosition(targetCube.gameObject);
                //MainController.DeployOperator(operatorObject, targetCube);
            }
            //Debug.Log("" + MainController.isDeployedOperator(operatorObject));
            //Debug.Log("" + MainController.getDeployedOperatorIndex(operatorObject));
            Time.timeScale = timeScaleBefore;

            //Debug.Log("Drag Ended");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Mouse Down: " + eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Mouse Enter");
        //transform.localPosition = hoverOn;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Mouse Exit");
        //transform.localPosition = hoverOff;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Mouse Up");
    }
}
