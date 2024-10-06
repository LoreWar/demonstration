using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class EnemyList
{
    public enum EnemyType
    {
        none,
        Bandit,
        Wolf
    }
    //����� ������������ ������ �� �������� enum EnemyType ������ id �������� � ���� ������ ������ ������ ���� �������� {2, 1, 1, -1, -1, -1, -1} ==> ����� ����� {Wolf, Bandit, Bandit, none, none, none, none}
    //��� ������ ����� ������� ��������� ���� ������ ��� ����������, ������ � ��� ������� ������� ���������
    //public EnemyType[] listEnemyScenariy = new EnemyType[7];
    //������ ��� id ����������� �� ���� ������
    public int[] listEnemyScenariy1 = new int[7];//����� 7 � ��������� �� �� ��� �� �����, � ����� ������������� � ����������
}
//[ExecuteAlways]
public class ButtonLocationInGlobalMap : MonoBehaviour
{
    public string Name;
    public int idLocation;

    //������ ��������� � �����������, ������� ���� �� �������, ����� ��� ����, ��� ����� ����� �� �������
    public int[] listItem;
    public int[] listEnemy;

    //�������� ����������� �� �������, �� ����������, ������, ����
    public List<EnemyList> listEnemyScenariy = new List<EnemyList>();

    //������� � ������� ����� ������� �� ���� �������
    public List<GameObject> canTransformPathWay = new List<GameObject>();
    //���� �� ������� ������ ������ ��������� � ������� �� �������
    public List<GameObject> bezierPath = new List<GameObject>();
    public Dictionary<GameObject, GameObject> bezierPathWay = new Dictionary<GameObject, GameObject>();
    //��� ���� � ���� ��� ���������(��������� ����, ��� ��������� �������(��������, ������� � �.�.))
    public List<int> sizePath = new List<int>();

    //��� ��������� � ��������, ������� ����� �� �������
    public GameObject CanvasWindow;
    //��� ��������� � ��������, ������� ����� �� ����������� ���� ���
    public GameObject controlBattleField;

    //��� ���������� �������� ��������� ��� ����� �� ������� � �����
    public GameObject buttonSearch;
    public GameObject buttonCamp;
    public GameObject buttonEnter;

    //��� ��������� � ������� ���������� �������� ������
    public GameObject teamCharacter;

    public GameObject _time;
    public GameObject nameLoc;
    public GameObject screenResult;

    //�������� ������� �������(����������� ������� ��������� � ������������),
    //� ����������� �� ��������� � ���� ����� �������� ��������(��������� ��������������� �� ������� ����������� � ���� � �������, ������� ���������)
    public int minlvlLocation;
    public int maxlvlLocation;
    public int lvlLocation;
    public int numberLocation;

    public int checkNumberTime = 0;

    public void ChangeName()
    {
        if (nameLoc.GetComponent<Text>().text != Name && gameObject.name != Name)
        {
            gameObject.name = "" + Name;
            nameLoc.GetComponent<Text>().text = "" + Name;
        }
    }

    void Awake()
    {
        teamCharacter = GameObject.Find("CharacterWorldTeam");
        screenResult = GameObject.Find("ScreenResult");
        CanvasWindow = GameObject.Find("Canvas");
        controlBattleField = GameObject.Find("ContrlBattleField");
        _time = GameObject.Find("Time");
        ChangeName();
    }

    //����� ������������� ��� ����� �� ������
    public void OnClick()
    {
        if (teamCharacter.GetComponent<CharacterControllOnGlobalMap>().goTravel == false)
        {
            Debug.Log("Number Location: " + Name + " " + idLocation);
            teamCharacter.GetComponent<CharacterControllOnGlobalMap>().NewLocation(gameObject);
        }
        else { }
    }
    
    //��������� ��������, ����� �������� ������ �� �������
    public void ButtonActivate()
    {
        if(buttonCamp != null && buttonSearch != null)
        {
            buttonCamp.SetActive(true);
            buttonSearch.SetActive(true);
            controlBattleField.GetComponent<BattleField>().nameLocation = Name;
            controlBattleField.GetComponent<BattleField>().lvlMonsterLocMin = minlvlLocation;
            controlBattleField.GetComponent<BattleField>().lvlMonsterLocMax = maxlvlLocation;
            controlBattleField.GetComponent<BattleField>().NumberLocation = numberLocation;
            controlBattleField.GetComponent<BattleField>().Location = teamCharacter.GetComponent<CharacterControllOnGlobalMap>().presentArea;
        }       
    }

    //�� ������������� ����� ��� ����� � �������, ������ � �.�.
    public void ButtonActivateEnter()
    {
        if (buttonEnter!= null)
        {
            buttonEnter.SetActive(true);
        }
    }

    //����� ��� ����������� ��������, ����� ������� ������ ������ � �������
    public void ButtonDeactivate()
    {
        if (buttonCamp != null && buttonSearch != null)
        {
            buttonCamp.SetActive(false);
            buttonSearch.SetActive(false);
            buttonEnter.SetActive(false);
        }        
    }

    //����� ������� ������ ������ �� �������
    public void ButtonSearch()
    {
        CanvasWindow.GetComponent<ControlLayer>().ButtonSearch();
        screenResult.GetComponent<ControlScreenWindow>().Generation(gameObject, listItem.Length);
    }

    public void ButtonExitSearch()
    {
        CanvasWindow.GetComponent<ControlLayer>().ButtonExitSearch();
        checkNumberTime = 0;
    }

    //����� ��� ������ � ������(������������� ���)
    public void ButtonCamp()
    {
        _time.GetComponent<CallendarTime>().TransTimeText(1);
    }

    public void ButtonEnter()
    {

    }
}
