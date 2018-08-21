using UnityEngine;
using UnityEngine.EventSystems;


public class BtnEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {    
    [SerializeField]
    private float changeCalc = 1.1f;
    private Vector3 changeTransform;

    private void Start()
    {
        changeTransform = transform.localScale;
    }
     
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = changeCalc * changeTransform;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale =  changeTransform;
    }
}
