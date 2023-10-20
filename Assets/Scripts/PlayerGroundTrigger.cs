using UnityEngine;

public class PlayerGroundTrigger : MonoBehaviour
{
    [SerializeField] private UI UIObject;
    private UI ui;
    private bool isTimerRunning = false;

    private void Start()
    {
        ui = UIObject.GetComponent<UI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Fall Trigger")
        {
            ui.EndTimer();
            ui.ShowScreen(false, 0f);
        }
        else if (other.name == "Start Trigger")
        {
            if (!isTimerRunning)
            {
                isTimerRunning = true;
                ui.StartTimer();
            }
        }
        else if (other.name == "Finish Trigger")
        {
            isTimerRunning = false;
            ui.EndTimer();
            ui.ShowScreen(true, 0f);
        }
        else if (other.gameObject.CompareTag("Platform"))
        {
            transform.parent.parent = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            transform.parent.parent = null;
        }
    }
}
