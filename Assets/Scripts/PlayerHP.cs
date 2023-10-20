using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private int healthPoints = 100;
    [SerializeField] private GameObject UIObject;

    private UI ui;

    public int PlayerHealth
    {
        get { return healthPoints; }
        set
        {
            healthPoints = value;

            if (healthPoints <= 0)
            {
                ui.ShowScreen(false, 0f);
            }
        }
    }

    private void Start()
    {
        ui = UIObject.GetComponent<UI>();
    }
}
