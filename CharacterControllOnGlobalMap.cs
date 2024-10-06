using UnityEngine;
[ExecuteAlways]
public class CharacterControllOnGlobalMap : MonoBehaviour
{
    public bool goTravel = false;
    public GameObject lastArea;
    public GameObject presentArea;
    public GameObject newArea;

    //
    public GameObject bezierObj;

    //
    public Transform P0;
    public Transform P1;
    public Transform P2;
    public Transform P3;

    public float time;

    public int sigmentsNumber = 30;

    //[Range(0, 1)]
    public float t;
    //выбор направление движения по которому пойдет игрок с одной локации на другую
    public void BezierPath()
    {
        if (newArea.GetComponent<ButtonLocationInGlobalMap>().idLocation < presentArea.GetComponent<ButtonLocationInGlobalMap>().idLocation)
        {
            P0 = bezierObj.transform.GetChild(3).GetComponent<Transform>();
            P1 = bezierObj.transform.GetChild(2).GetComponent<Transform>();
            P2 = bezierObj.transform.GetChild(1).GetComponent<Transform>();
            P3 = bezierObj.transform.GetChild(0).GetComponent<Transform>();
        }
        else
        {
            P0 = bezierObj.transform.GetChild(0).GetComponent<Transform>();
            P1 = bezierObj.transform.GetChild(1).GetComponent<Transform>();
            P2 = bezierObj.transform.GetChild(2).GetComponent<Transform>();
            P3 = bezierObj.transform.GetChild(3).GetComponent<Transform>();
        }
        
        t = 0;
        goTravel = true;
    }

    void Start()
    {
        gameObject.transform.position = new Vector3(presentArea.transform.position.x, presentArea.transform.position.y, presentArea.transform.position.z);
        presentArea.GetComponent<ButtonLocationInGlobalMap>().ButtonActivate();
    }

    //изменеие активных кнопок на локациях
    public void TransformMap()
    {
        presentArea.GetComponent<ButtonLocationInGlobalMap>().ButtonActivate();
        lastArea.GetComponent<ButtonLocationInGlobalMap>().ButtonDeactivate();
    }

    void FixedUpdate()
    {
        if (goTravel == true)
        {
            t += (1 / time) * Time.deltaTime;
        }
        else
        {

        }

        transform.position = Bezier.GetPoint(P0.position, P1.position, P2.position, P3.position, t);

        if (t >= 1)
        {        
            presentArea.GetComponent<ButtonLocationInGlobalMap>().ButtonActivate();
            Normal();
        }
    }

    public void Normal()
    {
        goTravel = false;     
    }

    //метод для изменения территорий 
    public void NewLocation(GameObject _newArea)
    {
        newArea = _newArea;

        for (int i = 0; i < newArea.GetComponent<ButtonLocationInGlobalMap>().canTransformPathWay.Count; i++)
        {
            if (presentArea == newArea.GetComponent<ButtonLocationInGlobalMap>().canTransformPathWay[i])
            {
                time = newArea.GetComponent<ButtonLocationInGlobalMap>().sizePath[i];
                bezierObj = newArea.GetComponent<ButtonLocationInGlobalMap>().bezierPath[i];               
                BezierPath();
                lastArea = presentArea;
                presentArea = newArea;
                lastArea.GetComponent<ButtonLocationInGlobalMap>().ButtonDeactivate();
                //_time.GetComponent<CallendarTime>().TransTimeText(sizePath[i]);
            }
            else {  }
        }
        newArea = null;
    }
}
