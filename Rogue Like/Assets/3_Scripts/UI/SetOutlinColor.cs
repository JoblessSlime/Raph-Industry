using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SetOutlinColor : MonoBehaviour
{
    private Material _material;

    [ColorUsage(true, true)]
    public Color outlineColor = Color.red;

    [Range(0f, 5f)]
    public float outlineSize = 1f;

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        _material = sr.material;

        UpdateOutline();
    }

    void Update()
    {
        // Example: Update in real-time for testing
        UpdateOutline();
    }

    void UpdateOutline()
    {
        if (_material != null)
        {
            _material.SetColor("_OutlineColor", outlineColor);
            _material.SetFloat("_OutlineSize", outlineSize);
            Debug.Log(_material.color);
            Debug.Log("OutlineChanged");
        }
    }
}
