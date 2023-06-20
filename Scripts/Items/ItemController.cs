using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public string item_name;
    public int amount;
    public bool is_expendable;
    public string content;
    public bool is_null;
    public bool is_chosen;
    public float cooldown;
    public float cooldown_now;
    public Text amount_text;
    public Image cdImage;
    public float realtime;
    public int position;
    public virtual void OnItemUse()
    {
        if (is_null) return;
        foreach (var adc in GetComponent<AudioList>().clipList)
        {
            SoundManager.Instance.Playsound(transform.position, adc, 3);
        }
        if (is_expendable)
        {
            amount--;
            OnAmountChanged();
            if (amount == 0)expire(); 
        }
        cooldown_now= cooldown;
        UpdateCD();
    }
    private void Start()
    {
        OnAmountChanged();
    }
    public virtual void OnAmountChanged()
    {
        if (is_null) amount_text.text = string.Empty;
        else if (!is_expendable) amount_text.text = "·ÇÏûºÄÆ·";
        else amount_text.text = amount.ToString();
    }
    public virtual void expire()
    {
        BackpackManager.Instance.OnDestroyingItem();
    }
    public virtual void OnItemChose()
    {
        is_chosen=true;
        BackpackManager.Instance.UpdateInfomation();
    }
    public virtual void OnItemDisChose()
    {
        is_chosen = false;
        BackpackManager.Instance.UpdateInfomation();
    }

    public void UpdateCD()
    {
        if (cooldown == 0)
        {
            cdImage.fillAmount = 0;
            return;
        }
        cdImage.fillAmount = cooldown_now / cooldown;
    }
    public virtual void OnPickedUp()
    {

    }
    public virtual void OnThrowed()
    {

    }
    public virtual void OnPlayerAttack()
    {

    }
    public virtual void OnPlayerBeenAttack()
    {

    }
}
class ItemComparerer:IComparer<GameObject>
{
    public int Compare(GameObject x,GameObject y)
    {
        int posx = x.GetComponent<ItemController>().position;
        int posy = y.GetComponent<ItemController>().position;
        return posx.CompareTo(posy);
    }
}