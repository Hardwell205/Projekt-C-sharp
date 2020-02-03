using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace tetris
{

    public partial class MainWindow : Window
    {

        public const int Size = 50;
        public const int Padding = 1;
        public const int Width = 10;
        public const int Height = 15;
        public int linesCompleted = 0;
        public int linesRecord = 0;
        public int tick = 300;

        Timer timer;
        private  int x = 0;
        private void Callback(object state)
        {
            Dispatcher.InvokeAsync(() =>
            {
                if (Board.ActiveFigure == null)
                {
                    Board.ActiveFigure = Board.NextFigure ?? Figure.GetFigure((Figures)new Random().Next(0, 5));
                    Board.NextFigure = Figure.GetFigure((Figures)new Random().Next(0, 5));
                    Board.NextFigure.X = 2;
                    Board.NextFigure.Y = 2;
                    DrawNextFigure();

                    Board.ActiveFigure.X = Board.Width/2;
                    Board.ActiveFigure.Y = Board.Height - 2;
                    DrawTiles();
                    return;
                }

                if (CheckBounds(Direction.Down))
                {
                    Board.ActiveFigure.Y -= 1;
                    DrawTiles();
                    return;
                }
                else
                {
                    if (Board.ActiveFigure.Y < Board.Height - 2)
                    {
                        Board.Tiles.AddRange(Board.ActiveFigure.Tiles.Select(tile => new Tile(tile.X + Board.ActiveFigure.X, tile.Y + Board.ActiveFigure.Y)));
                        Board.ActiveFigure = null;
                        RemoveLines();
                        return;
                    }
                    else
                    {
                        if (linesRecord < linesCompleted) linesRecord = linesCompleted;
                        LinesRecord.Text = linesRecord.ToString();
                        Board.ActiveFigure = null;
                        Board.Tiles = new List<Tile>();
                        linesCompleted = 0;
                        timer.Change(int.MaxValue, 0);
                        Announcement.Text = "Game Over!!";
                        Announcement.Visibility = Visibility.Visible;
                    }
                }

            });
        }

        private void RemoveLines()
        {
            for (int y = 0; y < Board.Height; y++)
            {
                if (Board.Tiles.Count(tile => tile.Y == y) == Board.Width)
                {
                    Board.Tiles = Board.Tiles.Where(tile => tile.Y != y).ToList();
                    Board.Tiles.Where(tile => tile.Y > y).ToList().ForEach(tile => tile.Y--);
                    linesCompleted++;
                    LinesNumber.Text = linesCompleted.ToString();
                    if (linesRecord < linesCompleted) linesRecord = linesCompleted;
                    LinesRecord.Text = linesRecord.ToString();
                    y--;
                }
            }

        }

        public MainWindow()
        {

            this.tick = Convert.ToInt32(ConfigurationManager.AppSettings["tick"]);

            InitializeComponent();
            InitBoard();
            DrawBoard();

            timer = new Timer(Callback, null, 1000, tick);

        }

        private void DrawNextFigure()
        {
            nextGrig.Children.Clear();

                        if (Board.ActiveFigure != null)
                foreach (var tile in Board.NextFigure.Tiles)
                {
                    Border border = new Border();
                    nextGrig.Children.Add(border);
                    border.Width = Size - Padding * 2;
                    border.Height = Size - Padding * 2;
                    Grid.SetRow(border, tile.Y + Board.NextFigure.Y);
                    Grid.SetColumn(border, tile.X + Board.NextFigure.X);
                    border.Background = new SolidColorBrush(Colors.OrangeRed);
                    border.Padding = new Thickness(0);
                }
        }

        private void DrawTiles()
        {
            TilesGrid.Children.Clear();
            ActiveFigureGrid.Children.Clear();

            if (Board.Tiles != null)
                foreach (var tile in Board.Tiles)
                {
                    Border border = new Border();
                    TilesGrid.Children.Add(border);
                    border.Width = Size - Padding*2;
                    border.Height = Size - Padding*2;
                    Grid.SetRow(border, tile.Y);
                    Grid.SetColumn(border, tile.X);
                    border.Background = new SolidColorBrush(Colors.Orange);
                    border.Padding = new Thickness(0);
                }

            if (Board.ActiveFigure != null)
                foreach (var tile in Board.ActiveFigure.Tiles)
                {
                    Border border = new Border();
                    ActiveFigureGrid.Children.Add(border);
                    border.Width = Size - Padding * 2;
                    border.Height = Size - Padding * 2;
                    Grid.SetRow(border, tile.Y + Board.ActiveFigure.Y);
                    Grid.SetColumn(border, tile.X + Board.ActiveFigure.X);
                    border.Background = new SolidColorBrush(Colors.OrangeRed);
                    border.Padding = new Thickness(0);
                }
        }

        public void InitBoard()
        {
            Board.Width = Width;
            Board.Height = Height;

            Board.Tiles = new List<Tile>();
        }

        private void DrawBoard()
        {
            Grid.RowDefinitions.Clear();
            for (int x = 0; x < Board.Width; x++)
            {
                Grid.ColumnDefinitions.Add(new ColumnDefinition() {Width = new GridLength(Size)});
                TilesGrid.ColumnDefinitions.Add(new ColumnDefinition() {Width = new GridLength(Size)});
                ActiveFigureGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(Size) });
            }

            for (int x = 0; x < Board.Height; x++)
            {
                Grid.RowDefinitions.Add(new RowDefinition() {Height = new GridLength(Size)});
                TilesGrid.RowDefinitions.Add(new RowDefinition() {Height = new GridLength(Size)});
                ActiveFigureGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(Size) });
            }


            for (int x = 0; x < Board.Width; x++)
            {
                for (int y = 0; y < Board.Height; y++)
                {
                    Border border = new Border();

                    Grid.Children.Add(border);

                    border.Width = Size - Padding*2;
                    border.Height = Size - Padding*2;
                    Grid.SetRow(border, y);
                    Grid.SetColumn(border, x);
                    border.Background = new SolidColorBrush(Colors.Beige);
                    border.Padding = new Thickness(0);
                }
            }

            for (int x = 0; x < 5; x++)
            {
                nextGrig.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(Size) });
            }

            for (int x = 0; x < 5; x++)
            {
                nextGrig.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(Size) });
            }

        }

        public Board Board = new Board();

        private void Grid_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {

                case Key.Down:
                    MoveActiveFigure(Direction.Down);
                    break;

                case Key.Right:
                    MoveActiveFigure(Direction.Right);
                    break;

                case Key.Left:
                    MoveActiveFigure(Direction.Left);
                    break;

                case Key.Space:
                    if (Board.ActiveFigure != null)
                    {
                        var testFigure = new Figure
                        {
                            Tiles = Board.ActiveFigure.Tiles.Select(tile => new Tile(tile.X, tile.Y)).ToList(),
                            Y = Board.ActiveFigure.Y, X = Board.ActiveFigure.X
                        };

                        RotateFigure(Direction.Right, testFigure);

                        if (CheckBounds(testFigure)) 
                            RotateFigure(Direction.Right);
                    }

                    break;

            }
        }

        private void RotateFigure(Direction direction, Figure figure = null)
        {
            if (figure == null) figure = Board.ActiveFigure;
            figure.Tiles.ForEach(tile =>
            {
                int x = tile.X;
                int y = tile.Y;


                if (x <= 0 && y < 0 || x >= 0 && y > 0)
                {
                    tile.X = y;
                    tile.Y = -1*x;
                }
                else 
                {
                    tile.Y = x * -1;
                    tile.X = y;
                }

            });

            figure.Orientation = figure.Orientation == Orientation.Horizontal
                ? Orientation.Vertical
                : Orientation.Horizontal;

            DrawTiles();

        }

        private bool MoveActiveFigure(Direction direction)
        {
            if (!CheckBounds(direction)) 
                return false;

            switch (direction)
            {
                
                case Direction.Down:
                    Board.ActiveFigure.Y -= 1;
                    break;
                
                case Direction.Left:
                    Board.ActiveFigure.X -= 1;
                    break;
                
                case Direction.Right:
                    Board.ActiveFigure.X += 1;
                    break;
            }

            DrawTiles();
            return true;
        }

        private bool CheckBounds(Figure figure = null)
        {
            if (Board.ActiveFigure == null && figure == null) return false;

            if (figure == null) figure = Board.ActiveFigure;

            if (figure.Tiles.Any(tile => Board.Tiles.Any(tile1 => figure.X + tile.X == tile1.X && figure.Y + tile.Y  == tile1.Y)))
                return false;

            if (figure.Tiles.Any(tile => figure.Y + tile.Y < 0)) return false;
            if (figure.Tiles.Any(tile => figure.Y + tile.Y >= Board.Height)) return false;
            if (figure.Tiles.Any(tile => figure.X + tile.X < 0)) return false;
            if (figure.Tiles.Any(tile => figure.X + tile.X >= Board.Width)) return false;

            return true;

        }

        private bool CheckBounds(Direction direction, Figure figure = null)
        {

            if (Board.ActiveFigure == null && figure == null) return false;
            if (figure == null) figure = Board.ActiveFigure;

            switch (direction)
            {
                case Direction.Down:
                    if (figure.Tiles.Any(tile => figure.Y + tile.Y - 1 < 0)) return false;

                    if (figure.Tiles.Any(tile => Board.Tiles.Any(tile1 => figure.X + tile.X == tile1.X && figure.Y + tile.Y - 1 == tile1.Y)))
                        return false;

                    break;


                case Direction.Left:
                    if (figure.Tiles.Any(tile => figure.X + tile.X - 1 < 0)) return false;


                    if (figure.Tiles.Any(tile => Board.Tiles.Any(tile1 => figure.X + tile.X - 1 == tile1.X && figure.Y + tile.Y == tile1.Y)))
                        return false;
                    break;

                case Direction.Right:
                    if (figure.Tiles.Any(tile => figure.X + tile.X + 1 >= Board.Width)) return false;


                    if (figure.Tiles.Any(tile => Board.Tiles.Any(tile1 => figure.X + tile.X + 1 == tile1.X && figure.Y + tile.Y == tile1.Y)))
                        return false;
                    break;
            }
            return true;
        }


        private void StartAgain(object sender, RoutedEventArgs e)
        {
            Announcement.Visibility = Visibility.Hidden;
            if (linesRecord < linesCompleted) linesRecord = linesCompleted;
            LinesRecord.Text = linesRecord.ToString();
            LinesNumber.Text = 0.ToString();
            
            Board.Tiles = new List<Tile>();
            DrawTiles();
            Board.ActiveFigure = null;
            linesCompleted = 0;
            timer.Change(0, tick);
            
        }
    }
}