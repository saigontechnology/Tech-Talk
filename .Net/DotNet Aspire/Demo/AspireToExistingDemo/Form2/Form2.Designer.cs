namespace Form2;

partial class Form2
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        btnLoad = new Button();
        gridStore = new DataGridView();
        ((System.ComponentModel.ISupportInitialize)gridStore).BeginInit();
        SuspendLayout();
        // 
        // btnLoad
        // 
        btnLoad.Location = new Point(12, 415);
        btnLoad.Name = "btnLoad";
        btnLoad.Size = new Size(75, 23);
        btnLoad.TabIndex = 0;
        btnLoad.Text = "Load";
        btnLoad.UseVisualStyleBackColor = true;
        // 
        // gridStore
        // 
        gridStore.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        gridStore.Location = new Point(12, 12);
        gridStore.Name = "gridStore";
        gridStore.Size = new Size(776, 397);
        gridStore.TabIndex = 1;
        // 
        // Form2
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(gridStore);
        Controls.Add(btnLoad);
        Name = "Form2";
        Text = "Form2";
        ((System.ComponentModel.ISupportInitialize)gridStore).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private Button btnLoad;
    private DataGridView gridStore;
}
