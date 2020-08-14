﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;

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
                lmao.Click+= linker;
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


    public class path
    {
        public StringBuilder optname;
        public unitDialogue next;
        public Line line;

        public path(String text)
        {
            optname = new StringBuilder(text);
            line = null;
            next = null;
        }
    }





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
            addOpt.Click+= addOption;
        }

        public void addOption(object Sender, RoutedEventArgs e)
        {
            object fuck = MaterialDesignThemes.Wpf.DialogHostEx.ShowDialog(hmm,null);
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
            
            
            for(int i = 0; i < pointa.paths.Count; i++)
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
                optname.TextChanged+= editOption;
                cols.Children.Add(optname);


                Label pointername = new Label();
                pointername.Width = 200;
                pointername.Content = (pointa.paths[i].next != null ? ("Dialogue "+pointa.paths[i].next.no.ToString() ): "Null");
                pointername.Margin = new Thickness(100,0,100,0);
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
                deleter.Click+= deleteOption;
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



 
    public partial class MainWindow : Window
    {
        Control draggedItem;
        Point itemRelativePosition;
        bool IsDragging;
        ScaleTransform scaler;

        DialogueEditor only;
        

        unitDialogue linkfrom;
        unitDialogue linkto;

        Dictionary<Button, object> Button_obj_Map;

        unitDialogue head;
        public int nodecount;

        Button prevOpen;

        public MainWindow()
        {
            only = new DialogueEditor();
            head = null;
            IsDragging = false;
            Button_obj_Map = new Dictionary<Button, object>();
            nodecount = 0;
            scaler = new ScaleTransform();
            prevOpen = null;

            InitializeComponent();
            jesus.LayoutTransform = scaler;
            jesus.MouseWheel += scale_Canvas;
            jesus.PreviewMouseWheel += panner_Canvas;


            linkfrom = null;
            linkto = null;
            SidePanel.Children.Add(only.hmm);

        }
        public void scale_Canvas(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                scaler.ScaleX *= 1.1;
                scaler.ScaleY *= 1.1;
            }
            else
            {
                scaler.ScaleX /= 1.1;
                scaler.ScaleY /= 1.1;
            }
            
        }

        public void panner_Canvas(object sender, MouseWheelEventArgs e)
        {

        }

        private void add_Dialogue(object sender, RoutedEventArgs e)
        {
            Button newBtn = new Button();
            newBtn.Height = 32;
            newBtn.Width = 150;
            newBtn.Style = (Style)this.FindResource("MaterialDesignFlatLightBgButton");
            newBtn.Content = "Dialogue "+nodecount.ToString();

            unitDialogue obj = new unitDialogue(nodecount);
            nodecount++;
            Button_obj_Map.Add(newBtn, obj);

            btn_mapper(newBtn);

            Point p = Mouse.GetPosition(jesus);
            Canvas.SetLeft(newBtn, p.X - 10);
            Canvas.SetTop(newBtn, p.Y - 10);
            jesus.Children.Add(newBtn);

            if (head == null)
                head = obj;

        }

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
            
        }

        public void problem_handler(object sender, ContextMenuEventArgs e)
        {
            Button temp = sender as Button;

            if(linkfrom!=null )
            {
                if (linkfrom.contextItemSelected == -1)
                {
                    linkfrom = null;
                }
                else if(linkfrom.contextItemSelected==linkfrom.paths.Count)
                {
                    jesus.Children.Remove(temp);
                    while (linkfrom.paths.Count > 0)
                    {
                        if (linkfrom.paths[0].line == null)
                        {
                            linkfrom.paths.RemoveAt(0);
                        }
                        else
                        {
                            lineDeleter(linkfrom.paths[0].line, null);
                        }
                    }
                    while (linkfrom.from.Count > 0)
                    {
                        lineDeleter(linkfrom.from[0].line,null);
                    }
                    linkfrom.paths = null;
                    linkfrom.from = null;
                    Button_obj_Map.Remove(temp);
                    linkfrom = null;
                }
                else {
                    var converter = new System.Windows.Media.BrushConverter();
                    var brush = (Brush)converter.ConvertFromString("#f5f0ff");
                    jesus.Background = brush;
                }
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
                if (linkfrom.Equals(linkto) || linkfrom.contextItemSelected==-1)
                {
                    linkfrom = null;
                    linkto = null;
                    return;
                }
                linkfrom.pathLinker(linkto);

                
                lineCreator(linkfrom, linkto);
                
                linkfrom = null;
                linkto = null;
                jesus.Background = new SolidColorBrush(Colors.GhostWhite);
            }

            if (prevOpen != null)
                prevOpen.Style = (Style)this.FindResource("MaterialDesignFlatLightBgButton");
            temp.Style = (Style)this.FindResource("MaterialDesignFlatDarkBgButton");
            prevOpen = temp;
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

            Button temp = sender as Button;

            lineUpdater(temp, e);
            
        }

        public void lineCreator(unitDialogue from, unitDialogue to)
        {

            Button temp1 = getKey(from);
            Button temp2 = getKey(to);
            Line line = new Line();
            line.Stroke = System.Windows.Media.Brushes.LightSteelBlue;

            Canvas.SetZIndex(line, -1);

            double X1 = Canvas.GetLeft(temp1) + 75;
            double Y1 = Canvas.GetTop(temp1) + 16;

            double X2 = Canvas.GetLeft(temp2) + 75;
            double Y2 = Canvas.GetTop(temp2) + 16;

            line.X1 = X1;
            line.Y1 = Y1;
            line.X2 = X2;
            line.Y2 = Y2;

            line.StrokeThickness = 5;
            List<unitDialogue> connx = new List<unitDialogue>();
            connx.Add(from);
            connx.Add(to);
            line.Tag = connx;
            line.MouseRightButtonDown+= new MouseButtonEventHandler(lineDeleter);
            linkfrom.paths[linkfrom.contextItemSelected].line = line;
            linkfrom.ContextBuilder();
            linkfrom.contextItemSelected = -1;
            jesus.Children.Add(line);
        }
        public void lineDeleter(object sender, MouseEventArgs e)
        {
            Line temp = sender as Line;
            List<unitDialogue> lmao1 = temp.Tag as List<unitDialogue>;

            int indexToDelete1 = -1;
            int indexToDelete2 = -1;
            
            for(int i = 0; i < lmao1[0].paths.Count; i++)
            {
                if (lmao1[0].paths[i].line.Equals(temp))
                {
                    indexToDelete1 = i;
                    break;
                }
            }

            for (int i = 0; i < lmao1[1].from.Count; i++)
            {
                if (lmao1[1].from[i].line.Equals(temp))
                {
                    indexToDelete2 = i;
                    break;
                }
            }

            lmao1[0].paths[indexToDelete1].line = null;
            lmao1[0].paths[indexToDelete1].next = null;
            lmao1[1].from.RemoveAt(indexToDelete2);

            lmao1[0].ContextBuilder();
            jesus.Children.Remove(temp);
        }


        public void lineUpdater(Button temp, MouseEventArgs e)
        {
            dynamic a;
            Button_obj_Map.TryGetValue(temp, out a);
            unitDialogue slave = a as unitDialogue;
            Point lmao = e.GetPosition(temp);
            Point canvasRelativePosition = e.GetPosition(jesus);
            for (int i = 0; i < slave.from.Count; i++)
            {
                slave.from[i].line.X2 = canvasRelativePosition.X-lmao.X + 75;
                slave.from[i].line.Y2 = canvasRelativePosition.Y-lmao.Y + 16;
            }

            for (int i = 0; i < slave.paths.Count; i++)
            {
                if (slave.paths[i].line != null)
                {
                    slave.paths[i].line.X1 = canvasRelativePosition.X - lmao.X + 75;
                    slave.paths[i].line.Y1 = canvasRelativePosition.Y - lmao.Y + 16;
                }
            }
        }

        public Button getKey(unitDialogue param)
        {
            Button temp = null;
            foreach(KeyValuePair<Button, object> entry in Button_obj_Map)
            {
                if (entry.Value == param)
                    temp = entry.Key;
            }
            return temp;
        }

        public void save(object sender, RoutedEventArgs e)
        {

        }
    }
}
