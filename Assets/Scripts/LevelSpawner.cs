using System.Collections.Generic;
using General;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField] private int firstSectionCount;
    [SerializeField] private float halfPlatformLength;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private PooledMonobehaviour platformPrefab;
    [SerializeField] private GameObject lastCeiling;
    [SerializeField] private GameObject lastGround;

    [SerializeField] private Vector3 lastLevelSectionPosition;
    [SerializeField] private float minimumLevelSectionSeparation;

    [SerializeField] private List<PooledAutoDisableMonobehaviour> levelSections;

    private Queue<Vector3> _platformEndPositions;
    private Queue<Vector3> _sectionEndPositions;

    private void Start()
    {
        _sectionEndPositions = new Queue<Vector3>();
        _platformEndPositions = new Queue<Vector3>();
        SpawnNewPlatforms();
        SpawnNewPlatforms();
        SpawnFirstSections();
    }

    private void Update()
    {
        CheckForNewPlatforms();
        CheckForCameraPassingSection();
    }


    public void ResetLevel()
    {
        Start();
    }

    private void SpawnFirstSections()
    {
        for (int i = 0; i < firstSectionCount; i++) SpawnSection();
    }

    private void CheckForCameraPassingSection()
    {
        if (!(cameraTransform.position.x > _sectionEndPositions.Peek().x)) return;
        SpawnSection();
        _sectionEndPositions.Dequeue();
    }

    private void SpawnSection()
    {
        Vector3 positionWithOffset = lastLevelSectionPosition;
        positionWithOffset.x += minimumLevelSectionSeparation;
        Transform lastTransform = SpawnSingleSection(positionWithOffset);
        _sectionEndPositions.Enqueue(positionWithOffset);
        //"End point" child is always 0
        lastLevelSectionPosition = lastTransform.GetChild(0).position;
    }

    private Transform SpawnSingleSection(Vector3 startPosition)
    {
        PooledAutoDisableMonobehaviour monobehaviour = levelSections[Random.Range(0, levelSections.Count)]
            .Get<PooledAutoDisableMonobehaviour>();

        Transform monobehaviourTransform = monobehaviour.transform;
        monobehaviourTransform.position = startPosition;

        return monobehaviourTransform;
    }

    private void CheckForNewPlatforms()
    {
        if (!(cameraTransform.position.x >= _platformEndPositions.Peek().x)) return;

        SpawnNewPlatforms();
        _platformEndPositions.Dequeue();
    }

    private void SpawnNewPlatforms()
    {
        PooledMonobehaviour newPlatformCeiling = platformPrefab.Get<PooledMonobehaviour>();
        Vector3 newCeilingPosition =
            new Vector3(lastCeiling.transform.GetChild(0).transform.position.x + halfPlatformLength,
                lastCeiling.transform.position.y, 0);
        newPlatformCeiling.transform.position = newCeilingPosition;
        lastCeiling = newPlatformCeiling.gameObject;
        _platformEndPositions.Enqueue(lastCeiling.transform.GetChild(0).transform.position);

        PooledMonobehaviour newPlatformGround = platformPrefab.Get<PooledMonobehaviour>();
        Vector3 newGroundPosition =
            new Vector3(lastGround.transform.GetChild(0).transform.position.x + halfPlatformLength,
                lastGround.transform.position.y, 0);
        newPlatformGround.transform.position = newGroundPosition;
        lastGround = newPlatformGround.gameObject;
    }
}