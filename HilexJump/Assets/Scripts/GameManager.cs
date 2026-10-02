using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    int score = 0;

    //Event to notify when the score changes
    public static UnityEngine.Events.UnityEvent<int> onScoreChanged;

    public static GameManager Instance;

    private void Awake()
    {
        if (onScoreChanged == null)
        {
            onScoreChanged = new UnityEngine.Events.UnityEvent<int>();
        }

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Init()
    {
        score = 0;
    }

    public void IncreaseScore()
    {
        score++;
        onScoreChanged?.Invoke(score);
        Debug.Log("Score: " + score);
    }
}
