using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;



//Desactivar warning XML comments
#pragma warning disable 1591
namespace Formacio.Service
{
  
    public static class Utils
    {
        public static bool isNull(string s)
        {
            if (String.IsNullOrEmpty(s) || s.Equals("undefined") || s.Equals("null"))
            {
                return true;
            }
            return false;

        }

        public static double? precioFormat(double? precio)
        {
            if (precio.HasValue)
            {
                return Math.Round(precio.Value, 2);
            }

            return null;
        }

    }

    public static class DateTimeUtils
    {
        public static DateTime? ToServer(DateTime? temps)
        {
            if (temps != null)
                return temps.GetValueOrDefault().ToLocalTime();
            else
                return null;
        }

        public static DateTime? ToClient(DateTime? temps)
        {
            if (temps != null)
                return temps.GetValueOrDefault().ToUniversalTime();
            else
                return null;
        }

        public static string ToClientPrint(DateTime temps)
        {
            var spainTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");

            DateTime dateSpain = TimeZoneInfo.ConvertTimeFromUtc(temps.ToUniversalTime(), spainTimeZone);

            return dateSpain.ToString("dd/MM/yyy HH:mm");
        }

        public static string ToClientPrint(DateTime? temps)
        {
            if (temps.HasValue)
            {
                var spainTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");

                DateTime dateSpain = TimeZoneInfo.ConvertTimeFromUtc(temps.Value.ToUniversalTime(), spainTimeZone);

                return dateSpain.ToString("dd/MM/yyy HH:mm");

                
            }
            else
            {
                return "";
            }
        }

        public static DateTime? toSpanish(DateTime? temps)
        {
            if (temps.HasValue)
            {
               
                TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");//GMT+1

                return TimeZoneInfo.ConvertTimeFromUtc(temps.Value.ToUniversalTime(), timeInfo);
            }
            else
            {
                return temps;
            }
        }
        




        //public static DateTime ToClientGrid(DateTime temps)
        //{
        //    return new DateTime(temps.Year, temps.Month, temps.Day, 0, 0, 0).ToUniversalTime();
        //}

        //public static DateTime? ToClientGrid(DateTime? temps)
        //{
        //    if (temps.HasValue)
        //    {
        //        return new DateTime(temps.Value.Year, temps.Value.Month, temps.Value.Day, 0, 0, 0).ToUniversalTime();
        //    }
        //    else
        //    {
        //        return temps;
        //    }
        //}

    }



}