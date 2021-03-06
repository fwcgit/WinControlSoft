﻿using ControlSoft.src.bean;
using ControlSoft.src.config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlSoft.UI
{


    public partial class SoftMonitoring : Form
    {


        [DllImport("Shell32.dll")]
        public static extern int ExtractIcon(IntPtr h, string strx, int ii);

        public SoftList softList;

        SoftMonitoring softMonitoring;
        public SoftMonitoring()
        {
            InitializeComponent();
            softMonitoring = this;
        }

        private void SoftMonitoring_Load(object sender, EventArgs e)
        {

             
             InitList();

             this.listView2.Columns.Add("进程名", listView2.Width-5, HorizontalAlignment.Center); //一步添加

             softList = AppConfig.appConfig.getSoftMonitoring();

             listView2.BeginUpdate();
             foreach (SoftBean soft in softList.softs)
            {
                ListViewItem lvi = new ListViewItem();

                lvi.Text = soft.name;
                listView2.Items.Add(lvi);
               
            }

            listView2.EndUpdate();
        }

        public void InitList()
        {
            listView1.Clear();

            this.listView1.Columns.Add("进程名", 180, HorizontalAlignment.Center); //一步添加
            this.listView1.Columns.Add("ID", 100, HorizontalAlignment.Center); //
            this.listView1.Columns.Add("路径", 200, HorizontalAlignment.Center); //一步添加

            listView1.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度

            Process[] pros = Process.GetProcesses();
            ImageList imageList = new ImageList();
            for (int i = 0; i < pros.Length; i++)
            {

                Process pro = pros[i];

                ListViewItem lvi = new ListViewItem();

                lvi.ImageIndex = i;     //通过与imageList绑定，显示imageList中第i项图标

                imageList.Images.Add(i+"",this.Icon);

                try
                {

                    string fileName = pro.MainModule.FileName;
                    if(!(null == fileName || fileName == ""))
                    {
                        lvi.Text = pro.ProcessName;
                        lvi.SubItems.Add(pro.Id + "");
                        lvi.SubItems.Add(fileName);
                        Icon icon = GetFileIcon(fileName);

                        if (null != icon)
                        {
                            imageList.Images.RemoveAt(i);
                            imageList.Images.Add(i+"",GetFileIcon(pro.MainModule.FileName));
                        }
                       
                        this.listView1.Items.Add(lvi);
                    }
                    
                }
                catch (Exception)
                {
                    //Acess denied 
                   
                }

            }

            listView1.SmallImageList = imageList;
            listView1.EndUpdate();  //结束数据处理，UI界面一次性绘制。

        }

        private Icon GetFileIcon(String fileName)
        {
            Icon icon = null;
            try
            {
                IntPtr ptr = (IntPtr)ExtractIcon(this.Handle, fileName, 0);

                if(!ptr.Equals(null))
                {
                    icon = Icon.FromHandle(ptr);
                    return icon;
                }
            }
            catch(Exception)
            {
                
            }

            return icon ;
        }

        
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                ContextMenuStrip menu = new AddOrganizationMenu(this,listView1,listView2);      //此处为下一步中创建的菜单  
                menu.Show(listView1, new Point(e.X, e.Y));



            }
            else if (e.Button == MouseButtons.Left)
            {
                
            }
        }

        private void listView2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                ContextMenuStrip menu = new AddOrganizationMenu(this, listView2);      //此处为下一步中创建的菜单  
                menu.Show(listView2, new Point(e.X, e.Y));



            }
            else if (e.Button == MouseButtons.Left)
            {

            }
        }
    }

    class AddOrganizationMenu : ContextMenuStrip
    {
        private ListView listView,selectListView;
        private SoftMonitoring softMonitoring;
        public AddOrganizationMenu(SoftMonitoring softMonitoring, ListView listView,ListView selectListView)
        {
            this.softMonitoring = softMonitoring;
            this.listView = listView;
            this.selectListView = selectListView;

            ToolStripDropDownItem tsi1 = (ToolStripDropDownItem)Items.Add("添加监控");                  //添加第一级菜单  
            ToolStripDropDownItem tsi2 = (ToolStripDropDownItem)Items.Add("打开程序位置");                  //添加第一级菜单  
            ToolStripDropDownItem tsi3 = (ToolStripDropDownItem)Items.Add("刷新");                  //添加第一级菜单  

            tsi1.Click += addMonitoring;
            tsi2.Click += OpenSoftLocation;
            tsi3.Click += RefreshList;
        }

        public AddOrganizationMenu(SoftMonitoring softMonitoring,ListView selectListView)
        {
            this.softMonitoring = softMonitoring;
            this.selectListView = selectListView;

            ToolStripDropDownItem tsi1 = (ToolStripDropDownItem)Items.Add("移除");                  //添加第一级菜单  
            tsi1.Click += DelMonitoring;
          
        }

        private void DelMonitoring(object sender, EventArgs e)
        {
            if (selectListView.SelectedItems[0].SubItems.Count >= 1)
            {
                string name = selectListView.SelectedItems[0].SubItems[0].Text;
                SoftDel(name);
                selectListView.SelectedItems[0].Remove();
                selectListView.Update();

                string json = JsonConvert.SerializeObject(softMonitoring.softList);
                AppConfig.appConfig.saveSoftMonitoring(json);
            }
        }

        private void addMonitoring(object sender, EventArgs e)
        {

            if (listView.SelectedItems[0].SubItems.Count >= 3)
            {
                string name = listView.SelectedItems[0].SubItems[0].Text;
                string path = listView.SelectedItems[0].SubItems[2].Text;

                if (SoftExist(name))
                {
                    MessageBox.Show("已经存在!");
                    return;
                }

                SoftBean soft = new SoftBean(name, path);
                softMonitoring.softList.softs.Add(soft);

                string json = JsonConvert.SerializeObject(softMonitoring.softList);
                AppConfig.appConfig.saveSoftMonitoring(json);

                AddSoftListView(name);
            }

        }

        private void OpenSoftLocation(object sender, EventArgs e)
        {

            if(listView.SelectedItems[0].SubItems.Count >= 3)
            {
                string path = listView.SelectedItems[0].SubItems[2].Text;
                if (!(null == path || path == ""))
                {
                    System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
                    psi.Arguments = "/e,/select," + listView.SelectedItems[0].SubItems[2].Text;
                    System.Diagnostics.Process.Start(psi);

                }

            }

        }

        private void RefreshList(object sender, EventArgs e)
        {
            softMonitoring.InitList();
        }

        private bool SoftExist(String name)
        {
            foreach(SoftBean soft in softMonitoring.softList.softs)
            {
                if (soft.name.Equals(name))
                {
                    return true;
                }
            }

            return false;
        }

        private bool SoftDel(String name)
        {
            foreach (SoftBean soft in softMonitoring.softList.softs)
            {
                if (soft.name.Equals(name))
                {
                    softMonitoring.softList.softs.Remove(soft);
                    return true;
                }
            }

            return false;
        }

        private void AddSoftListView(string name)
        {
            ListViewItem lvi = new ListViewItem();
           
            lvi.Text = name;
            selectListView.Items.Add(lvi);
            selectListView.Update();
        }
    }
}