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
    //можно использовать массив из значений enum EnemyType вместо id монстров в базе данных вместо такого вида сценари€ {2, 1, 1, -1, -1, -1, -1} ==> будет такой {Wolf, Bandit, Bandit, none, none, none, none}
    //дл€ такого нужно немного пределать базу данных дл€ протиников, сделав в ней нуливой элемент пустышкой
    //public EnemyType[] listEnemyScenariy = new EnemyType[7];
    //массив дл€ id противников из базы данных
    public int[] listEnemyScenariy1 = new int[7];//цифра 7 в скобочках ни на что не влиет, еЄ можно переназначить в инспекторе
}
//[ExecuteAlways]
public class ButtonLocationInGlobalMap : MonoBehaviour
{
    public string Name;
    public int idLocation;

    //массив предметов и противников, которые есть на локации, нужно дл€ окна, что можно найти на локации
    public int[] listItem;
    public int[] listEnemy;

    //сценарии противников на локации, их численость, состав, типы
    public List<EnemyList> listEnemyScenariy = new List<EnemyList>();

    //локации в которые можно перейти из этой локации
    public List<GameObject> canTransformPathWay = new List<GameObject>();
    //пути по котором группа игрока переходит с локации на локацию
    public List<GameObject> bezierPath = new List<GameObject>();
    public Dictionary<GameObject, GameObject> bezierPathWay = new Dictionary<GameObject, GameObject>();
    //вес пути в дн€х дл€ календар€(изменение даты, дл€ временных заданий(сюжетных, событий и т.д.))
    public List<int> sizePath = new List<int>();

    //дл€ обращени€ к скриптам, которые вес€т на канвасе
    public GameObject CanvasWindow;
    //дл€ обращени€ к скриптам, которые вес€т на контроллере пол€ бо€
    public GameObject controlBattleField;

    //дл€ управлени€ игровыми объектами при входе на локацию и уходе
    public GameObject buttonSearch;
    public GameObject buttonCamp;
    public GameObject buttonEnter;

    //дл€ обращени€ к скрипту управлени€ командой игрока
    public GameObject teamCharacter;

    public GameObject _time;
    public GameObject nameLoc;
    public GameObject screenResult;

    //задаютс€ правила локации(минимальный уровень противник и максимальный),
    //в зависимости от сложности в мире можно измен€ть значени€(сложность подразумеваетс€ от времени проведенной в игре и событий, которые произошли)
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

    //метод срабатывающий при клике на кнопку
    public void OnClick()
    {
        if (teamCharacter.GetComponent<CharacterControllOnGlobalMap>().goTravel == false)
        {
            Debug.Log("Number Location: " + Name + " " + idLocation);
            teamCharacter.GetComponent<CharacterControllOnGlobalMap>().NewLocation(gameObject);
        }
        else { }
    }
    
    //активаци€ объектов, когда персанаж входит на локацию
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

    //не реализованный метод дл€ входа в деревни, города и т.д.
    public void ButtonActivateEnter()
    {
        if (buttonEnter!= null)
        {
            buttonEnter.SetActive(true);
        }
    }

    //метод дл€ деактивации объектов, когда команда игрока уходит с локации
    public void ButtonDeactivate()
    {
        if (buttonCamp != null && buttonSearch != null)
        {
            buttonCamp.SetActive(false);
            buttonSearch.SetActive(false);
            buttonEnter.SetActive(false);
        }        
    }

    //метод нажати€ кнопки поиска на лоцации
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

    //метод дл€ отдыха в лагере(востановлени€ сил)
    public void ButtonCamp()
    {
        _time.GetComponent<CallendarTime>().TransTimeText(1);
    }

    public void ButtonEnter()
    {

    }
}
