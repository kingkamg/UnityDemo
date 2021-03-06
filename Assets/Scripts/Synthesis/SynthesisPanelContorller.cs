using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 合成面板控制器脚本
/// 控制合成面板的逻辑
/// </summary>
public class SynthesisPanelContorller : MonoBehaviour
{
    //单例
    public static SynthesisPanelContorller _Instance;

    private Transform m_Transform;
    private SynthesisPanelModel m_SynthesisPanelModel;
    private SynthesisPanelView m_SynthesisPanelView;
    private SynthesisController m_SynthesisController;

    private int tabCount = 2;       //tab个数
    private int currentIndex = -1;  //当前索引
    private int slotsCount = 25;    //物品槽数量
    private int demand;             //合成需求量
    private int added;              //已添加素材数量

    private List<GameObject> tabList;
    private List<GameObject> contentList;
    private List<GameObject> slotList;
    private List<GameObject> addedList;
    void Awake()
    {
        _Instance = this;
    }

    void Start()
    {
        Init();

        CreateAllTabs();
        CreateAllContens();
        CreateAllSlots();

        SwitchTabAndContents(0);
    }

    private void Init()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_SynthesisPanelView = gameObject.GetComponent<SynthesisPanelView>();
        m_SynthesisPanelModel = gameObject.GetComponent<SynthesisPanelModel>();
        m_SynthesisController = m_Transform.Find("Right").GetComponent<SynthesisController>();

        tabList = new List<GameObject>();
        contentList = new List<GameObject>();
        slotList = new List<GameObject>();
        addedList = new List<GameObject>();

        m_SynthesisController.InventoryItem_Prefab = m_SynthesisPanelView.InventoryItem_Prefab;
    }
    /// <summary>
    /// 生成所有Tab
    /// </summary>
    private void CreateAllTabs()
    {
        for (int i = 0; i < tabCount; i++)
        {
            GameObject go = Instantiate<GameObject>(m_SynthesisPanelView.TabItemType_Prefab, m_SynthesisPanelView.Tab_Transform);
            Sprite temp = m_SynthesisPanelView.GetSpriteByName(m_SynthesisPanelModel.TabIconName[i]);
            go.GetComponent<SynthesisTabContorller>().Init(i, temp);
            tabList.Add(go);
        }
    }
    /// <summary>
    /// 生成所有内容区域
    /// </summary>
    private void CreateAllContens()
    {
        for (int i = 0; i < tabCount; i++)
        {
            List<List<ContentItem>> temp = m_SynthesisPanelModel.GetJsonDataByName("SynthesisContentsJsonData");
            GameObject go = Instantiate<GameObject>(m_SynthesisPanelView.Content_Prefab, m_SynthesisPanelView.Content_Transform);
            go.GetComponent<SynthesisContentContorller>().Init(i, m_SynthesisPanelView.ContentItem_Prefab, temp[i]);
            contentList.Add(go);
        }
    }
    /// <summary>
    /// 生成所有图谱槽
    /// </summary>
    private void CreateAllSlots()
    {
        for (int i = 0; i < slotsCount; i++)
        {
            GameObject slotObj = Instantiate(m_SynthesisPanelView.ContentSlot_Prefab, m_SynthesisPanelView.Center_Transform);
            slotObj.name = "slot" + i;
            slotList.Add(slotObj);
        }
    }
    /// <summary>
    /// 图谱槽数据填充所需材料
    /// </summary>
    private void CreateSlotsContent(int id)
    {
        SynthesisMapItem temp = m_SynthesisPanelModel.GetMapItemByID(id);

        ClearSlots();
        BackMaterials();

        if (temp != null)
        {
            for (int i = 0; i < temp.MapContents.Length; i++)
            {
                if (temp.MapContents[i] != "0")
                {
                    Sprite sp = m_SynthesisPanelView.GetSpritByID(temp.MapContents[i]);
                    slotList[i].GetComponent<SynthesisSlotContorller>().Init(temp.MapContents[i], sp);
                }
            }
            demand = temp.Count;
            m_SynthesisController.Init(temp.MapID, temp.MapName, temp.MapName);
        }
        //清空合成物品图标
        else m_SynthesisController.ClearIcon();
    }
    /// <summary>
    /// 清空合成图谱槽
    /// </summary>
    private void ClearSlots()
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            slotList[i].GetComponent<SynthesisSlotContorller>().Reset();
        }
    }
    /// <summary>
    /// 切换tab和显示内容
    /// </summary>
    public void SwitchTabAndContents(int index)
    {
        if (index == currentIndex) return;

        for (int i = 0; i < tabCount; i++)
        {
            tabList[i].GetComponent<SynthesisTabContorller>().SetDefault();
            contentList[i].SetActive(false);
        }
        tabList[index].GetComponent<SynthesisTabContorller>().SetSelect();
        contentList[index].SetActive(true);
        this.currentIndex = index;
    }
    /// <summary>
    /// 将合成面板中的材料返回背包
    /// </summary>
    private void BackMaterials()
    {
        List<GameObject> itemList = new List<GameObject>();

        for (int i = 0; i < slotList.Count; i++)
        {
            Transform itemTransform = slotList[i].GetComponent<Transform>().Find("InventoryItem");
            if (itemTransform != null)
            {
                itemList.Add(itemTransform.gameObject);
            }
        }
        //Debug.Log("合成图谱中添加的材料数" + itemList.Count);
        if (itemList.Count > 0) InventoryPanelController._Instance.AddItem(itemList);
    }
    /// <summary>
    /// 将Item添加进合成面板 
    /// </summary>
    public void AddItemToSynthesisPanel(GameObject temp)
    {
        addedList.Add(temp);
        added++;
        Debug.Log(string.Format("合成物品{0}/{1}", added, demand));
        if (added == demand)
            m_SynthesisController.ButtonActive();
    }
    /// <summary>
    /// 制作完成后对物品数量操作
    /// </summary>
    private void MakeFinish()
    {
        Debug.Log(addedList.Count);
        for (int i = 0; i < addedList.Count; i++)
        {
            InventoryItemController iic = addedList[i].GetComponent<InventoryItemController>();
            iic.Num -= 1;
            if (iic.Num == 0) Destroy(iic.gameObject);
        }
        added = 0;
        addedList.Clear();
        StartCoroutine("BackToInventory");
    }
    private IEnumerator BackToInventory()
    {
        yield return new WaitForSeconds(0.3f);
        //m_SynthesisController.ClearIcon();
        BackMaterials();
    }
}
