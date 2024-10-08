using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class InteractAttack : MonoBehaviour
{
    public GameObject player;
    public string currentKey; // 현재 위치
    public GameObject attackPrefab;
    public AttackData attackdata;

    private Dictionary<string, List<string>> adjList; // 인접리스트
    private List<ManageKeyBoard.key> keyBoard;
    private Stack<int> attackStack;
    private bool isTabPressed = false; // 'tab' 키의 상태를 저장하는 변수
    private string[] currentAttackRange = new string[] { }; // 현재 공격 범위를 저장하는 배열

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitializeKeyBoard());
        attackStack = new Stack<int>();
        JsonLoad();
    }

    private IEnumerator InitializeKeyBoard()
    {
        // ManageKeyBoard 인스턴스를 찾아 adjList를 가져옴
        ManageKeyBoard manageKeyBoard = null;
        while (manageKeyBoard == null)
        {
            manageKeyBoard = FindObjectOfType<ManageKeyBoard>();
            yield return null; // 한 프레임 기다림
        }

        while (manageKeyBoard.keyBoard == null || manageKeyBoard.keyBoard.Count == 0)
        {
            yield return null; // 한 프레임 기다림
        }

        adjList = manageKeyBoard.adjList;
        keyBoard = manageKeyBoard.keyBoard;
    }



    // Update is called once per frame
    void Update()
    {
        // 'tab' 키 입력 처리
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isTabPressed = true;
            ShowAttackRange();
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            isTabPressed = false;
            HideAttackRange();
        }
        if (Input.GetKeyDown(KeyCode.Space)) // 스페이스바 입력 받기
        {
            Input_Attack();
        }
    }

    private void ShowAttackRange()
    {
        // 현재 공격 범위 계산
        CalculateCurrentAttackRange();
        // 공격 범위 표시
        foreach (string key in currentAttackRange)
        {
            GameObject tile = GameObject.Find("back_" + key);
            if (tile != null)
            {
                SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.color = Color.yellow;
                }
            }
        }
    }
    private void HideAttackRange()
    {
        // 공격 범위 표시 해제
        foreach (string key in currentAttackRange)
        {
            GameObject tile = GameObject.Find("back_" + key);
            if (tile != null)
            {
                SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.color = Color.white;
                }
            }
        }
        currentAttackRange = new string[] { };
    }
    private void CalculateCurrentAttackRange()
    {
        // 공격 범위 계산 로직
        if (attackStack.Count == 0)
        {
            currentAttackRange = new string[] { }; // 공격 스택이 비어있으면 공격 범위 없음
            return;
        }
        Player player = FindObjectOfType<Player>();
        currentKey = player.currentKey;
        string[] list = { };
        foreach (var pattern in attackdata.attackPatterns)
        {
            if (pattern.Key.Equals(currentKey))
            {
                int attackType = attackStack.Peek(); // 가장 최근의 공격 타입
                int stackCount = attackStack.Count;
                switch (attackType)
                {
                    case 1:
                        if (stackCount == 1) list = pattern.Value.horizonOne;
                        else if (stackCount == 2) list = pattern.Value.horizonTwo;
                        else if (stackCount >= 3) list = pattern.Value.horizonThree;
                        break;
                    case 2:
                        if (stackCount == 1) list = pattern.Value.upOne;
                        else if (stackCount == 2) list = pattern.Value.upTwo;
                        else if (stackCount >= 3) list = pattern.Value.upThree;
                        break;
                    case 3:
                        if (stackCount == 1) list = pattern.Value.downOne;
                        else if (stackCount == 2) list = pattern.Value.downTwo;
                        else if (stackCount >= 3) list = pattern.Value.downThree;
                        break;
                    case 4:
                        if (stackCount == 1) list = pattern.Value.aroundOne;
                        else if (stackCount == 2) list = pattern.Value.aroundTwo;
                        else if (stackCount >= 3) list = pattern.Value.aroundThree;
                        break;
                    default:
                        break;
                }
                break;
            }
        }
        currentAttackRange = list;
    }

    private void Input_Attack()//공격 입력 받기
    {
        if(attackStack.Count < 1)//스택 없으면 종료
        {
            Debug.Log("스택 비어있음");
            return;
        }
        for (int i = 1; i <= 3; ++i)//스택 비우기
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

        AttackEffect(list, attackStack.Count + 1);

        StartCoroutine(InitializeattackColor(list));//공격 표시 / 이후 애니메이션으로 구현
        attackStack.Clear();
    }

    private void AttackEffect(string[] attackList, int damage)
    {
        foreach (string key in attackList)
        {
            Debug.Log("attack" + key);
            GameObject.Find("back_" + key).GetComponent<SpriteRenderer>().color = Color.green;
            try
            {
                GameObject.Find(key).GetComponent<Transform>().Find("Enemy1(Clone)").Find("Enemy").GetComponent<Enemy>().GetDamage(damage);
            }
            catch
            {

            }
        }
    }

    private IEnumerator InitializeattackColor(string[] attackList)
    {
        yield return new WaitForSeconds(Timer.cycle / 2);

        foreach(string key in attackList)
        {
            GameObject.Find("back_" + key).GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void InputStack(string currentKey)//스택에 공격 넣기
    {
        foreach (ManageKeyBoard.key key in keyBoard)//현재 칸 찾기
        {
            if (key.name.Equals(currentKey))
            {
                if (key.isAttack)//공격 존재할 때
                {
                    if(attackStack.Count == 0)//스택 비어있을 때
                    {
                        attackStack.Push(key.attack);
                        GameObject.Find("back_space").GetComponent<Transform>().Find("Attack" + attackStack.Count.ToString()).GetComponent<SpriteRenderer>().sprite
                    = Resources.Load<Sprite>("Image/" + "attack" + key.attack.ToString());
                    }
                    else if (!attackStack.Peek().Equals(key.attack))//스택의 공격이 다른 경우
                    {
                        for (int i = 1; i <= attackStack.Count; ++i)//스택 비우기
                        {
                            GameObject.Find("back_space").GetComponent<Transform>().Find("Attack" + i.ToString()).GetComponent<SpriteRenderer>().sprite = null;
                        }
                        attackStack.Clear();
                        attackStack.Push(key.attack);
                        GameObject.Find("back_space").GetComponent<Transform>().Find("Attack" + attackStack.Count.ToString()).GetComponent<SpriteRenderer>().sprite
                    = Resources.Load<Sprite>("Image/" + "attack" + key.attack.ToString());
                    }
                    else if (attackStack.Count < 3)//최대 스택3 제약
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
                    Debug.Log("공격 할당 안됨");
                break;
            }
        }
        
    }

    // JSON 파일을 로드하고 데이터를 파싱하는 함수
    private void JsonLoad()
    {
        // JSON 파일 경로
        string path = Path.Combine(Application.dataPath, "Resources/Json/keyboardMapData.json");

        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);

            // JsonConvert를 사용해 Dictionary<string, AttackPattern> 포함 JSON 데이터를 파싱
            attackdata = JsonConvert.DeserializeObject<AttackData>(jsonData);

            if (attackdata != null && attackdata.attackPatterns != null)
            {
                
            }
            else
            {
                Debug.LogError("attackData 또는 attackPatterns가 null이거나 데이터가 없습니다");
            }
        }
        else
        {
            Debug.LogError("파일을 찾을 수 없습니다");
        }
    }
}



[System.Serializable]
public class AttackPattern
{
    public int[] position;        // 위치 데이터
    public string[] horizonOne;   // 1단계 가로 공격 범위
    public string[] horizonTwo;   // 2단계 가로 공격 범위
    public string[] horizonThree; // 3단계 가로 공격 범위
    public string[] upOne;        // 1단계 위쪽 공격 범위
    public string[] upTwo;        // 2단계 위쪽 공격 범위
    public string[] upThree;      // 3단계 위쪽 공격 범위
    public string[] downOne;      // 1단계 아래쪽 공격 범위
    public string[] downTwo;      // 2단계 아래쪽 공격 범위
    public string[] downThree;    // 3단계 아래쪽 공격 범위
    public string[] aroundOne;    // 1단계 주변 공격 범위
    public string[] aroundTwo;    // 2단계 주변 공격 범위
    public string[] aroundThree;  // 3단계 주변 공격 범위
}

[System.Serializable]
public class AttackData
{
    public Dictionary<string, AttackPattern> attackPatterns; // 패턴을 담는 Dictionary
}
