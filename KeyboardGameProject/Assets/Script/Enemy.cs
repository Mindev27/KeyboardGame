using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float maxHp;
    public int attackDamage;
    public int moveCycle;
    public int deathEXP;

    private float hp;
    private int nowCycle;
    private Dictionary<string, List<string>> adjList;
    private List<ManageKeyBoard.key> keyBoard;

    void Start()
    {
        ManageKeyBoard manageKeyBoard = FindObjectOfType<ManageKeyBoard>();
        deathEXP = 1;
        adjList = manageKeyBoard.adjList;
        nowCycle = moveCycle;
        keyBoard = manageKeyBoard.keyBoard;
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToPlayer()//�÷��̾� ���ؼ� �̵�
    {
        if(nowCycle > 0)//�ڽ��� �̵� ����Ŭ ���� ���缭 �ൿ
        {
            nowCycle -= 1;
            return;
        }

        bool addOK = false;// �ӽ�

        foreach (string keyName in adjList[this.transform.parent.name])//�ֺ� ĭ �� �ٸ� �� ���� ������ �̵�
        {
            foreach (ManageKeyBoard.key key in keyBoard)
            {

                if (key.name.Equals(keyName))
                {
                    if (!key.isEnemy)
                    {
                        MoveToKey(keyName);
                        key.isEnemy = true;
                        addOK = true;
                    }
                    break;
                }
            }
            if (addOK)
                break;
        }

        foreach (ManageKeyBoard.key key in keyBoard)
        {
            if (key.name.Equals(this.transform.parent.name))
            {
                key.isEnemy = false;
                break;
            }
        }
        nowCycle = moveCycle;
    }
    public void MoveToKey(string key)//�̵��� Ű���� ������Ʈ�� �θ� �̵�
    {
        //key = "back_" + key;
        GameObject keyObj = GameObject.Find(key);
        if (keyObj != null)
        {
            this.transform.parent = keyObj.transform;
            this.transform.position = keyObj.transform.position;
        }
    }

    public void GetDamage(int damage)//�÷��̾�� ������ �Ծ��� �� ó��
    {
        hp -= damage;
        if (hp <= 0)//����ġ ȹ�� �� �� ����
        {
            FindAnyObjectByType<Experience>().AddEXP(deathEXP);
            foreach (ManageKeyBoard.key key in keyBoard)
            {
                if (key.name.Equals(this.transform.parent.name))
                {
                    key.isEnemy = false;
                    break;
                }
            }
            Destroy(this.gameObject);
        }
        else//��� hpbar ����
        {
            GameObject hpPrefab = Resources.Load<GameObject>("Prefab/" + "EnemyHp");//������ ��������
            GameObject hpBar = Instantiate(hpPrefab, this.transform);
            hpBar.SetActive(true);
            hpBar.transform.Find("EnemyHp").GetComponent<Slider>().value = (hp / maxHp);//���� hp������ ���� hpBar ����
        }
    }

}
