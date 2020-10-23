using MainController.Map.Tile;
using MainController.Operator;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MainController.Ui
{

    public class UISelectAvatar : MonoBehaviour, IPointerDownHandler,
        IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public GameObject operatorObject;
        public Canvas parentCanvasOfImageToMove;
        public GameObject Avatar;
        public Text DpCost;
        public Text RedeployField;

        public bool isActive;
        public bool isRedeploy;

        private Vector3 hoverOn;
        private Vector3 hoverOff;
        private OperatorData _operatorData;
        private bool isCanPlace;

        private Vector3 screenPosition;
        private Vector3 offset;
        private float timeScaleBefore;

        private TileDescription targetCube;

        void Start()
        {
            hoverOff = transform.localPosition;
            hoverOn = transform.localPosition + new Vector3(0f, 10f, 0f);
            if (operatorObject.TryGetComponent(out OperatorController operatorController))
            {
                _operatorData = operatorController._operatorData;
            }
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (isActive)
            {
                screenPosition = Camera.main.WorldToScreenPoint(operatorObject.transform.position);
                offset = operatorObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));

                operatorObject.GetComponent<OperatorController>().ShowRange();
                MainController.mainInterfaceFields.selectOperatorUI.UpdateSelectInfo(_operatorData);
                MainController.mainInterfaceFields.selectOperatorUI.displaySelectedPanel.SetActive(true);
                MainController.SetCurrentTimeScale(0.5f);

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

        void onFieldPosition(TileDescription cube)
        {
            operatorObject.transform.position = cube.standByPosition.position;
            isCanPlace = true;
            targetCube = cube;
        }
        void noOnFieldPosition()
        {
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
                    MainController.mainInterfaceFields.selectRotateUI.placeSelectRotate(operatorObject, targetCube, true);
                }
                else
                {
                    MainController.TimeScaleReset();
                }
            }
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            MainController.mainInterfaceFields.selectOperatorUI.UpdateSelectInfo(_operatorData);
            MainController.mainInterfaceFields.selectOperatorUI.displaySelectedPanel.SetActive(true);
        }
        public void RedeployShow()
        {
            isRedeploy = true;
            RedeployField.transform.parent.gameObject.SetActive(true);
            MainController.mainInterfaceFields.operatorPanelCreate.checkDPCostAndUnitLimit();
        }
        public void RedeployHide()
        {
            isRedeploy = false;
            RedeployField.transform.parent.gameObject.SetActive(false);
            MainController.mainInterfaceFields.operatorPanelCreate.checkDPCostAndUnitLimit();
        }
    }
}