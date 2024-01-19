using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Snake : MonoBehaviour
{
    [SerializeField] private List<Transform> _tails;
    [Range(0, 4), SerializeField] private float _bonesDistance;
    [SerializeField] private GameObject _bonePrefab;
    [Range(0, 4), SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;

    private Transform _transform;

    public UnityEvent OnEat;

    private void Start()
    {
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        MoveHead(_moveSpeed);
        MoveTail();
        Rotate(_rotateSpeed);
    }

    private void MoveHead(float speed)
    {
        _transform.position = _transform.position + _transform.forward * speed * Time.deltaTime;
    }

    private void MoveTail()
    {
        float sqrDistance = _bonesDistance * _bonesDistance;
        Vector3 previousPosition = _transform.position;

        foreach (var bone in _tails)
        {
            if ((bone.position - previousPosition).sqrMagnitude > sqrDistance)
            {
                Vector3 currentBonePOsition = bone.position;
                bone.position = previousPosition;
                previousPosition = currentBonePOsition;
            }
            else
            {
                break;
            }
        }
    }

    private void Rotate(float speed)
    {
        float angle = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        _transform.Rotate(0, angle, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Food food))
        {
            Destroy(other.gameObject);

            GameObject bone = Instantiate(_bonePrefab, _transform.position, _transform.rotation);
            _tails.Add(bone.transform);
        }
    }
}
