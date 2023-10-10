using TMPro;
using UnityEngine;

/// <summary>
/// Populates the scroll view on the landing panel with Team formation UI buttons
/// </summary>
public class RedTeamListUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _teamSelectButton;

    [SerializeField]
    public TeamManagerSO _redTeamManager;

    [SerializeField]
    public RectTransform _scrollContentView;


    // Start is called before the first frame update
    void Start()
    {
        _redTeamManager = Resources.Load<TeamManagerSO>("RedTeam/RedTeamManager");

        float offset = -40.0f; // Initial margin from top
        float newHeight = 80.0f; // Rate at which to increase the Content view

        for (int i = 0; i < _redTeamManager.teams.Length; i++)
        {
            var button = Instantiate(_teamSelectButton, transform.parent); // Instantiate TMP button

            // Set name and position
            button.name = _redTeamManager.teams[i].name;
            button.transform.position += new Vector3 (0, offset, 0);
            offset += -50;

            // Access the child TextMeshPro component
            Transform child = button.transform.Find("Text (TMP)");
            TextMeshProUGUI childTextComponent = child.GetComponent<TextMeshProUGUI>();

            // Set button name
            childTextComponent.text = _redTeamManager.teams[i].name;

            // Increase Content view size if at least 5 teams are loaded in.
            if (i > 3)
            {
                _scrollContentView.sizeDelta = new Vector2(_scrollContentView.sizeDelta.x, _scrollContentView.sizeDelta.y + newHeight);
            }
        }
    }

}
