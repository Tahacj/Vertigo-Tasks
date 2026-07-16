using UnityEngine;

public class SpinUI : MonoBehaviour
{
    public float spinSpeed = -50f;
    
    [Header("Out of Orbit Settings")]
    public float wobbleSpeed = 2f;
    public float wobbleAmount = 5f;

    private RectTransform rectTransform;
    private Vector2 startPos;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            startPos = rectTransform.anchoredPosition;
        }
    }

    void Update()
    {
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);

        if (rectTransform != null)
        {
            float xOffset = Mathf.Sin(Time.time * wobbleSpeed) * wobbleAmount;
            float yOffset = Mathf.Cos(Time.time * wobbleSpeed * 1.3f) * wobbleAmount;
            rectTransform.anchoredPosition = startPos + new Vector2(xOffset, yOffset);
        }
    }
}