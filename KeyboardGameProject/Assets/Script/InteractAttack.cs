using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAttack : MonoBehaviour
{
    public GameObject player;
    public string currentKey; // ���� ��ġ
    public GameObject attackPrefab;

    private Dictionary<string, List<string>> adjList; // ��������Ʈ
    private List<ManageKeyBoard.key> keyBoard;
    private Stack<int> attackStack;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitializeKeyBoard());
        attackStack = new Stack<int>();
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
        if(attackStack.Count < 1)
        {
            Debug.Log("���� �������");
            return;
        }
        for (int i = 1; i <= 3; ++i)
        {
            GameObject.Find("back_space").GetComponent<Transform>().Find("Attack" + i.ToString()).GetComponent<SpriteRenderer>().sprite = null;
        }

        Player player = FindObjectOfType<Player>();
        currentKey = player.currentKey;

        switch (attackStack.Pop())
        {
            case 1:
                foreach (string key in adjList[currentKey])
                {
                    GameObject.Find("back_" + key).GetComponent<SpriteRenderer>().color = Color.green;
                    try
                    {
                        GameObject.Find(key).GetComponent<Transform>().Find("Enemy1(Clone)").GetComponent<Enemy>().GetDamage(1);
                    }
                    catch
                    {

                    }
                    
                }
                break;
            default:
                break;
        }

        StartCoroutine(InitializeattackColor(adjList[currentKey]));
        attackStack.Clear();
    }

    private IEnumerator InitializeattackColor(List<string> keyList)
    {
        yield return new WaitForSeconds(Timer.cycle / 2);

        foreach(string key in keyList)
        {
            GameObject.Find("back_" + key).GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void InputStack(string currentKey)
    {
        foreach (ManageKeyBoard.key key in keyBoard)
        {
            if (key.name.Equals(currentKey))
            {
                if (key.isAttack)
                {
                    if (attackStack.Count < 3)
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

}
