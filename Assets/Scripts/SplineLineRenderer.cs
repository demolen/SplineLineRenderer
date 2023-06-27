using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class SplineLineRenderer : MonoBehaviour
{
    public SplineContainer splineContainer;
    public LineRenderer lineRenderer;
    public int pointsPerSegment = 30;
    public int dashScale = 1;

    private Camera _camera;
    private bool _needsUpdate;
    private int _numDashesId;
    private List<GameObject> _knotGameObjects = new List<GameObject>();
    private Vector3 _lastKnotPosition = Vector3.positiveInfinity;

    private void Start()
    {
        Initialize();
    }

    private void Awake()
    {
        _numDashesId = Shader.PropertyToID("_NumberOfTheDashes");
    }

    private void OnDestroy()
    {
        MouseInput.Instance.onMouseDown.RemoveListener(HandleMouseDown);
    }

    private void OnValidate()
    {
        _needsUpdate = true;
    }

    private void Update()
    {
        UpdateKnotPositions();
        if (_needsUpdate)
        {
            UpdateLineRenderer();
        }
    }

    private void Initialize()
    {
        _needsUpdate = false;
        _camera = Camera.main;
        MouseInput.Instance.onMouseDown.AddListener(HandleMouseDown);
    }

    private void HandleMouseDown()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        GameObject currentHitObject = GetCurrentHitObject();
        _knotGameObjects.Add(currentHitObject);
        Vector3 worldPosition = GetWorldPosition(currentHitObject);

        if (!IsNewKnotPosition(worldPosition)) return;

        AddKnotToWorldPosition(worldPosition);
        UpdateLineRenderer();
    }

    private GameObject GetCurrentHitObject()
    {
        var currentHitCollider = MouseInput.Instance.GetCurrentHitCollider();
        return currentHitCollider != null ? currentHitCollider.gameObject : null;
    }

    private Vector3 GetWorldPosition(GameObject currentHitObject)
    {
        var mousePosition = currentHitObject != null
            ? _camera.WorldToScreenPoint(currentHitObject.transform.position)
            : Input.mousePosition;

        return _camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, -_camera.transform.position.z));
    }

    private bool IsNewKnotPosition(Vector3 worldPosition)
    {
        return (_lastKnotPosition - worldPosition).sqrMagnitude >= float.Epsilon;
    }

    private void AddKnotToWorldPosition(Vector3 worldPosition)
    {
        BezierKnot bezierKnot = new BezierKnot
        {
            Position = worldPosition,
            Rotation = Quaternion.Euler(0, 90, 270)
        };
        splineContainer.Spline.Add(bezierKnot);
        splineContainer.Spline.SetTangentMode(TangentMode.AutoSmooth);
        _lastKnotPosition = worldPosition;
        _needsUpdate = true;
    }

    private void UpdateLineRenderer()
    {
        if (lineRenderer == null || splineContainer == null) return;

        var spline = splineContainer.Spline;
        int totalPoints = (spline.Count - 1) * pointsPerSegment + 1;
        lineRenderer.positionCount = totalPoints;
        lineRenderer.material.SetInt(_numDashesId, (int) math.round(spline.GetLength()) * dashScale);

        if (totalPoints <= 0) return;

        Vector3[] positions = new Vector3[totalPoints];

        for (var i = 0; i < totalPoints; i++)
        {
            float t = (float) i / (totalPoints - 1);
            positions[i] = spline.EvaluatePosition(t);
        }

        lineRenderer.SetPositions(positions);
        _needsUpdate = false;
    }

    private void UpdateKnotPositions()
    {
        if (splineContainer == null) return;

        bool updated = false;
        for (int i = 0; i < _knotGameObjects.Count; i++)
        {
            if (_knotGameObjects[i] != null)
            {
                Vector3 updatedPosition = _knotGameObjects[i].transform.position;
                if (math.distance(splineContainer.Spline[i].Position, new float3(updatedPosition.x, updatedPosition.y, updatedPosition.z)) > float.Epsilon)
                {
                    updated = true;
                    splineContainer.Spline[i] = new BezierKnot
                    {
                        Position = updatedPosition,
                        Rotation = Quaternion.Euler(0, 90, 270)
                    };
                }
            }
        }

        if (updated)
        {
            _needsUpdate = true;
        }
    }
}