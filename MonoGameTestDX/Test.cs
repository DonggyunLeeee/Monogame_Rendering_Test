using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ClipperLib;
using Poly2Tri;
using Poly2Tri.Triangulation.Polygon;
using Test_Layer_Points;
using HalconDotNet;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using SharpDX.Direct3D9;
using SharpDX.Direct2D1;


public class PolygonShape
{
    public List<Vector2> Vertices { get; private set; }

    public PolygonShape(List<Vector2> vertices)
    {
        Vertices = vertices;
    }
        
    public List<VertexPositionColor> Triangulate(Color color)
    {
        List<VertexPositionColor> triangleVertices = new List<VertexPositionColor>();

        var points = new List<PolygonPoint>();
        foreach (var vertex in Vertices)
        {
            points.Add(new PolygonPoint(vertex.X, -vertex.Y));
        }
        var polyline = new Polygon(points);

        P2T.Triangulate(polyline);

        foreach (var triangle in polyline.Triangles)
        {
            triangleVertices.Add(new VertexPositionColor(new Vector3((float)triangle.Points[2].X, (float)-triangle.Points[2].Y, 0), color));
            triangleVertices.Add(new VertexPositionColor(new Vector3((float)triangle.Points[1].X, (float)-triangle.Points[1].Y, 0), color));
            triangleVertices.Add(new VertexPositionColor(new Vector3((float)triangle.Points[0].X, (float)-triangle.Points[0].Y, 0), color));
        }

        return triangleVertices;
    }

    public void Draw(GraphicsDevice graphicsDevice, BasicEffect basicEffect, Color color)
    {
        if (Vertices.Count < 3) return;

        List<VertexPositionColor> triangleVertices = Triangulate(color);

        foreach (var pass in basicEffect.CurrentTechnique.Passes)
        {
            pass.Apply();
            graphicsDevice.DrawUserPrimitives(Microsoft.Xna.Framework.Graphics.PrimitiveType.TriangleList, triangleVertices.ToArray(), 0, triangleVertices.Count / 3);
        }
    }
}

public class Game1 : Game
{
    private GraphicsDeviceManager graphics;
    private BasicEffect basicEffect;
    private List<Vector2> polygonA, polygonB;

    private List<List<Vector2>> polygons;
    private Vector2 posOffset;
    private double scale = 0.125;
    private bool isDragging = false;
    private Vector2 prevmousePosition;
    private int prevScrollValue;

    private Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch;
    RenderTarget2D renderTarget;
    Texture2D polygonTexture;
    bool needsUpdate = true;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        graphics.PreferredBackBufferWidth = 1820;
        graphics.PreferredBackBufferHeight = 980;
        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += OnClientSizeChanged;
    }

    private void OnClientSizeChanged(object sender, System.EventArgs e)
    {
        graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
        graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
        graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        base.Initialize();
        polygons = new List<List<Vector2>>();

        posOffset = Vector2.Zero;
        prevScrollValue = Mouse.GetState().ScrollWheelValue;
    }

    protected override void LoadContent()
    {
        basicEffect = new BasicEffect(GraphicsDevice);
        basicEffect.VertexColorEnabled = true;

        spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(GraphicsDevice);

        renderTarget = new RenderTarget2D(GraphicsDevice, Window.ClientBounds.Width, Window.ClientBounds.Height);

        polygonTexture = new Texture2D(GraphicsDevice, Window.ClientBounds.Width, Window.ClientBounds.Height);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
            Exit();

        var mouseState = Mouse.GetState();
        Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);

        int scrollDelta = mouseState.ScrollWheelValue - prevScrollValue;
        prevScrollValue = mouseState.ScrollWheelValue;

        if (scrollDelta != 0)
        {
            double scaleChange = scrollDelta > 0 ? 1.1 : 0.9;
            Vector2 centerToMouse = mousePosition - posOffset;
            scale *= scaleChange;
            posOffset = mousePosition - centerToMouse * (float)scaleChange;
            needsUpdate = true;
        }

        if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && (isDragging || IsMouseInsidePolygon(mousePosition)))
        {
            if (!isDragging)
            {
                isDragging = true;
                prevmousePosition = mousePosition;
            }
            else
            {
                Vector2 delta = mousePosition - prevmousePosition;
                posOffset += delta;
                prevmousePosition = mousePosition;
            }
            needsUpdate = true;
        }
        else
        {
            isDragging = false;
        }

        base.Update(gameTime);
    }

    private bool IsMouseInsidePolygon(Vector2 mousePosition)
    {
        // 마우스가 폴리곤 안에 있는지 확인하는 로직 (점 내적 방법 사용)
        bool ret = false;

        for(int i = 0; i < polygons.Count; i++)
        {
            List<Vector2> tmp = RealToScreen(polygons[i]);
            for(int j=0, k= polygons[i].Count - 1; j < polygons[i].Count; k = j++)
            {
                if (((tmp[j].Y > mousePosition.Y) != (tmp[k].Y > mousePosition.Y)) &&
                (mousePosition.X < (tmp[k].X - tmp[j].X) * (mousePosition.Y - tmp[j].Y) / (tmp[k].Y - tmp[j].Y) + tmp[j].X))
                {
                    ret = !ret;
                }
            }
        }

        return ret;
    }

    private List<Vector2> RealToScreen(List<Vector2> points)
    {
        List<Vector2> ret = new List<Vector2>();

        for (int i = 0; i < points.Count; i++)
        {
            double x = (double)points[i].X * scale + (double)posOffset.X;
            double y = (double)points[i].Y * scale + (double)posOffset.Y;
            ret.Add(new Vector2((float)x, (float)y));
        }

        return ret;
    }

    private List<IntPoint> ConvertToClipperPath(List<Vector2> polygon)
    {
        int precisionFactor = 1000;
        List<IntPoint> path = new List<IntPoint>();
        foreach (var vertex in polygon)
        {
            path.Add(new IntPoint((long)vertex.X * precisionFactor, (long)vertex.Y * precisionFactor));
        }
        return path;
    }

    private List<Vector2> ConvertToVectorPath(List<IntPoint> polygon)
    {
        float precisionFactor = 0.001f;
        List<Vector2> path = new List<Vector2>();
        foreach (var point in polygon)
        {
            path.Add(new Vector2(point.X * precisionFactor, point.Y * precisionFactor));
        }
        return path;
    }

    public List<List<Vector2>> SubtractPolygons(List<Vector2> subject, List<Vector2> clip)
    {
        var clipper = new Clipper();

        var subjectPath = ConvertToClipperPath(subject);
        var clipPath = ConvertToClipperPath(clip);

        clipper.AddPolygon(subjectPath, PolyType.ptSubject);
        clipper.AddPolygon(clipPath, PolyType.ptClip);

        List<List<IntPoint>> solution = new List<List<IntPoint>>();
        clipper.Execute(ClipType.ctDifference, solution, PolyFillType.pftNonZero, PolyFillType.pftNonZero);

        List<List<Vector2>> result = new List<List<Vector2>>();
        foreach (var poly in solution)
        {
            result.Add(ConvertToVectorPath(poly));
        }

        return result;
    }

    protected override void Draw(GameTime gameTime)
    {
        basicEffect.Projection = Matrix.CreateOrthographicOffCenter
        (
            0, GraphicsDevice.Viewport.Width,
            GraphicsDevice.Viewport.Height, 0,
            0, 1
        );

        ///////////////////////////////////////

        Stopwatch stopwatch = new Stopwatch();
        string root = "E:\\Test_Layer_Points\\layer_points";
        Layer_Points layer_points = new Layer_Points();
        layer_points.Read(root);
        polygons.Clear();

        if (needsUpdate)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Black);

            for (int i = 0; i < layer_points.point_index.Count; i++)
            {
                List<Vector2> polygon = new List<Vector2>();
                int start_pos = layer_points.point_index[i].pos_start;
                int count = layer_points.point_index[i].count;

                float[] arr_X = new float[count];
                float[] arr_Y = new float[count];

                Array.Copy(layer_points.point_X, start_pos, arr_X, 0, count);
                Array.Copy(layer_points.point_Y, start_pos, arr_Y, 0, count);

                for (int j = 0; j < count; j++)
                {
                    polygon.Add(new Vector2(arr_X[j], arr_Y[j]));
                }
                polygons.Add(polygon);
            }

            stopwatch.Start();

            spriteBatch.Begin();

            for (int i = 0; i < polygons.Count; i++)
            {
                var emptyPolygon = new PolygonShape(RealToScreen(polygons[i]));

                Color gray = (layer_points.point_index[i].polarity == 'P') ? Color.White : Color.Red;
                emptyPolygon.Draw(GraphicsDevice, basicEffect, gray);
            }

            spriteBatch.End();

            stopwatch.Stop();
            //MessageBox.Show($"Elapsed time: {stopwatch.ElapsedMilliseconds} ms");

            polygonTexture = renderTarget;

            GraphicsDevice.SetRenderTarget(null);

            needsUpdate = false;
        }

        GraphicsDevice.Clear(Color.Black);

        spriteBatch.Begin();
        spriteBatch.Draw(polygonTexture, Vector2.Zero, Color.White);
        spriteBatch.End();

        base.Draw(gameTime);
    }
}
