using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.LearningMode.Models
{
    public class AnswersRegister
    {
        public int Words { get; set; } = 0;

        public int Translations { get; set; } = 0;

        public int this[string propertyName]
        {
            get
            {
                PropertyInfo propertyInfo = GetPropertyInfo(propertyName);
                object? value = propertyInfo.GetValue(this, null);
                int number = Convert.ToInt32(value);
                return number;
            }
            set
            {
                PropertyInfo propertyInfo = GetPropertyInfo(propertyName);
                propertyInfo.SetValue(this, value, null);
            }

        }

        public PropertyInfo GetPropertyInfo(string propertyName)
        {
            Type myType = GetType();
            PropertyInfo? myPropInfo = myType.GetProperty(propertyName);
            if (myPropInfo == null)
            {
                throw new Exception($"Property AnswersRegister[\"{propertyName}\"] does not exist.");
            }
            return myPropInfo;
        }
    }
}
