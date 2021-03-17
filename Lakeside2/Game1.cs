using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lakeside2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private InputHandler input;

        const int SCREEN_WIDTH = 1280;
        const int SCREEN_HEIGHT = 720;
        const bool FULLSCREEN = false;

        const int INTERNAL_WIDTH = 320;
        const int INTERNAL_HEIGHT = 180;

        RenderTarget2D mainTarget;

        TileMap testMap;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            _graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            _graphics.IsFullScreen = FULLSCREEN;
            _graphics.ApplyChanges();

            mainTarget = new RenderTarget2D(GraphicsDevice, INTERNAL_WIDTH, INTERNAL_HEIGHT);

            input = new InputHandler();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            testMap = new TileMap(Content, 20, 10);

        }

        protected override void Update(GameTime gameTime)
        {
            input.update();
            if (input.isKeyPressed(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // draw everything to render target
            GraphicsDevice.SetRenderTarget(mainTarget);
            _spriteBatch.Begin();
            // most drawing happens here...
            testMap.draw(_spriteBatch, Vector2.Zero);
            _spriteBatch.End();

            // scale to screen size, draw to window
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(mainTarget, new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), Color.White);
            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
