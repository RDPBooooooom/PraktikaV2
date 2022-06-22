using System.Windows.Forms;
using OpenGL;
using OpenGL.Game;
using OpenGL.Mathematics;
using OpenGL.Platform;

namespace SAE.OpenGL.P5
{
    internal static class Program
    {
        private static int width = 800;
        private static int height = 600;

        //TODO: Create game instance
        private static Game game;
        private static Camera camera;
        
        static void Main()
        {
            game = new Game();
            camera = new Camera();

            Time.Initialize();
            Window.CreateWindow("OpenGL P5", 800, 600);

            // add a reshape callback to update the UI
            Window.OnReshapeCallbacks.Add(OnResize);

            // add a close callback to make sure we dispose of everything properly
            Window.OnCloseCallbacks.Add(OnClose);

            // Enable depth testing to ensure correct z-ordering of our fragments
            Gl.Enable(EnableCap.DepthTest);
            Gl.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            // Load shader files
            Material material = Material.Create("shaders\\vert.vs", "shaders\\frag.fs");
            material["color"].SetValue(new Vector3(1, 1, 1));

            SwapPolygonModeFill();

            //Create VAO Data
            //Create game object

            DynamicShape obj = new DynamicShape("Dyn Shape", material)
            {
                Transform =
                {
                    Position = new Vector3(0, 0, -10)
                }
            };

            //Add to scene
            game.SceneGraph.Add(obj);

            // Hook to the escape press event using the OpenGL.UI class library
            Input.Subscribe((char) Keys.Escape, Window.OnClose);
            
            Input.Subscribe('f', SwapPolygonModeFill);
            Input.Subscribe('l', SwapPolygonModeLine);

            // Make sure to set up mouse event handlers for the window
            Window.OnMouseCallbacks.Add(global::OpenGL.UI.UserInterface.OnMouseClick);
            Window.OnMouseMoveCallbacks.Add(global::OpenGL.UI.UserInterface.OnMouseMove);

            // Game loop
            while (Window.Open)
            {
                Window.HandleInput();

                OnPreRenderFrame();

                Input.Update();

                game.Update();

                game.SceneGraph.ForEach(SetTransform);
                game.Render();

                OnPostRenderFrame();

                Time.Update();
            }
        }


        #region Transformation

        private static void SetTransform(GameObject obj)
        {
            Matrix4 view = camera.GetRts(); 
            Matrix4 projection = GetProjectionMatrix();

            //--------------------------
            // Data passing to shader
            //--------------------------
            Material material = obj.Renderer.Material;

            material["projection"].SetValue(projection);
            material["view"].SetValue(view);
            material["model"].SetValue(obj.Transform.GetTrs());
        }

        private static Matrix4 GetProjectionMatrix()
        {
            float fov = 45;

            float aspectRatio = width / (float) height;
            Matrix4 projection =
                Matrix4.CreatePerspectiveFieldOfView(Mathf.ToRad(fov), aspectRatio, 0.1f, 1000f);
            //projection = Matrix4.CreateOrthographic0.0f, (float)screenWidth, 0.0f, (float)screenHeight, 0.1f, 100.0f);

            return projection;
        }

        #endregion

        #region Callbacks

        private static void SwapPolygonModeLine()
        {
            Gl.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
        }
        
        private static void SwapPolygonModeFill()
        {
            Gl.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
        }

        private static void OnResize()
        {
            width = Window.Width;
            height = Window.Height;

            global::OpenGL.UI.UserInterface.OnResize(Window.Width, Window.Height);
        }

        private static void OnClose()
        {
            // make sure to dispose of everything
            global::OpenGL.UI.UserInterface.Dispose();
            global::OpenGL.UI.BMFont.Dispose();
        }

        private static void OnPreRenderFrame()
        {
            // set up the OpenGL viewport and clear both the color and depth bits
            Gl.Viewport(0, 0, Window.Width, Window.Height);
            Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        private static void OnPostRenderFrame()
        {
            // Draw the user interface after everything else
            global::OpenGL.UI.UserInterface.Draw();

            // Swap the back buffer to the front so that the screen displays
            Window.SwapBuffers();
        }

        private static void OnMouseClick(int button, int state, int x, int y)
        {
            // take care of mapping the Glut buttons to the UI enums
            if (!global::OpenGL.UI.UserInterface.OnMouseClick(button + 1, (state == 0 ? 1 : 0), x, y))
            {
                // do other picking code here if necessary
            }
        }

        private static void OnMouseMove(int x, int y)
        {
            if (!global::OpenGL.UI.UserInterface.OnMouseMove(x, y))
            {
                // do other picking code here if necessary
            }
        }

        #endregion
    }
}