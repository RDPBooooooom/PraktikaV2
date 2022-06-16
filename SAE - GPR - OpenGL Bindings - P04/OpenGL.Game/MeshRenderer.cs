namespace OpenGL.Game
{
    public class MeshRenderer
    {
        #region Properties

        public Material Material { get; set; }
        public VAO Geometry { get; set; }

        #endregion

        #region Constructor

        public MeshRenderer(Material material, VAO geometry)
        {
            Material = material;
            Geometry = geometry;
        }

        #endregion

        #region Public Methods

        public void Render()
        {
            Geometry.Program.Use();
            Geometry.Draw();
        }

        #endregion
    }
}