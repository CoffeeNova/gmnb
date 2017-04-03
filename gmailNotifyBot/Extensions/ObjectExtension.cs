using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

namespace CoffeeJelly.gmailNotifyBot.Extensions
{
    public static class ObjectExtension
    {
        //public static void GetFields(this object obj, List<FieldInfo> fieldsInfo)
        //{
        //    if (obj == null) return;

        //    if (fieldsInfo == null)
        //        throw new ArgumentNullException(nameof(fieldsInfo),
        //            $"Initialize {nameof(fieldsInfo)} before calling.");

        //    var objType = obj.GetType();
        //    var fInfo = objType.GetFields();

        //    foreach (FieldInfo field in fInfo)
        //    {
        //        object propValue = field.GetValue(obj);
        //        if (field.FieldType.Assembly == objType.Assembly)
        //            propValue.GetFields(fieldsInfo);
        //        else
        //            fieldsInfo.Add(field);
        //    }
        //}

        //public static void FillPropertiesList(this object obj, List<KeyValuePair<PropertyInfo, object>> propertiesList)
        //{
        //    if (obj == null) return;

        //    if (propertiesList == null)
        //        throw new ArgumentNullException(nameof(propertiesList),
        //            $"Initialize {nameof(propertiesList)} before calling.");
        //    var objType = obj.GetType();
        //    var pInfo = objType.GetProperties();
        //    if (pInfo.Length == 0)
        //        return;

        //    foreach (PropertyInfo prop in pInfo)
        //    {
        //        object propValue = prop.GetValue(obj);
        //        try
        //        {
        //            var haveProp = propValue.GetType().GetProperties().Any();
        //            if(!haveProp)
        //                propertiesList.Add(new KeyValuePair<PropertyInfo, object>(prop, propValue));
        //            else
        //                propValue.FillPropertiesList(propertiesList);
        //        }
        //        catch(TargetParameterCountException)
        //        {
        //            propertiesList.Add(new KeyValuePair<PropertyInfo, object>(prop, propValue));
        //        }
        //    }
        //}
    }

    //public struct FieldInfo
    //{
    //    public string FieldName { get; set; }
    //    public Type FieldType { get; set; }
    //    public object FieldValue { get; set; }
    //}


    //public struct PropertyInfo
    //{
    //    public string PropertyName { get; set; }
    //    public Type PropertyType { get; set; }
    //    public object PropertyValue { get; set; }
    //}
}