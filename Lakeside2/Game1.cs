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

        public const int INTERNAL_WIDTH = 320;
        public const int INTERNAL_HEIGHT = 180;
        public static int SCREEN_WIDTH = INTERNAL_WIDTH * GameInfo.getResolutionScale();
        public static int SCREEN_HEIGHT = INTERNAL_HEIGHT * GameInfo.getResolutionScale();
        public const int TILE_WIDTH = INTERNAL_WIDTH / Tile.TILE_SIZE;
        public const int TILE_HEIGHT = INTERNAL_HEIGHT / Tile.TILE_SIZE;
        const bool FULLSCREEN = false;

        public static Texture2D WHITE_PIXEL;
        private InputHandler input;
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
            setResolution();

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
            MusicManager.init(Content);

            states = new Stack();
            states.Push(new TitleScreen(this, Content));
        }

        public void setResolution()
        {
            SCREEN_WIDTH = INTERNAL_WIDTH * GameInfo.getResolutionScale();
            SCREEN_HEIGHT = INTERNAL_HEIGHT * GameInfo.getResolutionScale();

            _graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            _graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            _graphics.IsFullScreen = FULLSCREEN;
            _graphics.ApplyChanges();
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
            }, states.Peek() is TitleScreen);
        }

        public void goToWorld(Player p, string filename, bool resetLocation = true)
        {
            if (states.Peek() is World)
            {
                startFade(() =>
                {
                    World w = (World)states.Peek();
                    if (resetLocation) w.setMap(SerializableMap.Load(Content, filename));
                    else w.setMap(SerializableMap.Load(Content, filename), p.getTileLocation());
                }, false);
            }
            else
            {
                startFade(() =>
                {
                    Vector2 oldLocation = p.getTileLocation();
                    setState(new World(Content, this, p, filename));
                    if (!resetLocation) p.setTileLocation(oldLocation);
                }, states.Peek() is TitleScreen);
            }
        }

        public void goToTitle()
        {
            startFade(() =>
            {
                setState(new TitleScreen(this, Content));
            }, true);
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
                }, false);
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
                }, false);
            }
            else
            {
                states.Pop();
            }
        }

        void startFade(Action midpoint, bool fullscreen)
        {
            if (fade != null) return;
            fade = new UiScreenFade(midpoint);
            fade.fullscreen = fullscreen;
        }



        protected override void Update(GameTime gameTime)
        {
            input.update();

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
