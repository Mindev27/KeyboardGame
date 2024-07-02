using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAttack : MonoBehaviour
{
    public GameObject player;
    public string currentKey; // ���� ��ġ
    private List<ManageKeyBoard.key> keyBoard;

    // Start is called before the first frame update
    void Start()
    {
        
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
            if(keyBoard == null)//����Ʈ�� ������� �� �޾ƿ���
            {
                ManageKeyBoard manageKeyBoard =  FindObjectOfType<ManageKeyBoard>();
                keyBoard = manageKeyBoard.keyBoard;
            }
            currentKey = player.GetComponent<Player>().currentKey;
            foreach (ManageKeyBoard.key key in keyBoard)
            {
                if (key.name.Equals(currentKey))
                {
                    if (key.isAttack)
                    {
                        InvokeAttack(key);
                    }
                    else
                        Debug.Log("���� �Ҵ� �ȵ�");
                    break;
                }
            }
        }

    }

    public void InvokeAttack(ManageKeyBoard.key key)//���� �ߵ�
    {
        GameObject.Find(key.name).GetComponent<Transform>().Find("Attack(Clone)").GetComponent<SpriteRenderer>().sprite = null;
        key.isAttack = false;
    }

}
