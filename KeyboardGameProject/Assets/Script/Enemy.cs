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
        enemyHpBar = transform.parent.Find("Canvas").Find("Slider").gameObject;
        moveCycleText = transform.parent.Find("Canvas").transform.Find("MoveCycleText").gameObject;
        player = GameObject.Find("Player");

        nowCycle = moveCycle;
        hp = maxhp;


        adjList = manageKeyBoard.adjList;
        keyBoard = manageKeyBoard.keyBoard;
        Rotate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToPlayer()
    {
        if (nowCycle > 0)
        {
            Rotate();
            nowCycle -= 1;
            moveCycleText.GetComponent<TextMeshProUGUI>().text = (nowCycle).ToString();
            return;
        }


        bool moved = false;

        // ���� Ÿ�Ͽ��� �� �÷��� ����
        foreach (ManageKeyBoard.key key in keyBoard)
        {
            if (key.name.Equals(this.transform.parent.parent.name))//�θ��� �θ� �ʿ�
            {
                key.isEnemy = false;
                break;
            }
        }

        // �÷��̾ ����� Ÿ���� ã��
        float minDistance = float.MaxValue;
        string bestKey = null;

        foreach (string keyName in adjList[this.transform.parent.parent.name])//�θ��� �θ� �ʿ�
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
            Rotate();
        }

        if (player.GetComponent<Player>().currentKey.Equals(bestKey))
        {
            Attack();
        }

        nowCycle = moveCycle;
        moveCycleText.GetComponent<TextMeshProUGUI>().text = (nowCycle).ToString();
    }

    private void Rotate()//���� ����(�÷��̾� ����)
    {
        //���� ���
        float angle = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x)
                            * Mathf.Rad2Deg - 180;//���� ������ ��ȯ �� 180�� ����(������ ����)
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward * 10);
    }

    public void MoveToKey(string key)
    {
        GameObject keyObj = GameObject.Find(key);
        if (keyObj != null)
        {
            transform.parent.parent = keyObj.transform;
            transform.parent.transform.position = keyObj.transform.position;
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
            Destroy(transform.parent.gameObject);
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
