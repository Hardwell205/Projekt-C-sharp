using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tetris
{
    public class Board
    {
        public int Height;
        public int Width;
        public List<Tile> Tiles = new List<Tile>();

        public Figure ActiveFigure;
        public Figure NextFigure { get; set; }
    }

    public enum Orientation
    {
        Horizontal,
        Vertical

    }


    public enum Figures
    {
        Stick = 0,
        Cube = 1,
        Left = 2,
        Rigth = 3,
        RigthS = 4,
        LeftS = 5,
        
    }



public class Figure
    {

    public static Figure GetFigure(Figures f)
    {
        switch (f)
        {
                case Figures.Cube:
                return new Figure
                {
                    Tiles = new List<Tile>(new Tile[]
                    {
                        new Tile(-1, -1),
                        new Tile(-1, 0),
                        new Tile(0, 0),
                        new Tile(0, -1)
                    })
                };

                case Figures.Stick:
                return new Figure
                {
                    Tiles = new List<Tile>(new Tile[]
                    {
                        new Tile(-1, 0),
                        new Tile(0, 0),
                        new Tile(1, 0),
                        new Tile(2, 0)
                    })
                };

                case Figures.Left:
                return new Figure
                {
                    Tiles = new List<Tile>(new Tile[]
                    {
                        new Tile(-1, 0),
                        new Tile(0, 0),
                        new Tile(1, 0),
                        new Tile(1, 1)
                    })
                };

                case Figures.Rigth:
                return new Figure
                {
                    Tiles = new List<Tile>(new Tile[]
                    {
                        new Tile(-1, 0),
                        new Tile(-1, 1),
                        new Tile(0, 0),
                        new Tile(1, 0)
                    })
                };

                case Figures.RigthS:
                return new Figure
                {
                    Tiles = new List<Tile>(new Tile[]
                    {
                        new Tile(-1, -1),
                        new Tile(-1, 0),
                        new Tile(0, 0),
                        new Tile(0, 1)
                    })  
                };

                case Figures.LeftS:
                return new Figure
                {
                    Tiles = new List<Tile>(new Tile[]
                    {
                        new Tile(0, -1),
                        new Tile(0, 0),
                        new Tile(-1, 0),
                        new Tile(-1, 1)
                    })
                };

        }

        return Figure.GetFigure(Figures.Cube);
    }

        public Orientation Orientation;

        public Figure()
        {
            this.Orientation = Orientation.Horizontal;
        }
        public void RotateLeft()
        {}

        public void RotateRight()
        {}

        public List<Tile> Tiles;


        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Tile
    {
        public Tile(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X;

        public int Y;

    }

    public enum Direction
    {
        Right,
        Down,
        Left
    }
}
