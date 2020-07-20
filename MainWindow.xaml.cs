using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;

namespace WpfApp1
{
    // Jesus is the canvas 
    public partial class MainWindow : Window
    {
        Control draggedItem;
        Point itemRelativePosition;
        bool IsDragging;

        TextBox enterDialogue;
        StackPanel hmm;

        unitDialogue currOpen;

        // Dictionary of buttons and side panel details
        Dictionary<Button, object> Button_obj_Map;

        // Dictionary of buttons and their positions on the canvas
        // Future : Replace with linked list or tree of some sort
        Dictionary<Button, double> orderMap;

        public int params_net;

        public MainWindow()
        {
            currOpen = null;
            IsDragging = false;
            Button_obj_Map = new Dictionary<Button, object>();
            hmm = new StackPanel();
            orderMap = new Dictionary<Button, double>();
            enterDialogue = new TextBox();
            enterDialogue.Style = (Style)Application.Current.FindResource("MaterialDesignFloatingHintTextBox");
            hmm.Children.Add(enterDialogue);
            InitializeComponent();
            params_net= 0;
            SidePanel.Children.Add(hmm);
        }

       // For addition operations with a right click context menu 

        private void add_Dialogue(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button newBtn = new Button();
            newBtn.Height = 32;
            newBtn.Width = 85;
            newBtn.Style = (Style)this.FindResource("MaterialDesignFlatMidBgButton");
            newBtn.Content = "Dialogue1";

            btn_mapper(newBtn);

            Point p = Mouse.GetPosition(jesus);
            Canvas.SetLeft(newBtn, p.X - 10);
            Canvas.SetTop(newBtn, p.Y - 10);
            jesus.Children.Add(newBtn);
            orderMap.Add(newBtn, p.X);

            unitDialogue obj = new unitDialogue();
            Button_obj_Map.Add(newBtn, obj);
        }

        // This is for all button operations 


        private void btn_mapper(object sender)
        {
            Button pointa = sender as Button;
            pointa.Click += btn_simpleclick;
            pointa.PreviewMouseLeftButtonDown += btn_PreviewMouseLeftButtonDown;
            pointa.PreviewMouseLeftButtonUp += btn_PreviewMouseLeftButtonUp;
            pointa.PreviewMouseMove += btn_PreviewMouseMove;

        }

        private void btn_simpleclick(object sender, RoutedEventArgs e)
        {
            /*if (currOpen != null)
            {
                Button_obj_Map.TryGetValue()
            }*/

            Button temp = sender as Button;
           // SidePanel.Children.Clear();
            dynamic a;
            Button_obj_Map.TryGetValue(temp, out a);
            enterDialogue.Text = a.dialogue.ToString();
           // SidePanel.Children.Add(a.fin);
           // SidePanel.Children.Add(a.apply);
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
            orderMap[temp] = p.X;

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

           

        private void Generate_handler(object sender, RoutedEventArgs e)
        {

        }
    }

    public class unitDialogue
    {
        public StringBuilder dialogue;
        public StringBuilder[] options;
        public List<unitDialogue> next;

        public unitDialogue()
        {
            dialogue = new StringBuilder("AYYY");
            
        }
    }


    public abstract class layer_template
    {
        public string type;
        public StringBuilder[] data;
        public List<object> ctrls;
        public StackPanel fin;
        public Button apply;

        public void sbinit()
        {
            for (int i = 0; i < data.Length; i++)
                data[i] = new StringBuilder("0");
        }
        public void applyinit()
        {
            apply = new Button();
            apply.Content = "Apply";
            apply.VerticalAlignment = VerticalAlignment.Bottom;
            apply.Click += apply_prototype;
        }

        public void apply_prototype(object sender, RoutedEventArgs e)
        {
            int i = 0;
            foreach(object iter in ctrls)
            {
                TextBox temp = iter as TextBox;
                data[i].Clear();
                data[i].Append(temp.Text);
                i++;
            }
        }

    }

    public class dense_ctr : layer_template
    {
        public dense_ctr()
        {
            type = new string("Dense");
            data = new StringBuilder[1];
            sbinit();
            ctrls = new List<object>();
            fin = new StackPanel();
            fin.Orientation = Orientation.Vertical;
            applyinit();

            Label l1 = new Label();
            l1.Content = "Dense";
            l1.Margin = new Thickness(0,10,0,10);

            TextBox t1 = new TextBox();t1.Style = (Style)Application.Current.FindResource("MaterialDesignFloatingHintTextBox");
            
            MaterialDesignThemes.Wpf.HintAssist.SetHint(t1, "Nodes");
            t1.Margin = new Thickness(0, 10, 0, 10);
            t1.Text = data[0].ToString();

            fin.Children.Add(l1);
            fin.Children.Add(t1);
            ctrls.Add(t1);
        }
    }
}
