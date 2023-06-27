using UnityEngine;

public class ChangeColorOnMouseOn : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    public Color onMouseColor;
    public Color defaultColor;
    private GameObject _currentGameObject;
    private GameObject _previousGameObject;
    private int _currentGameObjectId;
    private int _previousGameObjectId;

    // Start is called before the first frame update
    private void Start()
    {
        onMouseColor = Color.yellow;
        defaultColor = Color.white;
    }

    // Update is called once per frame
    private void Update()
    {
        if (MouseInput.Instance.GetCurrentHitCollider() != null)
        {
            _currentGameObjectId = MouseInput.Instance.GetCurrentHitCollider().gameObject.GetInstanceID();
            _currentGameObject = MouseInput.Instance.GetCurrentHitCollider().gameObject;

            if (_previousGameObjectId != _currentGameObjectId && _previousGameObject != null)
            {
                _previousGameObject.GetComponent<SpriteRenderer>().material.color = defaultColor;
            }

            _currentGameObject.GetComponent<SpriteRenderer>().material.color = onMouseColor;
            _previousGameObject = _currentGameObject;
            _previousGameObjectId = _currentGameObjectId;
        }
        else
        {
            if (_previousGameObject == null) return;
            _previousGameObject.GetComponent<SpriteRenderer>().material.color = defaultColor;
            _previousGameObject = null;
            _previousGameObjectId = -1;
        }
    }
}