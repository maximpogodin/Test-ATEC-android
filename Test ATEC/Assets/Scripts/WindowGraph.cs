using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class WindowGraph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;
    public GameObject popUp;
    List<GameObject> circles = new List<GameObject>();
    List<GameObject> popUps = new List<GameObject>();
    public ManageWindow mw;

    bool show_hide;

    private void Awake()
    {
        show_hide = false;
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.transform.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.transform.Find("labelTemplateY").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.transform.Find("dashTemplateX").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.transform.Find("dashTemplateY").GetComponent<RectTransform>();
    }

    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(50, 50);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        gameObject.AddComponent<Shadow>();
        gameObject.GetComponent<Shadow>().effectDistance = new Vector2(0, -2f);
        circles.Add(gameObject);
        return gameObject;
    }

    public void ShowGraph(List<int> valueList, int yMaximum)
    {
        circles.Clear();
        float graphHeight = graphContainer.sizeDelta.y;
        float xSize = 100f;

        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = i * xSize;
            float yPosition = (valueList[i] * 1.0f / yMaximum * 1.0f) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            if (lastCircleGameObject != null)
            {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;

            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -50f);
            labelX.GetComponent<Text>().text = (i + 1).ToString();

            RectTransform dashX = Instantiate(dashTemplateX);
            dashX.SetParent(graphContainer);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPosition, 0f);
        }

        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++)
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-50f, normalizedValue * graphHeight);

            labelY.GetComponent<Text>().text = Mathf.RoundToInt(normalizedValue * yMaximum).ToString();

            RectTransform dashY = Instantiate(dashTemplateY);
            dashY.SetParent(graphContainer);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(0f, normalizedValue * graphHeight);
            dashY.sizeDelta = new Vector2(graphContainer.sizeDelta.x, 3f);
        }
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = Color.white;
        gameObject.AddComponent<Shadow>();
        gameObject.GetComponent<Shadow>().effectDistance = new Vector2(0, -2f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 15f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * 0.5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
        rectTransform.SetSiblingIndex(6);
    }

    public void CloseGraph()
    {
        if (transform.Find("graphContainer").transform.childCount > 6)
        {
            for (int i = 6; i < transform.Find("graphContainer").transform.childCount; i++)
            {
                Destroy(transform.Find("graphContainer").transform.GetChild(i).gameObject);
            }
        }
        popUps.Clear();
    }

    public void ShowHidePopUp()//показать цифры
    {
        if (!show_hide)
        {
            for (int i = 0; i < circles.Count; i++)//создадим все popup
            {
                GameObject clonePop = Instantiate(popUp, circles[i].transform);
                clonePop.transform.Find("textPop").GetComponent<Text>().text = mw.pointsList[i].ToString();
                clonePop.SetActive(true);
                clonePop.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 75f);
                popUps.Add(clonePop);
            }

            show_hide = true;
        }
        else
        {
            show_hide = false;
            for (int i = 0; i < popUps.Count; i++)
            {
                popUps[i].GetComponent<Animator>().SetBool("Close", true);
                Destroy(popUps[i], 1.5f);//удаляем все popup
            }
            popUps.Clear();
        }
    }
}
