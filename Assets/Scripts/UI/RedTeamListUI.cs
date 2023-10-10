using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RedTeamListUI : MonoBehaviour
{

    public GameObject teamSelectButton;
    public TeamManagerSO redTeamManager;
    public RectTransform scrollContentView;


    // Start is called before the first frame update
    void Start()
    {
        redTeamManager = Resources.Load<TeamManagerSO>("RedTeam/RedTeamManager");

        float offset = -40.0f; // Initial margin from top
        float newHeight = 80.0f;

        for (int i = 0; i < redTeamManager.teams.Length; i++)
        {
            var button = Instantiate(teamSelectButton, transform.parent);
            button.name = redTeamManager.teams[i].name;
            button.transform.position += new Vector3 (0, offset, 0);
            offset += -50;

            // Access the child TextMeshPro component
            Transform child = button.transform.Find("Text (TMP)");
            TextMeshProUGUI childTextComponent = child.GetComponent<TextMeshProUGUI>();
            childTextComponent.text = redTeamManager.teams[i].name;

            if (i >= 5)
            {
                scrollContentView.sizeDelta = new Vector2(scrollContentView.sizeDelta.x, scrollContentView.sizeDelta.y + newHeight);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
