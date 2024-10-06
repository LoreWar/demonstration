using UnityEngine;
using UnityEngine.EventSystems;

public class CharactersPlayerOnBattlefield : Character, IPointerClickHandler
{
    //����� � ������� ������
    public int numberInTeam;

    //���������� ��� ���������� �������� ������
    public bool runAttack = false;
    public bool runBack = false;

    //����� ��� ��������� ��������� �� ������ ���� �����
    public void StartBattleSettings(int number)
    {
        //���������� ������ � �������, �������� �� ������������ �� ���� �����, ������ �� ������ �������������� �� ����� �������� � �.�.
        numberInTeam = number;

        //�������� �������������� ���������
        Strenght = storageTeam.GetComponent<StorageTeam>().fightTeamStat[number].GetComponent<CharacterScript>().Strenght;//�������� �� ���� ���������� ������������, ���������� ����������� ���������
        Agility = storageTeam.GetComponent<StorageTeam>().fightTeamStat[number].GetComponent<CharacterScript>().Agility;//�������� �� �������� ������ �������, ���������, ����� ����. ����� � ���. ������������ � �.�.
        Constitution = storageTeam.GetComponent<StorageTeam>().fightTeamStat[number].GetComponent<CharacterScript>().Constitution;//�������� �� ������������ �������� ���������, � ������ +������ �� �������
        Intellect = storageTeam.GetComponent<StorageTeam>().fightTeamStat[number].GetComponent<CharacterScript>().Intellect;//�������� �� ����/�������/�������� �������� ���������� ������������, �������, � ������ +������� ����� ������ ����������, �����������, ������� � �����������
        Charism = storageTeam.GetComponent<StorageTeam>().fightTeamStat[number].GetComponent<CharacterScript>().Charism;//�������� �� ��������� �����������(�� ����������� �� ������ ������)
        
        if(animator != null)
        {
            animator.SetBool("BattleField", true);
        }
        
        //���������� ��������� ������������/����������
        for (int i = 0; i < spel.Length; i++)
        {
            if (storageTeam.GetComponent<StorageTeam>().fightTeamStat[number].GetComponent<CharacterScript>().spel[i].Id != -1)
            {
                spel[i] = storageTeam.GetComponent<StorageTeam>().fightTeamStat[number].GetComponent<CharacterScript>().spel[i];
            }            
        }

        //���������� ������� ���������
        for (int i = 0; i < item.Length; i++)
        {
            if (storageTeam.GetComponent<StorageTeam>().fightTeamStat[number].GetComponent<CharacterScript>().item[i].Id != -1)
            {
                item[i] = storageTeam.GetComponent<StorageTeam>().fightTeamStat[number].GetComponent<CharacterScript>().item[i];
            }
        }

        //���������� ���������� ��������� � ����� ��������
        for (int i = 0; i < storageTeam.GetComponent<StorageTeam>().fightTeamStat[number].GetComponent<CharacterScript>().activeTalant.Length; i++)
        {
            if (storageTeam.GetComponent<StorageTeam>().fightTeamStat[number].GetComponent<CharacterScript>().activeTalant[i] == 1)
            {
                activeTalant[i] = true;
            }
        }

        //��������� ���������
        realHp = storageTeam.GetComponent<StorageTeam>().fightTeamStat[number].GetComponent<CharacterScript>().Hp;

        SetMaxHealth();

        if (realHp > maxHp)
        {
            realHp = maxHp;
        }

        SetHealth();
        CharacteristicsInBattle();

        sliderSpeed.maxValue = 100.0f;
        sliderSpeed.value = 0;
        ControllAnimations(0);
    }

    //����� ������ ���������, ����������� ���� � ����������� �������� ������� ����� >0
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            if (spelWindow.activeSelf)
            {
                spelWindow.GetComponent<SpelWindowInBattle>().characterAttack.GetComponent<CharactersPlayerOnBattlefield>().AttackMethod(gameObject);
                Choise();
            }
            else { }
        }
        else if (pointerEventData.button == PointerEventData.InputButton.Right)
        {
            if (spelWindow.activeSelf)
            {

            }
            else { }
        }
    }

    //����� ������ �������� � ���� ��� ������������� ����������� �������� ��������
    public void InvokeMethodRunAttack()
    {
        Invoke("RunAttack", 1f);
    }

    public void RunAttack()
    {
        if (runAttack == false)
        {
            runAttack = true;
            controlBattleField.GetComponent<BattleField>().ControllCameraAndBattleFild(0);
        }
        else
        {
            runAttack = false;
            controlBattleField.GetComponent<BattleField>().ControllCameraAndBattleFild(0);
        }
    }

    //����� ������ �������� � ��������� ������� ����� ������������� ����������� �������� ��������
    public void InvokeMethodRunBack()
    {
        Invoke("RunBack", 1f);
    }

    public void RunBack()
    {
        if (runBack == false)
        {
            runBack = true;
            controlBattleField.GetComponent<BattleField>().ControllCameraAndBattleFild(0);
        }
        else
        {
            runBack = false;
            controlBattleField.GetComponent<BattleField>().ControllCameraAndBattleFild(0);
        }
    }
    
    public void ScenariyForRange()
    {
        if ((int)spelDatabase.SpelObjects[spel[numberSpel].Id].spelRange == 1)
        {
            //��������� �������� ��� �������� ���
            controlBattleField.GetComponent<BattleField>().Normal(false, numberInTeam);
        }
    }

    void Update()
    {
        SetHealth();
        GreatLooksHp();
 
        if (Dead == false)
        {
            if (runAttack == true && t <= 1)
            {
                t += 1 * Time.deltaTime;
            }
            
            if(t > 1)
            {
                runAttack = false;
                t = 1;
                if((int)spelDatabase.SpelObjects[spel[numberSpel].Id].spelRange == 0)//�������� ��������� �����������
                {
                    //��������� �������� ��� �������� ���
                    controlBattleField.GetComponent<BattleField>().Normal(true, numberInTeam);
                }
            }           

            if (runBack == true && t >= 0)
            {
                t -= 1 * Time.deltaTime;
            }
            else if (t < 0 && runAttack == false)
            {
                runBack = false;
                ControllAnimations(0);
                t = 0;
            }

            gameObject.transform.position = Bezier.GetPoint(P0.position, P1.position, P2.position, P3.position, t);

            //��������� �������� �������� ������� �� �������� ���������
            if (controlBattleField.GetComponent<BattleField>().endTurn == true && sliderSpeed.value < sliderSpeed.maxValue)
            {
                sliderSpeed.value += speed * Time.deltaTime;
            }
            else if (controlBattleField.GetComponent<BattleField>().endTurn == true && sliderSpeed.value >= sliderSpeed.maxValue)
            {
                spelWindow.SetActive(true);
                spelWindow.GetComponent<SpelWindowInBattle>().ChangeCharacter(gameObject);
                controlBattleField.GetComponent<BattleField>().endTurn = false;
                sliderSpeed.value = sliderSpeed.maxValue;
            }
        }
        else if (controlBattleField.GetComponent<BattleField>().battleEnded == true || Dead == true) { sliderSpeed.value = 0; }    
    }

    //����� ��� ������ �������� ����� ����� ��������
    public void NormalZnach()
    {
        n = 0;
        numberSpel = 0;
        numberHero = 0;

        for (int i = 0; i < spel.Length; i++)
        {
            if (spel[i].Id != -1)
            {
                n++;
            }
        }

        for (int i = 0; i < controlBattleField.GetComponent<BattleField>().hero.Count; i++)
        {
            if (controlBattleField.GetComponent<BattleField>().hero[i] != null && controlBattleField.GetComponent<BattleField>().hero[i].GetComponent<CharactersPlayerOnBattlefield>().Dead == false)
            {
                numberHero++;
                characterEnemy.Add(controlBattleField.GetComponent<BattleField>().hero[i]);
            }
        }

        for (int i = 0; i < controlBattleField.GetComponent<BattleField>().enemy.Count; i++)
        {
            if (controlBattleField.GetComponent<BattleField>().enemy[i] != null && controlBattleField.GetComponent<BattleField>().enemy[i].GetComponent<EnemyMonsterOnBattleField>().Dead == false)
            {
                numberEnemy++;
                characterAlly.Add(controlBattleField.GetComponent<BattleField>().enemy[i]);
            }
        }
    }

    //����� ��� ��������� ������� � ����� ���� ��� �����
    public void AttackMethod(GameObject _target)
    {
        if ((int)spelDatabase.SpelObjects[spel[numberSpel].Id].spelType == 0)
        {
            PhysicalAttack(_target);
            Cursor.SetCursor(spelWindow.GetComponent<SpelWindowInBattle>().cursor[1], spelWindow.GetComponent<SpelWindowInBattle>().hotSpot, spelWindow.GetComponent<SpelWindowInBattle>().cursorMode);
        }
        else if ((int)spelDatabase.SpelObjects[spel[numberSpel].Id].spelType == 1)
        {
            MagicalAttack(_target);
            Cursor.SetCursor(spelWindow.GetComponent<SpelWindowInBattle>().cursor[1], spelWindow.GetComponent<SpelWindowInBattle>().hotSpot, spelWindow.GetComponent<SpelWindowInBattle>().cursorMode);
        }
        else if ((int)spelDatabase.SpelObjects[spel[numberSpel].Id].spelType == 2)
        {
            LeaderShip(_target);
        }
        else if ((int)spelDatabase.SpelObjects[spel[numberSpel].Id].spelType == 3)
        {
            HealingSpel(_target);
            Cursor.SetCursor(spelWindow.GetComponent<SpelWindowInBattle>().cursor[2], spelWindow.GetComponent<SpelWindowInBattle>().hotSpot, spelWindow.GetComponent<SpelWindowInBattle>().cursorMode);
        }
        else if ((int)spelDatabase.SpelObjects[spel[numberSpel].Id].spelType == 4)
        {
            ProtectingSpel(_target);
            Cursor.SetCursor(spelWindow.GetComponent<SpelWindowInBattle>().cursor[2], spelWindow.GetComponent<SpelWindowInBattle>().hotSpot, spelWindow.GetComponent<SpelWindowInBattle>().cursorMode);
        }

        
    }

    //���������� ���� � ������, �������� ���������� �����, ��������� ������ ������������� �����, ������� �������� �������� ��������
    public void PhysicalAttack(GameObject _target)
    {
        numberTargetEnemy = spel[numberSpel].NumberAttack;
        target = _target;

        targetList.Add(target);

        if (targetList.Count == numberTargetEnemy)
        {
            buttonAtack.SetActive(true);
        }
    }

    //���������� ���� � ������, �������� ���������� �����, ��������� ������ ������������� �����, ������� �������� �������� ��������
    public void MagicalAttack(GameObject _target)
    {
        numberTargetEnemy = spel[numberSpel].NumberAttack;
        target = _target;

        if (numberTargetEnemy >= 10)
        {
            for (int i = 0; i < controlBattleField.GetComponent<BattleField>().enemy.Count; i++)
            {
                if (controlBattleField.GetComponent<BattleField>().enemy[i] != null && controlBattleField.GetComponent<BattleField>().enemy[i].GetComponent<EnemyMonsterOnBattleField>().Dead == false)
                {

                    target = controlBattleField.GetComponent<BattleField>().enemy[i];
                    targetList.Add(target);
                }
            }
        }
        else if (numberTargetEnemy < 10)
        {
            targetList.Add(target);
        }

        if (targetList.Count == numberTargetEnemy)
        {
            buttonAtack.SetActive(true);
        }
    }

    //���������� ���� � ������, �������� ���������� �����, ��������� ������ ������������� �����, ������� �������� �������� ��������
    public void LeaderShip(GameObject _target)
    {

    }

    //���������� ���� � ������, �������� ���������� �����, ��������� ������ ������������� �����, ������� �������� �������� ��������
    public void HealingSpel(GameObject _target)
    {
        numberTargetAlly = spel[numberSpel].NumberAlly;

        target = _target;

        if (numberTargetAlly >= 10)
        {
            for (int i = 0; i < controlBattleField.GetComponent<BattleField>().hero.Count; i++)
            {
                if (controlBattleField.GetComponent<BattleField>().hero[i] != null && controlBattleField.GetComponent<BattleField>().hero[i].GetComponent<CharactersPlayerOnBattlefield>().Dead == false)
                {
                    target = controlBattleField.GetComponent<BattleField>().hero[i];
                    targetList.Add(target);
                }
            }
        }
        else if (numberTargetAlly < 10)
        {
            if(targetList.Count < numberTargetAlly)
            {
                targetList.Add(target);
            }
        }

        if (targetList.Count == numberTargetEnemy)
        {
            buttonAtack.SetActive(true);
        }
    }

    //���������� ���� � ������, �������� ���������� �����, ��������� ������ ������������� �����, ������� �������� �������� ��������
    public void ProtectingSpel(GameObject _target)
    {
        numberTargetAlly = spel[numberSpel].NumberAlly;

        target = _target;

        if (numberTargetAlly >= 10)
        {
            for (int i = 0; i < controlBattleField.GetComponent<BattleField>().hero.Count; i++)
            {
                if (controlBattleField.GetComponent<BattleField>().hero[i] != null && controlBattleField.GetComponent<BattleField>().hero[i].GetComponent<CharactersPlayerOnBattlefield>().Dead == false)
                {
                    target = controlBattleField.GetComponent<BattleField>().hero[i];
                    targetList.Add(target);
                }
            }
        }
        else if (numberTargetAlly < 10)
        {
            if (targetList.Count < numberTargetAlly)
            {
                targetList.Add(target);
            }
        }

        if (targetList.Count == numberTargetAlly)
        {
            buttonAtack.SetActive(true);
        }
    }

    //�����, ������� ������� ���./���. ���� ��������� ���������� ��� �������/���� ������ ������� �����
    public void MethodActivate()
    {
        if ((int)spelDatabase.SpelObjects[spel[numberSpel].Id].spelType == 0)
        {
            for (int i = 0; i < targetList.Count; i++)
            {
                targetList[i].GetComponent<EnemyMonsterOnBattleField>().Unchoise();
                targetList[i].GetComponent<EnemyMonsterOnBattleField>().GetPhysicDamage(Strenght);              
            }
           
            if(spelDatabase.SpelObjects[spel[numberSpel].Id].dataS.NumberDebuffBuff != -1)
            {
                if ((int)buffdebuffDatabase.ItemObjectsBuffDebuff[spelDatabase.SpelObjects[spel[numberSpel].Id].dataS.NumberDebuffBuff].typeBuffOrDebuff == 0)
                {
                    CreateSlots(spelDatabase.SpelObjects[spel[numberSpel].Id].dataS.NumberDebuffBuff);
                }
                else
                {
                    for (int i = 0; i < targetList.Count; i++)
                    {                        
                        targetList[i].GetComponent<EnemyMonsterOnBattleField>().CreateSlots(spelDatabase.SpelObjects[spel[numberSpel].Id].dataS.NumberDebuffBuff);
                    }
                }
            }
        }
        else if ((int)spelDatabase.SpelObjects[spel[numberSpel].Id].spelType == 1)
        {
            if (spelDatabase.SpelObjects[spel[numberSpel].Id].spelPrefab != null)
            {
                for (int i = 0; i < targetList.Count; i++)
                {
                    CreateSpel(targetList[i], i);
                    targetList[i].GetComponent<EnemyMonsterOnBattleField>().Unchoise();
                }
               
            }
            else
            {
                for (int i = 0; i < targetList.Count; i++)
                {
                    targetList[i].GetComponent<EnemyMonsterOnBattleField>().GetMagicDamage(Intellect);
                    targetList[i].GetComponent<EnemyMonsterOnBattleField>().CreateSlots(spelDatabase.SpelObjects[spel[numberSpel].Id].dataS.NumberDebuffBuff);
                    targetList[i].GetComponent<EnemyMonsterOnBattleField>().Unchoise();
                }
            }

            spel[numberSpel].Cooldown = spelDatabase.SpelObjects[spel[numberSpel].Id].dataS.Cooldown;
        }
        else if ((int)spelDatabase.SpelObjects[spel[numberSpel].Id].spelType == 2)
        {
            
        }
        else if ((int)spelDatabase.SpelObjects[spel[numberSpel].Id].spelType == 3)
        {
            if (spelDatabase.SpelObjects[spel[numberSpel].Id].spelPrefab != null)
            {
                for (int i = 0; i < targetList.Count; i++)
                {
                    CreateSpel(targetList[i], i);
                }
            }
            else
            {
                for (int i = 0; i < targetList.Count; i++)
                {
                    targetList[i].GetComponent<CharactersPlayerOnBattlefield>().GetHeal(Intellect);
                    targetList[i].GetComponent<CharactersPlayerOnBattlefield>().Unchoise();
                }
            }

        }
        else if ((int)spelDatabase.SpelObjects[spel[numberSpel].Id].spelType == 4)
        {
            if (spelDatabase.SpelObjects[spel[numberSpel].Id].spelPrefab != null)
            {
                for (int i = 0; i < targetList.Count; i++)
                {
                    CreateSpel(targetList[i], i);
                }
            }
            else
            {
                for (int i = 0; i < targetList.Count; i++)
                {
                    targetList[i].GetComponent<CharactersPlayerOnBattlefield>().GetShild(Intellect);
                    targetList[i].GetComponent<CharactersPlayerOnBattlefield>().Unchoise();
                }
            }

        }
    }
}
