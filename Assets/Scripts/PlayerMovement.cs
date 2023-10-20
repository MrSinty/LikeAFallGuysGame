using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float jumpVelocity = 5f;
    [SerializeField] private float floorOffsetY = 0.05f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody rb;
    private bool jumping = false;
    private bool onPlatform = false;
    private float vertical;
    private float horizontal;
    private float inputAmount;
    private Vector3 moveDirection;
    private Vector3 raycastFloorPos;
    private Vector3 floorMovement;
    private Vector3 gravity;
    private Vector3 combinedRaycast;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        moveDirection = Vector3.zero;
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        Vector3 correctedVertical = vertical * Camera.main.transform.forward;
        Vector3 correctedHorizontal = horizontal * Camera.main.transform.right;

        Vector3 combinedInput = correctedHorizontal + correctedVertical;

        moveDirection = new Vector3((combinedInput).normalized.x, 0, (combinedInput).normalized.z);

        float inputMagnitude = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
        inputAmount = Mathf.Clamp01(inputMagnitude);

        if (moveDirection != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(moveDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, rot, Time.fixedDeltaTime * inputAmount * rotateSpeed);
            transform.rotation = targetRotation;
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            jumping = true;
            gravity.y = jumpVelocity;
            rb.MovePosition(floorMovement);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = (moveDirection * moveSpeed * inputAmount) + gravity;

        floorMovement = new Vector3(rb.position.x, FindFloor().y + floorOffsetY, rb.position.z);

        bool isGrounded = FloorRaycasts(0, 0, 0.6f, out _);

        if (!isGrounded)
        {
            gravity += Vector3.up * Physics.gravity.y * Time.fixedDeltaTime;
        }

        if (isGrounded && floorMovement != rb.position && !jumping)
        {
            rb.MovePosition(floorMovement);
            gravity.y = 0;
        }

        if (isGrounded && jumping) jumping = false;
    }

    Vector3 FindFloor()
    {
        float raycastWidth = 0.5f;
        int floorAverage = 1;

        FloorRaycasts(0, 0, 1.6f, out Vector3 outPoint);
        combinedRaycast = outPoint;
        floorAverage += (getFloorAverage(raycastWidth, 0) + getFloorAverage(-raycastWidth, 0) +
            getFloorAverage(0, raycastWidth) + getFloorAverage(0, -raycastWidth));

        return combinedRaycast / floorAverage;
    }

    int getFloorAverage(float offsetx, float offsetz)
    {
        if (FloorRaycasts(offsetx, offsetz, 1.6f, out Vector3 outPoint))
        {
            combinedRaycast += outPoint;
            return 1;
        }
        else { return 0; }
    }

    bool IsGrounded()
    {
        Vector3 tempFloorPos = transform.TransformPoint(0, 0, 0);
        if (Physics.Raycast(tempFloorPos, -Vector3.up, out _, 0.6f, groundLayer))
            return true;
        else
            return false;
    }

    bool FloorRaycasts(float offsetx, float offsetz, float raycastLength, out Vector3 floorPos)
    {
        raycastFloorPos = transform.TransformPoint(0 + offsetx, 0 + 0.5f, 0 + offsetz);

        Debug.DrawRay(raycastFloorPos, Vector3.down, Color.magenta);
        if (Physics.Raycast(raycastFloorPos, -Vector3.up, out RaycastHit hit, raycastLength, groundLayer))
        {
            floorPos = hit.point;
            return true;
        }
        else
        {
            floorPos = Vector3.zero;
            return false;
        }
    }

    private void CheckEvent(Collider collision)
    {
        if (collision.Equals("Respawn Trigger"))
        {
            transform.position = new Vector3(0, 0.05f, 0);
        }
        else if (collision.CompareTag("Platform") && !onPlatform)
        {
            transform.parent = collision.transform;
            onPlatform = true;
        }
        else if (!collision.CompareTag("Platform") && onPlatform)
        {
            transform.parent = null;
            onPlatform = true;
        }
    }
}
