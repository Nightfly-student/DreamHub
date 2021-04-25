using System;
using System.Collections.Generic;
using System.Text;

namespace Lucid
{
    class ReadUser
    {
        private int Id;

        public int id
        {
            get { return Id; }
            set { Id = value; }
        }

        public ReadUser(int id)
        {
            this.id = id;
        }
        public override string ToString()
        {
            return "(" + id + ")";
        }
    }

}
