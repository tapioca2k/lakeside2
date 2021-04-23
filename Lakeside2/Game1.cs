using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.IO;
using Lakeside2.Map;
using Lakeside2.UI;
using Lakeside2.Serialization;
using Lakeside2.Editor;

namespace Lakeside2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Effect colorize;
        public static Color BG_COLOR = new Color(140, 145, 0);
        RenderTarget2D mainTarget;

        const int SCREEN_WIDTH = 1280;
        const int SCREEN_HEIGHT = 720;
        public const int INTERNAL_WIDTH = 320;
        public const int INTERNAL_HEIGHT = 180;
        public const int TILE_WIDTH = INTERNAL_WIDTH / Tile.TILE_SIZE;
        public const int TILE_HEIGHT = INTERNAL_HEIGHT / Tile.TILE_SIZE;
        const bool FULLSCREEN = false;

        public static Texture2D WHITE_PIXEL;
        private InputHandler input;
        public static MusicManager music;
        IGameState currentState;
        UiScreenFade fade;


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
            Fonts.loadFont(Content, "rainyhearts");
            colorize = Content.Load<Effect>("colorize");
            music = new MusicManager(Content);

            //currentState = new World(Content, this);
            currentState = new Overworld(Content, this, new Player(Content, null, null));
        }

        public void goToMap(Player p, string currentMap)
        {
            if (currentState is Overworld)
            {
                throw new Exception("Already in map");
            }

            fade = new UiScreenFade(() =>
            {
                setState(new Overworld(Content, this, p, currentMap));
            });
        }

        public void goToWorld(Player p, string filename)
        {
            if (currentState is World)
            {
                fade = new UiScreenFade(() =>
                {
                    World w = (World)currentState;
                    w.setMap(SerializableMap.Load(Content, filename));
                });
            }
            else
            {
                fade = new UiScreenFade(() =>
                {
                    setState(new World(Content, this, p, filename));
                });
            }
        }

        void setState(IGameState state)
        {
            currentState = state;
        }

        protected override void Update(GameTime gameTime)
        {
            input.update();
            /*
            if (input.isKeyPressed(Keys.Escape))
                Exit();
            */

            double dt = gameTime.ElapsedGameTime.TotalSeconds;

            currentState.onInput(input);
            currentState.update(dt);

            if (fade != null) fade.update(dt);
            if (fade != null && fade.finished) fade = null;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // draw everything to render target
            GraphicsDevice.SetRenderTarget(mainTarget);
            _spriteBatch.Begin();
            // most drawing happens here...
            currentState.draw(new SBWrapper(_spriteBatch));
            if (fade != null) fade.draw(new SBWrapper(_spriteBatch));
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
