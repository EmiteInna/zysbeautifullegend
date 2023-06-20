using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BackpackManager : MonoBehaviour
{
    [Header("Outlook")]
    public float backpackWitdh;
    public float backpackHeight;
    public float HorizontalNumber;
    public float VerticalNumber;
    public GameObject Backpackmenu;
    public Text itemname;
    public Text itemdescription;
    public Text itemamount;
    [Header("Attribute")]
    public static BackpackManager Instance;
    public bool is_opened;
    GameObject chosenItem;
    public List<GameObject> items;
    public GameObject nullitem;
    [Header("Used for test")]
    public GameObject testitem,testitem2;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Initialize();
    }
    private void Start()
    {
        KeyManager.Instance.OnBackpackPushed += OnOpen;
    }
    public void Initialize()
    {
        is_opened = false;
        Backpackmenu.SetActive(false);
        for(int i = 0; i < HorizontalNumber*VerticalNumber; i++)
        {
            GameObject item = Instantiate(nullitem, Backpackmenu.transform);
            item.GetComponent<ItemController>().position = i;
            items.Add(item);
        }
        chosenItem = nullitem;
    }
    public void SortAllItems()
    {
        items.Sort(new ItemComparerer());
        int height = Screen.height;
        int width=Screen.width;
        float x = width * (1 - backpackWitdh) / 3;
        float y=height*(1- backpackHeight) / 2;
        float deltax = width * backpackWitdh / HorizontalNumber;
        float deltay = height * backpackHeight / VerticalNumber;
        int num = 1;
        foreach(var item in items)
        {
            item.transform.position = new Vector3(x, y, 0);
            if(!item.GetComponent<ItemController>().is_null)item.GetComponent<ItemController>().UpdateCD();
            if (num % HorizontalNumber == 0)
            {
                x = width * (1 - backpackWitdh) / 3;
                y += deltay;
            }else 
            x += deltax;
            num++;
            
        }
    }
    public void OnOpen()
    {
        if (is_opened == false && GManager.Instance.gamestate == GameStates.PAUSED) return;
        if (GManager.Instance.gamestate == GameStates.DIALOG) return;
        AudioClip adc = GetComponent<AudioList>().clipList[0];
        SoundManager.Instance.Playsound(transform.position, adc, 1);
        is_opened = !is_opened;
        
        Backpackmenu.SetActive(is_opened);
        if(is_opened)SortAllItems();
        GManager.Instance.PauseGame();
    }
 
    public void OnUsingItem()
    {

        chosenItem.GetComponent<ItemController>().OnItemUse();
        if (chosenItem.GetComponent<ItemController>().is_null)
        {
            OnDestroyingItem();
        }
        else
        {
            ChooseItem(chosenItem);
        }
    }
    public void OnDestroyingItem()
    {
        if (chosenItem == nullitem) return;
        AudioClip adc = GetComponent<AudioList>().clipList[1];
        SoundManager.Instance.Playsound(transform.position, adc, 1);
        int num = 0;
        foreach (var item in items)
        {
            if (item.GetComponent<ItemController>().is_chosen == true)
            {
                item.GetComponent<ItemController>().OnThrowed();
                GameObject former = item;
                int pos = former.GetComponent<ItemController>().position;
                UnChooseItem(item);
                items.Remove(item);
                Destroy(former);
                GameObject newer = Instantiate(nullitem, Backpackmenu.transform);
                newer.GetComponent<ItemController>().position = pos;
                items.Insert(num, newer);
                break;
            }
            num++;
        }
        SortAllItems();
    }
    public void GetItem(GameObject item)//记得在get之后 UpdateInfomation()
    {
        if (item.tag != "item") return;
        bool flag = false;//是否已经找到合适的位置
        foreach(var i in items)
        {
            if(item.GetComponent<ItemController>().item_name == i.GetComponent<ItemController>().item_name)
            {
                if (item.GetComponent<ItemController>().is_expendable)
                {
                    i.GetComponent<ItemController>().amount += item.GetComponent<ItemController>().amount;
                    i.GetComponent<ItemController>().OnAmountChanged();
                    flag = true;
                }
                break;
            }
        }
        if (flag == true) return;
        int index = 0;
        foreach(var i in items)
        {
            if (i.GetComponent<ItemController>().is_null == true) {
                flag = true;
                GameObject former = i;
                items.Remove(i);
                Destroy(i);
                
                GameObject newitem=Instantiate(item, Backpackmenu.transform);
                newitem.GetComponent<ItemController>().position = index;                
                items.Insert(index, newitem);
                newitem.GetComponent<ItemController>().OnPickedUp();
                break;
            }
            index++;
        }
        SortAllItems();
        if(flag == true) return;
        //TODO:背包已满
        Debug.Log("背包已满");
    }
    //TODO:物品的拖拽和合并
    GameObject GetMouseitem()
    {
        Vector2 mouse=new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        GameObject result = null;
        int height = Screen.height;
        int width = Screen.width;
        float x = width * (1 - backpackWitdh) / 3;
        float y = height * (1 - backpackHeight) / 2;
        float deltax = width * backpackWitdh / HorizontalNumber;
        float deltay = height * backpackHeight / VerticalNumber;
        float distance = Mathf.Sqrt(deltax*deltax+deltay*deltay)/2f;
        int num = 1;
        int test = 0;
        foreach (var item in items)
        {
            
            if (Vector2.Distance(new Vector2(x, y), mouse) < distance)
            {
                distance = Vector3.Distance(new Vector3(x, y, 0), mouse);
                result = item;
                test = num;
            }
            if (num % HorizontalNumber == 0)
            {
                x = width * (1 - backpackWitdh) / 3;
                y += deltay;
            }
            else
                x += deltax;
            num++;

        }
        return result;
    }
    private void Update()
    {
        if (!is_opened) return;
        if (Input.GetMouseButtonDown(0))
        {
            GameObject g = GetMouseitem();
            if (g != null)
            {
                if (chosenItem.GetComponent<ItemController>().is_null == false)
                {
                    UnChooseItem(chosenItem);
                }
                if (g.GetComponent<ItemController>().is_null != true)
                  ChooseItem(g);
            }

        }
        if (Input.GetMouseButton(0))
        {
            if (GetMouseitem() != null)
            {
                if (chosenItem.GetComponent<ItemController>().is_null == false)
                {
                    chosenItem.transform.position = Input.mousePosition;
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (chosenItem.GetComponent<ItemController>().is_null == false)
            {
                GameObject g = GetMouseitem();
                if (g != null)
                {
                    int tmp = chosenItem.GetComponent<ItemController>().position;
                    chosenItem.GetComponent<ItemController>().position = g.GetComponent<ItemController>().position;
                    g.GetComponent<ItemController>().position = tmp;
                }
                    SortAllItems();
                
            }
        }
        
    }
    public void ChooseItem(GameObject g)
    {
        chosenItem = g;
        g.GetComponent<ItemController>().is_chosen = true;
        UpdateInfomation();
    }
    public void UnChooseItem(GameObject g)
    {
        g.GetComponent<ItemController>().is_chosen = false;
        chosenItem = nullitem;
        UpdateInfomation();
    }
    public void UpdateInfomation()
    {
        if (chosenItem == nullitem)
        {
            itemname.text = "";
            itemamount.text = "";
            itemdescription.text = "";
        }
        else
        {
            itemname.text = chosenItem.GetComponent<ItemController>().item_name;
            itemdescription.text = chosenItem.GetComponent<ItemController>().content;
            if (chosenItem.GetComponent<ItemController>().is_expendable)
                itemamount.text = string.Format("数量：{0}", chosenItem.GetComponent<ItemController>().amount);
            else
                itemamount.text = "非消耗品";
        }
    }
 

}
