//-----------------------------------------------------------------------
// <copyright company="Open Text Corporation">
//
// Copyright © 2019 Open Text Corporation.  All rights reserved.
//
// </copyright>
//-----------------------------------------------------------------------

namespace Custom.InputAccel.UimScript
{
    using System;
    using System.Windows;
    using Emc.InputAccel.CaptureClient;

    /// <summary>
    /// Interaction logic for NamedQueryLookup.xaml
    /// </summary>
    public partial class NamedQueryLookup : Window
    {
        private ITableResults _dataTbleResults;
        private ITableRow _selection;
        private int _key, _field1, _field2, _field3;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dataContext">Document data context.</param>
        public NamedQueryLookup(ITableResults results, string key, string field1=null, string field2=null, string field3=null)
        {
            _dataTbleResults = results;
            _key = results.GetFieldIndex(key);
            _field1 = (field1 != null) ? results.GetFieldIndex(field1) : -1;
            _field2 = (field2 != null) ? results.GetFieldIndex(field2) : -1;
            _field3 = (field3 != null) ? results.GetFieldIndex(field3) : -1;
            InitializeComponent();
            InitLables();
            PopulateList();
            ButtonOK.IsEnabled = ListBoxItems.Items.Count == 0 ? false : true;
        }

        /// <summary>
        /// Selected row. Null if no row is selected.
        /// </summary>
        internal ITableRow Selection
        {
            get { return _selection; }
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            ItemSelected();
            DialogResult = true;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void PopulateList()
        {
            foreach (ITableRow row in _dataTbleResults)
            {
                ListBoxItems.Items.Add( RowToString(row) );
            }
            ListBoxItems.SelectedIndex = 0;
            ListBoxItems.Focus();
        }

        private string RowToString(ITableRow row)
        {
            string s = row.AsString(_key);
            string option;

            option = (_field1 >= 0) ? row.AsString(_field1) : "";
            option = (_field2 >= 0) ? option + ", " + row.AsString(_field2) : option;
            option = (_field3 >= 0) ? option + ", " + row.AsString(_field3) : option;

            if (option.Length > 0)
            {
                s = s + " (" + option + ")";
            }
            return s;
        }

        private void ListBoxItems_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void ListBoxItems_SelectionChanged_1(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void ItemSelected()
        {
            if (ListBoxItems.SelectedIndex >= 0)
            {
                string selection = ListBoxItems.SelectedItem as string;
                selection = selection.Trim();
                if (selection.Length > 0)
                {
                    foreach (ITableRow row in _dataTbleResults)
                    {
                        if (RowToString(row).Equals(selection, StringComparison.InvariantCultureIgnoreCase))
                        {
                            _selection = row;
                        }
                    }
                }
            }
        }

        private void ListBoxItem_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ItemSelected();
            DialogResult = true;
        }

        private void InitLables()
        {
            Title = Properties.Resources.Label_SelectionLookup;
            ButtonOK.Content = Properties.Resources.ButtonOkCaption;
            ButtonCancel.Content = Properties.Resources.ButtonCancelCaption;
        }
    }
}
