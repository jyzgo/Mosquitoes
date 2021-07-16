using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMain : MonoBehaviour
{
    private GameObject main;
    private GameObject mainParent;
    private GameObject endText;
    private int maxX;
    private int maxY;
    private int minX;
    private int minY;
    private int speed = 1;

    private int r = 150;
    private float w = 0.3f;
    private float x;
    private float y;

    private float time = 0;

    private int endNum = 0;
    private bool isEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        maxX = Screen.width / 2-50;
        minX = (Screen.width / 2)*-1 + 50;
        maxY = Screen.height / 2 - 50;
        minY = (Screen.height / 2) * -1 + 50;
        main = this.transform.Find("mainParent/main").gameObject;
        mainParent = this.transform.Find("mainParent").gameObject;
        endText = this.transform.Find("End").gameObject;
        endText.SetActive(false);
        isEnd = false;
        endNum = 5;
        RandomMain(5);
        time = 0;
    }


    private void RandomMain(int num)
    {
        for (int i = 0; i < num; i++)
        {
            var x = Random.Range(minX, maxX);
            var y = Random.Range(minY, maxY);
            CloneMain(x, y);
        }
   
    }


    private void CloneMain(int x,int y)
    {
        var clone = GameObject.Instantiate(main);
        clone.SetActive(true);
        clone.transform.SetParent(mainParent.transform);
        clone.transform.localPosition = new Vector3(x, y);
        var mainclick =   clone.AddComponent<MainClick>();
        mainclick.SetTarget(main);
    }

    private void Clear()
    {
        for (int i = 0; i < mainParent.transform.childCount; i++)
        {
            var go = mainParent.transform.GetChild(i);
            if (go.name != "main")
            {
                GameObject.Destroy(go);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (isEnd) return;
        w += speed * Time.deltaTime;
        x = Mathf.Cos(w) * r;
        y = Mathf.Sin(w) * r;
        main.transform.localPosition = new Vector3(x, y, main.transform.localPosition.z);
        time += Time.deltaTime;
        if (time > 2)
        {
            time = 0;
            var num = Random.Range(5, 10);
            endNum += num;
            if (endNum >= 100)
            {
                isEnd = true;
                endText.SetActive(true);
                return;
            }
            RandomMain(num);
        }
    }
}
