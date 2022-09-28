using System;
using System.Drawing;
using System.Windows.Forms;

namespace 共通UI
{
    public partial class CButton : Button
    {
        private Color _BackColorSaved;
        public Color BackColorSaved
        {
            get
            {
                return (_BackColorSaved);
            }
            set
            {
                _BackColorSaved = value;
            }
        }

        public CButton()
        {
            InitializeComponent();
            this.EnabledChanged += new EventHandler(this.OnEnabledChanged);
            this.BackColor = BackColorSaved;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        private void OnEnabledChanged(object sender, EventArgs e)
        {
            if (this.Enabled)
            {
                this.BackColor = BackColorSaved;


            }
            else
            {
                this.BackColor = SystemColors.Control;

            }

        }
    }
}
