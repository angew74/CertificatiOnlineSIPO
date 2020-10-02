using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Unisys.CdR.Certi.WS.Business
{
    public enum PostDataParamType
    {
        Field,
        File
    }

    public class PostDataParam
    {
        public PostDataParam(string key, object value, PostDataParamType type)
        {
            Key = key;
            Value = value;
            Type = type;
        }
        public string Key;
        public string FileName;
        public object Value;
        public PostDataParamType Type;
    }
}
