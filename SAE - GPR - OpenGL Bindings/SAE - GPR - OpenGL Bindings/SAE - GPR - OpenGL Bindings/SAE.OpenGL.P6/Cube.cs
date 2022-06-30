using System;
using OpenGL;
using OpenGL.Game;
using OpenGL.Platform;
using OpenGL.Shapes;

namespace SAE.OpenGL.P6
{
    public class Cube : GameObject
    {
        private Material _mat;

        private int _uvFactor;
        private float _lastTime;

        public Cube(string name, Material mat) : base(name)
        {
            _uvFactor = 1;

            _mat = mat;

            Renderer = new MeshRenderer(_mat,
                GetVao(Shapes.VerticesTextureCube, Shapes.IndicesTextureCube, Shapes.ColorsTextureCube,
                    Shapes.UvTextureCube, _mat));
        }
        
        public Cube(string name, Material mat, Texture texture) : base(name)
        {
            _uvFactor = 1;

            _mat = mat;
            _mat["uv_factor"].SetValue(_uvFactor);

            Renderer = new MeshRenderer(_mat, texture,
                GetVao(Shapes.VerticesTextureCube, Shapes.IndicesTextureCube, Shapes.ColorsTextureCube,
                    Shapes.UvTextureCube, _mat));

            Input.Subscribe('m', UvSizeUp);
            Input.Subscribe('n', UvSizeDown);
        }

        private void UvSizeUp()
        {
            // Make sure it doesnt happen too often
            if (0.5 > Time.TimeSinceStart - _lastTime) return;
            _lastTime = Time.TimeSinceStart;

            _mat.Use();
            _uvFactor++;
            _mat["uv_factor"].SetValue(_uvFactor);
        }

        private void UvSizeDown()
        {
            // Make sure it doesnt happen too often
            if (0.5 > Time.TimeSinceStart - _lastTime) return;
            _lastTime = Time.TimeSinceStart;

            _mat.Use();
            _uvFactor--;
            _mat["uv_factor"].SetValue(_uvFactor);
        }
    }
}