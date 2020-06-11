using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication
{
     public class PersonFactory
    {
         public static Person CreatePerson(string personType)
         {
             Person person;
             if (personType == "Chinese")
             {
                 person = new Chinese();
             }
             else if (personType == "English")
             {
                 person = new English();
             }
             else
             {
                 person = new Person();
             }
             return person;
         }
    }
}
