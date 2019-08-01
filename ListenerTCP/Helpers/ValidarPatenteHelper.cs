using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ListenerTCP.Helpers
{
    public class ValidarPatenteHelper
    {
        public static bool ValidarPatente (string patente)
        {
            if (Regex.IsMatch(patente, "^[A-Z]{3}[0-9]{3}$") || Regex.IsMatch(patente, "^[a-z]{3}[0-9]{3}$"))
            {
                return true;
            }

            else if (Regex.IsMatch(patente, "^[A-Z]{2}[0-9]{3}[A-Z]{2}$") || Regex.IsMatch(patente, "^[a-z]{3}[0-9]{3}[a-z]{2}$"))
            {
                return true;
            }

            else if (patente.Contains("E/T"))
            {
                return true;
            }

            else
            {
                return false;
            }            
        }
    }
}
