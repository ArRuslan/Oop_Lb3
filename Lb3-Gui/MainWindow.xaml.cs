using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;
using Lib;

namespace Lb3_Gui {
    [SuppressMessage("ReSharper", "SuggestVarOrType_SimpleTypes")]
    [SuppressMessage("ReSharper", "SuggestVarOrType_BuiltInTypes")]
    [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
    public partial class MainWindow {
        private Image _mainImage;
        private Figure SelectedFigure {
            get {
                string[] selItem = FiguresCb.Text.Split(' ');
                if(selItem.Length < 2)
                    return _mainImage;
                int idx = int.Parse(selItem[0]);
                if(idx >= _mainImage.Figures.Count)
                    return _mainImage;
                return _mainImage[idx];
            }
        }
        
        public MainWindow() {
            InitializeComponent();
            _mainImage = new Image(540, 464);
            FiguresCb.Items.Add("Image");
            FiguresCb.Text = "Image";
        }
        
        private void _drawAll() {
            MainCanvas.Children.Clear();
            _mainImage.Draw(MainCanvas);
        }
        
        private void AddFigure_Btn_Click(object sender, RoutedEventArgs e) {
            Figure figure = Figure.Get(NewFiguresCb.Text);
            _mainImage.AddFigure(figure);
            _drawAll();
            FiguresCb.Items.Add($"{_mainImage.Figures.IndexOf(figure)} {figure.GetType().Name}");
        }
        
        private void FigureInfo_Btn_Click(object sender, RoutedEventArgs e) {
            string info = SelectedFigure.ToString();
            Console.WriteLine(info);
            Console.WriteLine("================================");
            MessageBox.Show(info);
        }
        
        private static double? ParseDouble(string str) {
            double d;
            try {
                d = double.Parse(str.Replace(".", ","));
            } catch (Exception) {
                return null;
            }
            return d;
        }
            
        private void MoveTo_Btn_Click(object sender, RoutedEventArgs e) {
            double? x = ParseDouble(TbMoveToX.Text);
            double? y = ParseDouble(TbMoveToY.Text);
            if(x == null || y == null) {
                MessageBox.Show("Введені значення не є корректними числами.");
                return;
            }
            SelectedFigure.MoveTo((double)x, (double)y);
            _drawAll();
        }

        private void Move_Btn_Click(object sender, RoutedEventArgs e) {
            double? x = ParseDouble(TbMoveX.Text);
            double? y = ParseDouble(TbMoveY.Text);
            if(x == null || y == null) {
                MessageBox.Show("Введені значення не є корректними числами.");
                return;
            }
            SelectedFigure.Move((double)x, (double)y);
            _drawAll();
        }
        
        private void MoveAllTo_Btn_Click(object sender, RoutedEventArgs e) {
            double? x = ParseDouble(TbMoveToX.Text);
            double? y = ParseDouble(TbMoveToY.Text);
            if(x == null || y == null) {
                MessageBox.Show("Введені значення не є корректними числами.");
                return;
            }
            _mainImage.MoveAllTo((double)x, (double)y);
            _drawAll();
        }
        
        private void MoveAll_Btn_Click(object sender, RoutedEventArgs e) {
            double? x = ParseDouble(TbMoveX.Text);
            double? y = ParseDouble(TbMoveY.Text);
            if(x == null || y == null) {
                MessageBox.Show("Введені значення не є корректними числами.");
                return;
            }
            _mainImage.MoveAll((double)x, (double)y);
            _drawAll();
        }
        
        private void SetScale_Btn_Click(object sender, RoutedEventArgs e) {
            double? scale = ParseDouble(TbScale.Text);
            if(scale == null || scale < 0) {
                MessageBox.Show("Введене значення масштабу не є корректним числом.");
                return;
            }
            SelectedFigure.Scale = (double)scale;
            _drawAll();
        }
        
        private void SaveImage_Btn_Click(object sender, RoutedEventArgs e) {
            TbImagePath.Text = TbImagePath.Text.Trim();
            try { new FileInfo(TbImagePath.Text); } catch(Exception) { return; }
            _mainImage.SaveToFile(TbImagePath.Text);
            _drawAll();
        }
        
        private void LoadImage_Btn_Click(object sender, RoutedEventArgs e) {
            TbImagePath.Text = TbImagePath.Text.Trim();
            try { new FileInfo(TbImagePath.Text); } catch(Exception) { return; }
            try { _mainImage = Image.LoadFromFile(TbImagePath.Text); } catch(Exception) { return; }
            _drawAll();
            FiguresCb.Items.Clear();
            FiguresCb.Items.Add("Image");
            FiguresCb.Text = "Image";
            foreach(Figure figure in _mainImage.Figures) {
                FiguresCb.Items.Add($"{_mainImage.Figures.IndexOf(figure)} {figure.GetType().Name}");
            }
        }
        
        private void MergeImage_Btn_Click(object sender, RoutedEventArgs e) {
            TbImagePath.Text = TbImagePath.Text.Trim();
            try { new FileInfo(TbImagePath.Text); } catch(Exception) { return; }
            Image otherImage;
            try { otherImage = Image.LoadFromFile(TbImagePath.Text); } catch(Exception) { return; }
            _mainImage.Merge(otherImage);
            _drawAll();
            foreach(Figure figure in otherImage.Figures) {
                FiguresCb.Items.Add($"{_mainImage.Figures.IndexOf(figure)} {figure.GetType().Name}");
            }
        }
    }
}