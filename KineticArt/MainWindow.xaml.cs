using KineticArt.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static KineticArt.Models.Actuator;
using static KineticArt.Models.ActuatorType;
using System.Text.Json;
using Microsoft.Win32;
using System.IO;
using KineticArt.Services;
using RotationDirection = KineticArt.Models.RotationDirection;

namespace KineticArt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //double angle = 0;

        //double[] angle = new double[8];
        //double[] speed = new double[8];
        //double[] phase = new double[8];

        Actuator[] actuators = new Actuator[8];

        TextBox[] angleBoxes = new TextBox[8];

        private bool updatingUI = false;

        private MotionPlayer motionPlayer = new MotionPlayer();


        DispatcherTimer timer;

      
        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval =
                TimeSpan.FromMilliseconds(16);
            timer.Tick += Update;
            timer.Start();

            actuators = new Actuator[]
            {
                new Actuator   // Line 0
                {
                    Type = ShapeType.Line,
                    PivotX = 150,
                    PivotY = 150,
                    InitialAngle = 15,
                    RotatedAngle = 0,
                    AngularSpeed = 1,
                    Direction = RotationDirection.CounterClockwise
                },

                new Actuator   // Line 1
                {
                    Type = ShapeType.Line,
                    PivotX = 300,
                    PivotY = 150,
                    InitialAngle = 105,
                    RotatedAngle = 0,
                    AngularSpeed = 1,
                    Direction = RotationDirection.CounterClockwise
                },

                new Actuator   // Line 2
                {
                    Type = ShapeType.Line,
                    PivotX = 150,
                    PivotY = 300,
                    InitialAngle = 105,
                    RotatedAngle = 0,
                    AngularSpeed = 1,
                    Direction = RotationDirection.CounterClockwise
                },

                new Actuator   // Line 3
                {
                    Type = ShapeType.Line,
                    PivotX = 300,
                    PivotY = 300,
                    InitialAngle = 15,
                    RotatedAngle = 0,
                    AngularSpeed = 1,
                    Direction = RotationDirection.CounterClockwise
                },



                new Actuator   // Square 0
                {
                    Type = ShapeType.Square,
                    PivotX = 150,
                    PivotY = 150,
                    InitialAngle = 245,
                    RotatedAngle = 0,
                    AngularSpeed = 1,
                    Direction = RotationDirection.Clockwise
                },

                new Actuator   // Square 1
                {
                    Type = ShapeType.Square,
                    PivotX = 300,
                    PivotY = 150,
                    InitialAngle = 245 + 90,
                    RotatedAngle = 0,
                    AngularSpeed = 1,
                    Direction = RotationDirection.Clockwise
                },

                new Actuator   // Square 2
                {
                    Type = ShapeType.Square,
                    PivotX = 150,
                    PivotY = 300,
                    InitialAngle = 245-90,
                    RotatedAngle = 0,
                    AngularSpeed =1,
                    Direction = RotationDirection.Clockwise
                },

                new Actuator   // Square 3
                {
                    Type = ShapeType.Square,
                    PivotX = 300,
                    PivotY = 300,
                    InitialAngle = 245 + 180,
                    RotatedAngle = 0,
                    AngularSpeed = 1,
                    Direction = RotationDirection.Clockwise
                },
            };

            angleBoxes = new TextBox[]
            {
                txtLine0, txtLine1, txtLine2, txtLine3,
                txtSquare0, txtSquare1,txtSquare2, txtSquare3
            };
        }

        void Update(object sender, EventArgs e)
        {
            double displayAngle;
            foreach (var a in actuators)
            {
                a.RotatedAngle += a.AngularSpeed * (int)a.Direction;

                displayAngle = ConvertAngleVisible(a.InitialAngle + a.RotatedAngle);
                a.DisplayAngle = displayAngle;
            }

            Draw();

            // Retrieve the input from Textboxes
            for (int i = 0; i < 8; i++)
            {
                angleBoxes[i].Text = actuators[i].DisplayAngle.ToString("F1");
            }

            updatingUI = true;

            for (int i = 0; i < 8; i++)
            {
                angleBoxes[i].Text =
                    actuators[i].DisplayAngle.ToString("F1");
            }

            updatingUI = false;


            //motionPlayer.Update(actuators);

            //Draw();

        }

        public double ConvertAngleVisible(double angle)
        {
            angle %= 360;

            if (angle < 0)
                angle += 360;

            return angle;
        }

        private void AngleTextChanged(object sender, TextChangedEventArgs e)
        {
            if (updatingUI)
                return;

            TextBox tb = sender as TextBox;

            int index = int.Parse(tb.Tag.ToString());

            if (double.TryParse(tb.Text, out double value))
            {
                actuators[index].RotatedAngle =
                    value - actuators[index].InitialAngle;

                Draw();
            }
        }

        void Draw()
        {
            DrawingCanvas.Children.Clear();

            foreach (var a in actuators)
            {
                if (a.Type == ActuatorType.ShapeType.Line)
                    DrawLine(a, 150);
                else
                    DrawSquare(a, 100);
            }
        }

         void DrawLine(Actuator a, double length)
        {
            double px = a.PivotX;
            double py = a.PivotY;
            double displayAngle = (a.InitialAngle + a.RotatedAngle) * Math.PI / 180;


            double x1 = px + length * Math.Cos(displayAngle);
            double y1 = py + length * Math.Sin(displayAngle);
            double x2 = px - length * Math.Cos(displayAngle);
            double y2 = py - length * Math.Sin(displayAngle);

            var line = new Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = Brushes.White,
                StrokeThickness = 4
            };

            DrawingCanvas.Children.Add(line);
        }

        void DrawSquare(Actuator a, double size)
        {
            double px = a.PivotX;
            double py = a.PivotY;

            double displayAngle = (a.InitialAngle + a.RotatedAngle) * Math.PI / 180;
            double cos = Math.Cos(displayAngle);
            double sin = Math.Sin(displayAngle);

            Point[] pts = new Point[4];

            pts[0] = Rotate(px, py, px, py, cos, sin);
            pts[1] = Rotate(px + size, py, px, py, cos, sin);
            pts[2] = Rotate(px + size, py + size, px, py, cos, sin);
            pts[3] = Rotate(px, py + size, px, py, cos, sin);

            var polygon = new Polygon
            {
                Stroke = Brushes.Gold,
                StrokeThickness = 4
            };

            polygon.Points = new PointCollection(pts);

            DrawingCanvas.Children.Add(polygon);
        }

        void DrawCircle(Actuator a, double diameter)
        {
            double px = a.PivotX;
            double py = a.PivotY;
        }

        Point Rotate (double x, double y, double cx, double cy, double cos, double sin)
        {
            double dx = x - cx;
            double dy = y - cy;

            return new Point(cx + dx * cos - dy * sin, cy + dx * sin + dy * cos);
        }

       

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
                btnPause.Content = "Resume";
            }
            else
            {
                timer.Start();
                btnPause.Content = "Pause";
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            // Lines
            txtLine0.Text = actuators[0].InitialAngle.ToString();
            txtLine1.Text = actuators[1].InitialAngle.ToString();
            txtLine2.Text = actuators[2].InitialAngle.ToString();
            txtLine3.Text = actuators[3].InitialAngle.ToString();

            // Squares
            txtSquare0.Text = actuators[4].InitialAngle.ToString();
            txtSquare1.Text = actuators[5].InitialAngle.ToString();
            txtSquare2.Text = actuators[6].InitialAngle.ToString();
            txtSquare3.Text = actuators[7].InitialAngle.ToString();
        }

        private void btnSavePattern_Click(object sender, RoutedEventArgs e)
        {
            Pattern p = new Pattern();

            p.AngleLine = new double[4];
            p.AngleSquare = new double[4];

            for (int i = 0; i < 4; i++)
            {
                p.AngleLine[i] = actuators[i].DisplayAngle;
                p.AngleSquare[i] = actuators[i + 4].DisplayAngle;
            }

            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter = "JSON files (*.json)|*.json";
            dialog.DefaultExt = "json";
            dialog.FileName = "Pattern.json";

            if (dialog.ShowDialog() == true)
            {
                string json = JsonSerializer.Serialize(
                    p,
                    new JsonSerializerOptions()
                    {
                        WriteIndented = true
                    });

                File.WriteAllText(dialog.FileName, json);
            }
        }
    }
}