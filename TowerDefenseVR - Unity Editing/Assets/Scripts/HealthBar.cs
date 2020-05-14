using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private float baseHeight;
    private float newHeight;

    [SerializeField]
    private GameObject text;

    private void Start()
    {
        baseHeight = this.transform.localScale.y;
    }

    public void ChangeHealth(float current, float total)
    {
        text.GetComponent<Text>().text = current.ToString() + "\n/\n" + total.ToString(); 
        float per = current / total;
        newHeight = baseHeight * per;
        this.transform.localScale = new Vector3(this.transform.localScale.x, newHeight);
    }
}
