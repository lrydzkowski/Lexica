using System;
using System.Reflection;

namespace Lexica.Learning.Models
{
    public class AnswerRegister
    {
        public AnswerRegisterValue Words { get; set; } = new AnswerRegisterValue();

        public AnswerRegisterValue Translations { get; set; } = new AnswerRegisterValue();

        public AnswerRegisterValue this[string propertyName]
        {
            get
            {
                PropertyInfo propertyInfo = GetPropertyInfo(propertyName);
                object? value = propertyInfo.GetValue(this, null);
                if (value == null)
                {
                    throw new Exception($"Value AnswerRegister['{propertyName}'] is null.");
                }
                AnswerRegisterValue number = (AnswerRegisterValue)value;
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