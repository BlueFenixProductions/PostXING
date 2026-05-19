using System.ComponentModel;
using System.Deployment.Application;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Genghis.Windows.Forms;
using PostXING.Controls.Navigation;

namespace PostXING.NavigationPages;

public class AboutPage : MainViewPage
{
	private Label label2;

	private HtmlLinkLabel htmlLinkLabel8;

	private HtmlLinkLabel htmlLinkLabel14;

	private HtmlLinkLabel htmlLinkLabel13;

	private HtmlLinkLabel htmlLinkLabel12;

	private HtmlLinkLabel htmlLinkLabel11;

	private HtmlLinkLabel htmlLinkLabel9;

	private HtmlLinkLabel htmlLinkLabel7;

	private HtmlLinkLabel htmlLinkLabel6;

	private HtmlLinkLabel htmlLinkLabel5;

	private HtmlLinkLabel htmlLinkLabel4;

	private HtmlLinkLabel htmlLinkLabel3;

	private HtmlLinkLabel htmlLinkLabel1;

	private Label label3;

	private HtmlLinkLabel htmlLinkLabel10;

	private PostXING.Controls.Navigation.LinkLabel linkLabel1;

	private PictureBox pictureBox2;

	private HtmlLinkLabel htmlLinkLabel15;

	private HtmlLinkLabel htmlLinkLabel16;

	private Label label1;

	private IContainer components;

	public AboutPage()
	{
		InitializeComponent();
		AssemblyCopyrightAttribute assemblyCopyrightAttribute = (AssemblyCopyrightAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), inherit: false)[0];
		label1.Text = assemblyCopyrightAttribute.Copyright;
		if (ApplicationDeployment.IsNetworkDeployed)
		{
			label2.Text = " ClickOnce version " + ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
		}
		else
		{
			label2.Text = " version " + Application.ProductVersion;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostXING.NavigationPages.AboutPage));
		this.label2 = new System.Windows.Forms.Label();
		this.htmlLinkLabel8 = new Genghis.Windows.Forms.HtmlLinkLabel();
		this.htmlLinkLabel14 = new Genghis.Windows.Forms.HtmlLinkLabel();
		this.htmlLinkLabel13 = new Genghis.Windows.Forms.HtmlLinkLabel();
		this.htmlLinkLabel12 = new Genghis.Windows.Forms.HtmlLinkLabel();
		this.htmlLinkLabel11 = new Genghis.Windows.Forms.HtmlLinkLabel();
		this.htmlLinkLabel9 = new Genghis.Windows.Forms.HtmlLinkLabel();
		this.htmlLinkLabel7 = new Genghis.Windows.Forms.HtmlLinkLabel();
		this.htmlLinkLabel6 = new Genghis.Windows.Forms.HtmlLinkLabel();
		this.htmlLinkLabel5 = new Genghis.Windows.Forms.HtmlLinkLabel();
		this.htmlLinkLabel4 = new Genghis.Windows.Forms.HtmlLinkLabel();
		this.htmlLinkLabel3 = new Genghis.Windows.Forms.HtmlLinkLabel();
		this.htmlLinkLabel1 = new Genghis.Windows.Forms.HtmlLinkLabel();
		this.label3 = new System.Windows.Forms.Label();
		this.htmlLinkLabel10 = new Genghis.Windows.Forms.HtmlLinkLabel();
		this.linkLabel1 = new PostXING.Controls.Navigation.LinkLabel();
		this.pictureBox2 = new System.Windows.Forms.PictureBox();
		this.htmlLinkLabel15 = new Genghis.Windows.Forms.HtmlLinkLabel();
		this.htmlLinkLabel16 = new Genghis.Windows.Forms.HtmlLinkLabel();
		this.label1 = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.pictureBox2).BeginInit();
		base.SuspendLayout();
		this.label2.Location = new System.Drawing.Point(64, 40);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(208, 23);
		this.label2.TabIndex = 18;
		this.label2.Text = "label2";
		this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.htmlLinkLabel8.Location = new System.Drawing.Point(16, 72);
		this.htmlLinkLabel8.Name = "htmlLinkLabel8";
		this.htmlLinkLabel8.Size = new System.Drawing.Size(312, 23);
		this.htmlLinkLabel8.TabIndex = 17;
		this.htmlLinkLabel8.Text = "By Chris Frazier (<a href=\"http://bluefenix.net\">http://bluefenix.net</a>)";
		this.htmlLinkLabel14.Location = new System.Drawing.Point(24, 344);
		this.htmlLinkLabel14.Name = "htmlLinkLabel14";
		this.htmlLinkLabel14.Size = new System.Drawing.Size(296, 23);
		this.htmlLinkLabel14.TabIndex = 32;
		this.htmlLinkLabel14.Text = "RssBandit ( <a href=\"http://rssbandit.org\">http://rssbandit.org</a> )";
		this.htmlLinkLabel13.Location = new System.Drawing.Point(24, 320);
		this.htmlLinkLabel13.Name = "htmlLinkLabel13";
		this.htmlLinkLabel13.Size = new System.Drawing.Size(296, 23);
		this.htmlLinkLabel13.TabIndex = 31;
		this.htmlLinkLabel13.Text = "Joel Ross ( <a href=\"http://www.rosscode.com\">http://www.rosscode.com</a> )";
		this.htmlLinkLabel12.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.htmlLinkLabel12.Location = new System.Drawing.Point(320, 184);
		this.htmlLinkLabel12.Name = "htmlLinkLabel12";
		this.htmlLinkLabel12.Size = new System.Drawing.Size(216, 48);
		this.htmlLinkLabel12.TabIndex = 30;
		this.htmlLinkLabel12.Text = "This Application is Extensible: ( <a href=\"http://weblogs.asp.net/rosherove/archive/2003/11/24/39484.aspx\">http://weblogs.asp.net/rosherove/archive/2003/11/24/39484.aspx</a> )";
		this.htmlLinkLabel11.Location = new System.Drawing.Point(24, 296);
		this.htmlLinkLabel11.Name = "htmlLinkLabel11";
		this.htmlLinkLabel11.Size = new System.Drawing.Size(296, 23);
		this.htmlLinkLabel11.TabIndex = 29;
		this.htmlLinkLabel11.Text = "Roy Osherove ( <a href=\"http://iserializable.com\">http://iserializable.com</a> )";
		this.htmlLinkLabel9.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.htmlLinkLabel9.Location = new System.Drawing.Point(320, 280);
		this.htmlLinkLabel9.Name = "htmlLinkLabel9";
		this.htmlLinkLabel9.Size = new System.Drawing.Size(208, 56);
		this.htmlLinkLabel9.TabIndex = 27;
		this.htmlLinkLabel9.Text = "FTP by Enterprise Distributed Technologies ( <a href=\"http://www.enterprisedt.com/products/edtftpnet/overview.html\">http://www.enterprisedt.com/products/edtftpnet/overview.html</a> )";
		this.htmlLinkLabel7.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.htmlLinkLabel7.Location = new System.Drawing.Point(320, 232);
		this.htmlLinkLabel7.Name = "htmlLinkLabel7";
		this.htmlLinkLabel7.Size = new System.Drawing.Size(216, 40);
		this.htmlLinkLabel7.TabIndex = 26;
		this.htmlLinkLabel7.Text = "Portions copyright 2002-2004 The Genghis Group (<a href=\"http://www.genghisgroup.com/\">http://www.genghisgroup.com/</a>). ";
		this.htmlLinkLabel6.Location = new System.Drawing.Point(24, 248);
		this.htmlLinkLabel6.Name = "htmlLinkLabel6";
		this.htmlLinkLabel6.Size = new System.Drawing.Size(296, 23);
		this.htmlLinkLabel6.TabIndex = 25;
		this.htmlLinkLabel6.Text = "Simon Mourier ( <a href=\"http://blogs.msdn.com/smourier\">http://blogs.msdn.com/smourier</a> )";
		this.htmlLinkLabel5.Location = new System.Drawing.Point(24, 224);
		this.htmlLinkLabel5.Name = "htmlLinkLabel5";
		this.htmlLinkLabel5.Size = new System.Drawing.Size(296, 23);
		this.htmlLinkLabel5.TabIndex = 24;
		this.htmlLinkLabel5.Text = "Nikhil Kothari ( <a href=\"http://www.nikhilk.net\">http://www.nikhilk.net</a> )";
		this.htmlLinkLabel4.Location = new System.Drawing.Point(24, 152);
		this.htmlLinkLabel4.Name = "htmlLinkLabel4";
		this.htmlLinkLabel4.Size = new System.Drawing.Size(296, 23);
		this.htmlLinkLabel4.TabIndex = 23;
		this.htmlLinkLabel4.Text = "Scott Watermasysk ( <a href=\"http://scottwater.com\">http://scottwater.com</a> )";
		this.htmlLinkLabel3.Location = new System.Drawing.Point(24, 176);
		this.htmlLinkLabel3.Name = "htmlLinkLabel3";
		this.htmlLinkLabel3.Size = new System.Drawing.Size(296, 23);
		this.htmlLinkLabel3.TabIndex = 22;
		this.htmlLinkLabel3.Text = "Cook Computing ( <a href=\"http://www.xml-rpc.net\">http://www.xml-rpc.net</a>)";
		this.htmlLinkLabel1.Location = new System.Drawing.Point(24, 200);
		this.htmlLinkLabel1.Name = "htmlLinkLabel1";
		this.htmlLinkLabel1.Size = new System.Drawing.Size(232, 23);
		this.htmlLinkLabel1.TabIndex = 20;
		this.htmlLinkLabel1.Text = "Mick Doherty ( <a href=\"http://dotnetrix.co.uk/\">http://dotnetrix.co.uk/</a> )";
		this.label3.Location = new System.Drawing.Point(8, 96);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(272, 23);
		this.label3.TabIndex = 19;
		this.label3.Text = "I hacked this thing together with the help of:";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.htmlLinkLabel10.Location = new System.Drawing.Point(24, 272);
		this.htmlLinkLabel10.Name = "htmlLinkLabel10";
		this.htmlLinkLabel10.Size = new System.Drawing.Size(296, 23);
		this.htmlLinkLabel10.TabIndex = 28;
		this.htmlLinkLabel10.Text = "Wes Haggard ( <a href=\"http://weblogs.asp.net/whaggard\">http://weblogs.asp.net/whaggard</a> )";
		this.linkLabel1.Image = (System.Drawing.Image)resources.GetObject("linkLabel1.Image");
		this.linkLabel1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.linkLabel1.Location = new System.Drawing.Point(416, 360);
		this.linkLabel1.Name = "linkLabel1";
		this.linkLabel1.Size = new System.Drawing.Size(104, 23);
		this.linkLabel1.TabIndex = 33;
		this.linkLabel1.TabStop = true;
		this.linkLabel1.Text = "Back to Editing!";
		this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel1_LinkClicked);
		this.pictureBox2.Image = (System.Drawing.Image)resources.GetObject("pictureBox2.Image");
		this.pictureBox2.Location = new System.Drawing.Point(336, 0);
		this.pictureBox2.Name = "pictureBox2";
		this.pictureBox2.Size = new System.Drawing.Size(184, 184);
		this.pictureBox2.TabIndex = 34;
		this.pictureBox2.TabStop = false;
		this.htmlLinkLabel15.Font = new System.Drawing.Font("Tahoma", 20.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.htmlLinkLabel15.Location = new System.Drawing.Point(91, 9);
		this.htmlLinkLabel15.Name = "htmlLinkLabel15";
		this.htmlLinkLabel15.Size = new System.Drawing.Size(150, 28);
		this.htmlLinkLabel15.TabIndex = 35;
		this.htmlLinkLabel15.Text = "<a href=\"http://www.postxing.net/\">PostXING</a>";
		this.htmlLinkLabel16.Location = new System.Drawing.Point(24, 127);
		this.htmlLinkLabel16.Name = "htmlLinkLabel16";
		this.htmlLinkLabel16.Size = new System.Drawing.Size(296, 23);
		this.htmlLinkLabel16.TabIndex = 36;
		this.htmlLinkLabel16.Text = "Tomer Gabel ( <a href=\"http://www.tomergabel.com\">http://www.tomergabel.com</a> )";
		this.label1.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(22, 368);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(208, 16);
		this.label1.TabIndex = 37;
		this.label1.Text = "label1";
		this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.BackColor = System.Drawing.SystemColors.Window;
		base.Controls.Add(this.label1);
		base.Controls.Add(this.htmlLinkLabel16);
		base.Controls.Add(this.htmlLinkLabel15);
		base.Controls.Add(this.linkLabel1);
		base.Controls.Add(this.htmlLinkLabel14);
		base.Controls.Add(this.htmlLinkLabel13);
		base.Controls.Add(this.htmlLinkLabel12);
		base.Controls.Add(this.htmlLinkLabel11);
		base.Controls.Add(this.htmlLinkLabel9);
		base.Controls.Add(this.htmlLinkLabel7);
		base.Controls.Add(this.htmlLinkLabel6);
		base.Controls.Add(this.htmlLinkLabel5);
		base.Controls.Add(this.htmlLinkLabel4);
		base.Controls.Add(this.htmlLinkLabel3);
		base.Controls.Add(this.htmlLinkLabel1);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.htmlLinkLabel10);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.htmlLinkLabel8);
		base.Controls.Add(this.pictureBox2);
		base.Name = "AboutPage";
		((System.ComponentModel.ISupportInitialize)this.pictureBox2).EndInit();
		base.ResumeLayout(false);
	}

	private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		base.Frame.GoHome();
	}
}
