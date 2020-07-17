using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI stageText;
    public TextMeshProUGUI detailsText;
    public GameObject score;

    public string[] stageInfoText;

    [HideInInspector]
    public int stageNum;
    void Start()
    {
        stageNum = 0;
        UpdateStageUI();
    }

    public void UpdateStageUI()
    {

        detailsText.text = stageInfoText[stageNum];
        int num = stageNum + 1;
        stageText.text = "Stage " + num.ToString();
        
    }
  
    public void NextStage()
    {
        if(stageNum < GameManager.instance.stages.Count-1)
        {
            stageNum++;
            GameManager.instance.GenerateStage(stageNum);
            UpdateStageUI();
        }
    }

    public void PreviousStage()
    {
        if(stageNum > 0)
        {
            stageNum--;
            GameManager.instance.GenerateStage(stageNum);
            UpdateStageUI();
        }
    }
     
    public void Score(Transform stage)
    {
        GameObject scoreText = Instantiate(score, score.transform.position, Quaternion.identity, stage);
        RectTransform scorePos = scoreText.GetComponent<RectTransform>();
        scorePos.localPosition = new Vector3(Random.Range(scorePos.localPosition.x-0.2f, scorePos.localPosition.x + 0.2f), scorePos.localPosition.y, scorePos.localPosition.z);
        Destroy(scoreText, 0.5f);
    }
}
