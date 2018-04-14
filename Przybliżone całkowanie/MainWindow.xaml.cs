using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OxyPlot;
using OxyPlot.Series;
using Przybliżone_całkowanie.Models;

namespace Przybliżone_całkowanie
{

    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            RegisterEvents();

        }

        private void RegisterEvents()
        {
            Recalculate();
            FX.TextChanged += InputTextChanged;
            XP.TextChanged += InputTextChanged;
            XK.TextChanged += InputTextChanged;
            Precision.TextChanged += InputTextChanged;
            
        }

        private void InputTextChanged(object sender, TextChangedEventArgs e)
        {
            Recalculate();
        }

        private void Recalculate()
        {
            try
            {
                var fx = new Models.InputFunction(FX.Text.Trim().Replace(' ', new char()));
                double.TryParse(XP.Text.Trim(), out var from);
                double.TryParse(XK.Text.Trim(), out var to);
                double.TryParse(Precision.Text.Trim(), out var prec);
                if (from > to)
                {
                    throw new Exception();
                }
                double result = 0, x, field = 0;
                var partsCount = (to - from) / prec;
                for (var i = 1; i <= prec; i++)
                {
                    x = from + i * partsCount;
                    field += fx.F(x - partsCount / 2);
                    if (i < prec) result += fx.F(x);
                }

                result = partsCount / 6 * (fx.F(from) + fx.F(to) + 2 * result + 4 * field);
                if (result < 0) result *= -1;
                Result.Content = "Wynik: " + result.ToString(CultureInfo.CurrentCulture);
                DrawCurve(fx, from, to);
            }
            catch (Exception ignoredException)
            {
                Result.Content = "Błąd danych wejściowych";
            }
        }

        private void DrawCurve(InputFunction fx, double from, double to)
        {
            var model = new PlotModel { Title = "Wykres funkcji f(x)" };
            
            var a=new AreaSeries();
            a.Fill = OxyColor.FromArgb(200, 45, 185, 82);
            for (double x = from; x <= to + (0.1 * 0.5); x += 0.1)
            {
                a.Points.Add(new DataPoint(x, fx.F(x)));
            }
            a.Color2 = OxyColor.FromArgb(0, 0, 0, 0);
            model.Series.Add(a);
            var fs = new FunctionSeries(fx.F, from-(20*from)/100, to+ (20 * to) / 100, 0.1, "f(x)");
            
            model.Series.Add(fs);
            CurveView.Model = model;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            //Recalculate();
        }
    }
}
