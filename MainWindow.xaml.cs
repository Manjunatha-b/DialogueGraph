using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;

namespace WpfApp1
{
  
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

            unitDialogue temp = getUnitDialogueFromButton(pointa) as unitDialogue;
            pointa.ContextMenu = temp.cm;
            pointa.ContextMenuOpening += linkfrom_init;
            pointa.ContextMenuClosing += problem_handler;
        }
        
        public void linkfrom_init(object sender, ContextMenuEventArgs e)
        {
            linkfrom = getUnitDialogueFromButton(sender);
        }

        public dynamic getUnitDialogueFromButton(object sender)
        {
            dynamic a;
            Button temp = sender as Button;
            Button_obj_Map.TryGetValue(temp, out a);
            return a;
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
            unitDialogue next= getUnitDialogueFromButton(sender) as unitDialogue;

            if (linkfrom == null)
            {
                if (only.open != null)
                {
                    only.open.dialogue.Clear();
                    only.open.dialogue.Append(only.enterDialogue.Text);
                }
                only.open = next;
                only.refresher(only.open);
            }

            else
            {
                linkto = next;
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
            Line line = new Line();
            line.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            line.StrokeThickness = 5;
            Canvas.SetZIndex(line, -1);

            var c1 = lineCreator_coordinatesHelper(from);
            var c2 = lineCreator_coordinatesHelper(to);

            lineCoordinatesUpdater(line, c1.Item1, c1.Item2,1);
            lineCoordinatesUpdater(line, c2.Item1, c2.Item2, 2);

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

        public Tuple<double,double> lineCreator_coordinatesHelper(unitDialogue lmao)
        {
            Button temp = getKey(lmao);
            double X1 = Canvas.GetLeft(temp) + 75;
            double Y1 = Canvas.GetTop(temp) + 16;
            return new Tuple<double,double>(X1,Y1);
        }

        public void lineCoordinatesUpdater(Line temp, double X, double Y,int pos)
        {
            if (pos == 1)
            {
                temp.X1 = X;
                temp.Y1 = Y;
            }
            else
            {
                temp.X2 = X;
                temp.Y2 = Y;
            }
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
            unitDialogue slave = getUnitDialogueFromButton(temp) as unitDialogue;
            Point lmao = e.GetPosition(temp);
            Point canvasRelativePosition = e.GetPosition(jesus);
            for (int i = 0; i < slave.from.Count; i++)
                lineCoordinatesUpdater(slave.from[i].line, canvasRelativePosition.X-lmao.X + 75, canvasRelativePosition.Y - lmao.Y + 16,2);

            for (int i = 0; i < slave.paths.Count; i++)
                if (slave.paths[i].line != null)
                    lineCoordinatesUpdater(slave.paths[i].line, canvasRelativePosition.X - lmao.X + 75, canvasRelativePosition.Y - lmao.Y + 16, 1);
        }

        public Button getKey(unitDialogue param)
        {
            Button temp = null;
            foreach(KeyValuePair<Button, object> entry in Button_obj_Map)
                if (entry.Value == param)
                    temp = entry.Key;
            return temp;
        }

        public void save(object sender, RoutedEventArgs e)
        {

        }
    }
}
