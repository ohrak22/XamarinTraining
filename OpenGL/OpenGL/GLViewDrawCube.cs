using System;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES11;
using OpenTK.Platform;
using OpenTK.Platform.Android;

using Android.Views;
using Android.Content;
using Android.Util;

namespace OpenGL
{
	class GLViewDrawCube : AndroidGameView
	{
		public GLViewDrawCube(Context context) : base(context)
		{
			// do not set context on render frame as we will be rendering
			// on separate thread and thus Android will not set GL context
			// behind our back
			AutoSetContextOnRenderFrame = false;

			// render on separate thread. this gains us
			// fluent rendering. be careful to not use GL calls on UI thread.
			// OnRenderFrame is called from rendering thread, so do all
			// the GL calls there
			RenderOnUIThread = false;
		}

		// This gets called when the drawing surface is ready
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			// Run the render loop
			Run();
		}

		// This method is called everytime the context needs
		// to be recreated. Use it to set any egl-specific settings
		// prior to context creation
		//
		// In this particular case, we demonstrate how to set
		// the graphics mode and fallback in case the device doesn't
		// support the defaults
		protected override void CreateFrameBuffer()
		{
			// the default GraphicsMode that is set consists of (16, 16, 0, 0, 2, false)
			try
			{
				Log.Verbose("GLCube", "Loading with default settings");

				// if you don't call this, the context won't be created
				base.CreateFrameBuffer();
				return;
			}
			catch (Exception ex)
			{
				Log.Verbose("GLCube", $"{ex}");
			}

			// this is a graphics setting that sets everything to the lowest mode possible so
			// the device returns a reliable graphics setting.
			try
			{
				Log.Verbose("GLCube", "Loading with custom Android settings (low mode)");
				GraphicsMode = new AndroidGraphicsMode(0, 0, 0, 0, 0, false);

				// if you don't call this, the context won't be created
				base.CreateFrameBuffer();
				return;
			}
			catch (Exception ex)
			{
				Log.Verbose("GLCube", $"{ex}");
			}
			throw new Exception("Can't load egl, aborting");
		}

		// This gets called on each frame render
		protected override void OnRenderFrame(FrameEventArgs e)
		{
			// you only need to call this if you have delegates
			// registered that you want to have called
			base.OnRenderFrame(e);

			GL.MatrixMode(All.Projection);
			GL.LoadIdentity();

			float aspect = (float)Width / (float)Height;
			GL.Ortho(-aspect, aspect, -1.0f, 1.0f, -1.0f, 1.0f);

			GL.MatrixMode(All.Modelview);
			GL.Rotate(3.0f, 3.0f, 0.0f, 1.0f);

			GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
			GL.ClearDepth(1.0f);
			GL.Enable(All.DepthTest);
			GL.DepthFunc(All.CullFace);
			GL.CullFace(All.Back);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.EnableClientState(All.VertexArray);
			GL.EnableClientState(All.ColorArray);
			
			GL.VertexPointer(3, All.Float, 0, cube_vertices);
			GL.ColorPointer(4, All.Float, 0, cube_colors);
			GL.DrawArrays(All.Triangles, 0, 12*3);
			GL.Finish();

			GL.DisableClientState(All.VertexArray);
			GL.DisableClientState(All.ColorArray);

			SwapBuffers();
		}

		float[] cube_vertices = {
			-0.5f,-0.5f,-0.5f, // triangle 1 : begin
			-0.5f,-0.5f, 0.5f,
			-0.5f, 0.5f, 0.5f, // triangle 1 : end
			0.5f, 0.5f,-0.5f, // triangle 2 : begin
			-0.5f,-0.5f,-0.5f,
			-0.5f, 0.5f,-0.5f, // triangle 2 : end
			0.5f,-0.5f, 0.5f,
			-0.5f,-0.5f,-0.5f,
			0.5f,-0.5f,-0.5f,
			0.5f, 0.5f,-0.5f,
			0.5f,-0.5f,-0.5f,
			-0.5f,-0.5f,-0.5f,
			-0.5f,-0.5f,-0.5f,
			-0.5f, 0.5f, 0.5f,
			-0.5f, 0.5f,-0.5f,
			0.5f,-0.5f, 0.5f,
			-0.5f,-0.5f, 0.5f,
			-0.5f,-0.5f,-0.5f,
			-0.5f, 0.5f, 0.5f,
			-0.5f,-0.5f, 0.5f,
			0.5f,-0.5f, 0.5f,
			0.5f, 0.5f, 0.5f,
			0.5f,-0.5f,-0.5f,
			0.5f, 0.5f,-0.5f,
			0.5f,-0.5f,-0.5f,
			0.5f, 0.5f, 0.5f,
			0.5f,-0.5f, 0.5f,
			0.5f, 0.5f, 0.5f,
			0.5f, 0.5f,-0.5f,
			-0.5f, 0.5f,-0.5f,
			0.5f, 0.5f, 0.5f,
			-0.5f, 0.5f,-0.5f,
			-0.5f, 0.5f, 0.5f,
			0.5f, 0.5f, 0.5f,
			-0.5f, 0.5f, 0.5f,
			0.5f,-0.5f, 0.5f
		};

		float[] cube_colors = {
			0.583f,  0.771f,  0.014f, 1.0f,
			0.609f,  0.115f,  0.436f, 1.0f,
			0.327f,  0.483f,  0.844f, 1.0f,
			0.822f,  0.569f,  0.201f, 1.0f,
			0.435f,  0.602f,  0.223f, 1.0f,
			0.310f,  0.747f,  0.185f, 1.0f,
			0.597f,  0.770f,  0.761f, 1.0f,
			0.559f,  0.436f,  0.730f, 1.0f,
			0.359f,  0.583f,  0.152f, 1.0f,
			0.483f,  0.596f,  0.789f, 1.0f,
			0.559f,  0.861f,  0.639f, 1.0f,
			0.195f,  0.548f,  0.859f, 1.0f,
			0.014f,  0.184f,  0.576f, 1.0f,
			0.771f,  0.328f,  0.970f, 1.0f,
			0.406f,  0.615f,  0.116f, 1.0f,
			0.676f,  0.977f,  0.133f, 1.0f,
			0.971f,  0.572f,  0.833f, 1.0f,
			0.140f,  0.616f,  0.489f, 1.0f,
			0.997f,  0.513f,  0.064f, 1.0f,
			0.945f,  0.719f,  0.592f, 1.0f,
			0.543f,  0.021f,  0.978f, 1.0f,
			0.279f,  0.317f,  0.505f, 1.0f,
			0.167f,  0.620f,  0.077f, 1.0f,
			0.347f,  0.857f,  0.137f, 1.0f,
			0.055f,  0.953f,  0.042f, 1.0f,
			0.714f,  0.505f,  0.345f, 1.0f,
			0.783f,  0.290f,  0.734f, 1.0f,
			0.722f,  0.645f,  0.174f, 1.0f,
			0.302f,  0.455f,  0.848f, 1.0f,
			0.225f,  0.587f,  0.040f, 1.0f,
			0.517f,  0.713f,  0.338f, 1.0f,
			0.053f,  0.959f,  0.120f, 1.0f,
			0.393f,  0.621f,  0.362f, 1.0f,
			0.673f,  0.211f,  0.457f, 1.0f,
			0.820f,  0.883f,  0.371f, 1.0f,
			0.982f,  0.099f,  0.879f, 1.0f
		};
	}
}