using System;
using UnityEditor.Media;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] GameObject stonePrefab;
    private int currentTrialNumber;
    private int currentTrialJumpCount;
    private int totalTrialCounts;
    private int totalJumpCount;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SpawnStones();
        currentTrialNumber = 1;
        currentTrialJumpCount = 0;
        totalTrialCounts = 0;
        totalJumpCount = 0;
    }

    void Update()
    {

    }

    private void SpawnStones()
    {
        Vector3 position = Vector3.zero;
        position.y = -2f;

        for (int i = 0; i < 10; i++)
        {
            position.x = CoordenadaX(i);
            Instantiate(stonePrefab, position, Quaternion.identity);
        }
    }

    public float CoordenadaX(int index)
    {
        return -9 * 1.8f / 2 + index * 1.8f;
    }

    public void FrogJump()
    {
        currentTrialJumpCount++;
    }

    public void StartNewTrial()
    {
        totalJumpCount += currentTrialJumpCount;
        totalTrialCounts++;
        currentTrialNumber++;
        currentTrialJumpCount = 0;
    }

    void OnGUI()
    {
        int tagWidth = 150;
        int tagHeight = 20;
        int margin = 10;

        GUI.Label(new Rect(margin, margin, tagWidth, tagHeight), "Ensayos actual:");
        GUI.Label(new Rect(margin + tagWidth, margin, tagWidth, tagHeight), currentTrialNumber.ToString());

        GUI.Label(new Rect(margin, margin + tagHeight, tagWidth, tagHeight), "Saltos del ensayo:");
        GUI.Label(new Rect(margin + tagWidth, margin + tagHeight, tagWidth, tagHeight), currentTrialJumpCount.ToString());

        GUI.Label(new Rect(Screen.width - margin - tagWidth * 2, margin, tagWidth, tagHeight), "Ensayos completados:");
        GUI.Label(new Rect(Screen.width - margin - tagWidth, margin, tagWidth, tagHeight), totalTrialCounts.ToString());

        GUI.Label(new Rect(Screen.width - margin - tagWidth * 2, margin + tagHeight, tagWidth, tagHeight), "Saltos totales:");
        GUI.Label(new Rect(Screen.width - margin - tagWidth, margin + tagHeight, tagWidth, tagHeight), totalJumpCount.ToString());

        if (totalTrialCounts > 0)
        {
            float media = (float)totalJumpCount / totalTrialCounts;
            GUI.Label(new Rect(Screen.width - margin - tagWidth * 2, margin + tagHeight * 2, tagWidth, tagHeight), "Media:");
            GUI.Label(new Rect(Screen.width - margin - tagWidth, margin + tagHeight * 2, tagWidth, tagHeight), media.ToString());
        }
    }
}
