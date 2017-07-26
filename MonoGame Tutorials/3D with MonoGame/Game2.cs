using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _3D_with_MonoGame
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game2 : Game
	{
		GraphicsDeviceManager graphics;

		Model model;
		VertexPositionTexture[] floorVerts;
		BasicEffect effect;
		Texture2D checkerboardTexture;
		//Vector3 cameraPosition = new Vector3(0, 10, 10);
		Vector3 cameraPosition = new Vector3(15, 10, 10);

		public Game2()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			graphics.IsFullScreen = true;
			graphics.PreferredBackBufferWidth = 800;
			graphics.PreferredBackBufferHeight = 480;
			graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
		}

		protected override void Initialize()
		{
			floorVerts = new VertexPositionTexture[6];
			floorVerts[0].Position = new Vector3(-20, -20, 0);
			floorVerts[1].Position = new Vector3(-20, 20, 0);
			floorVerts[2].Position = new Vector3(20, -20, 0);
			floorVerts[3].Position = floorVerts[1].Position;
			floorVerts[4].Position = new Vector3(20, 20, 0);
			floorVerts[5].Position = floorVerts[2].Position;

			int repetitions = 20;

			floorVerts[0].TextureCoordinate = new Vector2(0, 0);
			floorVerts[1].TextureCoordinate = new Vector2(0, repetitions);
			floorVerts[2].TextureCoordinate = new Vector2(repetitions, 0);

			floorVerts[3].TextureCoordinate = floorVerts[1].TextureCoordinate;
			floorVerts[4].TextureCoordinate = new Vector2(repetitions, repetitions);
			floorVerts[5].TextureCoordinate = floorVerts[2].TextureCoordinate;


			effect = new BasicEffect(graphics.GraphicsDevice);

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Notice that loading a model is very similar
			// to loading any other XNB (like a Texture2D).
			// The only difference is the generic type.
			model = Content.Load<Model>("robot");

			// We aren't using the content pipeline, so we need
			// to access the stream directly:
			using (var stream = TitleContainer.OpenStream("Content/checkerboard.png"))
			{
				checkerboardTexture = Texture2D.FromStream(this.GraphicsDevice, stream);
			}
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

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			DrawGround();

			DrawModel(new Vector3(-4, 0, 3));
			DrawModel(new Vector3(0, 0, 3));
			DrawModel(new Vector3(4, 0, 3));

			DrawModel(new Vector3(-4, 4, 3));
			DrawModel(new Vector3(0, 4, 3));
			DrawModel(new Vector3(4, 4, 3));
			
			base.Draw(gameTime);
		}

		void DrawGround()
		{
			// The assignment of effect.View and effect.Projection
			// are nearly identical to the code in the Model drawing code.
			//var cameraPosition = new Vector3(0, 40, 20);
			var cameraLookAtVector = Vector3.Zero;
			var cameraUpVector = Vector3.UnitZ;

			effect.View = Matrix.CreateLookAt(cameraPosition, cameraLookAtVector, cameraUpVector);

			float aspectRatio = graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
			float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
			float nearClipPlane = 1;
			float farClipPlane = 200;

			effect.Projection = Matrix.CreatePerspectiveFieldOfView(
				fieldOfView, aspectRatio, nearClipPlane, farClipPlane);

			// new code:
			effect.TextureEnabled = true;
			effect.Texture = checkerboardTexture;

			foreach (var pass in effect.CurrentTechnique.Passes)
			{
				pass.Apply();

				graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList,floorVerts,0,2);
			}
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
					effect.TextureEnabled = true;
					//var cameraPosition = new Vector3(0, 10, 0);
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
