using System;
using System.Windows.Forms;
using OpenGL;
using OpenGL.Game;
using OpenGL.Game.Math;
using OpenGL.Game.Shapes;
using OpenGL.Platform;

namespace SAE.OpenGL.P9
{
    internal static class Program
    {
        private static int width = 800;
        private static int height = 600;

        //TODO: Create game instance
        private static Game game;
        private static Camera camera;

        private static bool useAddition = true;
        private static bool disableWiggle = true;

        private static bool useBrightness = false;
        private static bool useContrast = false;
        private static bool useGrayscale = false;

        private static float amp = 1.0f;
        private static float frequency = 1.0f;

        private static float hue = 0f;
        private static float sat = 0f;
        private static float value = 0f;

        static void Main()
        {
            game = new Game();
            camera = new Camera();

            Time.Init();
            Window.CreateWindow("OpenGL P9", 800, 600);

            // add a reshape callback to update the UI
            Window.OnReshapeCallbacks.Add(OnResize);

            // add a close callback to make sure we dispose of everything properly
            Window.OnCloseCallbacks.Add(OnClose);

            // Enable depth testing to ensure correct z-ordering of our fragments
            Gl.Enable(EnableCap.DepthTest);
            Gl.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            //Load texture files
            Gl.ActiveTexture(1);
            Texture crateTexture = new Texture("..\\textures\\crate.jpg");
            Gl.BindTexture(crateTexture);

            // Scale up (magnify)
            Gl.TexParameteri(crateTexture.TextureTarget, TextureParameterName.TextureMagFilter,
                TextureParameter.Linear);
            // Scale down (minify)
            Gl.TexParameteri(crateTexture.TextureTarget, TextureParameterName.TextureMinFilter,
                TextureParameter.Linear);

            // Load shader files
            ShaderProgram material =
                new ShaderProgram(ShaderUtil.CreateShader("..\\shaders\\vert_old.vs", ShaderType.VertexShader),
                    ShaderUtil.CreateShader("..\\shaders\\frag_old.fs", ShaderType.FragmentShader));
            material["color"].SetValue(new Vector3(1, 1, 1));

            ShaderProgram textureMaterial = new ShaderProgram(
                ShaderUtil.CreateShader("..\\shaders\\vert.vs", ShaderType.VertexShader),
                ShaderUtil.CreateShader("..\\shaders\\frag.fs", ShaderType.FragmentShader));
            textureMaterial["color"]?.SetValue(new Vector3(1, 1, 1));
            textureMaterial["baseColorMap"]?.SetValue(1);


            SwapPolygonModeFill();

            //Create game object

            DynamicShape obj = new DynamicShape("DynShape", material)
            {
                Transform = new Transform()
                {
                    Position = new Vector3(5, 0, -10f)
                }
            };

            Cube cube = new Cube("myCube", textureMaterial, crateTexture)
            {
                Transform = new Transform()
                {
                    Position = new Vector3(0, 0, -10f),
                    Rotation = new Vector3(0, 0, 45)
                }
            };

            Cube cube2 = new Cube("myCube2", textureMaterial, crateTexture)
            {
                Transform = new Transform()
                {
                    Position = new Vector3(-5, 0, -10f),
                    Scale = new Vector3(2, 2, 2),
                    Rotation = new Vector3(30, 30, 30)
                }
            };

            MeshRenderer renderer = new MeshRenderer(textureMaterial, crateTexture,
                GameObject.GetVao(Shapes.VerticesTextureRectangle, Shapes.IndicesTextureRectangle,
                    Shapes.ColorsTextureRectangle, Shapes.UvTextureRectangle, textureMaterial));

            GameObject plane = new GameObject("MyPlane", renderer)
            {
                Transform = new Transform()
                {
                    Position = new Vector3(0, 0, -3.2f)
                }
            };

            //Add to scene
            game.SceneGraph.Add(cube);
            game.SceneGraph.Add(cube2);
            game.SceneGraph.Add(obj);
            game.SceneGraph.Add(plane);

            // Hook to the escape press event using the OpenGL.UI class library
            Input.Subscribe((char) Keys.Escape, Window.OnClose);

            Input.Subscribe('f', SwapPolygonModeFill);
            Input.Subscribe('l', SwapPolygonModeLine);
            Input.Subscribe('k', SwapTextParamLinear);
            Input.Subscribe('g', SwapTextParamNearest);
            Input.Subscribe('b', SwapAddition);
            Input.Subscribe('v', SwapWiggle);

            Input.Subscribe('.', IncreaseFreq);
            Input.Subscribe('-', DecreaseFreq);
            Input.Subscribe('z', IncreaseAmp);
            Input.Subscribe('u', DecreaseAmp);

            Input.Subscribe('Q', SwapBrightness);
            Input.Subscribe('W', SwapContrast);
            Input.Subscribe('E', SwapGrayscale);

            Input.Subscribe('A', IncreaseHue);
            Input.Subscribe('Y', DecreaseHue);
            Input.Subscribe('S', IncreaseSat);
            Input.Subscribe('X', DecreaseSat);
            Input.Subscribe('D', IncreaseValue);
            Input.Subscribe('C', DecreaseValue);

            // Make sure to set up mouse event handlers for the window
            Window.OnMouseCallbacks.Add(global::OpenGL.UI.UserInterface.OnMouseClick);
            Window.OnMouseMoveCallbacks.Add(global::OpenGL.UI.UserInterface.OnMouseMove);


            float time = 0;

            // Game loop
            while (Window.Open)
            {
                Window.HandleEvents();

                OnPreRenderFrame();

                Input.Update();

                game.Update();

                Matrix4 view = camera.Transform.GetRts();
                Matrix4 projection = GetProjectionMatrix();

                textureMaterial["time"]?.SetValue(time);
                textureMaterial["useAddition"]?.SetValue(useAddition);
                textureMaterial["disableWiggle"]?.SetValue(disableWiggle);
                textureMaterial["a"]?.SetValue(amp);
                textureMaterial["f"]?.SetValue(frequency);

                float r = Mathf.Sin(time);
                textureMaterial["brightness"]?.SetValue(useBrightness ? r : 0);
                textureMaterial["contrast"]?.SetValue(useContrast ? r : 0);
                textureMaterial["grayscale"]?.SetValue(useGrayscale ? Math.Abs(r) : 0);

                textureMaterial["hue"]?.SetValue(hue);
                textureMaterial["sat"]?.SetValue(sat);
                textureMaterial["value"]?.SetValue(value);


                game.SceneGraph.ForEach(g => Render(g, view, projection));

                OnPostRenderFrame();

                time += Time.DeltaTime;
                Time.Update();
            }
        }


        #region Transformation

        private static void Render(GameObject obj, Matrix4 view, Matrix4 projection)
        {
            //--------------------------
            // Data passing to shader
            //--------------------------
            obj.Render(view, projection);
        }

        private static Matrix4 GetProjectionMatrix()
        {
            float fov = 60f;

            float aspectRatio = width / (float) height;
            Matrix4 projection =
                Matrix4.CreatePerspectiveFieldOfView(0.45f, aspectRatio, 0.1f, 1000f);

            return projection;
        }

        #endregion

        #region Callbacks

        private static void IncreaseFreq()
        {
            frequency += 0.5f;
        }

        private static void DecreaseFreq()
        {
            frequency -= 0.5f;
        }

        private static void IncreaseAmp()
        {
            amp += 0.5f;
        }

        private static void DecreaseAmp()
        {
            amp -= 0.5f;
        }

        private static void IncreaseHue()
        {
            hue += 1f;
        }

        private static void DecreaseHue()
        {
            hue -= 1f;
        }

        private static void IncreaseSat()
        {
            sat = Math.Clamp(sat + 0.05f, -1f, 1f);
        }

        private static void DecreaseSat()
        {
            sat = Math.Clamp(sat - 0.05f, -1f, 1f);
        }

        private static void IncreaseValue()
        {
            value = Math.Clamp(value + 0.05f, -1f, 1f);
        }

        private static void DecreaseValue()
        {
            value = Math.Clamp(value - 0.05f, -1f, 1f);
        }

        private static void SwapWiggle()
        {
            disableWiggle = !disableWiggle;
        }

        private static void SwapAddition()
        {
            useAddition = !useAddition;
        }

        private static void SwapBrightness()
        {
            useBrightness = !useBrightness;
        }

        private static void SwapContrast()
        {
            useContrast = !useContrast;
        }

        private static void SwapGrayscale()
        {
            useGrayscale = !useGrayscale;
        }

        private static void SwapTextParamLinear()
        {
            // Scale up (magnify)
            Gl.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, TextureParameter.Linear);
            // Scale down (minify)
            Gl.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, TextureParameter.Linear);
        }

        private static void SwapTextParamNearest()
        {
            // Scale up (magnify)
            Gl.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, TextureParameter.Nearest);
            // Scale down (minify)
            Gl.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, TextureParameter.Nearest);
        }

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