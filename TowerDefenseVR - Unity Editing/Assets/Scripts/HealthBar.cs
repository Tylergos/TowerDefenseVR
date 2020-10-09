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

    public void ChangeHealth(float _current, float _total)
    {
        text.GetComponent<Text>().text = _current.ToString() + "\n/\n" + _total.ToString(); 
        float per = _current / _total;
        newHeight = baseHeight * per;
        this.transform.localScale = new Vector3(this.transform.localScale.x, newHeight);
    }
}
