using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [Header("Stages To Generate")]
    public List<GameObject> stages;
    public UIController ui;

    [HideInInspector]
    public StageInfo info = null;

    private GameObject currentStage = null;
    
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GenerateStage(0);
    }

    public void GenerateStage(int num)
    {
        if(currentStage != null)
        {
            Destroy(currentStage);
        }

        int currentStageID = num; 

        currentStage = Instantiate(stages[currentStageID], stages[currentStageID].transform.position, stages[currentStageID].transform.rotation, transform);

        info = currentStage.GetComponent<StageInfo>();

        if (info.camerapoint != null)
        {
            Debug.Log("Move Cam");
            CameraController.instance.MoveCamera(info.camerapoint);
        }
    }

    public void Respawn()
    {

      GameObject newShape = Instantiate(info.shape, info.spawnPoint.position, Quaternion.identity, currentStage.transform);
        info.shape = newShape;
    }

    public void ShowScore(Transform stage)
    {
        Debug.Log("Show Score");
        ui.Score(stage);
    }
}
