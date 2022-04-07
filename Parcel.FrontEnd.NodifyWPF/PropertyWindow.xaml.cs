using System;
using System.Collections.Generic;
using System.Windows;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.FrontEnd.NodifyWPF
{
    public partial class PropertyWindow : BaseWindow
    {
        public PropertyWindow(Window owner, ProcessorNode processor)
        {
            using (var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Parcel.FrontEnd.NodifyWPF.PreviewWindows.sql.xshd.xml"))
            {
                using (var reader = new System.Xml.XmlTextReader(stream))
                {
                    SQLSyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(reader, 
                        ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance);
                }
            }

            Processor = processor;
            Owner = owner;
            if (processor is INodeProperty nodeProperty)
                Editors = nodeProperty.Editors; 
            InitializeComponent();
        }

        #region View Properties
        public ProcessorNode Processor { get; }
        public List<PropertyEditor> Editors { get; }
        #endregion
        
        #region Syntax Highlighter

        private IHighlightingDefinition _SQLSyntaxHighlighting;
        public IHighlightingDefinition SQLSyntaxHighlighting
        {
            get => _SQLSyntaxHighlighting;
            set => SetField(ref _SQLSyntaxHighlighting, value);
        }
        #endregion

        #region Events
        private void AvalonEditor_OnInitialized(object? sender, EventArgs e)
        {
            TextEditor editor = sender as TextEditor;
            PropertyEditor property = editor!.DataContext as PropertyEditor;

            editor.Text = property!.Binding as string;
        }
        private void AvalonEditor_OnTextChanged(object? sender, EventArgs e)
        {
            TextEditor editor = sender as TextEditor;
            PropertyEditor property = editor!.DataContext as PropertyEditor;
            
            property!.Binding  = editor.Text;
        }
        #endregion
    }
}