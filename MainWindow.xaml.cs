using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp1
{

    public class unitDialogue
    {

        public StringBuilder dialogue;

        public List<StringBuilder> options;

        public List<unitDialogue> next;

        public ContextMenu cm;

        public unitDialogue()
        {
            dialogue = new StringBuilder("AYYY");
            next = new List<unitDialogue>();
            options = new List<StringBuilder>();
            cm = new ContextMenu();
        }

        public ContextMenu ContextBuilder()
        {
            cm.Items.Clear();
            for(int i = 0; i < options.Count; i++)
            {
                MenuItem lmao = new MenuItem();
                lmao.Header = options[i];
                cm.Items.Add(lmao);
            }
            return cm;
        }
    }

    public class edge
    {
        public Point start;
        public Point end;
        public string optname;
    }

    public class DialogueEditor
    {
        public unitDialogue open;
        public StackPanel hmm;
        public TextBox enterDialogue;
        public Button addOpt;
        public ListView opts;
        public ContextMenu editOpts;
        
        public DialogueEditor()
        {
            open = null;
            hmm = new StackPanel();
            opts = new ListView();
            enterDialogueInit();
            editOptsInit();
            addOptInit();
            refresher();
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
            addOpt.Click+= addOption;
        }

        public void editOptsInit()
        {
            editOpts = new ContextMenu();
            MenuItem edit_option = new MenuItem();
            edit_option.Header = "Edit";
            edit_option.Click += editOption;

            MenuItem delete_option = new MenuItem();
            delete_option.Header = "Delete";
            delete_option.Click += deleteOption;

            editOpts.Items.Add(edit_option);
            editOpts.Items.Add(delete_option);
        }

        public void addOption(object Sender, RoutedEventArgs e)
        {
            open.options.Add(new StringBuilder("ayy"));
            open.ContextBuilder();
            listBuilder(open);
        }

        public void editOption(object sender, RoutedEventArgs e)
        {

        }

        public void deleteOption(object sender, RoutedEventArgs e)
        {

        }

        public void refresher()
        {
            hmm.Children.Clear();
            hmm.Children.Add(enterDialogue);
            hmm.Children.Add(addOpt);
            hmm.Children.Add(opts);
        }

        public void listBuilder(unitDialogue pointa)
        {
            opts.Items.Clear();
            hmm.Children.Remove(opts);
            for(int i = 0; i < pointa.options.Count; i++)
            {
                opts.Items.Add(listItemBuilder(pointa.options[i].ToString()));
            }
            hmm.Children.Add(opts);
        }

        public ListViewItem listItemBuilder(String text)
        {
            ListViewItem temp = new ListViewItem();
            temp.Content = text;
            temp.ContextMenu = editOpts;
            return temp;
        }

        public void refresher(unitDialogue pointa)
        {
            open = pointa;
            hmm.Children.Clear();
            enterDialogue.Text = pointa.dialogue.ToString();
            hmm.Children.Add(enterDialogue);
            hmm.Children.Add(addOpt);
            listBuilder(pointa);
           
        }
    }



    // Jesus is the canvas 
    public partial class MainWindow : Window
    {
        Control draggedItem;
        Point itemRelativePosition;
        bool IsDragging;
        bool selectedItem;

        DialogueEditor only;
        
        // These two variables keep track of the current dialogue thats being shown in the bottom Editor part of the App
        unitDialogue linkfrom;
        unitDialogue linkto;

        // Dictionary that maps the buttons in the canvas to the (Dialogue String and option properties) unitDialogue object
        Dictionary<Button, object> Button_obj_Map;

        unitDialogue head;

        public MainWindow()
        {
            only = new DialogueEditor();
            head = null;
            IsDragging = false;
            Button_obj_Map = new Dictionary<Button, object>();
            selectedItem = false;

            InitializeComponent();

            linkfrom = null;
            linkto = null;
            SidePanel.Children.Add(only.hmm);

            /*TRIAL
            Line line = new Line();
            line.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            line.X1 = 0;
            line.X2 = 100;
            line.Y1 = 0;
            line.Y2 = 500;
            jesus.Children.Add(line);*/
        }


        private void add_Dialogue(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button newBtn = new Button();
            newBtn.Height = 32;
            newBtn.Width = 150;
            newBtn.Style = (Style)this.FindResource("MaterialDesignFlatMidBgButton");
            newBtn.Content = "Dialogue1";

            unitDialogue obj = new unitDialogue();
            Button_obj_Map.Add(newBtn, obj);

            btn_mapper(newBtn);

            Point p = Mouse.GetPosition(jesus);
            Canvas.SetLeft(newBtn, p.X - 10);
            Canvas.SetTop(newBtn, p.Y - 10);
            jesus.Children.Add(newBtn);

            if (head == null)
                head = obj;

        }

        // This is for all button operations 


        private void btn_mapper(object sender)
        {

            Button pointa = sender as Button;
            pointa.Click += btn_simpleclick;


            pointa.PreviewMouseLeftButtonDown += btn_PreviewMouseLeftButtonDown;
            pointa.PreviewMouseLeftButtonUp += btn_PreviewMouseLeftButtonUp;
            pointa.PreviewMouseMove += btn_PreviewMouseMove;

            dynamic a;
            Button_obj_Map.TryGetValue(pointa, out a);
            unitDialogue temp = a as unitDialogue;
            pointa.ContextMenu = a.cm;
            pointa.ContextMenuOpening += linkfrom_init;
            pointa.ContextMenuClosing += problem_handler;
        }
        
        public void linkfrom_init(object sender, ContextMenuEventArgs e)
        {
            dynamic a;
            Button temp = sender as Button;
            Button_obj_Map.TryGetValue(temp, out a);       
            linkfrom = a;
            debug.Content = (linkfrom.dialogue.ToString());
        }

        public void problem_handler(object sender, ContextMenuEventArgs e)
        {
            if (selectedItem == false)
            {
                linkfrom = null;
                debug.Content = "NULL";
            }
        }

        private void btn_simpleclick(object sender, RoutedEventArgs e)
        {
            Button temp = sender as Button;
            dynamic a;
            Button_obj_Map.TryGetValue(temp, out a);
            if (linkfrom == null)
            {
                
                if (only.open != null)
                {
                    only.open.dialogue.Clear();
                    only.open.dialogue.Append(only.enterDialogue.Text);
                }

                Button_obj_Map.TryGetValue(temp, out a);
                only.open = a as unitDialogue;
                only.refresher(only.open);

            }
            else
            {
                linkto = a;
                linkfrom.next.Add(linkto);
                linkfrom = a;
                linkto = null;
            }
        }

        private void btn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            IsDragging = true;
            draggedItem = (Button)sender;
            itemRelativePosition = e.GetPosition(draggedItem);
        }

        private void btn_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Button temp = sender as Button;
            Point p = Mouse.GetPosition(jesus);


            if (!IsDragging)
                return;

            IsDragging = false;
        }

        private void btn_PreviewMouseMove(object sender, MouseEventArgs e)
        {

            if (!IsDragging)
                return;

            Point canvasRelativePosition = e.GetPosition(jesus);

            Canvas.SetTop(draggedItem, canvasRelativePosition.Y - itemRelativePosition.Y);
            Canvas.SetLeft(draggedItem, canvasRelativePosition.X - itemRelativePosition.X);
        }    

        public ContextMenu contextGenerator (unitDialogue pointa)
        {
            ContextMenu temp = new ContextMenu();
            if (pointa.options.Count == 0)
            {
                temp.Items.Add(menuItemGenerator("Default"));
            }
            return temp;
        }
               

        public MenuItem menuItemGenerator(String text)
        {
            MenuItem temp = new MenuItem();
            temp.Header = text;
            return temp;
        }
    }   
}
