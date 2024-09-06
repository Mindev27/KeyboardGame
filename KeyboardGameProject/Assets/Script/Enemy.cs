using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Enemy : MonoBehaviour
{
    public float maxhp;//�ִ� hp
    public int attackDamage;//���� ������
    public int moveCycle;//�̵� ����Ŭ ������
    public int deathEXP;//�÷��̾� ȹ�� ����ġ
    public float hp;//���� hp

    private GameObject player;
    private GameObject moveCycleText;
    private GameObject enemyHpBar;//hpBar ������
    private int nowCycle;//���� ������
    private Dictionary<string, List<string>> adjList;
    private List<ManageKeyBoard.key> keyBoard;

    void Start()
    {
        ManageKeyBoard manageKeyBoard = FindObjectOfType<ManageKeyBoard>();
        enemyHpBar = transform.Find("Canvas").Find("Slider").gameObject;
        moveCycleText = this.transform.Find("Canvas").transform.Find("MoveCycleText").gameObject;
        player = GameObject.Find("Player");

        nowCycle = moveCycle;
        hp = maxhp;


        adjList = manageKeyBoard.adjList;
        keyBoard = manageKeyBoard.keyBoard;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToPlayer()
    {
        if (nowCycle > 0)
        {
            nowCycle -= 1;
            moveCycleText.GetComponent<TextMeshProUGUI>().text = (nowCycle).ToString();
            return;
        }

        

        bool moved = false;

        // ���� Ÿ�Ͽ��� �� �÷��� ����
        foreach (ManageKeyBoard.key key in keyBoard)
        {
            if (key.name.Equals(this.transform.parent.name))
            {
                key.isEnemy = false;
                break;
            }
        }

        // �÷��̾ ����� Ÿ���� ã��
        float minDistance = float.MaxValue;
        string bestKey = null;

        foreach (string keyName in adjList[this.transform.parent.name])
        {
            foreach (ManageKeyBoard.key key in keyBoard)
            {
                if (key.name.Equals(keyName) && !key.isEnemy)
                {
                    float distanceToPlayer = Vector2.Distance(GameObject.Find(keyName).transform.position, player.transform.position);
                    if (distanceToPlayer < minDistance)
                    {
                        minDistance = distanceToPlayer;
                        bestKey = keyName;
                        moved = true;
                    }
                }
            }
        }

        // ������ Ÿ�Ϸ� �̵�
        if (moved && bestKey != null)
        {
            MoveToKey(bestKey);
            foreach (ManageKeyBoard.key key in keyBoard)
            {
                if (key.name.Equals(bestKey))
                {
                    key.isEnemy = true;
                    break;
                }
            }
        }

        if (player.GetComponent<Player>().currentKey.Equals(bestKey))
        {
            Attack();
        }

        nowCycle = moveCycle;
        moveCycleText.GetComponent<TextMeshProUGUI>().text = (nowCycle).ToString();
    }

    public void MoveToKey(string key)
    {
        GameObject keyObj = GameObject.Find(key);
        if (keyObj != null)
        {
            this.transform.parent = keyObj.transform;
            this.transform.position = keyObj.transform.position;
        }
    }

    public void Attack()
    {
        player.GetComponent<PlayerInform>().PlayerDamaged((float)attackDamage);//�÷��̾� �� ������
    }

    public void GetDamage(int damage)//�÷��̾�� ������ �Ծ��� �� ó��
    {
        hp -= damage;
        if (hp <= 0)//����ġ ȹ�� �� �� ����
        {
            foreach (ManageKeyBoard.key key in keyBoard)
            {
                if (key.name.Equals(this.transform.parent.name))
                {
                    key.isEnemy = false;
                    break;
                }
            }

            FindAnyObjectByType<Experience>().AddEXP(deathEXP);
            Destroy(this.gameObject);
        }
        else
        {
            if (enemyHpBar.activeInHierarchy == false)
            {
                enemyHpBar.SetActive(true);
            }
            enemyHpBar.transform.GetComponentInChildren<Slider>().value = hp / maxhp;
        }
    }

}
