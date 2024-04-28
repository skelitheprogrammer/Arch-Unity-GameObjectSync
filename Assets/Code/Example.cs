//Attach this script to a GameObject with a Camera component

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Example : MonoBehaviour
{
    // Draws a line from "startVertex" var to the curent mouse position.
    public Material mat;

    public float Scale;

    public Vector2 Position;

    private Vector3[] _points =
    {
        new(0,0,0),
        new(.5f, 1, 0),
        new(1, 0, 0),
        new(0.75f, 0.25f, 0f),
        new(0.25f, 0.25f, 0),
        new(0, 0, 0)
    };

    private void Start()
    {
        RenderPipelineManager.endCameraRendering += RenderPipelineManagerOnbeginCameraRendering;
    }


    private void OnDestroy()
    {
        RenderPipelineManager.endCameraRendering -= RenderPipelineManagerOnbeginCameraRendering;
    }

    private void RenderPipelineManagerOnbeginCameraRendering(ScriptableRenderContext arg1, Camera arg2)
    {
        Draw();
    }


    private void Draw()
    {
        if (!mat)
        {
            Debug.LogError("Please Assign a material on the inspector");
            return;
        }

        GL.PushMatrix();
        mat.SetPass(0);
        GL.LoadOrtho();


        GL.Begin(GL.LINE_STRIP); // Line
        GL.Color(Color.red);
        foreach (Vector3 point in _points)
        {
            GL.Vertex(point);
        }
        GL.End();
        GL.PopMatrix();
    }
}

public class CustomPass : ScriptableRenderPass
{
}