using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceShooter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    enum Stat
    {
        SplashScreen,
        Game,
        Final,
        Pause
    }

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Stat Stat = Stat.Game;
        KeyboardState keyboardState, oldKeyboardState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            graphics.PreferredBackBufferWidth = 1680;
            graphics.PreferredBackBufferHeight = 1050;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
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
            SplashScreen.Background = Content.Load<Texture2D>("background");
            SplashScreen.Font = Content.Load<SpriteFont>("SplashFont");
            Star.Texture2D = Content.Load<Texture2D>("star");
            StarShip.Texture2D = Content.Load<Texture2D>("starship");
            Fire.Texture2D = Content.Load<Texture2D>("fire");
            Asteroid.Texture2D = Content.Load<Texture2D>("asteroid");
            Asteroids.Init(spriteBatch, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            // TODO: use this.Content to load your game content here
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
            keyboardState = Keyboard.GetState();
            switch (Stat)
            {
                case Stat.SplashScreen:
                    SplashScreen.Update();
                    if (keyboardState.IsKeyDown(Keys.Space)) Stat = Stat.Game;
                    break;
                case Stat.Game:
                    Asteroids.Update();
                    if (keyboardState.IsKeyDown(Keys.Escape)) Stat = Stat.SplashScreen;
                    if (keyboardState.IsKeyDown(Keys.Up)) Asteroids.StarShip.Up() ;
                    if (keyboardState.IsKeyDown(Keys.Left)) Asteroids.StarShip.Left();
                    if (keyboardState.IsKeyDown(Keys.Right)) Asteroids.StarShip.Right();
                    if (keyboardState.IsKeyDown(Keys.Down)) Asteroids.StarShip.Down();
                    if (keyboardState.IsKeyDown(Keys.LeftControl) && oldKeyboardState.IsKeyUp(Keys.LeftControl)) Asteroids.ShipFire();
                    break;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape)) Exit();
            // TODO: Add your update logic here
            oldKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            switch(Stat)
            {
                case Stat.SplashScreen:
                    SplashScreen.Draw(spriteBatch);
                    break;
                case Stat.Game:
                    Asteroids.Draw();
                    break;
            }
            

            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
