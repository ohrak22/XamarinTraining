using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _3D_with_MonoGame
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		Model model;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			graphics.IsFullScreen = true;
			graphics.PreferredBackBufferWidth = 800;
			graphics.PreferredBackBufferHeight = 480;
			graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here
			model = Content.Load<Model>("robot");
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				Exit();

			// TODO: Add your update logic here

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		//protected override void Draw(GameTime gameTime)
		//{
		//	GraphicsDevice.Clear(Color.CornflowerBlue);

		//	// TODO: Add your drawing code here
		//	// A model is composed of "Meshes" which are
		//	// parts of the model which can be positioned
		//	// independently, which can use different textures,
		//	// and which can have different rendering states
		//	// such as lighting applied.
		//	foreach (var mesh in model.Meshes)
		//	{
		//		// "Effect" refers to a shader. Each mesh may
		//		// have multiple shaders applied to it for more
		//		// advanced visuals. 
		//		foreach (BasicEffect effect in mesh.Effects)
		//		{
		//			// We could set up custom lights, but this
		//			// is the quickest way to get somethign on screen:
		//			effect.EnableDefaultLighting();

		//			// This makes lighting look more realistic on
		//			// round surfaces, but at a slight performance cost:
		//			effect.PreferPerPixelLighting = true;

		//			// The world matrix can be used to position, rotate
		//			// or resize (scale) the model. Identity means that
		//			// the model is unrotated, drawn at the origin, and
		//			// its size is unchanged from the loaded content file.
		//			//effect.World = Matrix.Identity;
		//			var modelPosition = new Vector3(0, 0, 3);
		//			effect.World = Matrix.CreateTranslation(modelPosition);

		//			// Move the camera 8 units away from the origin:
		//			//var cameraPosition = new Vector3(0, 8, 0);
		//			var cameraPosition = new Vector3(0, 30, 0);
		//			// Tell the camera to look at the origin:
		//			var cameraLookAtVector = Vector3.Zero;
		//			// Tell the camera that positive Z is up
		//			var cameraUpVector = Vector3.UnitZ;

		//			effect.View = Matrix.CreateLookAt(
		//				cameraPosition, cameraLookAtVector, cameraUpVector);

		//			// We want the aspect ratio of our display to match
		//			// the entire screen's aspect ratio:
		//			float aspectRatio =
		//				graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
		//			// Field of view measures how wide of a view our camera has.
		//			// Increasing this value means it has a wider view, making everything
		//			// on screen smaller. This is conceptually the same as "zooming out".
		//			// It also 
		//			float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
		//			// Anything closer than this will not be drawn (will be clipped)
		//			float nearClipPlane = 1;
		//			// Anything further than this will not be drawn (will be clipped)
		//			float farClipPlane = 200;

		//			effect.Projection = Matrix.CreatePerspectiveFieldOfView(
		//				fieldOfView, aspectRatio, nearClipPlane, farClipPlane);

		//		}

		//		// Now that we've assigned our properties on the effects we can
		//		// draw the entire mesh
		//		mesh.Draw();
		//	}

		//	base.Draw(gameTime);
		//}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);
			DrawModel(new Vector3(-4, 0, 0));
			DrawModel(new Vector3(0, 0, 0));
			DrawModel(new Vector3(4, 0, 0));
			DrawModel(new Vector3(-4, 0, 3));
			DrawModel(new Vector3(0, 0, 3));
			DrawModel(new Vector3(4, 0, 3));
			base.Draw(gameTime);
		}

		void DrawModel(Vector3 modelPosition)
		{
			foreach (var mesh in model.Meshes)
			{
				foreach (BasicEffect effect in mesh.Effects)
				{
					effect.EnableDefaultLighting();
					effect.PreferPerPixelLighting = true;
					effect.World = Matrix.CreateTranslation(modelPosition);
					var cameraPosition = new Vector3(0, 10, 0);
					var cameraLookAtVector = Vector3.Zero;
					var cameraUpVector = Vector3.UnitZ;
					effect.View = Matrix.CreateLookAt(
						cameraPosition, cameraLookAtVector, cameraUpVector);
					float aspectRatio =
						graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
					float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
					float nearClipPlane = 1;
					float farClipPlane = 200;
					effect.Projection = Matrix.CreatePerspectiveFieldOfView(
						fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
				}
				// Now that we've assigned our properties on the effects we can
				// draw the entire mesh
				mesh.Draw();
			}
		}
	}
}
