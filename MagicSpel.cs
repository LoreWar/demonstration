using UnityEngine;

[ExecuteAlways]
public class MagicSpel : MonoBehaviour
{
    public enum TypeAttack
    {
        bezier,
        parabala,
        perpendiculer,
        instant,
        naklone
    }

    [SerializeField]
    private Animator anim;
    [SerializeField]
    private SpriteRenderer sprite;

    public TypeAttack typeAttack;

    public bool Attack = false;

    //цель для снаряда
    public GameObject Target;

    //урон снаряда и время через которое она полетит при атаке
    public float damage;
    public float time;

    //сылка на родительский объект 
    public GameObject mainObj;
    public GameObject bezierObj;

    //ссылки на точки позиции построения пути снаряда
    public Transform P0;
    public Transform P1;
    public Transform P2;
    public Transform P3;

    //id баффа/дебаффа если он есть у данного типа заклинания
    public int idDebuffBuff = -1;

    #region//блок для построения пути движения снаряда заклинания
    //[Range(0, 1)]
    public float t;

    public void Point()
    {
        P0 = gameObject.transform.GetChild(0).GetComponent<Transform>();
        P1 = gameObject.transform.GetChild(1).GetComponent<Transform>();
        P2 = gameObject.transform.GetChild(2).GetComponent<Transform>();
        P3 = gameObject.transform.GetChild(3).GetComponent<Transform>();
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

    //построение пути кривой безье в зависимости от типа атаки заклинания
    public void BezierPointPath()
    {
        if ((int)typeAttack == 0)
        {
            if (P0 != null)
            {
                P0.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            }

            if (P3 != null)
            {
                P3.position = new Vector3(Target.transform.position.x, Target.transform.position.y, Target.transform.position.z);
            }

            if (P1 != null)
            {
                float _x = (Target.transform.position.x - gameObject.transform.position.x) * 0.35f;
                float _y = Random.Range(P0.position.y - 15f, P0.position.y + 15f);
                P1.position = new Vector3(P0.position.x + _x, _y, gameObject.transform.position.z);
            }

            if (P2 != null)
            {
                float _x = (Target.transform.position.x - gameObject.transform.position.x) * 0.72f;
                float _y = Random.Range(P3.position.y - 15f, P3.position.y + 15f);
                P2.position = new Vector3(P0.position.x + _x, _y, gameObject.transform.position.z);
            }
        }
        else if((int)typeAttack == 1)
        {

        }
        else if ((int)typeAttack == 2)
        {
            if (P0 != null)
            {
                P0.position = new Vector3(Target.transform.position.x, Target.transform.position.y + 15, gameObject.transform.position.z);
            }

            if (P3 != null)
            {
                P3.position = new Vector3(Target.transform.position.x, Target.transform.position.y, Target.transform.position.z);
            }

            if (P1 != null)
            {
                float _y = P0.position.y * 0.72f;
                P1.position = new Vector3(P0.position.x, _y, gameObject.transform.position.z);
            }

            if (P2 != null)
            {               
                float _y = P3.position.y * 1.25f;
                P2.position = new Vector3(P0.position.x, _y, gameObject.transform.position.z);
            }
        }
        else if ((int)typeAttack == 3)
        {

        }
        else if ((int)typeAttack == 4)
        {

        }

    }

    //отрисовка пути снаряда в окне сцены
    private void OnDrawGizmos()
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
    #endregion

    //вызов атаки у снаряда через заданное время(если нужно его можно задать при создании снаряда)
    public void Atack()
    {
        Invoke("CanAtack", time);
    }

    public void CanAtack()
    {
        Attack = true;
        if (Attack == true)
        {
            anim.SetBool("Attack", true);
        }    
    }

    void Update()
    {
        if(Attack == true && t <= 1)
        {
            t += 1 * Time.deltaTime;
        }
        else
        {

        }

        //путь по которому движется снаряд
        transform.position = Bezier.GetPoint(P0.position, P1.position, P2.position, P3.position, t);
        //изменение угла поворота снаряда в зависимости от движения пути
        transform.rotation = Quaternion.LookRotation(Bezier.GetFirstDirection(P0.position, P1.position, P2.position, P3.position, t));

        if(t >= 1)
        {
            Debug.Log("TargetSpel: " + gameObject.name);

            if (Target.GetComponent<CharactersPlayerOnBattlefield>())
            {
                Target.GetComponent<CharactersPlayerOnBattlefield>().GetMagicDamage(damage);

                if(idDebuffBuff != -1 && Target.GetComponent<CharactersPlayerOnBattlefield>().Dead == false)
                {
                    Target.GetComponent<CharactersPlayerOnBattlefield>().CreateSlots(idDebuffBuff);
                }             
            }
            else
            {
                Target.GetComponent<EnemyMonsterOnBattleField>().GetMagicDamage(damage);

                if (idDebuffBuff != -1 && Target.GetComponent<EnemyMonsterOnBattleField>().Dead == false)
                {
                    Target.GetComponent<EnemyMonsterOnBattleField>().CreateSlots(idDebuffBuff);
                }
            }

            DestroyArrow();
        }
    }

    public void DestroyArrow()
    {
        Destroy(mainObj);
    }
}
