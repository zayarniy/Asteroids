using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace SpaceShooter
{
    class Asteroids
    {
        public static int Width, Height;
        public static Random rnd=new Random();
        static public SpriteBatch SpriteBatch { get; set; }
        static Star[] stars;
        static public StarShip StarShip { get; set; }
        static List<Fire> fires = new List<Fire>();
        static List<Asteroid> asteroids = new List<Asteroid>();


        static public int GetIntRnd(int min, int max)
        {
            return rnd.Next(min, max);
        }

        static public void ShipFire()
        {
            fires.Add(new Fire(StarShip.GetPosForFire));
        }

        static public void Init(SpriteBatch SpriteBatch, int Width, int Height)
        {
            Asteroids.Width = Width;
            Asteroids.Height = Height;
            Asteroids.SpriteBatch = SpriteBatch;
            stars = new Star[50];
            for (int i = 0; i < stars.Length; i++)
                stars[i] = new Star(new Vector2(-rnd.Next(1, 10), 0));
            StarShip = new StarShip(new Vector2(0, Height / 2 - 20));
            for (int i = 0; i < 10; i++)
                asteroids.Add(new Asteroid());
        }

        static public void Draw()
        {
            foreach (Star star in stars)
                star.Draw();
            foreach (Fire fire in fires)
                fire.Draw();
            foreach (Asteroid asteroid in asteroids)
                asteroid.Draw();
            StarShip.Draw();

        }

        static public void Update()
        {
            foreach (Star star in stars)
                star.Update();
            foreach (Asteroid asteroid in asteroids)
                asteroid.Update();
            for(int i=0;i<fires.Count;i++)
            {
                
                fires[i].Update();
                Asteroid asteroidCrash = fires[i].Crash(asteroids);
                if (asteroidCrash != null)
                {
                    asteroids.Remove(asteroidCrash);
                    fires.RemoveAt(i);
                    i--;
                    continue;
                }

                if (fires[i].Hidden)
                {
                    fires.RemoveAt(i);
                    i--;
                }
                
                //if (fires[i].)
            }
        }

    }

    class Star
    {
        Vector2 Pos;
        Vector2 Dir;
        Color color;

        public static Texture2D Texture2D { get; set; }

        public Star(Vector2 Pos, Vector2 Dir)
        {
            this.Pos = Pos;
            this.Dir = Dir;
        }

        public Star(Vector2 Dir)
        {
            this.Dir = Dir;
            RandomSet();
        }




        public void Update()
        {
            Pos += Dir;
            if (Pos.X < 0)
            {
                RandomSet();
            }
        }

        public void RandomSet()
        {
            Pos = new Vector2(Asteroids.GetIntRnd(Asteroids.Width, Asteroids.Width + 300), Asteroids.GetIntRnd(0, Asteroids.Height));
            color = Color.FromNonPremultiplied(Asteroids.GetIntRnd(0, 256), Asteroids.GetIntRnd(0, 256), Asteroids.GetIntRnd(0, 256), Asteroids.GetIntRnd(0, 256));
        }

        public void Draw()
        {
            Asteroids.SpriteBatch.Draw(Texture2D, Pos, color);
        }

    }

    class Fire
    {
        Vector2 Pos;
        Vector2 Dir;
        const int speed = 5;
        Color color=Color.White;

        public static Texture2D Texture2D { get; set; }

        public Fire(Vector2 Pos)
        {
            this.Pos = Pos;
            this.Dir = new Vector2(speed,0);
        }


        public Asteroid Crash(List<Asteroid> asteroids)
        {
            foreach (Asteroid asteroid in asteroids)
                if (asteroid.IsIntersect(new Rectangle((int)Pos.X,(int) Pos.Y, Texture2D.Width, Texture2D.Height))) return asteroid;
            return null;
        }

        public bool Hidden
        {
            get
            {
                return Pos.X > Asteroids.Width;
            }
        }

        public void Update()
        {            
            if (Pos.X <=Asteroids.Width)
            {
                Pos += Dir;
            }
        }



        public void Draw()
        {
            Asteroids.SpriteBatch.Draw(Texture2D, Pos, color);
        }

    }

    class Asteroid
    {
        Vector2 Pos;
        Vector2 Dir;
        Vector2 center;        
        float scale;
        Point size;

        Color color=Color.White;

        float spinSpeed = 1;
        float rotation;

        public static Texture2D Texture2D { get; set; }

        public bool IsIntersect(Rectangle rectangle)
        {
            return rectangle.Intersects(new Rectangle((int)Pos.X, (int)Pos.Y,size.X,size.Y));
        }

        public Asteroid()
        {
            RandomSet();
        }

        public Asteroid(Vector2 Pos, Vector2 Dir, float Scale, float SpinSpeed)
        {
            this.Pos = Pos;
            this.Dir = Dir;
            this.scale = Scale;
            this.spinSpeed = SpinSpeed;
            center = new Vector2(Texture2D.Width / 2, Texture2D.Height / 2);
            rotation = 0;
            size = new Point((int)(Texture2D.Width * scale), (int)(Texture2D.Height * scale));
        }

        public Asteroid(Vector2 Dir)
        {
            this.Dir = Dir;
            RandomSet();
        }

        public void Update()
        {
            Pos += Dir;
            rotation += spinSpeed;
            if (Pos.X < -100)
            {
                RandomSet();
            }
        }

        public void RandomSet()
        {
            Pos = new Vector2(Asteroids.GetIntRnd(Asteroids.Width, Asteroids.Width + 300), Asteroids.GetIntRnd(0, Asteroids.Height));
            Dir=new Vector2(-(float)Asteroids.rnd.NextDouble()*2+0.1f, 0f);
            spinSpeed = (float)(Asteroids.rnd.NextDouble()-0.5)/4;
            scale = (float) Asteroids.rnd.NextDouble();
            center = new Vector2(Texture2D.Width/2, Texture2D.Height/2);
            size = new Point((int)(Texture2D.Width * scale), (int)(Texture2D.Height * scale));
            //Size = new Point(Asteroids.GetIntRnd(10, 20), Asteroids.GetIntRnd(20,40));
            //color = Color.FromNonPremultiplied(Asteroids.GetIntRnd(0, 256), Asteroids.GetIntRnd(0, 256), Asteroids.GetIntRnd(0, 256), Asteroids.GetIntRnd(0, 256));
        }

        public void Draw()
        {
            Asteroids.SpriteBatch.Draw(Texture2D,Pos,null,color,rotation,center,scale, SpriteEffects.None,0);
        }

    }


    class StarShip
    {
        Vector2 Pos;
        public int Speed { get; set; } = 3;
        
        Color color=Color.White;

        public static Texture2D Texture2D { get; set; }

        public StarShip(Vector2 Pos)
        {
            this.Pos = Pos;
        }

        public Vector2 GetPosForFire => new Vector2(Pos.X + 30, Pos.Y + 30);

        public void Up()
        {
            if (this.Pos.Y >0) this.Pos.Y -= Speed;
        }

        public void Down()
        {
           if (this.Pos.Y<Asteroids.Height-Texture2D.Height) this.Pos.Y += Speed;
        }

        public void Left()
        {
            if (this.Pos.X >0) this.Pos.X -= Speed;
        }

        public void Right()
        {
            if (this.Pos.X <Asteroids.Width-Texture2D.Width) this.Pos.X += Speed;            
        }




        public void Draw()
        {
            Asteroids.SpriteBatch.Draw(Texture2D, Pos, color);
        }

    }
}
