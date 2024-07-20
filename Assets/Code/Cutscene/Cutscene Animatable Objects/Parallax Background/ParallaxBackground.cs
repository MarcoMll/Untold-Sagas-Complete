using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ParallaxBackground : MonoBehaviour, IAnimatable
{
    [SerializeField] private Transform tilesParent;
    [SerializeField] private BackgroundTile[] foregroundTiles;
    [SerializeField] private BackgroundTile[] midgroundTiles;

    [SerializeField] private Transform startPoint, endPoint;
    [SerializeField] private float basicSpeed = 200f;

    private List<BackgroundTile> _foregrounds = new List<BackgroundTile>();
    private List<BackgroundTile> _midgrounds = new List<BackgroundTile>();

    private void Update()
    {
        UpdateTiles(_midgrounds, midgroundTiles, 0.8f);
        UpdateTiles(_foregrounds, foregroundTiles, 2f);
    }

    private void UpdateTiles(List<BackgroundTile> tiles, BackgroundTile[] tilePrefabs, float speedMultiplier)
    {
        for (int i = tiles.Count - 1; i >= 0; i--)
        {
            BackgroundTile tile = tiles[i];

            if (tile.transform.position.x >= (startPoint.position.x + endPoint.position.x) / 2 && !tile.HasSpawnedNext)
            {
                BackgroundTile newTile = SpawnTile(tilePrefabs[Random.Range(0, tilePrefabs.Length)]);
                newTile.StartMoving(endPoint, basicSpeed * speedMultiplier);
                tiles.Add(newTile);
                tile.HasSpawnedNext = true;
            }

            if (tile.transform.position.x >= endPoint.position.x + tile.Length / 2)
            {
                tiles.Remove(tile);
                Destroy(tile.gameObject);
            }
        }
    }

    public void ExecuteAnimation(float delay = 0)
    {
        // After a delay, spawn the first tiles at the center of the canvas
        StartCoroutine(SpawnFirstTilesAfterDelay(delay));
    }

    public bool AnimationFinished()
    {
        throw new System.NotImplementedException();
    }
    
    private IEnumerator SpawnFirstTilesAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        SpawnFirstMidgroundTile();
        SpawnFirstForegroundTile();
    }

    private void SpawnFirstForegroundTile()
    {
        BackgroundTile tile = foregroundTiles[Random.Range(0, foregroundTiles.Length)];
        BackgroundTile foregroundTile = Instantiate(tile, startPoint.position, Quaternion.identity, transform);
        foregroundTile.transform.localPosition = Vector3.zero; // Center of the canvas
        foregroundTile.StartMoving(endPoint, basicSpeed * 1.2f);
        foregroundTile.transform.parent = tilesParent;
        _foregrounds.Add(foregroundTile);
    }

    private void SpawnFirstMidgroundTile()
    {
        BackgroundTile tile = midgroundTiles[Random.Range(0, midgroundTiles.Length)];
        BackgroundTile midgroundTile = Instantiate(tile, startPoint.position, Quaternion.identity, transform);
        midgroundTile.transform.localPosition = Vector3.zero; // Center of the canvas
        midgroundTile.StartMoving(endPoint, basicSpeed * .8f);
        midgroundTile.transform.parent = tilesParent;
        _midgrounds.Add(midgroundTile);
    }

    private void SpawnForegroundTile()
    {
        BackgroundTile foregroundTile = SpawnTile(foregroundTiles[Random.Range(0, foregroundTiles.Length)]);
        foregroundTile.StartMoving(endPoint, basicSpeed * 1.2f);
        _foregrounds.Add(foregroundTile);
    }

    private void SpawnMidgroundTile()
    {
        BackgroundTile midgroundTile = SpawnTile(midgroundTiles[Random.Range(0, midgroundTiles.Length)]);
        midgroundTile.StartMoving(endPoint, basicSpeed * 0.8f);
        _midgrounds.Add(midgroundTile);
    }

    private BackgroundTile SpawnTile(BackgroundTile tile)
    {
        Vector3 spawnPosition = startPoint.position;
        spawnPosition.x -= tile.Length / 2;

        BackgroundTile spawnedTile = Instantiate(tile, spawnPosition, Quaternion.identity);
        spawnedTile.transform.parent = tilesParent;

        return spawnedTile;
    }
}