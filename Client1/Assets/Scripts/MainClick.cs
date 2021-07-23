using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainClick : MonoBehaviour
{
    private int speed = 300;
    private GameObject target;
    private bool isDestory = false;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.one * 3f;
        isDestory = false;
        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }
    public void SetTarget(GameObject go)
    {
        target = go;        
    }
    private void OnClick()
    {
        GameObject.Destroy(this.gameObject);
    }

    void Update()
    {
        if (isDestory) return;
        if (Vector3.Distance(transform.localPosition, target.transform.localPosition) < 10)
        {
            isDestory = true;
            GameObject.Destroy(this.gameObject);
        }
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, target.transform.localPosition, speed * Time.deltaTime);
    }
}
