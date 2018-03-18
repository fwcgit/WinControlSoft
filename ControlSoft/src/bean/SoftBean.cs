using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlSoft.src.bean
{
    public  class SoftBean
    {
        public String name;
        public String path;

        public SoftBean(string name,String path)
        {
            this.name = name;
            this.path = path;
        }

        public SoftBean()
        {
    
        }
    }
}
