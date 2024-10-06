using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    //ссылки для взаимодействия с скриптами на данных игровых объектах
    public GameObject buttonAtack;
    public GameObject controlBattleField;
    public GameObject storageTeam;
    public GameObject spelWindow;

    public GameObject objectChoise;
    public Transform posFace;
    public Transform posBack;

    //контроль слайдеров как объектов
    public GameObject gsliderHp, gsliderSpeed;

    //сылки на хранилища
    public ItemsDatabaseObject itemDatabase;
    public SpelsDatabaseObject spelDatabase;
    public DatabaseBuffsDebuffs buffdebuffDatabase;

    public bool canAtack = false;
    public bool Dead = false;

    //для работы со значениями слайдеров
    public Slider sliderHp;
    public Slider sliderShild;
    public Slider sliderGreatLookHp;

    public float realHp;
    public float maxHp;

    //Относится к системе баффам
    public GameObject AreaBuffDebaff;
    public List<GameObject> BuffsIconsStartTurn = new List<GameObject>();
    public List<GameObject> BuffsIconsEndTurn = new List<GameObject>();
    public List<GameObject> DebuffsIconsStartTurn = new List<GameObject>();
    public List<GameObject> DebuffsIconsEndTurn = new List<GameObject>();

    //для генерации иконок баффов и дебаффов
    public GameObject iconPrefabBuffDebuff;
    public float X_START;
    public float Y_START;
    public float X_SPACE_BETWEEN_ITEM;
    public float NUMBER_OF_COLUMN;
    public float Y_SPACE_BETWEEN_ITEMS;

    //базовые характеристики
    public int Strenght = 5, Agility = 5, Intellect = 5, Constitution = 5, Charism = 5;
    public float speed, chanceAvaid, chanceCrit;
    public float physicalAtk, magicalAtk;
    public int physicalArmor, magicalArmor;
    public float physicalDamageResist, magicalDamageResist;
    public int priorityAtk;

    //private double _speed;
    public Slider sliderSpeed;

    //переменные баффов/дебаффов для взаимодействия с характеристиками
    [SerializeField] private float UpSpeed;
    [SerializeField] private float DownSpeed;
    [SerializeField] private float UpPhysical;
    [SerializeField] private float DownPhysical;
    [SerializeField] private float UpAvaid;
    [SerializeField] private float DownAvaid;

    //хранилища доп. инфы
    public Item[] item = new Item[10];
    public Spel[] spel = new Spel[5];
    public bool[] activeTalant = new bool[25];

    //для использования навыков на поле боя
    public List<GameObject> characterEnemy = new List<GameObject>();
    public List<GameObject> characterAlly = new List<GameObject>();
    public List<GameObject> spelsList = new List<GameObject>();

    public int numberSpel;

    public GameObject target;
    public int numberTargetEnemy;
    public int numberTargetAlly;

    public int n;
    public int numberHero;
    public int numberEnemy;

    public List<GameObject> targetList = new List<GameObject>();


    #region //генерация ячеек баффов дебаффов и их сортировка
    public void CreateSlots(int Id)
    {
        bool check = false;
        int number = -1;
        if ((int)buffdebuffDatabase.ItemObjectsBuffDebuff[Id].typeBuffOrDebuff == 1)
        {
            //debuff
            if (buffdebuffDatabase.ItemObjectsBuffDebuff[Id].StartTurn == true)
            {
                if(DebuffsIconsStartTurn.Count != 0)
                {
                    for (int i = 0; i < DebuffsIconsStartTurn.Count; i++)
                    {
                        if(DebuffsIconsStartTurn[i] != null)
                        {
                            if (DebuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().Id == Id)
                            {
                                check = true;
                                number = i;
                                Debug.Log(check);
                            }
                            else
                            {
                                check = false;
                                Debug.Log(check);
                            }
                        }                       
                    }
                }

                if (check == false)
                {
                    var obj = Instantiate(iconPrefabBuffDebuff, Vector3.zero, Quaternion.identity, transform);           
                    obj.GetComponent<IconsBuffDebuff>().icons.sprite = buffdebuffDatabase.ItemObjectsBuffDebuff[Id].uiDisplay;
                    obj.GetComponent<IconsBuffDebuff>().Id = buffdebuffDatabase.ItemObjectsBuffDebuff[Id].data.Id;
                    obj.GetComponent<IconsBuffDebuff>().nameBuffDebuff = buffdebuffDatabase.ItemObjectsBuffDebuff[Id].data.Name;
                    obj.GetComponent<IconsBuffDebuff>().value = buffdebuffDatabase.ItemObjectsBuffDebuff[Id].data.value;
                    obj.GetComponent<IconsBuffDebuff>().numberTurns = buffdebuffDatabase.ItemObjectsBuffDebuff[Id].data.numberTurn;
                    obj.GetComponent<IconsBuffDebuff>().numberType = "" + buffdebuffDatabase.ItemObjectsBuffDebuff[Id].TypeBuffsDebuffs;
                    obj.GetComponent<IconsBuffDebuff>().numberTurnsText.text = "" + buffdebuffDatabase.ItemObjectsBuffDebuff[Id].data.numberTurn;
                    obj.GetComponent<IconsBuffDebuff>().stack = 1;
                    obj.GetComponent<IconsBuffDebuff>().stackText.text = obj.GetComponent<IconsBuffDebuff>().stack == 1 ? "" : obj.GetComponent<IconsBuffDebuff>().stack.ToString("n0");
                    DebuffsIconsStartTurn.Add(obj);
                }
                else if (buffdebuffDatabase.ItemObjectsBuffDebuff[Id].stackable == true)
                {
                    DebuffsIconsStartTurn[number].GetComponent<IconsBuffDebuff>().stack += 1;
                    DebuffsIconsStartTurn[number].GetComponent<IconsBuffDebuff>().stackText.text = DebuffsIconsStartTurn[number].GetComponent<IconsBuffDebuff>().stack == 1 ? "" : DebuffsIconsStartTurn[number].GetComponent<IconsBuffDebuff>().stack.ToString("n0");
                    DebuffsIconsStartTurn[number].GetComponent<IconsBuffDebuff>().numberTurns = buffdebuffDatabase.ItemObjectsBuffDebuff[Id].data.numberTurn;
                    DebuffsIconsStartTurn[number].GetComponent<IconsBuffDebuff>().numberTurnsText.text = "" + buffdebuffDatabase.ItemObjectsBuffDebuff[number].data.numberTurn;
                }
                else
                {
                    if(number != -1)
                    {
                        DebuffsIconsStartTurn[number].GetComponent<IconsBuffDebuff>().stack = 1;
                        DebuffsIconsStartTurn[number].GetComponent<IconsBuffDebuff>().stackText.text = DebuffsIconsEndTurn[number].GetComponent<IconsBuffDebuff>().stack == 1 ? "" : DebuffsIconsEndTurn[number].GetComponent<IconsBuffDebuff>().stack.ToString("n0");
                        DebuffsIconsStartTurn[number].GetComponent<IconsBuffDebuff>().numberTurns = buffdebuffDatabase.ItemObjectsBuffDebuff[Id].data.numberTurn;
                        DebuffsIconsStartTurn[number].GetComponent<IconsBuffDebuff>().numberTurnsText.text = "" + buffdebuffDatabase.ItemObjectsBuffDebuff[number].data.numberTurn;
                    }             
                }
            }
            else
            {
                if (DebuffsIconsEndTurn.Count != 0)
                {
                    for (int i = 0; i < DebuffsIconsEndTurn.Count; i++)
                    {
                        if (DebuffsIconsEndTurn[i].GetComponent<IconsBuffDebuff>().Id == Id)
                        {
                            check = true;
                            number = i;
                            Debug.Log(number);
                        }
                        else
                        {
                            check = false;
                        }
                    }
                }

                if (check == false)
                {
                    var obj = Instantiate(iconPrefabBuffDebuff, Vector3.zero, Quaternion.identity, transform);
                    obj.GetComponent<IconsBuffDebuff>().icons.sprite = buffdebuffDatabase.ItemObjectsBuffDebuff[Id].uiDisplay;
                    obj.GetComponent<IconsBuffDebuff>().Id = buffdebuffDatabase.ItemObjectsBuffDebuff[Id].data.Id;
                    obj.GetComponent<IconsBuffDebuff>().nameBuffDebuff = buffdebuffDatabase.ItemObjectsBuffDebuff[Id].data.Name;
                    
                    obj.GetComponent<IconsBuffDebuff>().value = buffdebuffDatabase.ItemObjectsBuffDebuff[Id].data.value;
                    obj.GetComponent<IconsBuffDebuff>().numberTurns = buffdebuffDatabase.ItemObjectsBuffDebuff[Id].data.numberTurn;
                    obj.GetComponent<IconsBuffDebuff>().numberType = "" + buffdebuffDatabase.ItemObjectsBuffDebuff[Id].TypeBuffsDebuffs;
                    obj.GetComponent<IconsBuffDebuff>().numberTurnsText.text = "" + buffdebuffDatabase.ItemObjectsBuffDebuff[Id].data.numberTurn; 
                    obj.GetComponent<IconsBuffDebuff>().stack += 1;
                    obj.GetComponent<IconsBuffDebuff>().stackText.text = obj.GetComponent<IconsBuffDebuff>().stack == 1 ? "" : obj.GetComponent<IconsBuffDebuff>().stack.ToString("n0");
                    DebuffsIconsEndTurn.Add(obj);
                }
                else if (buffdebuffDatabase.ItemObjectsBuffDebuff[Id].stackable == true)
                {
                    DebuffsIconsEndTurn[number].GetComponent<IconsBuffDebuff>().stack += 1;
                    DebuffsIconsEndTurn[number].GetComponent<IconsBuffDebuff>().stackText.text = DebuffsIconsStartTurn[number].GetComponent<IconsBuffDebuff>().stack == 1 ? "" : DebuffsIconsStartTurn[number].GetComponent<IconsBuffDebuff>().stack.ToString("n0");
                    DebuffsIconsEndTurn[number].GetComponent<IconsBuffDebuff>().numberTurns = buffdebuffDatabase.ItemObjectsBuffDebuff[Id].data.numberTurn;
                    DebuffsIconsEndTurn[number].GetComponent<IconsBuffDebuff>().numberTurnsText.text = "" + buffdebuffDatabase.ItemObjectsBuffDebuff[number].data.numberTurn;
                }
                else
                {
                    if (number != -1)
                    {
                        DebuffsIconsEndTurn[number].GetComponent<IconsBuffDebuff>().stack = 1;
                        DebuffsIconsEndTurn[number].GetComponent<IconsBuffDebuff>().stackText.text = DebuffsIconsEndTurn[number].GetComponent<IconsBuffDebuff>().stack == 1 ? "" : DebuffsIconsEndTurn[number].GetComponent<IconsBuffDebuff>().stack.ToString("n0");
                        DebuffsIconsEndTurn[number].GetComponent<IconsBuffDebuff>().numberTurns = buffdebuffDatabase.ItemObjectsBuffDebuff[Id].data.numberTurn;
                        DebuffsIconsEndTurn[number].GetComponent<IconsBuffDebuff>().numberTurnsText.text = "" + buffdebuffDatabase.ItemObjectsBuffDebuff[number].data.numberTurn;
                    }
                }
            }

        }
        else
        {
            //buff
            if (buffdebuffDatabase.ItemObjectsBuffDebuff[Id].StartTurn == true)
            {
                for (int i = 0; i < BuffsIconsStartTurn.Count; i++)
                {
                    if (BuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().Id == Id)
                    {
                        check = true;
                        number = i;
                    }
                    else
                    {
                        check = false;
                    }
                }

                if (check == false)
                {
                    var obj = Instantiate(iconPrefabBuffDebuff, Vector3.zero, Quaternion.identity, transform);                    
                    obj.GetComponent<IconsBuffDebuff>().icons.sprite = buffdebuffDatabase.ItemObjectsBuffDebuff[Id].uiDisplay;
                    obj.GetComponent<IconsBuffDebuff>().Id = buffdebuffDatabase.ItemObjectsBuffDebuff[Id].data.Id;
                    obj.GetComponent<IconsBuffDebuff>().nameBuffDebuff = buffdebuffDatabase.ItemObjectsBuffDebuff[Id].data.Name;
                    obj.GetComponent<IconsBuffDebuff>().value = buffdebuffDatabase.ItemObjectsBuffDebuff[Id].data.value;
                    obj.GetComponent<IconsBuffDebuff>().numberTurns = buffdebuffDatabase.ItemObjectsBuffDebuff[Id].data.numberTurn;
                    obj.GetComponent<IconsBuffDebuff>().numberType = "" + buffdebuffDatabase.ItemObjectsBuffDebuff[Id].TypeBuffsDebuffs;
                    obj.GetComponent<IconsBuffDebuff>().numberTurnsText.text = "" + buffdebuffDatabase.ItemObjectsBuffDebuff[Id].data.numberTurn;
                    obj.GetComponent<IconsBuffDebuff>().stack += 1;
                    obj.GetComponent<IconsBuffDebuff>().stackText.text = obj.GetComponent<IconsBuffDebuff>().stack == 1 ? "" : obj.GetComponent<IconsBuffDebuff>().stack.ToString("n0");
                    BuffsIconsStartTurn.Add(obj);
                }
                else if (buffdebuffDatabase.ItemObjectsBuffDebuff[Id].stackable == true)
                {
                    BuffsIconsStartTurn[number].GetComponent<IconsBuffDebuff>().stack += 1;
                    BuffsIconsStartTurn[number].GetComponent<IconsBuffDebuff>().stackText.text = DebuffsIconsStartTurn[number].GetComponent<IconsBuffDebuff>().stack == 1 ? "" : DebuffsIconsStartTurn[number].GetComponent<IconsBuffDebuff>().stack.ToString("n0");
                    BuffsIconsStartTurn[number].GetComponent<IconsBuffDebuff>().numberTurns = buffdebuffDatabase.ItemObjectsBuffDebuff[Id].data.numberTurn;
                    BuffsIconsStartTurn[number].GetComponent<IconsBuffDebuff>().numberTurnsText.text = "" + buffdebuffDatabase.ItemObjectsBuffDebuff[number].data.numberTurn;
                }
                else
                {
                    if (number != -1)
                    {
                        BuffsIconsStartTurn[number].GetComponent<IconsBuffDebuff>().stack = 1;
                        BuffsIconsStartTurn[number].GetComponent<IconsBuffDebuff>().stackText.text = DebuffsIconsEndTurn[number].GetComponent<IconsBuffDebuff>().stack == 1 ? "" : DebuffsIconsEndTurn[number].GetComponent<IconsBuffDebuff>().stack.ToString("n0");
                        BuffsIconsStartTurn[number].GetComponent<IconsBuffDebuff>().numberTurns = buffdebuffDatabase.ItemObjectsBuffDebuff[Id].data.numberTurn;
                        BuffsIconsStartTurn[number].GetComponent<IconsBuffDebuff>().numberTurnsText.text = "" + buffdebuffDatabase.ItemObjectsBuffDebuff[number].data.numberTurn;
                    }
                }
            }
            else
            {
                for (int i = 0; i < BuffsIconsEndTurn.Count; i++)
                {
                    if (BuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().Id == Id)
                    {
                        check = true;
                        number = i;
                    }
                    else
                    {
                        check = false;
                    }
                }

                if (check == false)
                {
                    var obj = Instantiate(iconPrefabBuffDebuff, Vector3.zero, Quaternion.identity, transform);
                    obj.GetComponent<IconsBuffDebuff>().icons.sprite = buffdebuffDatabase.ItemObjectsBuffDebuff[Id].uiDisplay;
                    obj.GetComponent<IconsBuffDebuff>().Id = buffdebuffDatabase.ItemObjectsBuffDebuff[Id].data.Id;
                    obj.GetComponent<IconsBuffDebuff>().nameBuffDebuff = buffdebuffDatabase.ItemObjectsBuffDebuff[Id].data.Name;
                    obj.GetComponent<IconsBuffDebuff>().value = buffdebuffDatabase.ItemObjectsBuffDebuff[Id].data.value;
                    obj.GetComponent<IconsBuffDebuff>().numberTurns = buffdebuffDatabase.ItemObjectsBuffDebuff[Id].data.numberTurn;
                    obj.GetComponent<IconsBuffDebuff>().numberType = "" + buffdebuffDatabase.ItemObjectsBuffDebuff[Id].TypeBuffsDebuffs;
                    obj.GetComponent<IconsBuffDebuff>().numberTurnsText.text = "" + buffdebuffDatabase.ItemObjectsBuffDebuff[Id].data.numberTurn;
                    obj.GetComponent<IconsBuffDebuff>().stack += 1;
                    obj.GetComponent<IconsBuffDebuff>().stackText.text = obj.GetComponent<IconsBuffDebuff>().stack == 1 ? "" : obj.GetComponent<IconsBuffDebuff>().stack.ToString("n0");
                    BuffsIconsEndTurn.Add(obj);
                }
                else if (buffdebuffDatabase.ItemObjectsBuffDebuff[Id].stackable == true)
                {
                    BuffsIconsEndTurn[number].GetComponent<IconsBuffDebuff>().stack += 1;
                    BuffsIconsEndTurn[number].GetComponent<IconsBuffDebuff>().stackText.text = DebuffsIconsStartTurn[number].GetComponent<IconsBuffDebuff>().stack == 1 ? "" : DebuffsIconsStartTurn[number].GetComponent<IconsBuffDebuff>().stack.ToString("n0");
                    BuffsIconsEndTurn[number].GetComponent<IconsBuffDebuff>().numberTurns = buffdebuffDatabase.ItemObjectsBuffDebuff[Id].data.numberTurn;
                    BuffsIconsEndTurn[number].GetComponent<IconsBuffDebuff>().numberTurnsText.text = "" + buffdebuffDatabase.ItemObjectsBuffDebuff[number].data.numberTurn;
                }
                else
                {
                    if (number != -1)
                    {
                        BuffsIconsEndTurn[number].GetComponent<IconsBuffDebuff>().stack = 1;
                        BuffsIconsEndTurn[number].GetComponent<IconsBuffDebuff>().stackText.text = DebuffsIconsEndTurn[number].GetComponent<IconsBuffDebuff>().stack == 1 ? "" : DebuffsIconsEndTurn[number].GetComponent<IconsBuffDebuff>().stack.ToString("n0");
                        BuffsIconsEndTurn[number].GetComponent<IconsBuffDebuff>().numberTurns = buffdebuffDatabase.ItemObjectsBuffDebuff[Id].data.numberTurn;
                        BuffsIconsEndTurn[number].GetComponent<IconsBuffDebuff>().numberTurnsText.text = "" + buffdebuffDatabase.ItemObjectsBuffDebuff[number].data.numberTurn;
                    }
                }
            }
            
        }

        BuffDebuffChangeCharacteristics();
        ChangePositionElements();
    }

    public void ChangePositionElements()
    {
        int n = -1;

        for(int i = 0; i < BuffsIconsStartTurn.Count; i++)
        {
            if(BuffsIconsStartTurn[i] != null)
            {
                n++;
                BuffsIconsStartTurn[i].GetComponent<RectTransform>().localPosition = GetPosition(n);
            }
        }

        for (int i = 0; i < BuffsIconsEndTurn.Count; i++)
        {
            if (BuffsIconsEndTurn[i] != null)
            {
                n++;
                BuffsIconsEndTurn[i].GetComponent<RectTransform>().localPosition = GetPosition(n);
            }
        }

        for (int i = 0; i < DebuffsIconsStartTurn.Count; i++)
        {
            if (DebuffsIconsStartTurn[i] != null)
            {
                n++;
                DebuffsIconsStartTurn[i].GetComponent<RectTransform>().localPosition = GetPosition(n);
            }
        }

        for (int i = 0; i < DebuffsIconsEndTurn.Count; i++)
        {
            if (DebuffsIconsEndTurn[i] != null)
            {
                n++;
                DebuffsIconsEndTurn[i].GetComponent<RectTransform>().localPosition = GetPosition(n);
            }
        }
    }

    private Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUMN)), 0f);
    }
    #endregion

    private void Awake()
    {
        StartBattle();
    }
    public void StartBattle()
    {
        storageTeam = GameObject.Find("TeamStorage");
        controlBattleField = GameObject.Find("ContrlBattleField");
        spelWindow = GameObject.Find("SpelsCharacterWindowInBattle");
        buttonAtack = GameObject.Find("ButtonAttack");
    }

    public void GreatLooksHp()
    {
        if (realHp < sliderGreatLookHp.value)
        {
            sliderGreatLookHp.value -= 10 * Time.deltaTime;
        }
        else
        {
            sliderGreatLookHp.value = realHp;
        }
    }
    
    #region //Подсчет значений баффов дебаффов
    public void BuffDebuffChangeCharacteristics()
    {
        NormalMnozhCharacteristics();
        SummaryBuffStartTurn();
        SummaryBuffEndTurn();
        SummaryDebuffStartTurn();
        SummaryDebuffEndTurn();
        CharacteristicsInBattle();
    }
    public void CharacteristicsInBattle()
    {
        ChangeSpeed();
        ChangePhysicalAtk();
        ChangeMagicalAtk();
        ChangeChanceAvade();
        ChangeChanceCrit();
    }

    
    public void NormalMnozhCharacteristics()
    {
        UpSpeed = 0;
        DownSpeed = 0;
        UpPhysical = 0;
        DownPhysical = 0;
    }
    public void SummaryBuffStartTurn()
    {
        if(BuffsIconsStartTurn.Count != 0)
        {
            for (int i = 0; i < BuffsIconsStartTurn.Count; i++)
            {
                if (BuffsIconsStartTurn[i] != null)
                {
                    if (BuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().nameBuffDebuff == "UpSpeed")
                    {
                        UpSpeed += BuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().stack * BuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().value;
                    }
                    else if (BuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().nameBuffDebuff == "UpPhysical")
                    {
                        UpPhysical += BuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().stack * BuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().value;
                    }
                    else if (BuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().nameBuffDebuff == "UpAvaid")
                    {
                        UpAvaid += BuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().stack * BuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().value;
                    }
                }
            }
        }
    }

    public void SummaryBuffEndTurn()
    {
        if(BuffsIconsEndTurn.Count != 0)
        {
            for (int i = 0; i < BuffsIconsEndTurn.Count; i++)
            {
                if (BuffsIconsEndTurn[i] != null)
                {
                    if (BuffsIconsEndTurn[i].GetComponent<IconsBuffDebuff>().nameBuffDebuff == "UpSpeed")
                    {
                        UpSpeed += BuffsIconsEndTurn[i].GetComponent<IconsBuffDebuff>().stack * BuffsIconsEndTurn[i].GetComponent<IconsBuffDebuff>().value;
                    }
                    else if (BuffsIconsEndTurn[i].GetComponent<IconsBuffDebuff>().nameBuffDebuff == "UpPhysical")
                    {
                        UpPhysical += BuffsIconsEndTurn[i].GetComponent<IconsBuffDebuff>().stack * BuffsIconsEndTurn[i].GetComponent<IconsBuffDebuff>().value;
                    }
                }
            }
        }      
    }

    public void SummaryDebuffStartTurn()
    {
        if(DebuffsIconsStartTurn.Count != 0)
        {
            for (int i = 0; i < DebuffsIconsStartTurn.Count; i++)
            {
                if (DebuffsIconsStartTurn[i] != null)
                {
                    if (DebuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().nameBuffDebuff == "DownSpeed")
                    {
                        DownSpeed -= DebuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().stack * DebuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().value;
                    }
                    else if (DebuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().nameBuffDebuff == "DownPhysical")
                    {
                        DownPhysical -= DebuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().stack * DebuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().value;
                    }
                    else if (DebuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().nameBuffDebuff == "DownAvaid")
                    {
                        DownAvaid -= DebuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().stack * DebuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().value;
                    }
                }
            }
        }    
    }

    public void SummaryDebuffEndTurn()
    {
        if(DebuffsIconsEndTurn.Count != 0)
        {
            for (int i = 0; i < DebuffsIconsEndTurn.Count; i++)
            {
                if (DebuffsIconsEndTurn[i] != null)
                {
                    if (DebuffsIconsEndTurn[i].GetComponent<IconsBuffDebuff>().nameBuffDebuff == "DownSpeed")
                    {
                        DownSpeed -= DebuffsIconsEndTurn[i].GetComponent<IconsBuffDebuff>().stack * DebuffsIconsEndTurn[i].GetComponent<IconsBuffDebuff>().value;
                    }
                    else if (DebuffsIconsEndTurn[i].GetComponent<IconsBuffDebuff>().nameBuffDebuff == "DownPhysical")
                    {
                        DownPhysical -= DebuffsIconsEndTurn[i].GetComponent<IconsBuffDebuff>().stack * DebuffsIconsEndTurn[i].GetComponent<IconsBuffDebuff>().value;
                    }
                }
            }
        }     
    }
    #endregion 

    #region //Подсчет ходов баффов дебаффов
    public void ChangeNumberTurnsBuffStartTurn()
    {
        bool check = false;
        for (int i = 0; i < BuffsIconsStartTurn.Count; i++)
        {
            if (BuffsIconsStartTurn[i] != null)
            {
                if (BuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().nameBuffDebuff == "HealHP")
                {
                    GetHeal(BuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().stack * BuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().value);
                }

                BuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().numberTurns--;
                BuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().numberTurnsText.text = "" + BuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().numberTurns;

                if (BuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().numberTurns == 0)
                {
                    Destroy(BuffsIconsStartTurn[i]);
                    BuffsIconsStartTurn[i] = null;
                }
            }
        }

        for (int i = 0; i < BuffsIconsStartTurn.Count; i++)
        {
            if (BuffsIconsStartTurn[i] != null)
            {
                check = true;
                break;
            }
        }

        if (check == false)
        {
            BuffsIconsStartTurn = new List<GameObject>();
        }
        else
        {

        }

        BuffDebuffChangeCharacteristics();
        ChangePositionElements();
    }

    public void ChangeNumberTurnsBuffEndTurn()
    {
        bool check = false;

        for (int i = 0; i < BuffsIconsEndTurn.Count; i++)
        {
            if (BuffsIconsEndTurn[i] != null)
            {
                BuffsIconsEndTurn[i].GetComponent<IconsBuffDebuff>().numberTurns--;
                BuffsIconsEndTurn[i].GetComponent<IconsBuffDebuff>().numberTurnsText.text = "" + BuffsIconsEndTurn[i].GetComponent<IconsBuffDebuff>().numberTurns;

                if (BuffsIconsEndTurn[i].GetComponent<IconsBuffDebuff>().numberTurns == 0)
                {
                    Destroy(BuffsIconsEndTurn[i]);
                    BuffsIconsEndTurn[i] = null;
                }
            }
        }

        for (int i = 0; i < BuffsIconsEndTurn.Count; i++)
        {
            if (BuffsIconsEndTurn[i] != null)
            {
                check = true;
                break;
            }
        }

        if (check == false)
        {
            BuffsIconsEndTurn = new List<GameObject>();
        }
        else
        {

        }

        BuffDebuffChangeCharacteristics();
        ChangePositionElements();
    }

    public void ChangeNumberTurnsDebuffStartTurn()
    {
        bool check = false;

        for (int i = 0; i < DebuffsIconsStartTurn.Count; i++)
        {
            if (DebuffsIconsStartTurn[i] != null)
            {
                if (DebuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().nameBuffDebuff == "DamageHP")
                {
                    realHp -= DebuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().stack * DebuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().value;
                    CheckHp();
                }

                DebuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().numberTurns--;
                DebuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().numberTurnsText.text = "" + DebuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().numberTurns;

                if (DebuffsIconsStartTurn[i].GetComponent<IconsBuffDebuff>().numberTurns == 0)
                {
                    Destroy(DebuffsIconsStartTurn[i]);
                    DebuffsIconsStartTurn[i] = null;
                }
            }
        }

        for (int i = 0; i < DebuffsIconsStartTurn.Count; i++)
        {
            if (DebuffsIconsStartTurn[i] != null)
            {
                check = true;
                break;
            }
        }

        if (check == false)
        {
            DebuffsIconsStartTurn = new List<GameObject>();
        }
        else
        {

        }

        BuffDebuffChangeCharacteristics();
        ChangePositionElements();
    }

    public void ChangeNumberTurnsDebuffEndTurn()
    {
        bool check = false;
        for (int i = 0; i < DebuffsIconsEndTurn.Count; i++)
        {
            if (DebuffsIconsEndTurn[i] != null)
            {

                DebuffsIconsEndTurn[i].GetComponent<IconsBuffDebuff>().numberTurns--;
                DebuffsIconsEndTurn[i].GetComponent<IconsBuffDebuff>().numberTurnsText.text = "" + DebuffsIconsEndTurn[i].GetComponent<IconsBuffDebuff>().numberTurns;

                if (DebuffsIconsEndTurn[i].GetComponent<IconsBuffDebuff>().numberTurns == 0)
                {
                    Destroy(DebuffsIconsEndTurn[i]);
                    DebuffsIconsEndTurn[i] = null;
                }
            }
        }

        

        for (int i = 0; i < DebuffsIconsEndTurn.Count; i++)
        {
            if (DebuffsIconsEndTurn[i] != null)
            {
                check = true;
                break;
            }
        }

        if(check == false)
        {
            DebuffsIconsEndTurn = new List<GameObject>();
        }
        else
        {

        }

        BuffDebuffChangeCharacteristics();
        ChangePositionElements();
    }

    #endregion 

    #region //Подсчет характеристик на поле битвы
    //метод подсчета скорости набора энергии для активации персонажа
    public void ChangeSpeed()
    {
        float _speed;
        if (Agility <= 20)
        {
            _speed = ((Agility * 0.5f) + 5) * ((100 + UpSpeed) / 100) * ((100 + DownSpeed) / 100);
        }
        else if (Agility > 20 && Agility <= 40)
        {
            _speed = (20 * 0.5f + (Agility - 20) * 0.5f + 5) * ((100 + UpSpeed) / 100) * ((100 + DownSpeed) / 100);
        }
        else
        {
            _speed = (20 * 0.5f + 20 * 0.3f + (Agility - 40) * 0.1f) * ((100 + UpSpeed) / 100) * ((100 + DownSpeed) / 100);
        }
        speed = _speed;
    }

    //метод подсчета шанса уклонения от физических навыков
    public void ChangeChanceAvade()
    {
        float _avaide;
        if (Agility <= 20)
        {
            _avaide = ((Agility * 0.5f) + 5) * ((100 + UpAvaid) / 100) * ((100 + DownAvaid) / 100);
        }
        else if (Agility > 20 && Agility <= 40)
        {
            _avaide = (20 * 0.5f + (Agility - 20) * 0.5f + 5) * ((100 + UpAvaid) / 100) * ((100 + DownAvaid) / 100);
        }
        else
        {
            _avaide = (20 * 0.5f + 20 * 0.3f + (Agility - 40) * 0.1f) * ((100 + UpAvaid) / 100) * ((100 + DownAvaid) / 100);
        }
        chanceAvaid = _avaide;
    }

    //метод подсчета шанса крит ударов для физических навыков
    public void ChangeChanceCrit()
    {
        float _crit;
        if (Agility <= 20)
        {
            _crit = ((Agility * 0.5f) + 5) * ((100 + UpSpeed) / 100) * ((100 + DownSpeed) / 100);
        }
        else if (Agility > 20 && Agility <= 40)
        {
            _crit = (20 * 0.5f + (Agility - 20) * 0.5f + 5) * ((100 + UpSpeed) / 100) * ((100 + DownSpeed) / 100);
        }
        else
        {
            _crit = (20 * 0.5f + 20 * 0.3f + (Agility - 40) * 0.1f) * ((100 + UpSpeed) / 100) * ((100 + DownSpeed) / 100);
        }
        chanceCrit = _crit;
    }

    public void ChangePhysicalAtk()
    {
        float _physicalAtk;
        _physicalAtk = Strenght * ((100 + UpPhysical) / 100) * ((100 + DownPhysical) / 100);
        physicalAtk = _physicalAtk;
    }

    public void ChangeMagicalAtk()
    {
        float _magicalAtk;
        _magicalAtk = Intellect;
        magicalAtk = _magicalAtk;
    }

    #endregion

    public void SetMaxHealth()
    {
        maxHp = Constitution * 3;
        sliderHp.maxValue = maxHp;
        sliderShild.maxValue = maxHp;
        sliderShild.value = 0;
        sliderGreatLookHp.maxValue = maxHp;
    }

    public void MetSliderEnabled()
    {
        gsliderHp.SetActive(false);
        gsliderSpeed.SetActive(false);
    }

    public void SetHealth()
    {
        sliderHp.value = realHp;

    }

    #region //получение урона, здоровья, барьера
    public void GetPhysicDamage(float damage)
    {
        float damageValue = 0;
        physicalDamageResist = 0 + (0.3f * physicalArmor);

        float Evade = Random.Range(0, 100.0f);

        if (chanceAvaid >= Evade)
        {
            Debug.Log(gameObject.name + " avaid");
            AvaidMethod();
        }
        else if (sliderShild.value != 0 && sliderShild.value >= damage)
        {
            sliderShild.value -= damage;
        }
        else if (sliderShild.value != 0 && sliderShild.value <= damage)
        {
            damageValue = damage - sliderShild.value;
            sliderShild.value = 0;
        }
        else if (sliderShild.value == 0)
        {
            damageValue = damage;
        }

        if(damageValue > 0)
        {
            realHp -= (damageValue * (1 - physicalDamageResist));
            SetHealth();
            SetTriggerAnimationGetDamage();
            CheckHp();
        }

        if(controlBattleField.GetComponent<BattleField>().meeleCount != 0)
        {
            controlBattleField.GetComponent<BattleField>().InvokeMethodMeeleCount();
        }
        else if (controlBattleField.GetComponent<BattleField>().rangeCount != 0)
        {
            controlBattleField.GetComponent<BattleField>().InvokeMethodRangeCount();
        }
                
    }

    public void GetMagicDamage(float damage)
    {
        float damageValue = 0;
        magicalDamageResist = 0 + (0.3f * magicalArmor);

        if (sliderShild.value != 0 && sliderShild.value >= damage)
        {
            sliderShild.value -= damage;
        }
        else if (sliderShild.value != 0 && sliderShild.value <= damage)
        {
            damageValue = damage - sliderShild.value;
            sliderShild.value = 0;
        }
        else if (sliderShild.value == 0)
        {
            damageValue = damage;
        }

        if (damageValue > 0)
        {
            realHp -= (damageValue * (1 - magicalDamageResist));
            SetHealth();
            SetTriggerAnimationGetDamage();
            CheckHp();
        }
    }

    public void GetShild(float shild)
    {
        sliderShild.value += shild;

        if (sliderShild.value >= sliderShild.maxValue)
        {
            sliderShild.value = sliderShild.maxValue;
        }
    }

    public void GetHeal(float heal)
    {
        realHp += heal;

        if (realHp >= maxHp)
        {
            realHp = maxHp;
        }

        SetHealth();
    }

    public void SetTriggerAnimationGetDamage()
    {
        animator.SetTrigger("GetDamage");
    }

    public void ResetTriggerAnimationGetDamage()
    {
        animator.ResetTrigger("GetDamage");
    }

    #endregion
    #region //активация в бою 

    public void MethodStartAction(GameObject target)
    {
        _target = target;
        _numberBarier = 0;
        _numberHeal = 0;
        targetList = new List<GameObject>();
    }

    public GameObject _target;
    public int _numberBarier;
    public int _numberHeal;

    //метод для создания барьера союзному персонажу до атаки, при прокачанном таланте в древе талантов
    public void BarierMethod()
    {
        if (activeTalant[8] == true)
        {
            Debug.Log("BarierMethod Activate");
            if(animator != null)
            {
                if (_target.GetComponent<CharactersPlayerOnBattlefield>())
                {
                    for (int i = _numberBarier; i < _target.GetComponent<CharactersPlayerOnBattlefield>().targetList.Count; i++)
                    {
                        int rand = Random.Range(0, 100);
                        Debug.Log("HealMethond random on Enemy: " + rand);
                        if (rand <= 50)
                        {
                            //место для вызова анимации
                            if (animator != null)
                            {
                                animator.SetInteger("NumberAnimation", 101);
                            }
                            else
                            {
                                BarierMethodActivate();
                            }
                            _numberBarier++;
                            return;
                        }
                        else
                        {
                            _numberBarier++;
                        }
                    }

                    if (_numberBarier == targetList.Count)
                    {
                        controlBattleField.GetComponent<BattleField>().ScenariyDeystviy();
                    }
                }
                else if (_target.GetComponent<EnemyMonsterOnBattleField>())
                {
                    for (int i = _numberBarier; i < targetList.Count; i++)
                    {
                        int rand = Random.Range(0, 100);
                        Debug.Log("BarierMethond random on Character:" + rand);
                        if (rand <= 50)
                        {
                            if(animator != null)
                            {
                                animator.SetInteger("NumberAnimation", 101);
                            }
                            else
                            {
                                BarierMethodActivate();
                            }
                            _numberBarier++;
                            return;
                        }
                        else
                        {                            
                            _numberBarier++;
                        }
                    }

                    if(_numberBarier == targetList.Count)
                    {
                        controlBattleField.GetComponent<BattleField>().ScenariyDeystviy();
                    }
                }
            }           
        }
    }
    public void BarierMethodActivate()
    {
        if (targetList[_numberBarier - 1].transform.GetChild(0).GetComponent<CharactersPlayerOnBattlefield>())
        {
            targetList[_numberBarier - 1].transform.GetChild(0).GetComponent<CharactersPlayerOnBattlefield>().GetShild(Intellect);
        }
        else if (targetList[_numberBarier - 1].transform.GetChild(0).GetComponent<EnemyMonsterOnBattleField>())
        {
            targetList[_numberBarier - 1].transform.GetChild(0).GetComponent<EnemyMonsterOnBattleField>().GetShild(Intellect);
        }
    }
    public void AvaidMethod()
    {
        //место для вызова анимации
        if(animator != null)
        {
            ControllAnimations(102);
        }
    }

    //метод контр атаки после уклонения от атаки(только против ближнего боя), при прокачанном таланте в древе талантов
    public void CounterAttackMethodActivate()
    {
        if (activeTalant[1] == true)
        {
            Debug.Log("ConterAttackMethod Activate");
            //место для вызова анимации
            if (animator != null)
            {
                ControllAnimations(103);
            }
        }
        else
        {
            controlBattleField.GetComponent<BattleField>().MeeleCount();
        }
    }

    public void CounterAttackAttackMethod()
    {
        if(_target.GetComponent<CharactersPlayerOnBattlefield>())
        {           
            _target.GetComponent<CharactersPlayerOnBattlefield>().GetPhysicDamage(physicalAtk);
            controlBattleField.GetComponent<BattleField>().MeeleCount();
        }
        else if (_target.GetComponent<EnemyMonsterOnBattleField>())
        {
            _target.GetComponent<EnemyMonsterOnBattleField>().GetPhysicDamage(physicalAtk);
            controlBattleField.GetComponent<BattleField>().MeeleCount();
        }
    }

    //метод для исцеления союзного персонажа после атаки, при прокачанном таланте в древе талантов
    public void HealMethod()
    {
        if (activeTalant[9] == true)
        {
            Debug.Log("HealMethod Activate");
            if(animator != null)
            {
                if (_target.GetComponent<CharactersPlayerOnBattlefield>())
                {
                    for (int i = _numberHeal; i < targetList.Count; i++)
                    {
                        int rand = Random.Range(0, 100);
                        Debug.Log("HealMethond random on Enemy:" + rand);
                        if (rand <= 50)
                        {
                            if (animator != null)
                            {
                                animator.SetInteger("NumberAnimation", 104);
                            }
                            else
                            {
                                HealMethodActivate();
                            }
                            _numberHeal++;

                            return;
                        }
                        else
                        {
                            _numberHeal++;
                        }
                    }

                    if (_numberHeal == targetList.Count)
                    {
                        controlBattleField.GetComponent<BattleField>().ScenariyDeystviy();
                    }
                }
                else if (_target.GetComponent<EnemyMonsterOnBattleField>())
                {
                    for (int i = _numberHeal; i < _target.GetComponent<EnemyMonsterOnBattleField>().targetList.Count; i++)
                    {
                        int rand = Random.Range(0, 100);
                        Debug.Log("HealMethond random on Character:" + rand);
                        if (rand <= 50)
                        {
                            if (animator != null)
                            {
                                animator.SetInteger("NumberAnimation", 104);
                            }
                            else
                            {
                                HealMethodActivate();
                            }
                            _numberHeal++;
                            return;
                        }
                        else
                        {
                            _numberHeal++;
                        }
                    }

                    if (_numberHeal == targetList.Count)
                    {
                        controlBattleField.GetComponent<BattleField>().ScenariyDeystviy();
                    }
                }
            }
            
        }
    }

    public void HealMethodActivate()
    {
        if(targetList[_numberHeal - 1].transform.GetChild(0).GetComponent<CharactersPlayerOnBattlefield>())
        {
            targetList[_numberHeal - 1].transform.GetChild(0).GetComponent<CharactersPlayerOnBattlefield>().GetHeal(Intellect);
        }
        else if (targetList[_numberHeal - 1].transform.GetChild(0).GetComponent<EnemyMonsterOnBattleField>())
        {
            targetList[_numberHeal - 1].transform.GetChild(0).GetComponent<EnemyMonsterOnBattleField>().GetHeal(Intellect);
        }
    }

    public void VigilanceMethod()
    {

    }

    #endregion

    #region //блок работы с аниматором
    public Animator animator;

    //контроллер у гуманоидных персонажей в зависимости от оружия в руках
    public void ControllAnimationsTree(int numberAnimation)
    {
        if (animator != null)
        {
            animator.SetInteger("NumberVariationsHandWeapons", numberAnimation);
        }
    }

    //контроллер анимации при старте сражения
    public void ControllAnimationsStartFight()
    {
        if (animator != null)
        {
            animator.SetBool("BattleField", true);
        }
    }

    //контроллер анимации
    public void ControllAnimations(int numberAnimation)
    {
        if (animator != null)
        {
            Debug.Log("Number Animation: " + numberAnimation);
            animator.SetInteger("NumberAnimation", numberAnimation);
        }
    }

    //Вызов для прехода анимации к атаке
    public void InvokeCanAttackMethod()
    {
        Invoke("CanAttack", 1f);
    }

    public void CanAttack()
    {
        Debug.Log("CanAttack method use");
        if (animator != null)
        {
            animator.SetBool("CanAttack", true);
        }
    }

    //Вызов для выхода анимации с атаки
    public void CannotAttack()
    {
        if (animator != null)
        {
            Debug.Log("CannotAttack method use");
            animator.SetBool("CanAttack", false);
        }
    }

    public void ControllAnimationsEndFight()
    {
        if (animator != null)
        {
            animator.SetBool("BattleField", false);
        }
    }

    //Тригер при получении урона
    public void ControllAnimationsStartGetDamage()
    {
        if (animator != null)
        {
            animator.SetTrigger("GetDamage");
        }
    }

    public void ControllAnimationsEndGetDamage()
    {
        if (animator != null)
        {
            animator.ResetTrigger("GetDamage");
        }
    }

    public void ControllAnimationsFightPhysicalMeele(int numberAnimation)
    {
        gameObject.transform.position = new Vector3(targetList[0].transform.position.x - 4, targetList[0].transform.position.y);
    }

    public void ControllAnimationsFightRange(int numberAnimation)
    {
        if (animator != null)
        {
            animator.SetInteger("NumberAnimation", numberAnimation);
            if (gameObject.GetComponent<ControlSetingsBody>().numberWeaponRightHand != 0 && gameObject.GetComponent<ControlSetingsBody>().numberWeaponLeftHand == 0)
            {
                animator.SetBool("OnlyRightOneHand", true);
            }
        }
    }
    #endregion

    public void Choise()
    {
        objectChoise.SetActive(true);
    }

    //метод создания снарядов магической способности
    public void CreateSpel(GameObject _target, int _i)
    {
        if (spelDatabase.SpelObjects[spel[numberSpel].Id].spelPrefab != null)
        {
            GameObject target = _target;
            GameObject SpelAdd = Instantiate(spelDatabase.SpelObjects[spel[numberSpel].Id].spelPrefab, gameObject.transform);
            float _x;
            float _y;

            //рандомизация позиции снаряда от кастующего персонажа
            if(_i == 0)
            {
                _x = Random.Range(0, -10f);
                _y = Random.Range(10f, 15f);
            }
            else if (_i / 2 == 0)
            {
                _x = Random.Range(-10f, -5f);
                _y = Random.Range(-10f, -5f);
            }
            else
            {
                _x = Random.Range(5f, 10f);
                _y = Random.Range(5f, 10f);
            }


            SpelAdd.transform.position = new Vector3(gameObject.transform.position.x + _x, gameObject.transform.position.y + _y, gameObject.transform.position.z);

            SpelAdd.transform.GetChild(0).GetComponent<MagicSpel>().Target = target;

            if (spelDatabase.SpelObjects[spel[numberSpel].Id].dataS.NumberDebuffBuff != -1)
            {
                SpelAdd.transform.GetChild(0).GetComponent<MagicSpel>().idDebuffBuff = spelDatabase.SpelObjects[spel[numberSpel].Id].dataS.NumberDebuffBuff;
            }

            SpelAdd.transform.GetChild(0).GetComponent<MagicSpel>().time = 0;//заменив 0 на _i можно задать время запуска снарядов
            SpelAdd.transform.GetChild(0).GetComponent<MagicSpel>().BezierPointPath();//путь к цели
            SpelAdd.transform.GetChild(0).GetComponent<MagicSpel>().damage = Intellect;//урон в зависимости от интелекта персанажа

            spelsList.Add(SpelAdd);           
        }
    }

    public void SpelsAttackActivate()
    {
        for(int i = 0; i < spelsList.Count; i++)
        {
            spelsList[i].transform.GetChild(0).GetComponent<MagicSpel>().Atack();
            spelsList[i] = null;
        }

        spelsList = new List<GameObject>();
    }

    #region //очистка баффов и дебаффов при смерти или использования способностей, предметов
    
    public void ClearAllBuffDebuff()
    {
        ClearBuffStartTurn("all");
        ClearBuffEndTurn("all");
        ClearDebuffStartTurn("all");
        ClearDebuffEndTurn("all");
    }

    //сделана для вариативности очистки баффов/дебаффов
    public void ClearBuffStartTurn(string clearType)
    {
        if(BuffsIconsStartTurn.Count != 0)
        {
            if(clearType == "all")
            {
                for(int i = 0; i < BuffsIconsStartTurn.Count; i++)
                {
                    Destroy(BuffsIconsStartTurn[i]);
                    BuffsIconsStartTurn[i] = null;
                }
            }

            BuffsIconsStartTurn = new List<GameObject>();
        }
    }

    public void ClearBuffEndTurn(string clearType)
    {
        if (BuffsIconsEndTurn.Count != 0)
        {
            if (clearType == "all")
            {
                for (int i = 0; i < BuffsIconsEndTurn.Count; i++)
                {
                    Destroy(BuffsIconsEndTurn[i]);
                    BuffsIconsEndTurn[i] = null;
                }
            }

            BuffsIconsEndTurn = new List<GameObject>();
        }
    }

    public void ClearDebuffStartTurn(string clearType)
    {
        if (DebuffsIconsStartTurn.Count != 0)
        {
            if (clearType == "all")
            {
                for (int i = 0; i < DebuffsIconsStartTurn.Count; i++)
                {
                    Destroy(DebuffsIconsStartTurn[i]);
                    DebuffsIconsStartTurn[i] = null;
                }
            }

            DebuffsIconsStartTurn = new List<GameObject>();
        }
    }

    public void ClearDebuffEndTurn(string clearType)
    {
        if (DebuffsIconsEndTurn.Count != 0)
        {
            if (clearType == "all")
            {
                for (int i = 0; i < DebuffsIconsEndTurn.Count; i++)
                {
                    Destroy(DebuffsIconsEndTurn[i]);
                    DebuffsIconsEndTurn[i] = null;
                }
            }

            DebuffsIconsEndTurn = new List<GameObject>();
        }
    }
    #endregion

    public void Unchoise()
    {
        objectChoise.SetActive(false);
    }
    public void CheckHp()
    {
        if (realHp <= 0)
        {
            realHp = 0;
            Dead = true;
            ClearAllBuffDebuff();
            MetSliderEnabled();          
        }

        controlBattleField.GetComponent<BattleField>().CheckBattlefieldResult();
    } 

    //метод для управления камеров и полем битвы для изменения ракурса обзора в зависимости от сценария действий(от действий персанажа игрока/персанажа противника)
    public void TransformCamera(int _number)
    {
        controlBattleField.GetComponent<BattleField>().ControllCameraAndBattleFild(_number);
    }

    #region //блок для пути движения персонажей к цели

    public GameObject bezierObj;

    public Transform P0;
    public Transform P1;
    public Transform P2;
    public Transform P3;

    public float t;

    public void Point()
    {
        //P0 = gameObject.transform.GetChild(0).GetComponent<Transform>();
        //P1 = gameObject.transform.GetChild(1).GetComponent<Transform>();
        //P2 = gameObject.transform.GetChild(2).GetComponent<Transform>();
        //P3 = gameObject.transform.GetChild(3).GetComponent<Transform>();
    }
    public void BezierPath()
    {
        if (P0 == null)
        {
            P0 = bezierObj.transform.GetChild(0).GetComponent<Transform>();
        }

        if (P1 == null)
        {
            P1 = bezierObj.transform.GetChild(1).GetComponent<Transform>();
        }

        if (P2 == null)
        {
            P2 = bezierObj.transform.GetChild(2).GetComponent<Transform>();
        }

        if (P3 == null)
        {
            P3 = bezierObj.transform.GetChild(3).GetComponent<Transform>();
        }

        BezierPointPath();
    }

    //метод прокладывания пути по 4 точкам кривой безье при использовании способностей ближнего боя
    public void BezierPointPath()
    {
        if (P0 != null)
        {
            P0.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }

        if (P3 != null)
        {
            if (targetList[0].transform.GetChild(0).GetComponent<CharactersPlayerOnBattlefield>())
            {
                P3.position = new Vector3(targetList[0].transform.GetChild(0).GetComponent<CharactersPlayerOnBattlefield>().posFace.position.x, targetList[0].transform.GetChild(0).GetComponent<CharactersPlayerOnBattlefield>().posFace.position.y, 0);
            }
            else if(targetList[0].GetComponent<EnemyMonsterOnBattleField>())
            {
                P3.position = new Vector3(targetList[0].GetComponent<EnemyMonsterOnBattleField>().posFace.position.x, targetList[0].GetComponent<EnemyMonsterOnBattleField>().posFace.position.y, 0);
            }           
        }

        if (P1 != null)
        {
            float _x = (targetList[0].transform.position.x - gameObject.transform.position.x) * 0.35f;
            float _y = (targetList[0].transform.position.y - gameObject.transform.position.y) * 0.35f;
            P1.position = new Vector3(P0.position.x + _x, P0.position.y + _y, gameObject.transform.position.z);
        }

        if (P2 != null)
        {
            float _x = (targetList[0].transform.position.x - gameObject.transform.position.x) * 0.72f;
            float _y = (targetList[0].transform.position.y - gameObject.transform.position.y) * 0.72f;
            P2.position = new Vector3(P0.position.x + _x, P0.position.y + _y, gameObject.transform.position.z);
        }
        Debug.Log("Проложили путь");
    }

    private void OnDrawGizmos()
    {
        if(P0 != null && P3 != null)
        {
            Vector3 preveousePoint = P0.position;

            int sigmentsNumber = 30;
            for (int i = 0; i < sigmentsNumber + 1; i++)
            {
                float paremetr = (float)i / sigmentsNumber;
                Vector3 point = Bezier.GetPoint(P0.position, P1.position, P2.position, P3.position, paremetr);
                Gizmos.DrawLine(preveousePoint, point);
                preveousePoint = point;
            }
        }
    }

    #endregion
}