using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace WpfApp1
{

    // This is the basic unit of a dialogue,
    public class unitDialogue
    {
        // this variable holds the main dialogue
        public StringBuilder dialogue;

        // this variable holds the string val of each option
        public List<StringBuilder> options;

        // FIX LATER //this variable holds the list of pointers to which each option points
        public List<unitDialogue> next;

        public ListView opts;

        // this variable gives the right click line draw / link functionality to each option
        public ContextMenu cm;

        public ContextMenu optionListEdit;

        public bool selectedOpt;

        // Basic constructor 
        public unitDialogue()
        {
            cm = new ContextMenu();
            dialogue = new StringBuilder("AYYY");
            next = new List<unitDialogue>();
            options = new List<StringBuilder>();
            opts = new ListView();
            optionListEdit = new ContextMenu();
            selectedOpt = false;

            // Adding the default option to link to another dialogue with no. of options as 0;
            MenuItem default_item = new MenuItem();
            default_item.Header = "Default";
            cm.Items.Add(default_item);


            // Adding the edit and delete options to the context menu of LISTVIEW for OPTIONS
            MenuItem edit_option = new MenuItem();
            edit_option.Header = "Edit";
            edit_option.Click += editOption;

            MenuItem delete_option = new MenuItem();
            delete_option.Header = "Delete";
            delete_option.Click += deleteOption;

            optionListEdit.Items.Add(edit_option);
            optionListEdit.Items.Add(delete_option);
        }

        public void addOption()
        {
            string option_name = " ayyyy";

            MenuItem temp = new MenuItem();
            temp.Header = option_name;
            temp.Click+= linkOption;
            this.cm.Items.Add(temp);
            this.cm.ContextMenuOpening += link_init;

            options.Add(new StringBuilder(option_name));

            ListViewItem tempUI = new ListViewItem();
            tempUI.Content = option_name;
            tempUI.ContextMenu = optionListEdit;
            opts.Items.Add(tempUI);
        }

        public void link_init(object sender, ContextMenuEventArgs e)
        {
            
        }

        public void editOption(object sender, RoutedEventArgs e)
        {

        }

        public void deleteOption(object sender, RoutedEventArgs e)
        {

        }

        public void linkOption(object sender, RoutedEventArgs e)
        {
            
        }
    }

    public class edge
    {
        public Point start;
        public Point end;
        public string optname;
    }




    // Jesus is the canvas 
    public partial class MainWindow : Window
    {
        Control draggedItem;
        Point itemRelativePosition;
        bool IsDragging;

        // These three elements are common to the bottom half part of the editor for adding text and options to a dialog
        // hmm is the stackpanel that holds the above two elements
        TextBox enterDialogue;
        Button addOptBtn;
        StackPanel hmm;

        
        // These two variables keep track of the current dialogue thats being shown in the bottom Editor part of the App
        Button currOpen;
        unitDialogue currOpenDialog;

        unitDialogue linkfrom;
        unitDialogue linkto;

        

        // Dictionary that maps the buttons in the canvas to the (Dialogue String and option properties) unitDialogue object
        Dictionary<Button, object> Button_obj_Map;

        unitDialogue head;

        public int params_net;

        public MainWindow()
        {
            currOpen = null;
            currOpenDialog = null;
            head = null;
            IsDragging = false;
            Button_obj_Map = new Dictionary<Button, object>();
            hmm = new StackPanel();

            enterDialogue = new TextBox();
            enterDialogue.Style = (Style)Application.Current.FindResource("MaterialDesignFilledTextFieldTextBox");
            enterDialogue.TextWrapping = TextWrapping.Wrap;
            enterDialogue.Margin = new Thickness(5, 0, 5, 10);
            hmm.Children.Add(enterDialogue);

            InitializeComponent();

            linkfrom = null;
            linkto = null;

            addOptBtn = new Button();
            addOptBtn.Height = 32;
            addOptBtn.Width = 150;
            addOptBtn.Style = (Style)this.FindResource("MaterialDesignFlatMidBgButton");
            addOptBtn.Content = "Add Options ";
            addOptBtn.Margin = new Thickness(0, 0, 5, 10);

            //SidePanel.Children.Add(addOptBtn);
            addOptBtn.Click += add_option;

            hmm.Children.Add(addOptBtn);

            SidePanel.Children.Add(hmm);

            /*TRIAL
            Line line = new Line();
            line.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            line.X1 = 0;
            line.X2 = 100;
            line.Y1 = 0;
            line.Y2 = 500;
            jesus.Children.Add(line);*/

        }

        public void add_option(object sender, RoutedEventArgs e)
        {
            if (currOpenDialog != null)
            {
                currOpenDialog.addOption();
                refresher();
            }
            
        }

        public void refresher()
        {
            SidePanel.Children.Clear();
            hmm.Children.Clear();
            hmm.Children.Add(enterDialogue);
            hmm.Children.Add(addOptBtn);
            hmm.Children.Add(currOpenDialog.opts);
            SidePanel.Children.Add(hmm);
        }

        // For addition operations with a right click context menu 

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
            if (linkfrom.selectedOpt == false)
            {
                linkfrom = null;
            }

            debug.Content = "NULL";
        }
        private void btn_simpleclick(object sender, RoutedEventArgs e)
        {
            if (linkfrom == null)
            {
                dynamic a;
                if (currOpen != null)
                {
                    Button_obj_Map.TryGetValue(currOpen, out a);
                    a.dialogue.Clear();
                    a.dialogue.Append(enterDialogue.Text);
                }

                Button temp = sender as Button;
                currOpen = temp;
                Button_obj_Map.TryGetValue(temp, out a);
                currOpenDialog = a;
                enterDialogue.Text = a.dialogue.ToString();
                refresher();
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
    }   
}
