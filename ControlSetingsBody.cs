using UnityEngine;


public class ControlSetingsBody : MonoBehaviour
{
    //================== Method find Object on create

    public GameObject storage;
    public GameObject character;
    public GameObject characterBodyGameObject;

    public void OnCreateBody(GameObject _character)
    {
        storage = GameObject.Find("Storage");
        character = _character;      
        ControlMethods();        
    }

    //================== 
    //ссылки на объекты хранения активируемых элементов внешности, брони, оружия
    public GameObject Body, Hair, bHair, Eyes, Puplis, Brows, Nose, Lips, aBody, aHead, aHand, aLegs, aFoots, wRightHand, wLeftHand;
    //номера дочерних объекток для активации снаряжения, оружия.
    public int numberHead, numberChest, numberHand, numberLegs, numberFoots, numberWeaponRightHand, numberWeaponLeftHand;

    public void StartGame()
    {
        if (character.GetComponent<CharacterScript>().item[0].Id != -1)
        {
            numberHead = character.GetComponent<CharacterScript>().item[0].number;
        }

        if (character.GetComponent<CharacterScript>().item[1].Id != -1)
        {
            numberChest = character.GetComponent<CharacterScript>().item[1].number;
        }

        if (character.GetComponent<CharacterScript>().item[2].Id != -1)
        {
            numberHand = character.GetComponent<CharacterScript>().item[2].number;
        }

        if (character.GetComponent<CharacterScript>().item[3].Id != -1)
        {
            numberLegs = character.GetComponent<CharacterScript>().item[3].number;
        }

        if (character.GetComponent<CharacterScript>().item[4].Id != -1)
        {
            numberFoots = character.GetComponent<CharacterScript>().item[4].number;
        }

        if (character.GetComponent<CharacterScript>().item[8].Id != -1)
        {
            numberWeaponRightHand = character.GetComponent<CharacterScript>().item[8].number;
        }

        if (character.GetComponent<CharacterScript>().item[9].Id != -1)
        {
            numberWeaponLeftHand = character.GetComponent<CharacterScript>().item[9].number;
        }
    }
    public void ControlMethods()
    {
        StartGame();

        ChangeHair(character.GetComponent<CharacterScript>().Hair, character.GetComponent<CharacterScript>().colorHair);
        ChangeEyes(character.GetComponent<CharacterScript>().Eyes, character.GetComponent<CharacterScript>().colorEyes);
        ChangeBrows(character.GetComponent<CharacterScript>().Brown, character.GetComponent<CharacterScript>().colorHair);
        ChangeNose(character.GetComponent<CharacterScript>().Nose, character.GetComponent<CharacterScript>().Skin);
        ChangeLips(character.GetComponent<CharacterScript>().Mouth, character.GetComponent<CharacterScript>().Skin);
        ChangeBodyColor(character.GetComponent<CharacterScript>().Skin);

        ControlUnEquipMethods();
        ControlEquipMethods();
    }

    public void ControlEquipMethods()
    {
        if (character.GetComponent<CharacterScript>().item[0].Id != -1)
        {
            ChangeHeadArmor(character.GetComponent<CharacterScript>().item[0].number);
        }
        else { ChangeHeadArmor(0); }

        if (character.GetComponent<CharacterScript>().item[1].Id != -1)
        {
            ChangeBodyArmor(character.GetComponent<CharacterScript>().item[1].number);
        }
        else { ChangeBodyArmor(0); }

        if (character.GetComponent<CharacterScript>().item[2].Id != -1)
        {
            ChangeHandArmor(character.GetComponent<CharacterScript>().item[2].number);
        }
        else { ChangeHandArmor(0); }

        if (character.GetComponent<CharacterScript>().item[3].Id != -1)
        {
            ChangeLegsArmor(character.GetComponent<CharacterScript>().item[3].number);
        }
        else { ChangeLegsArmor(0); }

        if (character.GetComponent<CharacterScript>().item[4].Id != -1)
        {
            ChangeFootsArmor(character.GetComponent<CharacterScript>().item[4].number);
        }
        else { ChangeFootsArmor(0); }

        if (character.GetComponent<CharacterScript>().item[8].Id != -1)
        {
            ChangeWeaponRH(character.GetComponent<CharacterScript>().item[8].number);
        }
        else { ChangeWeaponRH(0); }

        if (character.GetComponent<CharacterScript>().item[9].Id != -1)
        {          
            ChangeWeaponLH(character.GetComponent<CharacterScript>().item[9].number);
        }
        else { ChangeWeaponLH(0); }
    }
    public void ControlUnEquipMethods()
    {       
        if (character.GetComponent<CharacterScript>().item[0].Id != -1)
        {
            RemoveArmorHead(numberHead);
        }
        else { RemoveArmorHead(numberHead); }

        if (character.GetComponent<CharacterScript>().item[1].Id != -1)
        {
            RemoveArmorBody(numberChest);
        }
        else { Debug.Log("false"); RemoveArmorBody(numberChest); }

        if (character.GetComponent<CharacterScript>().item[2].Id != -1)
        {
            RemoveArmorHands(numberHand);
        }
        else { Debug.Log("false"); RemoveArmorHands(numberHand); }

        if (character.GetComponent<CharacterScript>().item[3].Id != -1)
        {
            RemoveArmorLegs(numberLegs);
        }
        else { Debug.Log("false"); RemoveArmorLegs(numberLegs); }

        if (character.GetComponent<CharacterScript>().item[4].Id != -1)
        {
            RemoveArmorFoots(numberFoots);
        }
        else { Debug.Log("false"); RemoveArmorFoots(numberFoots); }

        if (character.GetComponent<CharacterScript>().item[8].Id != -1)
        {
            RemoveWeaponRightHand(numberWeaponRightHand);
        }
        else { RemoveWeaponRightHand(numberWeaponRightHand); }

        if (character.GetComponent<CharacterScript>().item[9].Id != -1)
        {
            RemoveWeaponLeftHand(numberWeaponLeftHand);
        }
        else { RemoveWeaponLeftHand(numberWeaponLeftHand); }

    }

    //==================================================================

    #region Appearance

    public void ChangeHair(int n, int _n)
    {
        Hair.transform.GetChild(n).gameObject.SetActive(true);
        bHair.transform.GetChild(n).gameObject.SetActive(true);
        bHair.transform.GetChild(n).gameObject.GetComponent<SpriteRenderer>().color = new Color(storage.GetComponent<DataBase>().hairColor[_n].r, storage.GetComponent<DataBase>().hairColor[_n].g, storage.GetComponent<DataBase>().hairColor[_n].b, storage.GetComponent<DataBase>().hairColor[_n].a);
        Hair.transform.GetChild(n).gameObject.GetComponent<SpriteRenderer>().color = new Color(storage.GetComponent<DataBase>().hairColor[_n].r, storage.GetComponent<DataBase>().hairColor[_n].g, storage.GetComponent<DataBase>().hairColor[_n].b, storage.GetComponent<DataBase>().hairColor[_n].a);
        Brows.transform.GetChild(character.GetComponent<CharacterScript>().Brown).gameObject.GetComponent<SpriteRenderer>().color = new Color(storage.GetComponent<DataBase>().hairColor[_n].r, storage.GetComponent<DataBase>().hairColor[_n].g, storage.GetComponent<DataBase>().hairColor[_n].b, storage.GetComponent<DataBase>().hairColor[_n].a);
    }

    public void ChangeEyes(int n, int _n)
    {
        Eyes.transform.GetChild(n).gameObject.SetActive(true);
        Puplis.GetComponent<SpriteRenderer>().color = new Color(storage.GetComponent<DataBase>().eyesColor[_n].r, storage.GetComponent<DataBase>().eyesColor[_n].g, storage.GetComponent<DataBase>().eyesColor[_n].b, storage.GetComponent<DataBase>().eyesColor[_n].a);
    }

    public void ChangeBrows(int n, int _n)
    {
        Brows.transform.GetChild(n).gameObject.SetActive(true);
        Brows.transform.GetChild(n).gameObject.GetComponent<SpriteRenderer>().color = new Color(storage.GetComponent<DataBase>().hairColor[_n].r, storage.GetComponent<DataBase>().hairColor[_n].g, storage.GetComponent<DataBase>().hairColor[_n].b, storage.GetComponent<DataBase>().hairColor[_n].a);
    }

    public void ChangeNose(int n, int _n)
    {
        Nose.transform.GetChild(n).gameObject.SetActive(true);
        Nose.transform.GetChild(n).gameObject.GetComponent<SpriteRenderer>().color = new Color(storage.GetComponent<DataBase>().skinColor[_n].r, storage.GetComponent<DataBase>().skinColor[_n].g, storage.GetComponent<DataBase>().skinColor[_n].b, storage.GetComponent<DataBase>().skinColor[_n].a);
    }

    public void ChangeLips(int n, int _n)
    {
        Lips.transform.GetChild(n).gameObject.SetActive(true);
        Lips.transform.GetChild(n).gameObject.GetComponent<SpriteRenderer>().color = new Color(storage.GetComponent<DataBase>().skinColor[_n].r, storage.GetComponent<DataBase>().skinColor[_n].g, storage.GetComponent<DataBase>().skinColor[_n].b, storage.GetComponent<DataBase>().skinColor[_n].a);
    }

    public void ChangeBodyColor(int _n)
    {
        for(int i = 0; i < Body.transform.childCount; i++)
        {
            Body.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().color = new Color(storage.GetComponent<DataBase>().skinColor[_n].r, storage.GetComponent<DataBase>().skinColor[_n].g, storage.GetComponent<DataBase>().skinColor[_n].b, storage.GetComponent<DataBase>().skinColor[_n].a);
        }
    }
    #endregion

    //==================================================================

    #region Equipments

    public void ChangeHeadArmor(int _n)
    {      
        aHead.transform.GetChild(_n).gameObject.SetActive(true);
    }

    public void ChangeBodyArmor(int _n)
    {
        aBody.transform.GetChild(_n).gameObject.SetActive(true);
    }

    public void ChangeHandArmor(int _n)
    {
        aHand.transform.GetChild(_n).gameObject.SetActive(true);
    }

    public void ChangeLegsArmor(int _n)
    {
        aLegs.transform.GetChild(_n).gameObject.SetActive(true);
    }

    public void ChangeFootsArmor(int _n)
    {
        aFoots.transform.GetChild(_n).gameObject.SetActive(true);
    }

    public void ChangeWeaponRH(int _n)
    {
        wRightHand.transform.GetChild(_n).gameObject.SetActive(true);
    }

    public void ChangeWeaponLH(int _n)
    {
        wLeftHand.transform.GetChild(_n).gameObject.SetActive(true);
    }
    #endregion

    //==================================================================

     #region RemoveEquipments
    public void RemoveArmorHead(int _n)
    {
        if (aHead.transform.GetChild(_n).gameObject.activeSelf)
        {
            aHead.transform.GetChild(_n).gameObject.SetActive(false);
        }

        if (character.GetComponent<CharacterScript>().item[0].Id != -1)
        {
            numberHead = character.GetComponent<CharacterScript>().item[0].number;
        }
    }

    public void RemoveArmorBody(int _n)
    {
        if (aBody.transform.GetChild(_n).gameObject.activeSelf)
        {
            aBody.transform.GetChild(_n).gameObject.SetActive(false);
        }

        if (character.GetComponent<CharacterScript>().item[1].Id != -1)
        {
            numberChest = character.GetComponent<CharacterScript>().item[1].number;
        }
    }

    public void RemoveArmorHands(int _n)
    {
        if (aHand.transform.GetChild(_n).gameObject.activeSelf)
        {
            aHand.transform.GetChild(_n).gameObject.SetActive(false);
        }

        if (character.GetComponent<CharacterScript>().item[2].Id != -1)
        {
            numberHand = character.GetComponent<CharacterScript>().item[2].number;
        }
    }

    public void RemoveArmorLegs(int _n)
    {
        if (aLegs.transform.GetChild(_n).gameObject.activeSelf)
        {
            aLegs.transform.GetChild(_n).gameObject.SetActive(false);
        }

        if (character.GetComponent<CharacterScript>().item[3].Id != -1)
        {
            numberLegs = character.GetComponent<CharacterScript>().item[3].number;
        }
    }

    public void RemoveArmorFoots(int _n)
    {
        if (aFoots.transform.GetChild(_n).gameObject.activeSelf)
        {
            aFoots.transform.GetChild(_n).gameObject.SetActive(false);
        }

        if (character.GetComponent<CharacterScript>().item[4].Id != -1)
        {
            numberFoots = character.GetComponent<CharacterScript>().item[4].number;
        }
    }

    public void RemoveWeaponRightHand(int _n)
    {
        if (wRightHand.transform.GetChild(_n).gameObject.activeSelf)
        {
            wRightHand.transform.GetChild(_n).gameObject.SetActive(false);
        }

        if (character.GetComponent<CharacterScript>().item[8].Id != -1)
        {
            numberWeaponRightHand = character.GetComponent<CharacterScript>().item[8].number;
        }
    }

    public void RemoveWeaponLeftHand(int _n)
    {
        if (wLeftHand.transform.GetChild(_n).gameObject.activeSelf)
        {
            wLeftHand.transform.GetChild(_n).gameObject.SetActive(false);
        }

        if (character.GetComponent<CharacterScript>().item[9].Id != -1)
        {
            numberWeaponLeftHand = character.GetComponent<CharacterScript>().item[9].number;
        }
    }

    #endregion
}
