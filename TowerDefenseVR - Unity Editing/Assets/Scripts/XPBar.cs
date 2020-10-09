using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    private float baseLength;
    private float newLength;

    [SerializeField]
    private GameObject text;

    private void Start()
    {
        baseLength = this.transform.localScale.y;
    }

    public void ChangeXP(float _current, float _total)
    {
        text.GetComponent<Text>().text = _current.ToString() + "\n/\n" + _total.ToString();
        float per = _current / _total;
        newLength = baseLength * per;
        this.transform.localScale = new Vector3(newLength, this.transform.localScale.y);
    }
}
