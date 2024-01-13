using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDB
{
    internal class Books
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int PublicationYear { get; set; }
        public int AuthorID { get; set; }
    }
}
