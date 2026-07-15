using Unity.VisualScripting;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Transform target;
    [SerializeField] private float horizontalParallaxFactor = 1f;
    [SerializeField] private float verticalParallaxFactor = 0.1f;

    private Vector2 startPosition;
    private Vector2 endPositionRight;
    private Vector2 endPositionLeft;
    private Vector2 distance;
    private Vector2 tempDistance;

    private Transform rightEndPoint;
    private Transform leftEndPoint;

    private void Awake()
    {
        GetEndPoint();
    }

    private void GetEndPoint()
    {
        leftEndPoint = transform.GetChild(0);
        rightEndPoint = transform.GetChild(1);
    }

    private void Start()
    {
        startPosition.x = transform.position.x;

        endPositionLeft.x = Mathf.Abs(leftEndPoint.position.x);
        endPositionRight.x = Mathf.Abs(rightEndPoint.position.x);
    }

    private void LateUpdate()
    {
        GetHorizontalParallax();
        GetVerticalParallax();

        transform.position = new Vector3(startPosition.x + distance.x, startPosition.y + distance.y, transform.position.z);

        HandleHorizontalFollow();
    }

    private void GetHorizontalParallax()
    {
        tempDistance.x = target.position.x * (1 - horizontalParallaxFactor);
        distance.x = target.position.x * horizontalParallaxFactor;
    }

    private void GetVerticalParallax()
    {
        tempDistance.y = target.transform.position.y * (1 - verticalParallaxFactor);
        distance.y = target.transform.position.y * verticalParallaxFactor;
    }

    private void HandleHorizontalFollow()
    {
        if (tempDistance.x > startPosition.x + endPositionRight.x)
        {
            startPosition.x += endPositionRight.x;
        }
        else if (tempDistance.x < startPosition.x - endPositionLeft.x)
        {
            startPosition.x -= endPositionLeft.x;
        }
    }
}