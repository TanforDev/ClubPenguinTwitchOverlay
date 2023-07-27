using DG.Tweening;
using UnityEngine;

public class Snowball : Entity
{

    [SerializeField] private float speed = 100;
    [SerializeField] private float height = 100;

    [SerializeField] private float randomizer = 10;

    private Tween throwTween;

    public void Shoot(Vector2 target)
    {
        Vector2 origin = RectTransform.anchoredPosition;
        RectTransform.localScale = Vector3.one;

        target = target + new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * randomizer;

        float cumulativeDistance = 0f;
        float sampleCount = 10; // Number of points to sample
        Vector2 prevPoint = origin;
        for (int i = 1; i <= sampleCount; i++)
        {
            float t = i / (float)sampleCount;
            Vector2 point = SampleParabola(origin, target, height, t, Vector2.up);
            cumulativeDistance += Vector2.Distance(prevPoint, point);
            prevPoint = point;
        }

        float currentTime = 0;
        if (throwTween != null)
        {
            throwTween.Kill();
        }
        throwTween = DOTween.To(() => currentTime, x => currentTime = x, 1, cumulativeDistance / speed)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                RectTransform.anchoredPosition = SampleParabola(origin, target, 100, currentTime, Vector2.up);
            })
            .OnComplete(() =>
            {
                RectTransform.localScale = new Vector3(1, 0.8f, 1);
            });
    }
    Vector2 SampleParabola(Vector2 start, Vector2 end, float height, float t, Vector2 outDirection)
    {
        float parabolicT = t * 2 - 1;
        //start and end are not level, gets more complicated
        Vector2 travelDirection = end - start;
        Vector2 up = outDirection;
        Vector2 result = start + t * travelDirection;
        result += ((-parabolicT * parabolicT + 1) * height) * up.normalized;
        return result;
    }
}