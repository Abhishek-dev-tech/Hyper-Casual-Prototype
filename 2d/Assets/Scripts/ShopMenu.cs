using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    public GameObject scrollBar;

    private float scrollPos;
    private float[] pos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(scrollBar.GetComponent<Scrollbar>().value <= 0)
        {
            scrollBar.GetComponent<Scrollbar>().value = 0;
        }
        pos = new float[transform.childCount];
        float distance = 1 / (pos.Length - 1);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }
        if (Input.GetMouseButton(0))
        {
            scrollPos = scrollBar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
                {
                    print("sjffffffffffffffffffsjjj");
                    scrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollBar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }
        for (int i = 0; i < pos.Length; i++)
        {

            if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
            {
                print("sjsjjj");
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1, 1), 0.1f);
                for (int a = 0; a < pos.Length; a++)
                {

                    if (a != i)
                    {
                        transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                    }
                }
            }
        }
    }
}
