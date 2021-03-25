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

        public static Color BG_COLOR = new Color(140, 145, 0);

        public const int INTERNAL_WIDTH = 320;
        public const int INTERNAL_HEIGHT = 180;

        public const int TILE_WIDTH = INTERNAL_WIDTH / Tile.TILE_SIZE;
        public const int TILE_HEIGHT = INTERNAL_HEIGHT / Tile.TILE_SIZE;

        public static Texture2D WHITE_PIXEL;

        RenderTarget2D mainTarget;

        World world;

        Effect colorize;

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

            WHITE_PIXEL = new Texture2D(GraphicsDevice, 1, 1);
            WHITE_PIXEL.SetData<Color>(new Color[] { Color.White });

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Fonts.loadFont(Content, "Arial");
            colorize = Content.Load<Effect>("colorize");

            world = new World(Content, "default.txt");
        }

        protected override void Update(GameTime gameTime)
        {
            input.update();
            if (input.isKeyPressed(Keys.Escape))
                Exit();

            double dt = gameTime.ElapsedGameTime.TotalSeconds;

            world.onInput(input);
            world.update(dt);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // draw everything to render target
            GraphicsDevice.SetRenderTarget(mainTarget);
            _spriteBatch.Begin();
            // most drawing happens here...
            world.draw(_spriteBatch);
            _spriteBatch.End();

            // scale to screen size, draw to window
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(BG_COLOR);
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, effect: colorize);
            _spriteBatch.Draw(mainTarget, new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), Color.White);
            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
