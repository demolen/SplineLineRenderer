
using UnityEngine;
using UnityEngine.Splines;

public class SplineAnimateActivator : MonoBehaviour
{
    private SplineAnimate _splineAnimator;
    private bool _splineAnimatorAdded = false;
    public SplineContainer referenceToTheSplineContainer;
    public float maxSpeed = 1;

    // Update is called once per frame
    private void Update()
    {
        if (referenceToTheSplineContainer.Spline.Count < 2 || _splineAnimatorAdded != false) return;
        _splineAnimator = gameObject.AddComponent<SplineAnimate>();
        _splineAnimator.Container = referenceToTheSplineContainer;
        _splineAnimator.Alignment = SplineAnimate.AlignmentMode.SplineElement;
        _splineAnimator.ObjectUpAxis = SplineComponent.AlignAxis.ZAxis;
        _splineAnimator.ObjectForwardAxis = SplineComponent.AlignAxis.YAxis;
        _splineAnimator.AnimationMethod = SplineAnimate.Method.Speed;
        _splineAnimator.MaxSpeed = maxSpeed;
        _splineAnimatorAdded = true;
    }
}
