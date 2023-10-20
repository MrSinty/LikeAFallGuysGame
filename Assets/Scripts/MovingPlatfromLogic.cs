using System.Collections;
using UnityEngine;

public class MovingPlatfromLogic : PlatformBase
{
    [SerializeField] private float platformSpeed = 1f;
    [SerializeField] private float waitBeforeMove = 0f;
    [SerializeField] private Vector3 targetLocalPosition = new Vector3(0f, 1f, 0f);
    [SerializeField] private Color32 colorWhenIdle = new Color32(74, 102, 153, 255);
    [SerializeField] private Color32 colorWhenActivated = new Color32(74, 102, 153, 255);

    private AudioSource audioSource;
    private Vector3 targetPosition;
    private bool moving = false;
    private float elapsedTime = 0;
    private float timeToMove;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.time = 0.5f;
    }

    private void Update()
    {
        if (moving)
        {
            if (audioSource.time > timeToMove - 2f)
                audioSource.Stop();

            elapsedTime += Time.deltaTime;

            float elapsedPercetage = elapsedTime / timeToMove;
            elapsedPercetage = Mathf.SmoothStep(0, 1, elapsedPercetage);
            transform.position = Vector3.Lerp(transform.position, targetPosition, elapsedPercetage);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!moving)
                StartCoroutine(MoveBlock());
        }
    }

    IEnumerator MoveBlock()
    {
        platformRenderer.material.color = colorWhenActivated;
        yield return new WaitForSeconds(waitBeforeMove);

        moving = true;

        audioSource.Play();
        CalculateTarget(targetLocalPosition);
        yield return new WaitForSeconds(timeToMove);

        audioSource.Play();
        platformRenderer.material.color = colorWhenIdle;
        CalculateTarget(-targetLocalPosition);
        yield return new WaitForSeconds(timeToMove);

        moving = false;
    }

    private void CalculateTarget(Vector3 targetPos)
    {
        elapsedTime = 0f;

        targetPosition = transform.TransformPoint(targetPos);
        float distance = Vector3.Distance(transform.position, targetPosition);
        timeToMove = distance / platformSpeed;
    }
}
