using System;

namespace OpenGL.Game
{
    public class MeshRenderer
    {
        #region Properties

        public Material Material { get; set; }

        public Texture Texture { get; set; }

        public VAO Geometry { get; set; }

        #endregion

        #region Constructor

        public MeshRenderer(Material material, VAO geometry)
        {
            Material = material;
            Geometry = geometry;
        }
        
        public MeshRenderer(Material material, Texture texture, VAO geometry)
        {
            Material = material;
            Texture = texture;
            Geometry = geometry;
        }

        #endregion

        #region Public Methods

        public void Render(Matrix4 mvp)
        {
            Geometry.Program.Use();
            Material["mvp"].SetValue(mvp);
            
            Geometry.Draw();
        }

        #endregion
    }
}