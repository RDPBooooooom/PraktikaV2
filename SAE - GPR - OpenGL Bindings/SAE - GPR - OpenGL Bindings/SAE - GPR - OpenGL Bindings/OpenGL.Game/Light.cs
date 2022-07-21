namespace OpenGL.Game
{
    public class Light
    {
        public float AmbientIntensity { get; set; }
        public float DiffuseIntensity { get; set; }
        public float SpecularIntensity { get; set; }
        public float Hardness { get; set; }

        public Transform Position { get; set; }
        public Vector3 AmbientColor { get; set; }
        public Vector3 Color { get; set; }

        public Light(float ambientIntensity, float diffuseIntensity, float specularIntensity, float hardness, Vector3 ambientColor, Vector3 color)
        {
            AmbientIntensity = ambientIntensity;
            DiffuseIntensity = diffuseIntensity;
            SpecularIntensity = specularIntensity;
            Hardness = hardness;
            AmbientColor = ambientColor;
            Color = color;
            Position = new Transform();
        }

        public Matrix4 GetLightData(Camera cam)
        {
            return new Matrix4(
                new Vector4(Position.Position, AmbientIntensity),
                new Vector4(AmbientColor, DiffuseIntensity),
                new Vector4(Color, SpecularIntensity),
                new Vector4(cam.Transform.Position, Hardness)
            );
        }
    }
}