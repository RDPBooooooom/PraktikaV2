using System;
using System.Diagnostics;
using OpenGL.Mathematics;

namespace OpenGL.Game
{
    public class GameObject
    {
        #region Properties

        public string Name { get; set; }
        public MeshRenderer Renderer { get; set; }
        public Transform Transform { get; set; }

        #endregion

        #region Constructor

        public GameObject(string name, MeshRenderer renderer)
        {
            Name = name;
            Renderer = renderer;

            Initialize();
        }

        protected GameObject(string name)
        {
            Name = name;
            Initialize();
        }

        #endregion

        #region Public Methods

        public void Initialize()
        {
            Transform = new Transform();
        }

        public virtual void Update()
        {
        }

        #endregion
    }
}