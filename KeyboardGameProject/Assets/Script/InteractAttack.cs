using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class InteractAttack : MonoBehaviour
{
    public GameObject player;
    public string currentKey; // ���� ��ġ
    public GameObject attackPrefab;
    public AttackData attackdata;

    private Dictionary<string, List<string>> adjList; // ��������Ʈ
    private List<ManageKeyBoard.key> keyBoard;
    private Stack<int> attackStack;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitializeKeyBoard());
        attackStack = new Stack<int>();
        JsonLoad();
    }

    private IEnumerator InitializeKeyBoard()
    {
        // ManageKeyBoard �ν��Ͻ��� ã�� adjList�� ������
        ManageKeyBoard manageKeyBoard = null;
        while (manageKeyBoard == null)
        {
            manageKeyBoard = FindObjectOfType<ManageKeyBoard>();
            yield return null; // �� ������ ��ٸ�
        }

        while (manageKeyBoard.keyBoard == null || manageKeyBoard.keyBoard.Count == 0)
        {
            yield return null; // �� ������ ��ٸ�
        }

        adjList = manageKeyBoard.adjList;
        keyBoard = manageKeyBoard.keyBoard;
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))//�����̽��� �Է� �ޱ�
        {
            Input_Attack();
        }
    }

    private void Input_Attack()//���� �Է� �ޱ�
    {
        if(attackStack.Count < 1)//���� ������ ����
        {
            Debug.Log("���� �������");
            return;
        }
        for (int i = 1; i <= 3; ++i)//���� ����
        {
            GameObject.Find("back_space").GetComponent<Transform>().Find("Attack" + i.ToString()).GetComponent<SpriteRenderer>().sprite = null;
        }

        Player player = FindObjectOfType<Player>();
        currentKey = player.currentKey;

        string[] list = { };

        foreach(var pattern in attackdata.attackPatterns)
        {
            if (pattern.Key.Equals(currentKey))
            {
                switch (attackStack.Pop())
                {
                    case 1:
                        switch (attackStack.Count)
                        {
                            case 0:
                                list = pattern.Value.horizonOne;
                                break;
                            case 1:
                                list = pattern.Value.horizonTwo;
                                break;
                            case 2:
                                list = pattern.Value.horizonThree;
                                break;
                            default:
                                break;
                        }
                        break;
                    case 2:
                        switch (attackStack.Count)
                        {
                            case 0:
                                list = pattern.Value.upOne;
                                break;
                            case 1:
                                list = pattern.Value.upTwo;
                                break;
                            case 2:
                                list = pattern.Value.upThree;
                                break;
                            default:
                                break;
                        }
                        break;
                    case 3:
                        switch (attackStack.Count)
                        {
                            case 0:
                                list = pattern.Value.downOne;
                                break;
                            case 1:
                                list = pattern.Value.downTwo;
                                break;
                            case 2:
                                list = pattern.Value.downThree;
                                break;
                            default:
                                break;
                        }
                        break;
                    case 4:
                        switch (attackStack.Count)
                        {
                            case 0:
                                list = pattern.Value.aroundOne;
                                break;
                            case 1:
                                list = pattern.Value.aroundTwo;
                                break;
                            case 2:
                                list = pattern.Value.aroundThree;
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        AttackEffect(list);

        StartCoroutine(InitializeattackColor(list));//���� ǥ�� / ���� �ִϸ��̼����� ����
        attackStack.Clear();
    }

    private void AttackEffect(string[] attackList)
    {
        string x = "";

        foreach (string key in attackList)
        {
            // Debug.Log("attack" + key);
            x += key;
            GameObject.Find("back_" + key).GetComponent<SpriteRenderer>().color = Color.green;
            try
            {
                GameObject.Find(key).GetComponent<Transform>().Find("Enemy1(Clone)").GetComponent<Enemy>().GetDamage(1);
            }
            catch
            {

            }
        }

        Debug.Log("attackList : " + x);
    }

    private IEnumerator InitializeattackColor(string[] attackList)
    {
        yield return new WaitForSeconds(Timer.cycle / 2);

        foreach(string key in attackList)
        {
            GameObject.Find("back_" + key).GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void InputStack(string currentKey)//���ÿ� ���� �ֱ�
    {
        foreach (ManageKeyBoard.key key in keyBoard)//���� ĭ ã��
        {
            if (key.name.Equals(currentKey))
            {
                if (key.isAttack)//���� ������ ��
                {
                    if(attackStack.Count == 0)//���� ������� ��
                    {
                        attackStack.Push(key.attack);
                        GameObject.Find("back_space").GetComponent<Transform>().Find("Attack" + attackStack.Count.ToString()).GetComponent<SpriteRenderer>().sprite
                    = Resources.Load<Sprite>("Image/" + "attack" + key.attack.ToString());
                    }
                    else if (!attackStack.Peek().Equals(key.attack))//������ ������ �ٸ� ���
                    {
                        for (int i = 1; i <= attackStack.Count; ++i)//���� ����
                        {
                            GameObject.Find("back_space").GetComponent<Transform>().Find("Attack" + i.ToString()).GetComponent<SpriteRenderer>().sprite = null;
                        }
                        attackStack.Clear();
                        attackStack.Push(key.attack);
                        GameObject.Find("back_space").GetComponent<Transform>().Find("Attack" + attackStack.Count.ToString()).GetComponent<SpriteRenderer>().sprite
                    = Resources.Load<Sprite>("Image/" + "attack" + key.attack.ToString());
                    }
                    else if (attackStack.Count < 3)//�ִ� ����3 ����
                    {
                        attackStack.Push(key.attack);
                        GameObject.Find("back_space").GetComponent<Transform>().Find("Attack" + attackStack.Count.ToString()).GetComponent<SpriteRenderer>().sprite
                    = Resources.Load<Sprite>("Image/" + "attack" + key.attack.ToString());
                    }

                    GameObject.Find(key.name).GetComponent<Transform>().Find("Attack(Clone)").GetComponent<SpriteRenderer>().sprite = null;
                    key.isAttack = false;
                    key.attack = 0;
                }
                else
                    Debug.Log("���� �Ҵ� �ȵ�");
                break;
            }
        }
        
    }

    // JSON ������ �ε��ϰ� �����͸� �Ľ��ϴ� �Լ�
    private void JsonLoad()
    {
        // Resources �������� json ������ �ε� (Ȯ���ڴ� �������� ����)
        TextAsset jsonFile = Resources.Load<TextAsset>("Json/keyboardMapData");

        if (jsonFile != null)
        {
            string jsonData = jsonFile.text;

            // JsonConvert�� ����� Dictionary<string, AttackPattern> ���� JSON �����͸� �Ľ�
            attackdata = JsonConvert.DeserializeObject<AttackData>(jsonData);

            if (attackdata != null && attackdata.attackPatterns != null)
            {
                Debug.Log("attackData �ε� �Ϸ�");
            }
            else
            {
                Debug.LogError("attackData �Ǵ� attackPatterns�� null�̰ų� �����Ͱ� �����ϴ�");
            }
        }
        else
        {
            Debug.LogError("������ ã�� �� �����ϴ�");
        }
    }

}



[System.Serializable]
public class AttackPattern
{
    public int[] position;        // ��ġ ������
    public string[] horizonOne;   // 1�ܰ� ���� ���� ����
    public string[] horizonTwo;   // 2�ܰ� ���� ���� ����
    public string[] horizonThree; // 3�ܰ� ���� ���� ����
    public string[] upOne;        // 1�ܰ� ���� ���� ����
    public string[] upTwo;        // 2�ܰ� ���� ���� ����
    public string[] upThree;      // 3�ܰ� ���� ���� ����
    public string[] downOne;      // 1�ܰ� �Ʒ��� ���� ����
    public string[] downTwo;      // 2�ܰ� �Ʒ��� ���� ����
    public string[] downThree;    // 3�ܰ� �Ʒ��� ���� ����
    public string[] aroundOne;    // 1�ܰ� �ֺ� ���� ����   
    public string[] aroundTwo;    // 2�ܰ� �ֺ� ���� ����
    public string[] aroundThree;  // 3�ܰ� �ֺ� ���� ����
}

[System.Serializable]
public class AttackData
{
    public Dictionary<string, AttackPattern> attackPatterns; // ������ ��� Dictionary
}
