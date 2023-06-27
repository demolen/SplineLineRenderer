
using UnityEngine;
using UnityEngine.Events;

public class MouseInput : MonoBehaviour
{
    public static MouseInput Instance;
    public UnityEvent onMouseDown;

    private Camera _camera;
    private Collider2D _currentHitCollider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _camera = Camera.main;
    }

    private void FixedUpdate()
    {
       
        Vector2 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        _currentHitCollider = Physics2D.OverlapPoint(mousePosition);

        if (_currentHitCollider != null)
        {
            Debug.Log("Target name: " + _currentHitCollider.name);
        }

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            onMouseDown?.Invoke();
        }
    }

    public Collider2D GetCurrentHitCollider()
    {
        return _currentHitCollider;
    }
}