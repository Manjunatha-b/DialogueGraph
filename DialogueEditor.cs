using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public class DialogueEditor
    {
        public MainWindow mw;
        public unitDialogue open;
        public StackPanel hmm;
        public Label dialogName;
        public TextBox enterDialogue;
        public Button addOpt;
        public ListView opts;
        public ContextMenu editOpts;
        public ScrollViewer slave;
        public MaterialDesignThemes.Wpf.DialogHost namer;

        public DialogueEditor()
        {
            mw = (MainWindow)Application.Current.MainWindow;
            open = null;
            hmm = new StackPanel();
            opts = new ListView();

            slave = new ScrollViewer();
            slave.Height = 190;
            slave.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            headerLabelInit();
            enterDialogueInit();
            addOptInit();
        }

        public void headerLabelInit()
        {
            dialogName = new Label();
            dialogName.Content = "Null Lad";
            dialogName.FontSize = 20;
            dialogName.FontWeight = FontWeights.Bold;
            dialogName.Margin = new Thickness(4, 0, 0, 5);
        }

        public void enterDialogueInit()
        {

            enterDialogue = new TextBox();
            enterDialogue.Style = (Style)Application.Current.FindResource("MaterialDesignFilledTextFieldTextBox");
            enterDialogue.TextWrapping = TextWrapping.Wrap;
            enterDialogue.Margin = new Thickness(5, 0, 5, 10);

        }

        public void addOptInit()
        {
            addOpt = new Button();
            addOpt.Height = 32;
            addOpt.Width = 150;
            addOpt.Style = (Style)Application.Current.FindResource("MaterialDesignFlatMidBgButton");
            addOpt.Content = "Add Options ";
            addOpt.Margin = new Thickness(0, 0, 5, 10);
            addOpt.Click += addOption;
        }

        public void addOption(object Sender, RoutedEventArgs e)
        {
            object fuck = MaterialDesignThemes.Wpf.DialogHostEx.ShowDialog(hmm, null);
            if (open.paths.Count == 1 && open.paths[0].optname.ToString().Equals("Default"))
            {
                if (open.paths[0].next != null)
                {
                    mw.lineDeleter(open.paths[0].line, null);
                }
                open.paths.Clear();
            }
            open.paths.Add(new path("ayy"));
            open.ContextBuilder();
            refresher(open);
        }

        public void editOption(object sender, RoutedEventArgs e)
        {
            TextBox temp = sender as TextBox;
            path tagla = temp.Tag as path;
            tagla.optname = new StringBuilder(temp.Text);
            open.ContextBuilder();
        }

        public void deleteOption(object sender, RoutedEventArgs e)
        {
            Button lmao = sender as Button;
            path temp = lmao.Tag as path;

            if (temp.next != null)
            {
                mw.lineDeleter(temp.line, null);
                temp.next = null;
            }
            open.paths.Remove(temp);
            open.ContextBuilder();
            refresher(open);
        }

        public void listBuilder(unitDialogue pointa)
        {
            ListView temp = new ListView();

            for (int i = 0; i < pointa.paths.Count; i++)
            {
                ListViewItem lmao = new ListViewItem();
                StackPanel cols = new StackPanel();
                cols.Orientation = Orientation.Horizontal;

                Label nums = new Label();
                nums.Content = (i + 1).ToString() + ".)";
                nums.Margin = new Thickness(10, 0, 100, 0);
                cols.Children.Add(nums);

                TextBox optname = new TextBox();
                optname.Width = 200;
                optname.Text = pointa.paths[i].optname.ToString();
                optname.Tag = pointa.paths[i];
                optname.TextChanged += editOption;
                cols.Children.Add(optname);

                Label pointername = new Label();
                pointername.Width = 200;
                pointername.Content = (pointa.paths[i].next != null ? ("Dialogue " + pointa.paths[i].next.no.ToString()) : "Null");
                pointername.Margin = new Thickness(100, 0, 100, 0);
                cols.Children.Add(pointername);

                Button deleter = new Button();
                MaterialDesignThemes.Wpf.PackIcon b = new MaterialDesignThemes.Wpf.PackIcon();
                b.Kind = MaterialDesignThemes.Wpf.PackIconKind.Delete;
                deleter.Content = b;
                deleter.Style = (Style)mw.FindResource("MaterialDesignIconForegroundButton");
                deleter.Width = 26;
                deleter.Height = 26;
                deleter.Tag = pointa.paths[i];
                deleter.Margin = new Thickness(100, 0, 0, 0);
                deleter.Click += deleteOption;
                cols.Children.Add(deleter);

                lmao.Content = cols;
                temp.Items.Add(lmao);
            }
            slave.Content = temp;
            hmm.Children.Add(slave);

        }

        public void refresher(unitDialogue pointa)
        {
            open = pointa;
            hmm.Children.Clear();
            slave.Content = null;
            enterDialogue.Text = pointa.dialogue.ToString();
            dialogName.Content = "Dialogue " + open.no.ToString();
            hmm.Children.Add(dialogName);
            hmm.Children.Add(enterDialogue);
            hmm.Children.Add(addOpt);
            listBuilder(pointa);
        }
    }
}
