using System.Collections.Generic;
using OpenGL;
using OpenGL.Game;
using OpenGL.Platform;
using OpenGL.Shapes;
using static OpenGL.GenericVAO;

namespace SAE.OpenGL.P5
{
    public class DynamicShape : GameObject
    {
        private Material _mat;

        private static readonly Vector3[] Colors = new Vector3[]
        {
            new Vector3(1f, 0f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 0f, 1f)
        };

        public DynamicShape(string name, Material mat) : base(name)
        {
            _mat = mat;

            Renderer = new MeshRenderer(mat, GetVao(Shapes.VerticesCube, Shapes.IndicesCube, Shapes.ColorsCube));

            RegisterShapeSwap();
        }

        public void RegisterShapeSwap()
        {
            Input.Subscribe('1', SwapCube);
            Input.Subscribe('2', SwapTriangle);
            Input.Subscribe('3', SwapRect);
            Input.Subscribe('4', SwapPyramid);
            Input.Subscribe('5', SwapRealPyramid);
        }

        private void SwapCube()
        {
            Renderer.Geometry = GetVao(Shapes.VerticesCube, Shapes.IndicesCube, Shapes.ColorsCube);
        }

        private void SwapTriangle()
        {
            Renderer.Geometry = GetVao(Shapes.VerticesTriangle, Shapes.IndicesTriangle, Shapes.ColorsTriangle);
        }

        private void SwapRect()
        {
            Renderer.Geometry = GetVao(Shapes.VerticesRectangle, Shapes.IndicesRectangle, Shapes.ColorsRectangle);
        }

        private void SwapPyramid()
        {
            Renderer.Geometry = GetVao(Shapes.VerticesPyramid, Shapes.IndicesPyramid, Shapes.ColorsPyramid);
        }

        private void SwapRealPyramid()
        {
            Renderer.Geometry = GetVao(Shapes.VerticesRealPyramid, Shapes.IndicesRealPyramid, Shapes.ColorsRealPyramid);
        }

        private VAO GetVao(Vector3[] vertices, uint[] indices, Vector3[] colors)
        {
            List<IGenericVBO> vbos = new List<IGenericVBO>
            {
                new GenericVBO<Vector3>(new VBO<Vector3>(vertices), "in_position"),
                new GenericVBO<Vector3>(new VBO<Vector3>(colors), "in_color"),
                new GenericVBO<uint>(new VBO<uint>(indices,
                    BufferTarget.ElementArrayBuffer,
                    BufferUsageHint.StaticRead))
            };

            return new VAO(_mat, vbos.ToArray());
        }
    }
}