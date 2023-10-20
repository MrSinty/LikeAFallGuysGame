using System.Collections;
using UnityEngine;

public class WindPlatformLogic : PlatformBase
{
    [SerializeField] private float waitBeforeChangeWind = 2f;

    private AudioSource audioSource;

    private float windForceX;
    private float windForceY = 0f;
    private float windForceZ;
    private Vector3 windVector;

    private readonly Color32 idleColor = new Color32(74, 102, 153, 255);
    private readonly Color32 attackColor = new Color32(164, 172, 185, 255);

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(ChangingWindDirection());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetPlayerObject(other);
            AddWind();
            TurnWindSoundOn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DeleteWind();
            TurnWindSoundOff();
            ForgetPlayerObject();
        }
    }

    IEnumerator ChangingWindDirection()
    {
        windForceX = Random.Range(-200, 200);
        windForceZ = Random.Range(-200, 200);
        windVector = new Vector3(windForceX, windForceY, windForceZ);
        yield return new WaitForSeconds(waitBeforeChangeWind);
        if (Player != null)
            AddWind();
        StartCoroutine(ChangingWindDirection());
    }

    private void AddWind()
    {
        platformRenderer.material.color = attackColor;
        Player.GetComponent<ConstantForce>().force = windVector;
    }

    private void DeleteWind()
    {
        platformRenderer.material.color = idleColor;
        Player.GetComponent<ConstantForce>().force = new Vector3(0f, 0f, 0f);
    }

    private void TurnWindSoundOn()
    {
        audioSource.mute = false;
    }

    private void TurnWindSoundOff()
    {
        audioSource.mute = true;
    }
}
