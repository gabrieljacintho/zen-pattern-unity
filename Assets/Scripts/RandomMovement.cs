using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public float speed = 2f;               // Movement speed of the object
    public float changeDirectionTime = 2f; // Time interval to change target direction
    public float curveIntensity = 2f;      // Intensity of the curvature (how fast it curves)

    private Vector2 targetDirection;
    private Vector2 currentDirection;
    private float timer;

    void Start()
    {
        SetNewDirection();
        currentDirection = targetDirection;
        timer = changeDirectionTime;
    }

    void Update()
    {
        // Gradually change the current direction toward the target direction
        currentDirection = Vector2.Lerp(currentDirection, targetDirection, Time.deltaTime * curveIntensity);
        currentDirection.Normalize();

        // Move the object in the curved direction
        transform.Translate(currentDirection * speed * Time.deltaTime);

        // Countdown timer to set a new target direction
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SetNewDirection();
            timer = changeDirectionTime;
        }
    }

    void SetNewDirection()
    {
        // Pick a random direction by generating a random angle
        float angle = Random.Range(0f, 360f);
        targetDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        targetDirection.Normalize();
    }
}
