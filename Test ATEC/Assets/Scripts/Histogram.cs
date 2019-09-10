using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Histogram : MonoBehaviour
{
    // Start is called before the first frame update
    public RectTransform chartContainer;
    public GameObject charLine;
    public RectTransform labelTemplateX;
    public RectTransform labelTemplateY;
    public RectTransform dashTemplateY;

    List<GameObject> lines = new List<GameObject>();

    void Awake()
    {

    }

    private GameObject CreateChartLine(Vector2 anchoredPosition, float width, float height, Vector3 color)//создание чарт линии
    {
        GameObject gameObject = Instantiate(charLine, transform.Find("Content").transform, false);
        //gameObject.GetComponent<Image>().color = new Color(color.x, color.y, color.z);//задаем цвет линиям
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(width, height);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        gameObject.AddComponent<Shadow>();
        gameObject.GetComponent<Shadow>().effectDistance = new Vector2(0, -2f);
        lines.Add(gameObject);
        return gameObject;
    }

    public void ShowChart(List<int> valueList, int yMaximum)
    {
        lines.Clear();
        float chartHeight = chartContainer.sizeDelta.y;
        float xPosition = 50f;
        float r = 98f, g = 0f, b = 238f;

        for (int i = 0; i < valueList.Count; i++)
        {
            float yPosition = (valueList[i] * 1.0f / yMaximum * 1.0f) * chartHeight;
            GameObject chartGameObject = CreateChartLine(new Vector2(xPosition, 0f), 900f/valueList.Count - 100f, Mathf.Lerp(0f, yPosition, 1f), new Vector3(r, g, b));
            xPosition += chartGameObject.GetComponent<RectTransform>().sizeDelta.x + 50f;
            chartGameObject.transform.Find("Text").GetComponent<Text>().text = valueList[i].ToString();
            g += 20f / valueList.Count;
            //RectTransform labelX = Instantiate(labelTemplateX);
            //labelX.SetParent(chartContainer);
            //labelX.gameObject.SetActive(true);
            //labelX.anchoredPosition = new Vector2(xPosition, -100f);
            //labelX.GetComponent<Text>().text = pacients[i];

            //RectTransform dashX = Instantiate(dashTemplateX);
            //dashX.SetParent(graphContainer);
            //dashX.gameObject.SetActive(true);
            //dashX.anchoredPosition = new Vector2(xPosition, 0f);
        }

        int separatorCount = 10;
        for (int i = 0; i < separatorCount; i++)
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(chartContainer);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-100f, normalizedValue * chartHeight);
            labelY.GetComponent<Text>().text = Mathf.RoundToInt(normalizedValue * yMaximum).ToString();

            RectTransform dashY = Instantiate(dashTemplateY);
            dashY.SetParent(chartContainer);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(0f, normalizedValue * chartHeight);
            dashY.sizeDelta = new Vector2(transform.Find("Content").GetComponent<RectTransform>().sizeDelta.x, 3f);
            dashY.SetSiblingIndex(0);
        }

        //считаем средний балл
        int avg = 0;
        for (int i = 0; i < valueList.Count; i++)
            avg += valueList[i];
        avg = avg / valueList.Count;
        transform.Find("YLabelChart").GetComponent<Text>().text = "Средний балл по курсу: " + avg.ToString();
    }

    public void Clear()//очистка гистограммы
    {
        if (transform.Find("Content").transform.childCount > 0)
        {
            for (int i = 0; i < transform.Find("Content").transform.childCount; i++)
            {
                Destroy(transform.Find("Content").transform.GetChild(i).gameObject);
            }
        }
    }
}
