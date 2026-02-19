using UnityEngine;

public class MoveGround : MonoBehaviour
{
    public Vector3 moveDirection = Vector3.right;
    public float moveDistance = 5f;
    public float moveSpeed = 2f;

    Vector3 startPos;
    Vector3 lastPos;

    void Start()
    {
      startPos = transform.position;
      lastPos = startPos;

      //“®‚«o‚µ‚ğƒ‰ƒ“ƒ_ƒ€‚É
      if(Random.value < 0.5f)
        {
            moveDirection = -moveDirection;
        }
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time * moveSpeed, moveDistance);
        transform.position = startPos + moveDirection.normalized * t;
    }

    private void LateUpdate()
    {
        lastPos = transform.position;
    }

    public Vector3 GetDelta()
    {
        return transform.position - lastPos;
    }
}
