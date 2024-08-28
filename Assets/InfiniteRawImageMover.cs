using UnityEngine;
using UnityEngine.UI;

public class InfiniteRawImageMover : MonoBehaviour
{
    [SerializeField] private Vector2 speed;
    [SerializeField] private RawImage image;
    void Update()
    {
        var rect = image.uvRect;
        rect.x += speed.x * Time.deltaTime;
        rect.y += speed.y * Time.deltaTime;
        image.uvRect = rect;
    }
}
