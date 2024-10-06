using UnityEngine;
using UnityEngine.EventSystems;

public class CharactersPlayerOnBattlefield : Character, IPointerClickHandler
{
    //номер в команде игрока
    public int numberInTeam;

    //переменные для управления движения игрока
    public bool runAttack = false;
    public bool runBack = false;

    //метод для настройки персанажа на старте поля битвы
    public void StartBattleSettings(int number)
    {
        //присвоения номера в команде, отвечает за расположение на поле битве, ссылку на навыки взаимодействия во время сражения и т.д.
        numberInTeam = number;

        //основные характеристики персанажа
        Strenght = storageTeam.GetComponent<StorageTeam>().fightTeamStat[number].GetComponent<CharacterScript>().Strenght;//отвечает за урон физических способностей, количество переносимых предметов
        Agility = storageTeam.GetComponent<StorageTeam>().fightTeamStat[number].GetComponent<CharacterScript>().Agility;//отвечает за скорость набора энергии, уклонения, шанса крит. удара у физ. способностей и т.д.
        Constitution = storageTeam.GetComponent<StorageTeam>().fightTeamStat[number].GetComponent<CharacterScript>().Constitution;//отвечает за максимальное здоровье персонажа, в планах +резист от дебафов
        Intellect = storageTeam.GetComponent<StorageTeam>().fightTeamStat[number].GetComponent<CharacterScript>().Intellect;//отвечает за урон/лечения/величину барьеров магических способностей, навыков, в планах +пасивно можно видеть снаряжение, способности, таланты у противников
        Charism = storageTeam.GetComponent<StorageTeam>().fightTeamStat[number].GetComponent<CharacterScript>().Charism;//отвечает за лидерские способности(не реализованы на данный момент)
        
        if(animator != null)
        {
            animator.SetBool("BattleField", true);
        }
        
        //заполнение выбранных способностей/заклинаний
        for (int i = 0; i < spel.Length; i++)
        {
            if (storageTeam.GetComponent<StorageTeam>().fightTeamStat[number].GetComponent<CharacterScript>().spel[i].Id != -1)
            {
                spel[i] = storageTeam.GetComponent<StorageTeam>().fightTeamStat[number].GetComponent<CharacterScript>().spel[i];
            }            
        }

        //заполнение надетых предметов
        for (int i = 0; i < item.Length; i++)
        {
            if (storageTeam.GetComponent<StorageTeam>().fightTeamStat[number].GetComponent<CharacterScript>().item[i].Id != -1)
            {
                item[i] = storageTeam.GetComponent<StorageTeam>().fightTeamStat[number].GetComponent<CharacterScript>().item[i];
            }
        }

        //заполнение изученными талантами в древе талантов
        for (int i = 0; i < storageTeam.GetComponent<StorageTeam>().fightTeamStat[number].GetComponent<CharacterScript>().activeTalant.Length; i++)
        {
            if (storageTeam.GetComponent<StorageTeam>().fightTeamStat[number].GetComponent<CharacterScript>().activeTalant[i] == 1)
            {
                activeTalant[i] = true;
            }
        }

        //остальные насиройки
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

    //метод выбора персанажа, срабатывает если у способности значение союзных целей >0
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

    //вызов метода движение к цели при использовании способности ближнего действия
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

    //вызов метода движение к стартовой позиции после использовании способности ближнего действия
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
            //активация сценария для дальнего боя
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
                if((int)spelDatabase.SpelObjects[spel[numberSpel].Id].spelRange == 0)//проверка дальности способности
                {
                    //активация сценария для ближнего боя
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

            //изменение значения слайдера энергии от скорости персанажа
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

    //метод для сброса значений после конца действий
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

    //метод для изменения курсора и выбор цели для атаки
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

    //добавления цели в список, проверка количества целей, появления кнопки подьверждения атаки, которая запустит сценарий действия
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

    //добавления цели в список, проверка количества целей, появления кнопки подьверждения атаки, которая запустит сценарий действия
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

    //добавления цели в список, проверка количества целей, появления кнопки подьверждения атаки, которая запустит сценарий действия
    public void LeaderShip(GameObject _target)
    {

    }

    //добавления цели в список, проверка количества целей, появления кнопки подьверждения атаки, которая запустит сценарий действия
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

    //добавления цели в список, проверка количества целей, появления кнопки подьверждения атаки, которая запустит сценарий действия
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

    //метод, который нанесет физ./маг. урон выбранным протиникам или исчелит/даст барьер союзным целям
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
