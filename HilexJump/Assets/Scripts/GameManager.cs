using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    int score = 0;

    [SerializeField]
    int combo = 1;

    //Event to notify when the score changes
    public static UnityEngine.Events.UnityEvent<int> onScoreChanged;
    public static UnityEngine.Events.UnityEvent<int> onComboChanged;

    public static GameManager Instance;

    private void Awake()
    {
        if (onScoreChanged == null)
        {
            onScoreChanged = new UnityEngine.Events.UnityEvent<int>();
        }
        if (onComboChanged == null)
        {
            onComboChanged = new UnityEngine.Events.UnityEvent<int>();
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
        IncreaseCombo();
        score += 10 * combo;
        onScoreChanged?.Invoke(score);
    }

    public void IncreaseCombo()
    {
        combo++;
        onComboChanged?.Invoke(combo);
    }

    public void ResetCombo()
    {
        combo = 0;
        onComboChanged?.Invoke(combo);
    }
}
