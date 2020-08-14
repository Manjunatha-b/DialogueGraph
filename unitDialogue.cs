using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;


namespace WpfApp1
{
    public class unitDialogue
    {
        public int no;
        public StringBuilder dialogue;
        public List<path> paths;
        public List<path> from;
        public ContextMenu cm;
        public int contextItemSelected;
        public unitDialogue(int p1)
        {
            dialogue = new StringBuilder("AYYY");
            paths = new List<path>();
            from = new List<path>();
            cm = new ContextMenu();
            contextItemSelected = -1;
            no = p1;

            defaultContextInit();
            deleteContextInit();
        }

        public void defaultContextInit()
        {
            MenuItem lmao = new MenuItem();
            lmao.Header = "Default";
            lmao.Click += linker;
            lmao.Tag = 0;
            cm.Items.Add(lmao);
            paths.Add(new path("Default"));
        }

        public void deleteContextInit()
        {
            MenuItem delet = new MenuItem();
            delet.Header = "Delete";
            delet.Tag = paths.Count;
            delet.Click += linker;
            cm.Items.Add(delet);
        }

        public void ContextBuilder()
        {
            cm.Items.Clear();
            for (int i = 0; i < paths.Count; i++)
            {
                MenuItem lmao = new MenuItem();
                lmao.Header = paths[i].optname.ToString();
                if (paths[i].line != null)
                {
                    lmao.IsEnabled = false;
                }
                lmao.Click += linker;
                lmao.Tag = i;
                cm.Items.Add(lmao);
            }
            deleteContextInit();
        }

        public void linker(object sender, RoutedEventArgs e)
        {
            MenuItem temp = sender as MenuItem;
            int index = int.Parse(temp.Tag.ToString());
            contextItemSelected = index;
        }

        public void pathLinker(unitDialogue to)
        {
            if (contextItemSelected == -1)
                return;
            paths[contextItemSelected].next = to;
            to.from.Add(this.paths[contextItemSelected]);
        }
    }
}
