using General;
using UnityEngine;

public class PooledAutoDisableMonobehaviour : PooledMonobehaviour
{
    [SerializeField] private float _poolRemoveDistance = 50;
    private Transform _cameraTransform;

    private void Start()
    {
        if (!(Camera.main is null)) _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (_cameraTransform.position.x - transform.position.x >= _poolRemoveDistance) gameObject.SetActive(false);
    }
}