using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUi : MonoBehaviour
{
    [SerializeField]
    Button returnMenu;

    [SerializeField]
    TextMeshProUGUI scoreText;

    [SerializeField]
    TextMeshProUGUI comboText;


    private void Start()
    {
        GameManager.onScoreChanged.AddListener(UpdateScore);
        GameManager.onComboChanged.AddListener(UpdateCombo);
        returnMenu.onClick.AddListener(ReturnToMenu);

        TriggerFinish.onFinishTriggered.AddListener(DisplayMenu);


    }

    void DisplayMenu()
    {
        Debug.Log("Display Menu");
        returnMenu.gameObject.SetActive(true);
    }

    void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score;
    }

    void UpdateCombo(int combo)
    {
        if (combo <= 1)
        {
            comboText.text = "";
            comboText.color = Color.white;
            return;
        }
        comboText.text = "x" + combo;

        switch (combo)
        {
            case 2:
                comboText.color = Color.green;
                break;
            case 5:
                comboText.color = Color.yellow;
                break;
            case 10:
                comboText.color = new Color(1f, 0.5f, 0f); // orange
                break;
            case 20:
                comboText.color = Color.red;
                break;
        }
    }

    void ReturnToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
