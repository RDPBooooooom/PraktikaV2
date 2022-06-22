using OpenGL;
using OpenGL.Game;
using OpenGL.Platform;
using OpenGL.Shapes;

namespace SAE.OpenGL.P4
{
    public class DynamicShape : GameObject
    {

        private Material _mat;

        public DynamicShape(string name, Material mat) : base(name)
        {
            _mat = mat;

            VAO cube = new VAO(mat, new VBO<Vector3>(Shapes.VerticesCube),
                new VBO<uint>(Shapes.IndicesCube, BufferTarget.ElementArrayBuffer, BufferUsageHint.StaticRead));

            Renderer = new MeshRenderer(mat, cube);

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
            Renderer.Geometry = new VAO(_mat, new VBO<Vector3>(Shapes.VerticesCube),
                new VBO<uint>(Shapes.IndicesCube, BufferTarget.ElementArrayBuffer, BufferUsageHint.StaticRead));
        }
        private void SwapTriangle()
        {
            Renderer.Geometry = new VAO(_mat, new VBO<Vector3>(Shapes.VerticesTriangle),
                new VBO<uint>(Shapes.IndicesTriangle, BufferTarget.ElementArrayBuffer, BufferUsageHint.StaticRead));
        }

        private void SwapRect()
        {
            Renderer.Geometry = new VAO(_mat, new VBO<Vector3>(Shapes.VerticesRectangle),
                new VBO<uint>(Shapes.IndicesRectangle, BufferTarget.ElementArrayBuffer, BufferUsageHint.StaticRead));
        }
        
        private void SwapPyramid()
        {
            Renderer.Geometry = new VAO(_mat, new VBO<Vector3>(Shapes.VerticesPyramid),
                new VBO<uint>(Shapes.IndicesPyramid, BufferTarget.ElementArrayBuffer, BufferUsageHint.StaticRead));
        }
        
        private void SwapRealPyramid()
        {
            Renderer.Geometry = new VAO(_mat, new VBO<Vector3>(Shapes.VerticesRealPyramid),
                new VBO<uint>(Shapes.IndicesRealPyramid, BufferTarget.ElementArrayBuffer, BufferUsageHint.StaticRead));
        }
    }
}