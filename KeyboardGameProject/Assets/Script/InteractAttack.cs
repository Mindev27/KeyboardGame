using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAttack : MonoBehaviour
{
    public GameObject player;
    public string currentKey; // ���� ��ġ
    public GameObject attackPrefab;

    private List<ManageKeyBoard.key> keyBoard;
    private Stack<int> attackStack;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitializeKeyBoard());
        attackStack = new Stack<int>();
        //CreateStack();
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

        keyBoard = manageKeyBoard.keyBoard;
    }

    // Update is called once per frame
    void Update()
    {
        Input_Attack();
    }

    private void Input_Attack()//���� �Է� �ޱ�
    {
        if (Input.GetKeyDown(KeyCode.Space))//�����̽��� �Է� �ޱ�
        {
          
        }
    }

    private void CreateStack()
    {
        for (int i = 1; i <= 3; ++i)
        {
            GameObject attack1 = Instantiate(attackPrefab, GameObject.Find("back_space").transform);
            attack1.SetActive(true);
            attack1.name = "Attack" + i.ToString();
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
