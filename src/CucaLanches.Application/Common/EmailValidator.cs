using System.Net.Mail;

namespace CucaLanches.Application.Common;

public static class EmailValidator
{
   public static bool IsValid(string email)
   {
      try
      {

         var mail = new MailAddress(email);
         return mail.Address == email;
      }
      catch (Exception e)
      {
         Console.WriteLine(e);
         return false;
      }
   }
}