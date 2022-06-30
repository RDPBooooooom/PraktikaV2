namespace OpenGL.Mathematics
{
    public class Transform
    {
        #region Properties

        public Vector3 Position { get; set; } = Vector3.Zero;

        public Vector3 Scale { get; set; } = Vector3.One;
        
        public Vector3 Rotation { get; set; } = Vector3.Zero;

        #endregion

        #region Methods

        private Matrix4 GetRotationMatrix()
        {
            Matrix4 modelRotationX = Matrix4.CreateRotation(new Vector3(1.0f, 0.0f, 0.0f), Mathf.ToRad(Rotation.X));
            Matrix4 modelRotationY = Matrix4.CreateRotation(new Vector3(0.0f, 1.0f, 0.0f), Mathf.ToRad(Rotation.Y));
            Matrix4 modelRotationZ = Matrix4.CreateRotation(new Vector3(0.0f, 0.0f, 1.0f), Mathf.ToRad(Rotation.Z));
            return modelRotationX * modelRotationY * modelRotationZ;
        }

        /// <summary>
        /// Creates the TRS (Translation, Rotation, Scale) Matrix based on <see cref="Position"/>, <see cref="Rotation"/> and <see cref="Scale"/>
        /// </summary>
        /// <returns>Returns TRS Matrix</returns>
        public Matrix4 GetTrs()
        {
            Matrix4 modelTranslation = Matrix4.CreateTranslation(Position);
            Matrix4 modelRotation = GetRotationMatrix();
            Matrix4 modelScale = Matrix4.CreateScaling(Scale);

            return modelTranslation * modelRotation * modelScale;
        }

        public Matrix4 GetRts()
        {
            Matrix4 modelTranslation = Matrix4.CreateTranslation(Position);
            Matrix4 modelRotation = GetRotationMatrix();
            Matrix4 modelScale = Matrix4.CreateScaling(Scale);

            return modelRotation * modelTranslation * modelScale;
        }

        public Vector3 GetForward()
        {
            Vector3 forward = GetRotationMatrix() * new Vector3(0.0f, 0.0f, 1.0f);
            return forward;
        }

        public Vector3 GetRight()
        {
            Vector3 right = GetRotationMatrix() * new Vector3(-1f, 0.0f, 0f);
            return right;
        }

        public Vector3 GetUp()
        {
            Vector3 up = GetRotationMatrix() * new Vector3(0.0f, 1f, 0f);
            return up;
        }

        #endregion
    }
}