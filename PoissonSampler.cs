using System;
using System.Collections.Generic;
using UnityEngine;

public class PoissonSampler {
    public static List < Vector3 > GeneratePoisson(int gridSize, float radius, int reject) {
        var cellSize = radius / Mathf.Sqrt(3);
        var backgroundGridSize = Mathf.CeilToInt(gridSize / cellSize);

        int[, , ] backgroundGrid = new int[backgroundGridSize, backgroundGridSize, backgroundGridSize];

        for (int i = 0; i < backgroundGridSize; i++) {
            for (int j = 0; j < backgroundGridSize; j++) {
                for (int k = 0; k < backgroundGridSize; k++) {
                    backgroundGrid[i, j, k] = -1;
                }
            }
        }

        var initialSample = new Vector3(gridSize / 2.0 f, gridSize / 2.0 f, gridSize / 2.0 f);

        var points = new List < Vector3 > () {
            initialSample
        };
        var activePoints = new List < Vector3 > () {
            initialSample
        };

        var initialIndex = (int)(backgroundGridSize / 2.0 f);
        backgroundGrid[initialIndex, initialIndex, initialIndex] = 0;

        while (activePoints.Count > 0) {
            var centerPointIndex = UnityEngine.Random.Range(0, activePoints.Count);
            var centerPoint = activePoints[centerPointIndex];

            var valid = false;

            for (int i = 0; i < reject; i++) {
                var randomPosition = (UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(radius, 2 * radius)) + centerPoint;

                if (CheckPoint(randomPosition, gridSize, radius, cellSize, backgroundGrid, points)) {
                    points.Add(randomPosition);
                    activePoints.Add(randomPosition);

                    backgroundGrid[
                        (int)(randomPosition.x / cellSize),
                        (int)(randomPosition.y / cellSize),
                        (int)(randomPosition.z / cellSize)] = points.Count - 1;

                    valid = true;

                    break;
                }
            }

            if (valid == false) {
                activePoints.RemoveAt(centerPointIndex);
            }
        }

        return points;
    }

    private static bool CheckPoint(Vector3 candicatePoint, int gridSize, float radius, float cellSize, int[, , ] backgroundGrid, List < Vector3 > points) {
        if (checkPosition(candicatePoint.x) && checkPosition(candicatePoint.y) && checkPosition(candicatePoint.z)) {
            var cellX = (int)(candicatePoint.x / cellSize);
            var cellY = (int)(candicatePoint.y / cellSize);
            var cellZ = (int)(candicatePoint.z / cellSize);

            var startX = Math.Max(0, cellX - 2);
            var startY = Math.Max(0, cellY - 2);
            var startZ = Math.Max(0, cellZ - 2);

            var endX = Math.Min(backgroundGrid.GetLength(0), cellX + 2);
            var endY = Math.Min(backgroundGrid.GetLength(0), cellY + 2);
            var endZ = Math.Min(backgroundGrid.GetLength(0), cellZ + 2);

            for (int x = startX; x < endX; x++) {
                for (int y = startY; y < endY; y++) {
                    for (int z = startZ; z < endZ; z++) {
                        var index = backgroundGrid[x, y, z];

                        if (index >= 0) {
                            float distance = (candicatePoint - points[index]).sqrMagnitude;

                            if (distance < (radius * radius)) {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        return false;

        bool checkPosition(float pos) => pos >= 0 && pos < gridSize;
    }
}

public class PoissonSampler2D {
    public static List < Vector3 > GeneratePoisson(int gridSize, float radius, int reject) {
        var cellSize = radius / Mathf.Sqrt(2);
        var backgroundGridSize = Mathf.CeilToInt(gridSize / cellSize);

        int[, ] backgroundGrid = new int[backgroundGridSize, backgroundGridSize];

        for (int i = 0; i < backgroundGridSize; i++) {
            for (int j = 0; j < backgroundGridSize; j++) {

                backgroundGrid[i, j] = -1;

            }
        }

        var initialSample = new Vector3(gridSize / 2.0 f, 0, gridSize / 2.0 f);

        var points = new List < Vector3 > () {
            initialSample
        };
        var activePoints = new List < Vector3 > () {
            initialSample
        };

        var initialIndex = (int)(backgroundGridSize / 2.0 f);
        backgroundGrid[initialIndex, initialIndex] = 0;

        while (activePoints.Count > 0) {
            var centerPointIndex = UnityEngine.Random.Range(0, activePoints.Count);
            var centerPoint = activePoints[centerPointIndex];

            var valid = false;

            for (int i = 0; i < reject; i++) {
                var randomPosition = (UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(radius, 2 * radius)) + centerPoint;

                randomPosition.y = 0;
                if (CheckPoint(randomPosition, gridSize, radius, cellSize, backgroundGrid, points)) {
                    points.Add(randomPosition);
                    activePoints.Add(randomPosition);

                    backgroundGrid[
                        (int)(randomPosition.x / cellSize),
                        (int)(randomPosition.z / cellSize)] = points.Count - 1;

                    valid = true;

                    break;
                }
            }

            if (valid == false) {
                activePoints.RemoveAt(centerPointIndex);
            }
        }

        return points;
    }

    private static bool CheckPoint(Vector3 candicatePoint, int gridSize, float radius, float cellSize, int[, ] backgroundGrid, List < Vector3 > points) {
        if (checkPosition(candicatePoint.x) && checkPosition(candicatePoint.z)) {
            var cellX = (int)(candicatePoint.x / cellSize);
            var cellZ = (int)(candicatePoint.z / cellSize);

            var startX = Math.Max(0, cellX - 2);
            var startZ = Math.Max(0, cellZ - 2);

            var endX = Math.Min(backgroundGrid.GetLength(0), cellX + 2);
            var endZ = Math.Min(backgroundGrid.GetLength(0), cellZ + 2);

            for (int x = startX; x < endX; x++) {
                for (int z = startZ; z < endZ; z++) {
                    var index = backgroundGrid[x, z];

                    if (index >= 0) {
                        float distance = (candicatePoint - points[index]).sqrMagnitude;

                        if (distance < (radius * radius)) {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        return false;

        bool checkPosition(float pos) => pos >= 0 && pos < gridSize;
    }
}