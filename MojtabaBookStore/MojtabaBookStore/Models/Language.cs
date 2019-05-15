using System.Collections.Generic;

namespace MojtabaBookStore.Models
{
    public class Language
    {
        public int LanguageID { get; set; }
        public string LanguageName { get; set; }

        public List<Book> Books { get; set; }
    }
}
