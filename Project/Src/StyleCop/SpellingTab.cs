//-----------------------------------------------------------------------
// <copyright file="SpellingTab.cs">
//   MS-PL
// </copyright>
// <license>
//   This source code is subject to terms and conditions of the Microsoft 
//   Public License. A copy of the license can be found in the License.html 
//   file at the root of this distribution. If you cannot locate the  
//   Microsoft Public License, please send an email to dlr@microsoft.com. 
//   By using this source code in any fashion, you are agreeing to be bound 
//   by the terms of the Microsoft Public License. You must not remove this 
//   notice, or any other, from this software.
// </license>
//-----------------------------------------------------------------------
namespace StyleCop
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// Options dialog to manage words for spelling.
    /// </summary>
    internal class SpellingTab : UserControl, IPropertyControlPage
    {
        private const string RecognizedWordsPropertyName = "RecognizedWords";

        private const string DeprecatedWordsPropertyName = "DeprecatedWords";

        #region Private Fields

        /// <summary>
        /// The Remove button.
        /// </summary>
        private Button removeRecognizedWordButton;

        /// <summary>
        /// The Add button.
        /// </summary>
        private Button addRecognizedWordButton;

        /// <summary>
        /// The current words box.
        /// </summary>
        private ListView recognizedWordsListView;

        /// <summary>
        /// The static text label.
        /// </summary>
        private Label label2;

        /// <summary>
        /// The add prefix box.
        /// </summary>
        private TextBox addRecognizedWordTextBox;

        /// <summary>
        /// The static text label.
        /// </summary>
        private Label label1;

        /// <summary>
        /// True if the page is dirty.
        /// </summary>
        private bool dirty;

        /// <summary>
        /// The default column on the ListView control.
        /// </summary>
        private ColumnHeader recognizedWordsColumnHeader;

        /// <summary>
        /// The tab control which hosts this page.
        /// </summary>
        private PropertyControl tabControl;

        /// <summary>
        /// Contains help text.
        /// </summary>
        private Label label3;
        private Label label4;
        private Button addDeprecatedWordButton;
        private TextBox addDeprecatedWordTextBox;
        private Label label5;
        private TextBox addAlternateWordTextBox;
        private Label label6;
        private ListView deprecatedWordsListView;
        private ColumnHeader deprecatedWordsColumnHeader;
        private Button removeDeprecatedWordButton;
        private Label label7;
        
        /// <summary>
        /// Stores the form's accept button while focus is on the add textbox.
        /// </summary>
        private IButtonControl formAcceptButton;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the SpellingTab class.
        /// </summary>
        public SpellingTab()
        {
            this.InitializeComponent();
        }
        
        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets the name of the the tab.
        /// </summary>
        public string TabName
        {
            get 
            { 
                return Strings.SpellingTab; 
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether any data on the page is dirty.
        /// </summary>
        public bool Dirty
        {
            get 
            { 
                return this.dirty; 
            }

            set
            {
                Param.Ignore(value); 
                
                if (this.dirty != value)
                {
                    this.dirty = value;
                    this.tabControl.DirtyChanged();
                }
            }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Initializes the page.
        /// </summary>
        /// <param name="propertyControl">The tab control object.</param>
        public void Initialize(PropertyControl propertyControl)
        {
            Param.AssertNotNull(propertyControl, "propertyControl");

            this.tabControl = propertyControl;
            
            // Get the list of allowed words from the parent settings.
            this.AddParentWords();

            // Get the list of allowed words from the local settings.
            CollectionProperty recognizedWordsProperty = this.tabControl.LocalSettings.GlobalSettings.GetProperty(RecognizedWordsPropertyName) as CollectionProperty;

            if (recognizedWordsProperty != null && recognizedWordsProperty.Values.Count > 0)
            {
                foreach (string value in recognizedWordsProperty)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        ListViewItem item = this.recognizedWordsListView.Items.Add(value);
                        item.Tag = true;
                        this.SetBoldState(item, this.recognizedWordsListView);
                    }
                }
            }
            
            // Select the first item in the list.
            if (this.recognizedWordsListView.Items.Count > 0)
            {
                this.recognizedWordsListView.Items[0].Selected = true;
            }

            // Get the list of deprecated words from the local settings.
            CollectionProperty deprecatedWordsProperty = this.tabControl.LocalSettings.GlobalSettings.GetProperty(DeprecatedWordsPropertyName) as CollectionProperty;

            if (deprecatedWordsProperty != null && deprecatedWordsProperty.Values.Count > 0)
            {
                foreach (string value in deprecatedWordsProperty)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        var valueParts = value.Split(',');
                        if (valueParts.Length == 2)
                        {
                            ListViewItem item = this.deprecatedWordsListView.Items.Add(valueParts[0].Trim() + ", " + valueParts[1].Trim());
                            item.Tag = true;
                            this.SetBoldState(item, this.deprecatedWordsListView);
                        }
                    }
                }
            }

            // Select the first item in the list.
            if (this.deprecatedWordsListView.Items.Count > 0)
            {
                this.deprecatedWordsListView.Items[0].Selected = true;
            }

            this.EnableDisableRemoveButtons();

            this.dirty = false;
            this.tabControl.DirtyChanged();
        }

        /// <summary>
        /// Called before all pages are applied.
        /// </summary>
        /// <returns>Returns false if no pages should be applied.</returns>
        public bool PreApply()
        {
            return true;
        }

        /// <summary>
        /// Called after all pages have been applied.
        /// </summary>
        /// <param name="wasDirty">The dirty state of the page before it was applied.</param>
        public void PostApply(bool wasDirty)
        {
            Param.Ignore(wasDirty);
        }

        /// <summary>
        /// Saves the data and clears the dirty flag.
        /// </summary>
        /// <returns>Returns true if the data is saved, false if not.</returns>
        public bool Apply()
        {
            List<string> values = new List<string>(this.recognizedWordsListView.Items.Count);

            foreach (ListViewItem word in this.recognizedWordsListView.Items)
            {
                // Only save local tags.
                if ((bool)word.Tag)
                {
                    values.Add(word.Text);
                }
            }

            this.tabControl.LocalSettings.GlobalSettings.SetProperty(new CollectionProperty(this.tabControl.Core, RecognizedWordsPropertyName, values));

            values = new List<string>(this.deprecatedWordsListView.Items.Count);

            foreach (ListViewItem word in this.deprecatedWordsListView.Items)
            {
                // Only save local tags.
                if ((bool)word.Tag)
                {
                    values.Add(word.Text);
                }
            }

            this.tabControl.LocalSettings.GlobalSettings.SetProperty(new CollectionProperty(this.tabControl.Core, DeprecatedWordsPropertyName, values));
  
            this.dirty = false;
            this.tabControl.DirtyChanged();

            return true;
        }

        /// <summary>
        /// Called when the page is activated.
        /// </summary>
        /// <param name="activated">Indicates whether the page is being activated or deactivated.</param>
        public void Activate(bool activated)
        {
            Param.Ignore(activated);
        }

        /// <summary>
        /// Refreshes the bold state of items on the page.
        /// </summary>
        public void RefreshSettingsOverrideState()
        {
            // Loop through the existing items and remove all parent items.
            List<ListViewItem> itemsToRemove = new List<ListViewItem>();
            foreach (ListViewItem prefix in this.recognizedWordsListView.Items)
            {
                if (!(bool)prefix.Tag)
                {
                    itemsToRemove.Add(prefix);
                }
            }

            foreach (ListViewItem itemToRemove in itemsToRemove)
            {
                this.recognizedWordsListView.Items.Remove(itemToRemove);
            }

            // Loop through the existing items and remove all parent items.
            itemsToRemove = new List<ListViewItem>();
            foreach (ListViewItem listViewItem in this.deprecatedWordsListView.Items)
            {
                if (!(bool)listViewItem.Tag)
                {
                    itemsToRemove.Add(listViewItem);
                }
            }

            foreach (ListViewItem itemToRemove in itemsToRemove)
            {
                this.deprecatedWordsListView.Items.Remove(itemToRemove);
            }
           
            // Add any new parent items now.
            this.AddParentWords();

            // Loop through the list again and set the bold state for locally added items.
            foreach (ListViewItem listViewItem in this.recognizedWordsListView.Items)
            {
                if ((bool)listViewItem.Tag)
                {
                    this.SetBoldState(listViewItem, this.recognizedWordsListView);
                }
            }

            // Loop through the list again and set the bold state for locally added items.
            foreach (ListViewItem listViewItem in this.deprecatedWordsListView.Items)
            {
                if ((bool)listViewItem.Tag)
                {
                    this.SetBoldState(listViewItem, this.deprecatedWordsListView);
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpellingTab));
            this.removeRecognizedWordButton = new System.Windows.Forms.Button();
            this.addRecognizedWordButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.addRecognizedWordTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.recognizedWordsListView = new System.Windows.Forms.ListView();
            this.recognizedWordsColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.addDeprecatedWordButton = new System.Windows.Forms.Button();
            this.addDeprecatedWordTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.addAlternateWordTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.deprecatedWordsListView = new System.Windows.Forms.ListView();
            this.deprecatedWordsColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.removeDeprecatedWordButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // removeRecognizedWordButton
            // 
            resources.ApplyResources(this.removeRecognizedWordButton, "removeRecognizedWordButton");
            this.removeRecognizedWordButton.Name = "removeRecognizedWordButton";
            this.removeRecognizedWordButton.Click += new System.EventHandler(this.RemoveRecognizedWordButtonClick);
            // 
            // addRecognizedWordButton
            // 
            resources.ApplyResources(this.addRecognizedWordButton, "addRecognizedWordButton");
            this.addRecognizedWordButton.Name = "addRecognizedWordButton";
            this.addRecognizedWordButton.Click += new System.EventHandler(this.AddRecognizedWordButtonClick);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // addRecognizedWordTextBox
            // 
            resources.ApplyResources(this.addRecognizedWordTextBox, "addRecognizedWordTextBox");
            this.addRecognizedWordTextBox.Name = "addRecognizedWordTextBox";
            this.addRecognizedWordTextBox.GotFocus += new System.EventHandler(this.AddWordGotFocus);
            this.addRecognizedWordTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddRecognizedWordKeyDown);
            this.addRecognizedWordTextBox.LostFocus += new System.EventHandler(this.AddWordLostFocus);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // recognizedWordsListView
            // 
            resources.ApplyResources(this.recognizedWordsListView, "recognizedWordsListView");
            this.recognizedWordsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.recognizedWordsColumnHeader});
            this.recognizedWordsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.recognizedWordsListView.HideSelection = false;
            this.recognizedWordsListView.MultiSelect = false;
            this.recognizedWordsListView.Name = "recognizedWordsListView";
            this.recognizedWordsListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.recognizedWordsListView.UseCompatibleStateImageBehavior = false;
            this.recognizedWordsListView.View = System.Windows.Forms.View.Details;
            this.recognizedWordsListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.WordListItemSelectionChanged);
            this.recognizedWordsListView.SizeChanged += new System.EventHandler(this.RecognizedWordsListViewSizeChanged);
            this.recognizedWordsListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RecognizedWordListKeyDown);
            // 
            // recognizedWordsColumnHeader
            // 
            resources.ApplyResources(this.recognizedWordsColumnHeader, "recognizedWordsColumnHeader");
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // addDeprecatedWordButton
            // 
            resources.ApplyResources(this.addDeprecatedWordButton, "addDeprecatedWordButton");
            this.addDeprecatedWordButton.Name = "addDeprecatedWordButton";
            this.addDeprecatedWordButton.Click += new System.EventHandler(this.AddDeprecatedWordButtonClick);
            // 
            // addDeprecatedWordTextBox
            // 
            resources.ApplyResources(this.addDeprecatedWordTextBox, "addDeprecatedWordTextBox");
            this.addDeprecatedWordTextBox.Name = "addDeprecatedWordTextBox";
            this.addDeprecatedWordTextBox.GotFocus += new System.EventHandler(this.AddWordGotFocus);
            this.addDeprecatedWordTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddDeprecatedWordKeyDown);
            this.addDeprecatedWordTextBox.LostFocus += new System.EventHandler(this.AddWordLostFocus);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // addAlternateWordTextBox
            // 
            resources.ApplyResources(this.addAlternateWordTextBox, "addAlternateWordTextBox");
            this.addAlternateWordTextBox.Name = "addAlternateWordTextBox";
            this.addAlternateWordTextBox.GotFocus += new System.EventHandler(this.AddWordGotFocus);
            this.addAlternateWordTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddDeprecatedWordKeyDown);
            this.addAlternateWordTextBox.LostFocus += new System.EventHandler(this.AddWordLostFocus);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // deprecatedWordsListView
            // 
            resources.ApplyResources(this.deprecatedWordsListView, "deprecatedWordsListView");
            this.deprecatedWordsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.deprecatedWordsColumnHeader});
            this.deprecatedWordsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.deprecatedWordsListView.HideSelection = false;
            this.deprecatedWordsListView.MultiSelect = false;
            this.deprecatedWordsListView.Name = "deprecatedWordsListView";
            this.deprecatedWordsListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.deprecatedWordsListView.UseCompatibleStateImageBehavior = false;
            this.deprecatedWordsListView.View = System.Windows.Forms.View.Details;
            this.deprecatedWordsListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.WordListItemSelectionChanged);
            this.deprecatedWordsListView.SizeChanged += new System.EventHandler(this.DeprecatedWordsListViewSizeChanged);
            this.deprecatedWordsListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DeprecatedWordListKeyDown);
            // 
            // deprecatedWordsColumnHeader
            // 
            resources.ApplyResources(this.deprecatedWordsColumnHeader, "deprecatedWordsColumnHeader");
            // 
            // removeDeprecatedWordButton
            // 
            resources.ApplyResources(this.removeDeprecatedWordButton, "removeDeprecatedWordButton");
            this.removeDeprecatedWordButton.Name = "removeDeprecatedWordButton";
            this.removeDeprecatedWordButton.Click += new System.EventHandler(this.RemoveDeprecatedWordButtonClick);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // SpellingTab
            // 
            this.Controls.Add(this.label7);
            this.Controls.Add(this.deprecatedWordsListView);
            this.Controls.Add(this.removeDeprecatedWordButton);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.addAlternateWordTextBox);
            this.Controls.Add(this.addDeprecatedWordButton);
            this.Controls.Add(this.addDeprecatedWordTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.recognizedWordsListView);
            this.Controls.Add(this.removeRecognizedWordButton);
            this.Controls.Add(this.addRecognizedWordButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.addRecognizedWordTextBox);
            this.Controls.Add(this.label1);
            this.Name = "SpellingTab";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// Add prefixes from the parent settings.
        /// </summary>
        private void AddParentWords()
        {
            if (this.tabControl.ParentSettings != null)
            {
                PropertyCollection globalPropertyCollection = this.tabControl.ParentSettings.GlobalSettings;

                CollectionProperty parentProperty = globalPropertyCollection.GetProperty(RecognizedWordsPropertyName) as CollectionProperty;

                if (parentProperty != null)
                {
                    if (parentProperty.Values.Count > 0)
                    {
                        foreach (string value in parentProperty)
                        {
                            if (!string.IsNullOrEmpty(value))
                            {
                                ListViewItem item = this.recognizedWordsListView.Items.Add(value);
                                item.Tag = false;
                            }
                        }
                    }
                }

                parentProperty = globalPropertyCollection.GetProperty(DeprecatedWordsPropertyName) as CollectionProperty;

                if (parentProperty != null)
                {
                    if (parentProperty.Values.Count > 0)
                    {
                        foreach (string value in parentProperty)
                        {
                            if (!string.IsNullOrEmpty(value))
                            {
                                var splitValue = value.Split(',');
                                if (splitValue.Length == 2)
                                {
                                    ListViewItem item = this.deprecatedWordsListView.Items.Add(splitValue[0].Trim() + ", " + splitValue[1].Trim());
                                    item.Tag = false;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets the bold state of the item.
        /// </summary>
        /// <param name="item">The item to set.</param>
        /// <param name="listView">The ListView to use.</param>
        private void SetBoldState(ListViewItem item, ListView listView)
        {
            Param.AssertNotNull(item, "item");

            // Dispose the item's current font if necessary.
            if (!object.Equals(item.Font, listView.Font) && item.Font != null)
            {
                item.Font.Dispose();
            }

            // Create and set the new font.
            if ((bool)item.Tag)
            {
                item.Font = new Font(listView.Font, FontStyle.Bold);
            }
            else
            {
                item.Font = new Font(listView.Font, FontStyle.Regular);
            }
        }

        /// <summary>
        /// Event that is fired when the add button is clicked.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void AddRecognizedWordButtonClick(object sender, System.EventArgs e)
        {
            Param.Ignore(sender, e);

            string recognizedText = this.addRecognizedWordTextBox.Text;

            if (recognizedText.Length == 0 || recognizedText.Length < 2 || recognizedText.Contains(" "))
            {
                AlertDialog.Show(
                    this.tabControl.Core,
                    this, 
                    Strings.EnterValidWord, 
                    Strings.Title, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Exclamation);
                return;
            }

            foreach (ListViewItem item in this.recognizedWordsListView.Items)
            {
                if (item.Text == recognizedText)
                {
                    item.Selected = true;
                    item.EnsureVisible();
                    this.addRecognizedWordTextBox.Clear();
                    return;
                }
            }

            ListViewItem addedItem = this.recognizedWordsListView.Items.Add(recognizedText);
            addedItem.Tag = true;
            addedItem.Selected = true;
            this.recognizedWordsListView.EnsureVisible(addedItem.Index);
            this.SetBoldState(addedItem, this.recognizedWordsListView);

            this.addRecognizedWordTextBox.Clear();
            
            this.dirty = true;
            this.tabControl.DirtyChanged();
        }

        /// <summary>
        /// Event that is fired when the add deprecated word button is clicked.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void AddDeprecatedWordButtonClick(object sender, System.EventArgs e)
        {
            Param.Ignore(sender, e);

            string deprecatedText = this.addDeprecatedWordTextBox.Text;
            string alternateText = this.addAlternateWordTextBox.Text;

            if (deprecatedText.Length == 0 || deprecatedText.Length < 2 ||
                alternateText.Length == 0 || alternateText.Length < 2 ||
                deprecatedText.Contains(" ") || alternateText.Contains(" "))
            {
                AlertDialog.Show(this.tabControl.Core, this, Strings.EnterValidDeprecatedWord, Strings.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string newDeprecatedAndAlternateText = string.Concat(deprecatedText.Trim(), ", ", alternateText.Trim());

            foreach (ListViewItem item in this.deprecatedWordsListView.Items)
            {
                if (item.Text == newDeprecatedAndAlternateText)
                {
                    item.Selected = true;
                    item.EnsureVisible();
                    this.addDeprecatedWordTextBox.Clear();
                    this.addAlternateWordTextBox.Clear();
                    return;
                }
            }

            ListViewItem addedItem = this.deprecatedWordsListView.Items.Add(newDeprecatedAndAlternateText);
            addedItem.Tag = true;
            addedItem.Selected = true;
            this.deprecatedWordsListView.EnsureVisible(addedItem.Index);
            this.SetBoldState(addedItem, this.deprecatedWordsListView);

            this.addDeprecatedWordTextBox.Clear();
            this.addAlternateWordTextBox.Clear();

            this.dirty = true;
            this.tabControl.DirtyChanged();
        }

        /// <summary>
        /// Event that is fired when the remove button is clicked.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void RemoveRecognizedWordButtonClick(object sender, EventArgs e)
        {
            Param.Ignore(sender, e);

            if (this.recognizedWordsListView.SelectedItems.Count > 0)
            {
                int index = this.recognizedWordsListView.SelectedIndices[0];

                this.recognizedWordsListView.Items.RemoveAt(index);
                this.EnableDisableRemoveButtons();

                if (this.recognizedWordsListView.Items.Count > index)
                {
                    this.recognizedWordsListView.Items[index].Selected = true;
                }
                else if (this.recognizedWordsListView.Items.Count > 0)
                {
                    this.recognizedWordsListView.Items[this.recognizedWordsListView.Items.Count - 1].Selected = true;
                }

                this.dirty = true;
                this.tabControl.DirtyChanged();
            }
        }

        /// <summary>
        /// Event that is fired when the remove button is clicked.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void RemoveDeprecatedWordButtonClick(object sender, EventArgs e)
        {
            Param.Ignore(sender, e);

            if (this.deprecatedWordsListView.SelectedItems.Count > 0)
            {
                int index = this.deprecatedWordsListView.SelectedIndices[0];

                this.deprecatedWordsListView.Items.RemoveAt(index);
                this.EnableDisableRemoveButtons();

                if (this.deprecatedWordsListView.Items.Count > index)
                {
                    this.deprecatedWordsListView.Items[index].Selected = true;
                }
                else if (this.deprecatedWordsListView.Items.Count > 0)
                {
                    this.deprecatedWordsListView.Items[this.deprecatedWordsListView.Items.Count - 1].Selected = true;
                }

                this.dirty = true;
                this.tabControl.DirtyChanged();
            }
        }

        /// <summary>
        /// Called when the current selection changes in the ListView.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void WordListItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            Param.Ignore(sender, e);
            this.EnableDisableRemoveButtons();
        }

        /// <summary>
        /// Called when a key is clicked while focus is on the list.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void RecognizedWordListKeyDown(object sender, KeyEventArgs e)
        {
            Param.AssertNotNull(sender, "sender");
            Param.AssertNotNull(e, "e");

            if (e.KeyCode == Keys.Delete)
            {
                if (this.addRecognizedWordTextBox.Text.Length > 0)
                {
                    // Simulate a click of the remove button.
                    this.RemoveRecognizedWordButtonClick(sender, e);
                }
            }
        }

        /// <summary>
        /// Called when a key is clicked while focus is on the list.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void DeprecatedWordListKeyDown(object sender, KeyEventArgs e)
        {
            Param.AssertNotNull(sender, "sender");
            Param.AssertNotNull(e, "e");

            if (e.KeyCode == Keys.Delete)
            {
                if (this.addDeprecatedWordTextBox.Text.Length > 0 && this.addAlternateWordTextBox.Text.Length > 0)
                {
                    // Simulate a click of the remove button.
                    this.RemoveDeprecatedWordButtonClick(sender, e);
                }
            }
        }

        /// <summary>
        /// Sets the enabled state of the remove buttons.
        /// </summary>
        private void EnableDisableRemoveButtons()
        {
            if (this.recognizedWordsListView.SelectedItems.Count > 0)
            {
                // Get the currently selected item.
                ListViewItem selectedItem = this.recognizedWordsListView.SelectedItems[0];
                this.removeRecognizedWordButton.Enabled = (bool)selectedItem.Tag;
            }
            else
            {
                this.removeRecognizedWordButton.Enabled = false;
            }

            if (this.deprecatedWordsListView.SelectedItems.Count > 0)
            {
                // Get the currently selected item.
                ListViewItem selectedItem = this.deprecatedWordsListView.SelectedItems[0];
                this.removeDeprecatedWordButton.Enabled = (bool)selectedItem.Tag;
            }
            else
            {
                this.removeDeprecatedWordButton.Enabled = false;
            }
        }

        /// <summary>
        /// Called when a key is clicked while focus is on the add textbox.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void AddRecognizedWordKeyDown(object sender, KeyEventArgs e)
        {
            Param.AssertNotNull(sender, "sender");
            Param.AssertNotNull(e, "e");

            if (e.KeyCode == Keys.Return)
            {
                if (this.addRecognizedWordTextBox.Text.Length > 0)
                {
                    // Simulate a click of the add button.
                    this.AddRecognizedWordButtonClick(sender, e);
                }
            }
        }

        /// <summary>
        /// Called when a key is clicked while focus is on the add textbox.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void AddDeprecatedWordKeyDown(object sender, KeyEventArgs e)
        {
            Param.AssertNotNull(sender, "sender");
            Param.AssertNotNull(e, "e");

            if (e.KeyCode == Keys.Return)
            {
                if (this.addDeprecatedWordTextBox.Text.Length > 0 && this.addAlternateWordTextBox.Text.Length > 0)
                {
                    // Simulate a click of the add button.
                    this.AddDeprecatedWordButtonClick(sender, e);
                }
            }
        }

        /// <summary>
        /// Called when the add TextBox receives the input focus.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void AddWordGotFocus(object sender, EventArgs e)
        {
            Param.Ignore(sender, e);

            // Save the current form accept button, and then clear it. This will allow
            // the add textbox to capture the return key.
            this.formAcceptButton = this.ParentForm.AcceptButton;
            this.ParentForm.AcceptButton = null;
        }

        /// <summary>
        /// Called when the add TextBox loses the input focus.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void AddWordLostFocus(object sender, EventArgs e)
        {
            Param.Ignore(sender, e);

            // Reset the form accept button now that the add textbox no longer has the input focus.
            if (this.formAcceptButton != null)
            {
                this.ParentForm.AcceptButton = this.formAcceptButton;
                this.formAcceptButton = null;
            }
        }

        /// <summary>
        /// Resizes the column inside the ListView.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void RecognizedWordsListViewSizeChanged(object sender, EventArgs e)
        {
            this.recognizedWordsColumnHeader.Width = this.recognizedWordsListView.Width - 64;
        }

        /// <summary>
        /// Resizes the column inside the ListView.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void DeprecatedWordsListViewSizeChanged(object sender, EventArgs e)
        {
            this.deprecatedWordsColumnHeader.Width = this.deprecatedWordsListView.Width - 64;
        }

        #endregion Private Methods
    }
}