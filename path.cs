using System;
using System.Text;
using System.Windows.Shapes;

namespace WpfApp1
{
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
}
