using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusNumberMovement : MonoBehaviour
{
    private RectTransform rectTransform;
    private Text text;
    private float time = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        text = GetComponent<Text>();
        Destroy(gameObject, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time <= 0.8)
            return;
        rectTransform.Translate(Vector3.up * Time.deltaTime * 400);
        text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a * 0.8f);
    }
}
