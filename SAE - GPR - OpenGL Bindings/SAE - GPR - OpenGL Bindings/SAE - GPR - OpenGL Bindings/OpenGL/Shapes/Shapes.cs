namespace OpenGL.Shapes
{
    /// <summary>
    /// Holds static Vertices & Indices for some shapes
    /// </summary>
    public static class Shapes
    {
        #region Pyramid

        public static readonly Vector3[] VerticesPyramid =
        {
            new Vector3(-0.1f, -0.5f, 0.0f), // front_corner
            new Vector3(0.5f, 0.0f, 1.0f), // right_corner
            new Vector3(-0.5f, 0.0f, 1.0f), // left_corner
            new Vector3(0.0f, 0.5f, 0.5f) // top
        };

        public static readonly uint[] IndicesPyramid =
        {
            0, 1, 2,
            //Sides
            0, 2, 3, // left
            1, 3, 2, // back
            0, 3, 1 // right
        };

        public static Vector3[] ColorsPyramid =
        {
            new Vector3(1f, 1f, 1f),
            new Vector3(1f, 0f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 0f, 1f)
        };

        #endregion

        #region Real pyramid

        public static readonly Vector3[] VerticesRealPyramid =
        {
            new Vector3(-0.5f, -0.5f, 1.0f), // back_left
            new Vector3(-0.5f, -0.5f, 0.0f), // front_left
            new Vector3(0.5f, -0.5f, 0.0f), // front_right
            new Vector3(0.5f, -0.5f, 1.0f), // back_right
            new Vector3(0.0f, 0.5f, 0.5f) // middle_top
        };

        public static readonly uint[] IndicesRealPyramid =
        {
            // BOT
            0, 3, 1,
            1, 3, 2,
            // Front
            1, 4, 2,
            // LEFT
            0, 1, 4,
            // Back
            0, 3, 4,
            // Right
            3, 4, 2
        };
        
        public static Vector3[] ColorsRealPyramid =
        {
            new Vector3(1f, 0f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 0f, 1f),
            new Vector3(1f, 0f, 0f),
            new Vector3(1f, 1f, 1f)
        };

        #endregion

        #region Cube

        public static readonly Vector3[] VerticesCube =
        {
            new Vector3(-0.5f, 0.5f, 0.0f), // front_top_Left
            new Vector3(0.5f, 0.5f, 0.0f), //front_top_right
            new Vector3(-0.5f, -0.5f, 0.0f), // front_bot_left
            new Vector3(0.5f, -0.5f, 0.0f), // front_bot_right
            new Vector3(-0.5f, 0.5f, 1.0f), // back_top_left
            new Vector3(0.5f, 0.5f, 1.0f), // back_top_right
            new Vector3(-0.5f, -0.5f, 1.0f), // back_bot_left
            new Vector3(0.5f, -0.5f, 1.0f), // back_bot_right
        };

        public static readonly uint[] IndicesCube =
        {
            // TOP
            0, 4, 1,
            1, 4, 5,
            // Front
            2, 0, 3,
            3, 0, 1,
            // BOT
            6, 2, 7,
            7, 2, 3,
            // Right
            7, 3, 5,
            5, 3, 1,
            // LEFT
            4, 0, 6,
            6, 0, 2,
            // BACK
            4, 6, 5,
            5, 6, 7
        };
        
        public static Vector3[] ColorsCube =
        {
            //One Side
            new Vector3(1f, 0f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 0f, 1f),
            new Vector3(1f, 1f, 1f),
            //One Side
            new Vector3(1f, 0f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 0f, 1f),
            new Vector3(1f, 1f, 1f),
            //One Side
            new Vector3(1f, 0f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 0f, 1f),
            new Vector3(1f, 1f, 1f),
            //One Side
            new Vector3(1f, 0f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 0f, 1f),
            new Vector3(1f, 1f, 1f),
            //One Side
            new Vector3(1f, 0f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 0f, 1f),
            new Vector3(1f, 1f, 1f),
            //One Side
            new Vector3(1f, 0f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 0f, 1f),
            new Vector3(1f, 1f, 1f),
        };

        #endregion

        #region Rectangle

        public static readonly Vector3[] VerticesRectangle =
        {
            new Vector3(-0.5f, 0.5f, 1.0f), // left_top
            new Vector3(-0.5f, -0.5f, 1.0f), // left_down
            new Vector3(0.5f, -0.5f, 1.0f), // right_down
            new Vector3(0.5f, 0.5f, 1.0f), // right_top
        };

        public static readonly uint[] IndicesRectangle =
        {
            0, 3, 1,
            1, 3, 2
        };

        
        public static Vector3[] ColorsRectangle =
        {
            //One Side
            new Vector3(1f, 0f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 0f, 1f),
            new Vector3(1f, 1f, 1f),
        };

        
        #endregion

        #region Triangle

        public static readonly Vector3[] VerticesTriangle =
        {
            new Vector3(-0.5f, 0.5f, 0.0f), // left_top
            new Vector3(-0.5f, -0.5f, 0.0f), // left_down
            new Vector3(0.5f, 0.5f, 0.0f) // right_top
        };

        public static readonly uint[] IndicesTriangle =
        {
            0, 2, 1
        };

        public static Vector3[] ColorsTriangle =
        {
            new Vector3(1f, 0f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 0f, 1f)
        };
        
        #endregion
    }
}