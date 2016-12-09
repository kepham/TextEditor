﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
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

namespace TextEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DocumentManager _documentManager;
        private readonly PrintManager _printManager;
        public MainWindow()
        {
            InitializeComponent();

            _documentManager = new DocumentManager(body);
            _printManager = new PrintManager(body);
            if (_documentManager.OpenDocument())
                status.Text = "Document loaded.";
        }

        private void TextEditorToolbar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (toolbar.IsSynchronizing) return;
            ComboBox source = e.OriginalSource as ComboBox;
            if (source == null) return;
            switch (source.Name)
            {
                case "fonts":
                    //change the font face
                    _documentManager.ApplyToSelection(TextBlock.FontFamilyProperty, source.SelectedItem);
                    break;
                case "fontSize":
                    _documentManager.ApplyToSelection(TextBlock.FontSizeProperty, source.SelectedItem);
                    break;
            }
            body.Focus();
        }

        private void body_SelectionChanged(object sender, RoutedEventArgs e)
        {
            toolbar.SynchronizeWith(body.Selection);
        }

        private void NewDocument(object sender, ExecutedRoutedEventArgs e)
        {
            _documentManager.NewDocument();
            status.Text = "New Document";
        }

        private void OpenDocument(object sender, ExecutedRoutedEventArgs e)
        {
            if (_documentManager.OpenDocument())
                status.Text = "Document loaded.";
        }
        private void SaveDocument(object sender, ExecutedRoutedEventArgs e)
        {
            if (_documentManager.SaveDocument())
                status.Text = "Document saved.";
        }
        private void SaveDocumentAs(object sender,ExecutedRoutedEventArgs e)
        {
            if (_documentManager.SaveDocumentAs())
                status.Text = "Document saved.";
        }
        private void ApplicationClose(object sender,ExecutedRoutedEventArgs e)
        {
            Close();
        }


        private void SaveDocument_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _documentManager.CanSaveDocument();
        }

        private void PrintDocument(object sender, ExecutedRoutedEventArgs e)
        {
            if (_printManager.Print())
                status.Text = "Document sent to printer.";
        }

        private void PrintPreview(object sender, ExecutedRoutedEventArgs e)
        {
            _printManager.PrintPreview();
        }
    }
}
