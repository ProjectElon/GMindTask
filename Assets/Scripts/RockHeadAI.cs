using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RockHeadAI : MonoBehaviour
{
    [SerializeField] private Vector2[] _points;
    [SerializeField] private float _waitTimeBetweenTransitions = 2.0f;
    [SerializeField] private float _timeToReachTargetPoint = 2.0f;
    private float _moveSpeed;
    private int _targetPointIndex;

    private enum State
    {
        Moving,
        Waiting
    }

    private State _state;
    private float _waitTimer;

    private void Awake()
    {
        MoveTowardsNextTarget();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameRunning)
        {
            return;
        }

        switch (_state)
        {
            case State.Moving:
                float distance = Vector2.Distance(transform.position, _points[_targetPointIndex]);
                if (distance >= 0.01f)
                {
                    transform.position = Vector2.MoveTowards(transform.position, _points[_targetPointIndex], _moveSpeed * Time.deltaTime);
                }
                else
                {
                    MoveTowardsNextTarget();
                }
                break;

            case State.Waiting:
                _waitTimer -= Time.deltaTime;
                if (_waitTimer <= 0.0f)
                {
                    _state = State.Moving;
                }
                break;
        }
    }

    private void MoveTowardsNextTarget()
    {
        _state = State.Waiting;
        _targetPointIndex = (_targetPointIndex + 1) % _points.Length;
        _waitTimer = _waitTimeBetweenTransitions;
        _moveSpeed = Vector3.Distance(transform.position, _points[_targetPointIndex]) / _timeToReachTargetPoint;
    }
}
