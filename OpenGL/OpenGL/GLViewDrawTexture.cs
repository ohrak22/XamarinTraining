using System;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES11;
using OpenTK.Platform;
using OpenTK.Platform.Android;

using Android.Views;
using Android.Content;
using Android.Util;
using Android.Graphics;

namespace OpenGL
{
	class GLViewDrawTexture : AndroidGameView
	{
		int textureId;
		Context context;

		public GLViewDrawTexture(Context context) : base(context)
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

			this.context = context;
		}

		// This gets called when the drawing surface is ready
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			GL.Enable(All.Texture2D);
			GL.GenTextures(1, out textureId);
			GL.ShadeModel(All.Smooth);


			GL.BindTexture(All.Texture2D, textureId);

			// setup texture parameters
			GL.TexParameterx(All.Texture2D, All.TextureMagFilter, (int)All.Linear);
			GL.TexParameterx(All.Texture2D, All.TextureMinFilter, (int)All.Linear);
			GL.TexParameterx(All.Texture2D, All.TextureWrapS, (int)All.ClampToEdge);
			GL.TexParameterx(All.Texture2D, All.TextureWrapT, (int)All.ClampToEdge);

			BitmapFactory.Options options = new BitmapFactory.Options();
			options.InScaled = false;

			Bitmap b = BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.pattern, options);

			Android.Opengl.GLUtils.TexImage2D((int)All.Texture2D, 0, b, 0);

			b.Recycle();

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
			GL.Rotate(3.0f, 0.0f, 0.0f, 1.0f);

			GL.BindTexture(All.Texture2D, textureId);
			GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.FrontFace(All.Cw);

			GL.VertexPointer(2, All.Float, 0, square_vertices);
			GL.EnableClientState(All.VertexArray);
			GL.TexCoordPointer(2, All.Float, 0, square_texture_coords);
			GL.EnableClientState(All.TextureCoordArray);

			GL.DrawArrays(All.TriangleStrip, 0, 4);
			
			SwapBuffers();
		}

		float[] square_vertices = {
			-0.5f, -0.5f,
			0.5f, -0.5f,
			-0.5f, 0.5f,
			0.5f, 0.5f,
		};

		float[] square_texture_coords = {
			0f,1f,
			0f,0f,
			1f,1f,
			1f,0f,
		};
	}
}