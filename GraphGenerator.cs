using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphGenerator: MonoBehaviour {
    public float spacing;
    public int Length;
    public int Width;
    public int Height;
    public GameObject Node;
    public GameObject Edge;

    public Material Selected;
    public Material Path;

    [Range(1, 50)]
    [SerializeField] float radius;

    [Range(0.5 f, 5)]
    [SerializeField] float MaxEdgeDistance;

    [Range(2, 500)]
    [SerializeField] int regionSize;

    [Range(1, 30)]
    [SerializeField] int rejectionSamples;

    public StaticValues.GraphStyle style;

    public StaticValues.Algorithm algorithm;

    private Graph graph;

    void Start() {
        Screen.fullScreen = true;

        spacing = StaticValues.spacing;
        Length = StaticValues.Length;
        Width = StaticValues.Width;
        Height = StaticValues.Height;

        Length = (Length < 1) ? 1 : Length;
        Width = (Width < 1) ? 1 : Width;
        Height = (Height < 1) ? 1 : Height;

        radius = StaticValues.radius;

        MaxEdgeDistance = StaticValues.MaxEdgeDistance;
        regionSize = StaticValues.regionSize;

        rejectionSamples = StaticValues.rejectionSamples;

        style = StaticValues.graphStyle;

        algorithm = StaticValues.algorithm;

    }
    bool first = true;
    private void Update() {
        if (first) {
            GenerateGraph();
            startSearch();
            first = false;
        }
    }

    void startSearch() {
        switch (algorithm) {
        case StaticValues.Algorithm.AStar:
            graph.AStar();
            break;
        case StaticValues.Algorithm.Dijkstra:
            graph.dijkstra();
            break;
        case StaticValues.Algorithm.BFS:
            graph.BFS();
            break;
        case StaticValues.Algorithm.DFS:
            graph.DFS();
            break;
        }
    }

    private void GenerateGraph() {
        switch (style) {
        case StaticValues.GraphStyle.Line:
            graph = new Graph(Length);
            Line(Length);
            break;
        case StaticValues.GraphStyle.Grid:
            graph = new Graph(Width * Length);
            Grid(Width, Length);
            break;
        case StaticValues.GraphStyle.Lattice:
            graph = new Graph(Width * Length * Height);
            Lattice(Width, Length, Height);
            break;
        case StaticValues.GraphStyle.Poisson3D:
            PoissonGenerator();
            break;
        case StaticValues.GraphStyle.Poisson2D:
            PoissonGenerator2D();
            break;
        }
    }

    public void DeleteAll() {
        foreach(GameObject o in FindObjectsOfType < GameObject > ()) {
            if (o != gameObject)
                Destroy(o);
        }
    }

    private Edge NewEdge(Node n1, Node n2) {
        Vector3 difference = n1.transform.position - n2.transform.position;

        Edge edge = NewEdge((n1.transform.position.x + n2.transform.position.x) / 2, (n1.transform.position.y + n2.transform.position.y) / 2, (n1.transform.position.z + n2.transform.position.z) / 2, (float) Math.Sqrt(Math.Pow(n1.transform.position.x - n2.transform.position.x, 2) + Math.Pow(n1.transform.position.y - n2.transform.position.y, 2) + Math.Pow(n1.transform.position.z - n2.transform.position.z, 2)) / 2);
        if (edge != null) {
            edge.transform.up = difference.normalized;
        } else {
            return null;
        }

        return edge;
    }

    public Node NewNode(float xPosition = 0, float yPosition = 0, float zPosition = 0) {
        GameObject currentNodeGameObject = Instantiate(Node, new Vector3(xPosition, yPosition, zPosition), Quaternion.identity);
        Node currentNode = currentNodeGameObject.GetComponent < Node > ();
        graph.AddNode(currentNode);
        return currentNode;
    }

    public Edge NewEdge(float xPosition = 0, float yPosition = 0, float zPosition = 0, float length = 1) {
        if (length <= 0) {
            return null;
        }
        GameObject edgeGameObject = Instantiate(Edge, new Vector3(xPosition, yPosition, zPosition), Quaternion.identity);
        edgeGameObject.transform.localScale = new Vector3(edgeGameObject.transform.localScale.x, length, edgeGameObject.transform.localScale.z);
        Edge edge = edgeGameObject.GetComponent < Edge > ();
        return edge;
    }

    public Node[] Line(int length, float yPosition = 0, float zPosition = 0) {
        Node[] line = new Node[length];
        Node previousNode = null;
        int index = 0;
        for (float f = -length / 2.0 f; f < length / 2.0 f; f++) {
            Node currentNode = NewNode(f * spacing + 1, yPosition, zPosition);

            if (previousNode != null) {
                ConnectNodesFully(previousNode, currentNode);
            }

            previousNode = currentNode;

            line[index] = currentNode;
            index++;
        }
        return line;
    }

    public Node[, ] Grid(int width, int length, float yPosition = 0) {
        Node[, ] grid = new Node[width, length];
        Node[] previousRow = new Node[length];

        int index = 0;
        for (float i = -width / 2.0 f; i < width / 2.0 f; i++) {
            Node[] row = Line(length, yPosition, zPosition: i * spacing + 1);

            for (int rowIndex = 0; rowIndex < length; rowIndex++) {
                if (previousRow[rowIndex] != null) {
                    ConnectNodesFully(previousRow[rowIndex], row[rowIndex]);
                }
                grid[index, rowIndex] = row[rowIndex];
            }

            previousRow = row;
            index++;
        }
        return grid;
    }

    public void Lattice(int width, int length, int height, float yPosition = 0) {
        Node[, ] previousGrid = new Node[width, length];

        for (float i = -height / 2.0 f; i < height / 2.0 f; i++) {
            Node[, ] grid = Grid(width, length, i * spacing + 1);

            for (int columnIndex = 0; columnIndex < width; columnIndex++) {
                for (int rowIndex = 0; rowIndex < length; rowIndex++) {
                    if (previousGrid[columnIndex, rowIndex] != null) {
                        ConnectNodesFully(previousGrid[columnIndex, rowIndex], grid[columnIndex, rowIndex]);
                    }
                }
            }

            previousGrid = grid;
        }
    }

    public int PoissonGenerator() {
        List < Vector3 > points;
        List < Node > nodes = new List < Node > ();
        points = PoissonSampler.GeneratePoisson(regionSize, radius, rejectionSamples);
        graph = new Graph(points.Count);
        if (points != null) {
            for (int i = 0; i < points.Count; i++) {
                Node currentNode = NewNode(points[i].x, points[i].y, points[i].z);
                nodes.Add(currentNode);
            }

            foreach(var node in nodes) {
                Collider[] NearNodes = Physics.OverlapSphere(node.transform.position, radius * MaxEdgeDistance);
                foreach(var NearNode in NearNodes) {
                    if (NearNode.gameObject.GetComponent < Node > () != null) {
                        ConnectNodesFully(node, NearNode.gameObject.GetComponent < Node > ());
                    }
                }
            }
        }

        return points.Count;
    }

    public int PoissonGenerator2D() {
        List < Vector3 > points;
        List < Node > nodes = new List < Node > ();
        points = PoissonSampler2D.GeneratePoisson(regionSize, radius, rejectionSamples);
        graph = new Graph(points.Count);
        if (points != null) {
            for (int i = 0; i < points.Count; i++) {
                Node currentNode = NewNode(points[i].x, points[i].y, points[i].z);
                nodes.Add(currentNode);
            }

            foreach(var node in nodes) {
                Collider[] NearNodes = Physics.OverlapSphere(node.transform.position, radius * MaxEdgeDistance);
                foreach(var NearNode in NearNodes) {
                    if (NearNode.gameObject.GetComponent < Node > () != null) {
                        ConnectNodesFully(node, NearNode.gameObject.GetComponent < Node > ());
                    }
                }
            }
        }

        return points.Count;
    }

    public void ConnectNodesFully(Node currentNode, Node previousNode) {

        Edge edge = NewEdge(currentNode, previousNode);

        graph.ConnectNodesFully(currentNode, previousNode, edge);

    }

}