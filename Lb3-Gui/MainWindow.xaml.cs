using System;
using System.Windows;
using Lib;

namespace Lb3_Gui {
    public partial class MainWindow {
        private Image _mainImage;
        private Figure _selectedFigure {
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
        }
        
        private void _drawAll() {
            MainCanvas.Children.Clear();
            foreach (Figure figure in _mainImage.Figures) {
                figure.Draw(MainCanvas);
            }
        }
        
        private void AddFigure_Btn_Click(Object sender, RoutedEventArgs e) {
            Figure figure = Figure.Get(NewFiguresCb.Text);
            _mainImage.AddFigure(figure);
            _drawAll();
            FiguresCb.Items.Add($"{_mainImage.Figures.IndexOf(figure)} {figure.GetType().Name}");
        }
        
        private void FigureInfo_Btn_Click(Object sender, RoutedEventArgs e) {
            MessageBox.Show(_selectedFigure.ToInfoString());
        }
        
        private void MoveTo_Btn_Click(Object sender, RoutedEventArgs e) {
            _selectedFigure.MoveTo(double.Parse(TbMoveToX.Text), Double.Parse(TbMoveToY.Text));
            _drawAll();
        }
        
        private void Move_Btn_Click(Object sender, RoutedEventArgs e) {
            _selectedFigure.Move(double.Parse(TbMoveX.Text), Double.Parse(TbMoveY.Text));
            _drawAll();
        }
        
        private void SetScale_Btn_Click(Object sender, RoutedEventArgs e) {
            _selectedFigure.Scale = double.Parse(TbScale.Text);
            _drawAll();
        }
        
        private void SaveImage_Btn_Click(Object sender, RoutedEventArgs e) {
            _mainImage.SaveToFile(TbImagePath.Text);
            _drawAll();
        }
        
        private void LoadImage_Btn_Click(Object sender, RoutedEventArgs e) {
            _mainImage = Image.LoadFromFile(TbImagePath.Text);
            _drawAll();
        }
        
        private void MergeImage_Btn_Click(Object sender, RoutedEventArgs e) {
            Image otherImage = Image.LoadFromFile(TbImagePath.Text);
            _mainImage.Merge(otherImage);
            _drawAll();
            foreach(Figure figure in otherImage.Figures) {
                FiguresCb.Items.Add($"{_mainImage.Figures.IndexOf(figure)} {figure.GetType().Name}");
            }
        }
    }
}