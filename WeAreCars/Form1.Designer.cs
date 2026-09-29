namespace WeAreCars
{
    partial class Dashboard
    {
        private System.ComponentModel.IContainer components = null;    // designer variable.
        protected override void Dispose(bool disposing)   // Removes used resources.
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        #region Windows Form Designer generated code

        //Used by the designer to set up the form’s UI automatically.
        //You normally don’t edit this manually.
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "Dashboard";
        }

        #endregion
    }
}

