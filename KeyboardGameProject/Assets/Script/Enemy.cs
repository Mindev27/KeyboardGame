using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp;
    public int attackDamage;
    public int moveCycle;
    public int deathEXP;

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
                    float distanceToPlayer = Vector2.Distance(GameObject.Find(keyName).transform.position, GameObject.Find("Player").transform.position);
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

        nowCycle = moveCycle;
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


    public void GetDamage(int damage)//�÷��̾�� ������ �Ծ��� �� ó��
    {
        hp -= damage;
        if (hp <= 0)//����ġ ȹ�� �� �� ����
        {
            FindAnyObjectByType<Experience>().AddEXP(deathEXP);
            Destroy(this.gameObject);
        }
    }

}
