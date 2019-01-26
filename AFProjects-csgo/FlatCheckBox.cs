using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AFProjects_csgo
{
    public partial class FlatCheckbox : Control
    {
        public bool m_checked { get; set; }
        public Bitmap m_checked_img { get; set; }
        public Bitmap m_notchecked_img { get; set; }

        public FlatCheckbox()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            BackgroundImageLayout = ImageLayout.Zoom;
            BackgroundImage = m_checked ? m_checked_img : m_notchecked_img;
            base.OnPaint(pe);
        }

        protected override void OnClick(EventArgs e)
        {
            m_checked = m_checked ? false : true;
            BackgroundImage = m_checked ? m_checked_img : m_notchecked_img;
            base.OnClick(e);
        }
    }
}
