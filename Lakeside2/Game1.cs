using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.IO;
using Lakeside2.WorldMap;
using Lakeside2.UI;
using Lakeside2.Serialization;
using Lakeside2.Editor;
using System.Collections;

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
        Stack states;
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

            Window.Title = GameInfo.title;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Fonts.loadFont(Content, "Arial");
            Fonts.loadFont(Content, "rainyhearts");
            colorize = Content.Load<Effect>("colorize");
            music = new MusicManager(Content);

            states = new Stack();
            states.Push(new TitleScreen(this, Content));
            //states.Push(new Overworld(Content, this, new Player(Content, null, null)));
        }

        public void goToOverworld(Player p, string currentMap)
        {
            if (states.Peek() is Overworld)
            {
                throw new Exception("Already in overworld");
            }

            startFade(() =>
            {
                setState(new Overworld(Content, this, p, currentMap));
            });
        }

        public void goToWorld(Player p, string filename)
        {
            if (states.Peek() is World)
            {
                startFade(() =>
                {
                    World w = (World)states.Peek();
                    w.setMap(SerializableMap.Load(Content, filename));
                });
            }
            else
            {
                startFade(() =>
                {
                    setState(new World(Content, this, p, filename));
                });
            }
        }

        void setState(IGameState state)
        {
            // clear stack and set state
            while (states.Count > 0) states.Pop();
            states.Push(state);
        }

        public void pushState(IGameState state, bool fading = false)
        {
            if (fading)
            {
                startFade(() =>
                {
                    states.Push(state);
                });
            }
            else
            {
                states.Push(state);
            }
        }

        public void popState(bool fading = false)
        {
            if (fading)
            {
                startFade(() =>
                {
                    states.Pop();
                });
            }
            else
            {
                states.Pop();
            }
        }

        void startFade(Action midpoint)
        {
            if (fade != null) return;
            fade = new UiScreenFade(midpoint);
            fade.fullscreen = states.Peek() is TitleScreen;
        }



        protected override void Update(GameTime gameTime)
        {
            input.update();
            /*
            if (input.isKeyPressed(Keys.Escape))
                Exit();
            */

            double dt = gameTime.ElapsedGameTime.TotalSeconds;
            TimeOfDay.addSeconds(dt * 360);

            if (states.Count > 0)
            {
                IGameState state = (IGameState)states.Peek();
                state.onInput(input);
                state.update(dt);
            }

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
            if (states.Count > 0)
            {
                IGameState state = (IGameState)states.Peek();
                state.draw(new SBWrapper(_spriteBatch));
            }
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
