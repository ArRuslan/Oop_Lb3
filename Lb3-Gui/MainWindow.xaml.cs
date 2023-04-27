using System;
using System.Diagnostics;
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
        
        private void AddFigure_Btn_Click(Object sender, RoutedEventArgs e) {
            Figure figure = Figure.Get(NewFiguresCb.Text);
            _mainImage.AddFigure(figure);
            FiguresCb.Items.Add($"{_mainImage.Figures.IndexOf(figure)} {figure.GetType().Name}");
        }
        
        private void FigureInfo_Btn_Click(Object sender, RoutedEventArgs e) {
            MessageBox.Show(_selectedFigure.ToInfoString());
        }
    }
}